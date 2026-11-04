using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PolyHubNet
{
    [DataContract]
    public sealed class RecordingAnalysis
    {
        [DataMember(Order = 1, IsRequired = true)] public int Version { get; set; }
        [DataMember(Order = 2, IsRequired = true)] public int SampleRate { get; set; }
        [DataMember(Order = 3, IsRequired = true)] public string[] ChannelNames { get; set; }
        [DataMember(Order = 4, IsRequired = true)] public long QpcFrequency { get; set; }
        [DataMember(Order = 5, IsRequired = true)] public long TotalSamples { get; set; }
        [DataMember(Order = 6, IsRequired = true)] public long CommittedBytes { get; set; }
        [DataMember(Order = 7, IsRequired = true)] public long FirstSourceQpc { get; set; }
        [DataMember(Order = 8, IsRequired = true)] public long LastSourceQpc { get; set; }
        [DataMember(Order = 9, IsRequired = true)] public HeogMarkupSettings HeogSettings { get; set; }
        [DataMember(Order = 10, IsRequired = true)] public List<HeogMarkupInterval> Intervals { get; set; }
        [DataMember(Order = 11, IsRequired = true)] public List<long> MovementSamples { get; set; }
    }

    public static class RecordingAnalysisStore
    {
        public static string GetPath(string recordingPath)
        {
            return Path.ChangeExtension(recordingPath, ".analysis.json");
        }

        public static RecordingAnalysis Load(string recordingPath, SessionReader reader)
        {
            string path = GetPath(recordingPath);
            if (!File.Exists(path)) return null;
            RecordingAnalysis analysis;
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                analysis = (RecordingAnalysis)CreateSerializer().ReadObject(stream);
            Validate(analysis, reader);
            return analysis;
        }

        public static void Save(string recordingPath, SessionReader reader, HeogMarkupSettings settings,
            List<HeogMarkupInterval> intervals, List<long> movementSamples)
        {
            if (reader == null || reader.BlockCount == 0) throw new InvalidOperationException("No recording is open.");
            var analysis = new RecordingAnalysis
            {
                Version = 1,
                SampleRate = reader.Header.SampleRate,
                ChannelNames = reader.ChannelNames,
                QpcFrequency = reader.Header.QpcFrequency,
                TotalSamples = reader.Header.TotalSamplesPerChannel,
                CommittedBytes = reader.Header.CommittedBytes,
                FirstSourceQpc = reader.GetFirstSourceQpc(),
                LastSourceQpc = reader.GetLastSourceQpc(),
                HeogSettings = settings,
                Intervals = intervals,
                MovementSamples = movementSamples
            };
            Validate(analysis, reader);
            string path = GetPath(recordingPath);
            string temporary = path + ".tmp." + Guid.NewGuid().ToString("N");
            try
            {
                using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    CreateSerializer().WriteObject(stream, analysis);
                    stream.Flush(true);
                }
                // The previous save remains intact until a complete replacement is ready.
                if (File.Exists(path)) File.Replace(temporary, path, null);
                else File.Move(temporary, path);
            }
            finally
            {
                if (File.Exists(temporary)) File.Delete(temporary);
            }
        }

        private static DataContractJsonSerializer CreateSerializer()
        {
            return new DataContractJsonSerializer(typeof(RecordingAnalysis),
                new DataContractJsonSerializerSettings { MaxItemsInObjectGraph = int.MaxValue });
        }

        private static void Validate(RecordingAnalysis analysis, SessionReader reader)
        {
            if (analysis == null || analysis.Version != 1)
                throw new InvalidDataException("Unsupported recording analysis format.");
            if (reader == null || reader.BlockCount == 0 || analysis.SampleRate != reader.Header.SampleRate ||
                analysis.QpcFrequency != reader.Header.QpcFrequency ||
                analysis.TotalSamples != reader.Header.TotalSamplesPerChannel ||
                analysis.CommittedBytes != reader.Header.CommittedBytes ||
                analysis.FirstSourceQpc != reader.GetFirstSourceQpc() || analysis.LastSourceQpc != reader.GetLastSourceQpc() ||
                analysis.ChannelNames == null || analysis.ChannelNames.Length != reader.ChannelNames.Length)
                throw new InvalidDataException("Analysis does not match the opened EEG recording.");
            for (int i = 0; i < analysis.ChannelNames.Length; i++)
                if (!string.Equals(analysis.ChannelNames[i], reader.ChannelNames[i], StringComparison.Ordinal))
                    throw new InvalidDataException("Analysis channel names do not match the recording.");
            HeogMarkupSettings settings = analysis.HeogSettings;
            if (settings == null || double.IsNaN(settings.RangeThresholdMicrovolts) ||
                double.IsInfinity(settings.RangeThresholdMicrovolts) || settings.RangeThresholdMicrovolts < 0.1 ||
                settings.RangeThresholdMicrovolts > 10000 || settings.DetectionWindowMilliseconds < 1 ||
                settings.DetectionWindowMilliseconds > 10000 || settings.MinimumStableDurationMilliseconds < 1 ||
                settings.MinimumStableDurationMilliseconds > 60000 || analysis.MovementSamples == null)
                throw new InvalidDataException("Invalid analysis settings.");
            long firstSample = reader.GetBlockIndexRecord(0).FirstSampleIndex;
            SessionBlockRecord lastBlock = reader.GetBlockIndexRecord(reader.BlockCount - 1);
            long endSample = lastBlock.FirstSampleIndex + lastBlock.SampleCount;
            long previousEnd = firstSample;
            if (analysis.Intervals != null)
                foreach (HeogMarkupInterval interval in analysis.Intervals)
                {
                    if (interval == null || interval.StartSampleIndex < previousEnd ||
                        interval.EndSampleIndex <= interval.StartSampleIndex || interval.EndSampleIndex > endSample ||
                        interval.EndQpc <= interval.StartQpc || interval.TargetLabel < -1 ||
                        interval.TargetLabel > HeogMarkup.MaximumTargetLabel)
                        throw new InvalidDataException("Invalid analysis interval.");
                    previousEnd = interval.EndSampleIndex;
                }
            int intervalIndex = 0;
            long previousMarker = firstSample - 1;
            foreach (long marker in analysis.MovementSamples)
            {
                if (marker <= previousMarker || marker >= endSample || analysis.Intervals == null)
                    throw new InvalidDataException("Invalid HEOG marker position.");
                while (intervalIndex < analysis.Intervals.Count && analysis.Intervals[intervalIndex].EndSampleIndex <= marker)
                    intervalIndex++;
                if (intervalIndex == analysis.Intervals.Count || analysis.Intervals[intervalIndex].IsStable ||
                    marker < analysis.Intervals[intervalIndex].StartSampleIndex)
                    throw new InvalidDataException("HEOG marker is outside its movement interval.");
                previousMarker = marker;
            }
        }
    }
}
