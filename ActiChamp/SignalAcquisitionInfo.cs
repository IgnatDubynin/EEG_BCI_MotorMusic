using System;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

namespace PolyHubIpc
{
    // PHS/ring version 2: causal second-order Butterworth HP -> LP;
    // optional 50 Hz notch (2 Hz bandwidth). REF is the mean of the named inputs.
    public enum PhysicalInputKind { Eeg = 0, Aux = 1, Ground = 2 }
    [Flags]
    public enum PhysicalInputRole { None = 0, Signal = 1, Reference = 2, HeogLeft = 4, HeogRight = 8, VeogUp = 16, VeogDown = 32 }
    public sealed class PhysicalInputInfo
    {
        public PhysicalInputKind Kind { get; set; }
        public int Number { get; set; } // One-based within EEG/AUX; zero for GND.
        public string Name { get; set; }
        public PhysicalInputRole Role { get; set; }
        public uint ImpedanceOhm { get; set; } = uint.MaxValue; // No reliable numeric measurement.
    }

    public sealed class SignalAcquisitionInfo
    {
        public const int PrepareRecordingCommand = 3100;
        public long ImpedanceMeasuredUtcTicks { get; set; }
        public PhysicalInputInfo[] PhysicalInputs { get; set; } = new PhysicalInputInfo[0];
        internal const int PhysicalInputBytes = 80;
        public double EegLowCutoffHz { get; set; }
        public double EegHighCutoffHz { get; set; }
        public double EogLowCutoffHz { get; set; }
        public double EogHighCutoffHz { get; set; }
        public bool Notch50Hz { get; set; }
        public string[] ReferenceChannels { get; set; }
        public int EegChannelCount { get; set; }
        public int EogChannelCount { get; set; }

        // Fixed-width binary fields; identical layout in x86 and x64 processes.
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BinaryHeader
        {
            public int Flags; // bit 0: parameters known; bit 1: notch enabled
            public int EegChannelCount;
            public int EogChannelCount;
            public int ReferenceCount;
            public double EegLowCutoffHz;
            public double EegHighCutoffHz;
            public double EogLowCutoffHz;
            public double EogHighCutoffHz;
            public long ImpedanceMeasuredUtcTicks;
            public int PhysicalInputCount;
        }
        internal const int HeaderBytes = 60;
        internal const int NameBytes = 64; // Existing 32 UTF-16 code units, including terminator.

        internal static BinaryHeader Encode(SignalAcquisitionInfo info, int channels, int sampleRate)
        {
            if (info == null) return new BinaryHeader(); // Unknown is distinct from filters/REF off.
            if (info.ReferenceChannels == null || info.PhysicalInputs == null) throw new InvalidDataException("Missing reference names.");
            var h = new BinaryHeader {
                Flags = 1 | (info.Notch50Hz ? 2 : 0), EegChannelCount = info.EegChannelCount,
                EogChannelCount = info.EogChannelCount, ReferenceCount = info.ReferenceChannels.Length,
                EegLowCutoffHz = info.EegLowCutoffHz, EegHighCutoffHz = info.EegHighCutoffHz,
                EogLowCutoffHz = info.EogLowCutoffHz, EogHighCutoffHz = info.EogHighCutoffHz,
                ImpedanceMeasuredUtcTicks = info.ImpedanceMeasuredUtcTicks, PhysicalInputCount = info.PhysicalInputs.Length
            };
            Validate(h, channels, sampleRate);
            foreach (string name in info.ReferenceChannels)
                if (string.IsNullOrWhiteSpace(name) || name.Length > 31 || name.IndexOf('\0') >= 0)
                    throw new InvalidDataException("Invalid reference name (maximum 31 characters).");
            var keys = new System.Collections.Generic.HashSet<string>();
            foreach (var input in info.PhysicalInputs)
            {
                ValidateInput(input);
                if (!keys.Add(input.Kind + ":" + input.Number)) throw new InvalidDataException("Duplicate physical input.");
            }
            return h;
        }
        internal static void Validate(BinaryHeader h, int channels, int sampleRate)
        {
            if ((h.Flags & ~3) != 0 || h.Flags == 2 || h.EegChannelCount < 0 || h.EogChannelCount < 0 ||
                (long)h.EegChannelCount + h.EogChannelCount > channels || h.ReferenceCount < 0 ||
                h.ReferenceCount > 65536 || h.PhysicalInputCount < 0 || h.PhysicalInputCount > 65536 ||
                h.ImpedanceMeasuredUtcTicks < 0 || h.ImpedanceMeasuredUtcTicks > DateTime.MaxValue.Ticks)
                throw new InvalidDataException("Invalid acquisition header.");
            double[] values = { h.EegLowCutoffHz, h.EegHighCutoffHz, h.EogLowCutoffHz, h.EogHighCutoffHz };
            foreach (double v in values)
                if (double.IsNaN(v) || double.IsInfinity(v) || v < 0 || (v > 0 && v >= sampleRate / 2.0))
                    throw new InvalidDataException("Invalid filter frequency.");
            if ((h.EegLowCutoffHz > 0 && h.EegHighCutoffHz > 0 && h.EegLowCutoffHz >= h.EegHighCutoffHz) ||
                (h.EogLowCutoffHz > 0 && h.EogHighCutoffHz > 0 && h.EogLowCutoffHz >= h.EogHighCutoffHz))
                throw new InvalidDataException("Invalid filter band.");
            if (h.Flags == 0 && (h.PhysicalInputCount != 0 || h.ImpedanceMeasuredUtcTicks != 0 || h.EegChannelCount != 0 || h.EogChannelCount != 0 || h.ReferenceCount != 0 ||
                h.EegLowCutoffHz != 0 || h.EegHighCutoffHz != 0 || h.EogLowCutoffHz != 0 || h.EogHighCutoffHz != 0))
                throw new InvalidDataException("Invalid unknown acquisition header.");
        }
        internal static SignalAcquisitionInfo Decode(MemoryMappedViewAccessor view, BinaryHeader h, long namesOffset)
        {
            if ((h.Flags & 1) == 0) return null;
            string[] names = new string[h.ReferenceCount];
            for (int i = 0; i < names.Length; i++)
            {
                char[] chars = new char[32];
                for (int j = 0; j < chars.Length; j++) chars[j] = (char)view.ReadUInt16(namesOffset + i * (long)NameBytes + j * 2);
                int length = Array.IndexOf(chars, '\0');
                if (length <= 0 || string.IsNullOrWhiteSpace(new string(chars, 0, length)))
                    throw new InvalidDataException("Invalid reference name.");
                names[i] = new string(chars, 0, length);
            }
            long physicalOffset = namesOffset + (long)h.ReferenceCount * NameBytes;
            var inputs = new PhysicalInputInfo[h.PhysicalInputCount];
            var keys = new System.Collections.Generic.HashSet<string>();
            for (int i = 0; i < inputs.Length; i++)
            {
                long at = physicalOffset + (long)i * PhysicalInputBytes;
                var input = new PhysicalInputInfo {
                    Kind = (PhysicalInputKind)view.ReadInt32(at), Number = view.ReadInt32(at + 4),
                    Name = ReadName(view, at + 8), Role = (PhysicalInputRole)view.ReadInt32(at + 72),
                    ImpedanceOhm = view.ReadUInt32(at + 76)
                };
                ValidateInput(input);
                if (!keys.Add(input.Kind + ":" + input.Number)) throw new InvalidDataException("Duplicate physical input.");
                inputs[i] = input;
            }
            return new SignalAcquisitionInfo {
                PhysicalInputs = inputs, ImpedanceMeasuredUtcTicks = h.ImpedanceMeasuredUtcTicks,
                EegChannelCount = h.EegChannelCount, EogChannelCount = h.EogChannelCount,
                EegLowCutoffHz = h.EegLowCutoffHz, EegHighCutoffHz = h.EegHighCutoffHz,
                EogLowCutoffHz = h.EogLowCutoffHz, EogHighCutoffHz = h.EogHighCutoffHz,
                Notch50Hz = (h.Flags & 2) != 0, ReferenceChannels = names
            };
        }
        internal static void WriteNames(MemoryMappedViewAccessor view, long offset, string[] names)
        {
            if (names == null) return;
            for (int i = 0; i < names.Length; i++)
                for (int j = 0; j < 32; j++)
                    view.Write(offset + i * (long)NameBytes + j * 2, (ushort)(j < names[i].Length ? names[i][j] : 0));
        }
        private static void ValidateInput(PhysicalInputInfo input)
        {
            if (input == null || !Enum.IsDefined(typeof(PhysicalInputKind), input.Kind) ||
                (input.Kind == PhysicalInputKind.Ground ? input.Number != 0 : input.Number <= 0) ||
                string.IsNullOrWhiteSpace(input.Name) || input.Name.Length > 31 || input.Name.IndexOf('\0') >= 0 ||
                ((int)input.Role & ~63) != 0 || (input.ImpedanceOhm != uint.MaxValue && input.ImpedanceOhm >= 999900))
                throw new InvalidDataException("Invalid physical input metadata.");
        }
        private static string ReadName(MemoryMappedViewAccessor view, long at)
        {
            var chars = new char[32];
            for (int i = 0; i < chars.Length; i++) chars[i] = (char)view.ReadUInt16(at + i * 2);
            int end = Array.IndexOf(chars, '\0');
            if (end <= 0) throw new InvalidDataException("Invalid physical input name.");
            return new string(chars, 0, end);
        }
        internal static void WritePhysicalInputs(MemoryMappedViewAccessor view, long at, PhysicalInputInfo[] inputs)
        {
            if (inputs == null) return;
            foreach (var input in inputs)
            {
                view.Write(at, (int)input.Kind); view.Write(at + 4, input.Number);
                WriteNames(view, at + 8, new[] { input.Name });
                view.Write(at + 72, (int)input.Role); view.Write(at + 76, input.ImpedanceOhm);
                at += PhysicalInputBytes;
            }
        }
        public static uint MeanImpedance(uint first, uint second, uint third)
        {
            if (first >= 999900 || second >= 999900 || third >= 999900) return uint.MaxValue;
            return (uint)(((ulong)first + second + third + 1) / 3);
        }
        private static string Number(double value) { return value.ToString("0.###", CultureInfo.InvariantCulture); }
        private static string Band(double low, double high)
        {
            return low == 0 && high == 0 ? "off" : (low > 0 ? Number(low) : "off") + "–" + (high > 0 ? Number(high) : "off") + " Hz";
        }
        public static string Status(int sampleRate, SignalAcquisitionInfo info)
        {
            string text = sampleRate.ToString(CultureInfo.InvariantCulture) + " Hz | ";
            if (info == null) return text + "Filters: not recorded | REF: not recorded";
            if (info.EegChannelCount > 0) text += "EEG: " + Band(info.EegLowCutoffHz, info.EegHighCutoffHz) + " | ";
            if (info.EogChannelCount > 0) text += "EOG: " + Band(info.EogLowCutoffHz, info.EogHighCutoffHz) + " | ";
            return text + "Notch: " + (info.Notch50Hz ? "50 Hz" : "off") + " | REF: " +
                (info.ReferenceChannels.Length == 0 ? "none" : string.Join(", ", info.ReferenceChannels) +
                 (info.ReferenceChannels.Length > 1 ? " (mean)" : ""));
        }
    }
}
