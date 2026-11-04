using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace PolyHubNet
{
    // End coordinates are exclusive. Samples are the source of truth; QPC is metadata.
    [DataContract]
    public sealed class HeogMarkupInterval
    {
        [DataMember(Order = 1, IsRequired = true)]
        public long StartQpc { get; private set; }
        [DataMember(Order = 2, IsRequired = true)]
        public long EndQpc { get; private set; }
        [DataMember(Order = 3, IsRequired = true)]
        public bool IsStable { get; private set; }
        // Annotation: existing note IDs, 9-14 background squares, 0 unassigned.
        // Negative labels precede the selected start and are not displayed.
        [DataMember(Order = 4, IsRequired = true)]
        public int TargetLabel { get; set; }
        [DataMember(Order = 5, IsRequired = true)]
        public long StartSampleIndex { get; private set; }
        [DataMember(Order = 6, IsRequired = true)]
        public long EndSampleIndex { get; private set; }

        public HeogMarkupInterval(long startQpc, long endQpc, bool isStable)
            : this(startQpc, endQpc, isStable, -1, -1) { }

        internal HeogMarkupInterval(long startQpc, long endQpc, bool isStable, long firstSample, long endSample)
        {
            StartQpc = startQpc;
            EndQpc = endQpc;
            IsStable = isStable;
            StartSampleIndex = firstSample;
            EndSampleIndex = endSample;
        }
    }

    public static class HeogMarkup
    {
        private static readonly int[] NoteLabels = { 41, 43, 45, 47, 48, 50, 52, 53 };
        public const int MaximumTargetLabel = 53;

        public static bool IsNoteLabel(int value)
        {
            return Array.IndexOf(NoteLabels, value) >= 0;
        }

        // Optional movementSamples contains source-file sample indices, not QPC ticks.
        public static List<HeogMarkupInterval> AnalyzeFile(string path, HeogMarkupSettings settings,
            IProgress<int> progress, List<long> movementSamples = null)
        {
            ValidateSettings(settings);
            using (var reader = new SessionReader())
            {
                reader.Open(path);
                int channel = EogRejection.FindChannel(reader.ChannelNames, "HEOG");
                if (channel < 0) throw new InvalidOperationException("Recording has no HEOG channel.");
                if (reader.BlockCount == 0) throw new InvalidOperationException("Recording has no signal blocks.");
                if (reader.Header.QpcFrequency <= 0 || reader.Header.SampleRate <= 0)
                    throw new InvalidOperationException("Invalid recording timing.");
                var segments = new List<Segment>();
                Segment current = null;
                var block = new SignalBlock();
                var timer = Stopwatch.StartNew();
                Action<int> report = percent =>
                {
                    if (progress != null && (timer.ElapsedMilliseconds >= 100 || percent == 100))
                    { progress.Report(percent); timer.Restart(); }
                };
                SessionBlockRecord previous = default(SessionBlockRecord);
                for (int index = 0; index < reader.BlockCount; index++)
                {
                    SessionBlockRecord record = reader.GetBlockIndexRecord(index);
                    // Delivery-clock jitter does not break consecutive signal samples.
                    if (index > 0 && (record.Sequence != previous.Sequence + 1 ||
                        record.FirstSampleIndex != previous.FirstSampleIndex + previous.SampleCount))
                        current = null;
                    reader.ReadBlockInto(index, block);
                    int offset = channel * block.SampleCount;
                    for (int sample = 0; sample < block.SampleCount; sample++)
                    {
                        float value = block.Data[offset + sample];
                        if (float.IsNaN(value) || float.IsInfinity(value)) { current = null; continue; }
                        if (current == null)
                        {
                            current = new Segment { FirstSample = record.FirstSampleIndex + sample };
                            segments.Add(current);
                        }
                        current.Values.Add(value);
                    }
                    previous = record;
                    report((int)((index + 1L) * 40 / reader.BlockCount));
                }
                var intervals = new List<HeogMarkupInterval>();
                var movements = new List<long>();
                foreach (Segment segment in segments)
                {
                    int length = segment.Values.Count;
                    List<Transition> transitions = DetectTransitions(segment.Values.ToArray(), reader.Header.SampleRate,
                        settings, position => report(40 + (int)((segment.FirstSample + position) * 59 /
                            Math.Max(1L, reader.Header.TotalSamplesPerChannel))));
                    int cursor = 0;
                    int minimumStable = Math.Max(1, (int)Math.Round(reader.Header.SampleRate *
                        settings.MinimumStableDurationMilliseconds / 1000.0));
                    foreach (Transition transition in transitions)
                    {
                        AppendInterval(reader, intervals, segment.FirstSample + cursor,
                            segment.FirstSample + transition.First, transition.First - cursor >= minimumStable);
                        AppendInterval(reader, intervals, segment.FirstSample + Math.Max(cursor, transition.First),
                            segment.FirstSample + transition.End, false);
                        cursor = Math.Max(cursor, transition.End);
                        movements.Add(segment.FirstSample + transition.Peak);
                    }
                    AppendInterval(reader, intervals, segment.FirstSample + cursor,
                        segment.FirstSample + length, length - cursor >= minimumStable);
                }
                if (movementSamples != null)
                { movementSamples.Clear(); movementSamples.AddRange(movements); }
                AssignDefaultLabels(intervals, FindFirstNoteFixation(reader, intervals));
                report(100);
                return intervals;
            }
        }

        // The first note-on event chooses a fixation, not its ground-truth note.
        // The known left-to-right task starts at 41 regardless of the predicted ID.
        public static HeogMarkupInterval FindFirstNoteFixation(SessionReader reader,
            IList<HeogMarkupInterval> intervals)
        {
            for (int index = 0; index < reader.EventCount; index++)
            {
                SessionUserEvent evt = reader.GetEventRecord(index);
                if (evt.Code != (int)PolyHubIpc.ServiceEventCode.UserEvent ||
                    evt.State != 1 || !IsNoteLabel(evt.Value0)) continue;
                long sample = reader.FindSampleIndexByQpc(evt.QpcTimestamp);
                foreach (HeogMarkupInterval interval in intervals)
                    if (interval.IsStable && sample >= interval.StartSampleIndex && sample < interval.EndSampleIndex)
                        return interval;
                // Do not silently move the anchor to a later successful prediction.
                return null;
            }
            return null;
        }

        // One label per fixation; no repeated endpoints. Null start leaves the draft hidden.
        public static void AssignDefaultLabels(IList<HeogMarkupInterval> intervals, HeogMarkupInterval firstFixation)
        {
            int index = 0;
            bool started = false;
            foreach (HeogMarkupInterval interval in intervals)
            {
                if (ReferenceEquals(interval, firstFixation)) started = true;
                if (!started || !interval.IsStable) { interval.TargetLabel = -1; continue; }
                int phase = index++ % (2 * NoteLabels.Length - 2);
                interval.TargetLabel = NoteLabels[phase < NoteLabels.Length ? phase : 2 * NoteLabels.Length - 2 - phase];
            }
        }

        // A manually removed boundary joins its two sides, retaining only the left label
        // (or the right label if the left side has no label). Other boundaries stay intact.
        public static bool RemoveMovement(SessionReader reader, List<HeogMarkupInterval> intervals,
            List<long> movementSamples, long sample)
        {
            if (reader == null || intervals == null || movementSamples == null) return false;
            int markerIndex = movementSamples.IndexOf(sample);
            if (markerIndex < 0) return false;
            int middle = intervals.FindIndex(range => !range.IsStable &&
                range.StartSampleIndex <= sample && sample < range.EndSampleIndex);
            if (middle < 0) return false;
            long previous = markerIndex > 0 ? movementSamples[markerIndex - 1] : long.MinValue;
            long next = markerIndex + 1 < movementSamples.Count ? movementSamples[markerIndex + 1] : long.MaxValue;
            int first = middle, last = middle;
            while (first > 0 && intervals[first - 1].StartSampleIndex > previous &&
                CanJoin(reader, intervals[first - 1], intervals[first])) first--;
            while (last + 1 < intervals.Count && intervals[last + 1].EndSampleIndex <= next &&
                CanJoin(reader, intervals[last], intervals[last + 1])) last++;
            int label = -1;
            for (int i = first; i <= last; i++)
                if (intervals[i].IsStable && intervals[i].TargetLabel >= 0)
                { label = intervals[i].TargetLabel; break; }
            var merged = new HeogMarkupInterval(intervals[first].StartQpc, intervals[last].EndQpc, true,
                intervals[first].StartSampleIndex, intervals[last].EndSampleIndex) { TargetLabel = label };
            intervals.RemoveRange(first, last - first + 1);
            intervals.Insert(first, merged);
            movementSamples.RemoveAt(markerIndex);
            return true;
        }

        private static bool CanJoin(SessionReader reader, HeogMarkupInterval left, HeogMarkupInterval right)
        {
            long sample = right.StartSampleIndex;
            if (sample < 0 || left.EndSampleIndex != sample || reader.BlockCount == 0) return false;
            int low = 0, high = reader.BlockCount - 1;
            while (low < high)
            {
                int mid = low + (high - low + 1) / 2;
                if (reader.GetBlockIndexRecord(mid).FirstSampleIndex <= sample) low = mid;
                else high = mid - 1;
            }
            SessionBlockRecord block = reader.GetBlockIndexRecord(low);
            if (sample > block.FirstSampleIndex)
                return sample < block.FirstSampleIndex + block.SampleCount;
            if (sample != block.FirstSampleIndex || low == 0) return false;
            SessionBlockRecord previous = reader.GetBlockIndexRecord(low - 1);
            return block.Sequence == previous.Sequence + 1 &&
                previous.FirstSampleIndex + previous.SampleCount == sample;
        }

        // Preserve the processor's window grid: first full window, then StepSamples.
        // Fixation boundaries never restart the grid. EndQpc remains exclusive.
        public static List<HeogMarkupInterval> GetAnalysisWindows(SessionReader reader,
            IList<HeogMarkupInterval> fixations, int windowSamples, int stepSamples, int sampleRate)
        {
            var windows = new List<HeogMarkupInterval>();
            if (reader == null || fixations == null || fixations.Count == 0) return windows;
            if (sampleRate != reader.Header.SampleRate || windowSamples <= 0 ||
                stepSamples <= 0 || stepSamples > windowSamples || reader.Header.QpcFrequency <= 0)
                throw new ArgumentException("Model window parameters do not match the recording.");
            int buffered = 0, sinceAnalysis = 0, fixationIndex = 0;
            bool analyzed = false;
            long previousSequence = 0;
            double ticksPerSample = (double)reader.Header.QpcFrequency / sampleRate;
            for (int index = 0; index < reader.BlockCount; index++)
            {
                SessionBlockRecord block = reader.GetBlockIndexRecord(index);
                if (index > 0 && block.Sequence != previousSequence + 1)
                { buffered = sinceAnalysis = 0; analyzed = false; }
                previousSequence = block.Sequence;
                for (int sample = 0; sample < block.SampleCount; sample++)
                {
                    if (buffered < windowSamples) buffered++;
                    if (buffered < windowSamples) continue;
                    if (analyzed)
                    {
                        if (++sinceAnalysis < stepSamples) continue;
                    }
                    analyzed = true;
                    sinceAnalysis = 0;
                    long endSample = block.QpcTimestamp + (long)Math.Round(sample * ticksPerSample);
                    long start = endSample - (long)Math.Round((windowSamples - 1.0) * ticksPerSample);
                    long end = block.QpcTimestamp + (long)Math.Round((sample + 1.0) * ticksPerSample);
                    long firstSample = block.FirstSampleIndex + sample - windowSamples + 1;
                    long endSampleIndex = block.FirstSampleIndex + sample + 1;
                    while (fixationIndex < fixations.Count && (fixations[fixationIndex].StartSampleIndex >= 0
                        ? fixations[fixationIndex].EndSampleIndex <= firstSample
                        : fixations[fixationIndex].EndQpc <= start)) fixationIndex++;
                    if (fixationIndex == fixations.Count) return windows;
                    HeogMarkupInterval fixation = fixations[fixationIndex];
                    bool inside = fixation.StartSampleIndex >= 0
                        ? firstSample >= fixation.StartSampleIndex && endSampleIndex <= fixation.EndSampleIndex
                        : start >= fixation.StartQpc && end <= fixation.EndQpc;
                    if (fixation.IsStable && inside)
                        windows.Add(new HeogMarkupInterval(start, end, true, firstSample, endSampleIndex));
                }
            }
            return windows;
        }

        private sealed class Segment
        {
            internal long FirstSample;
            internal readonly List<float> Values = new List<float>();
        }

        internal sealed class Transition
        {
            internal int First, End, Peak;
        }

        // Defaults: 10 ms median, change over 50 ms, 400 ms median of that
        // change as the local trend. A linear drift cancels; a short step remains.
        internal static List<Transition> DetectTransitions(float[] values, int sampleRate,
            HeogMarkupSettings settings, Action<int> progress = null)
        {
            ValidateSettings(settings);
            if (sampleRate <= 0) throw new ArgumentOutOfRangeException("sampleRate");
            var result = new List<Transition>();
            int span = Math.Max(2, (int)Math.Round(sampleRate * settings.DetectionWindowMilliseconds / 1000.0));
            int lag = Math.Max(2, span / 2);
            if (values.Length <= lag) return result;
            int smoothRadius = Math.Max(1, span / 20);
            float[] smoothed = MedianFilter(values, smoothRadius, progress != null ? (Action<int>)(i => progress(i / 3)) : null);
            int left = lag / 2, right = lag - left;
            var change = new float[values.Length];
            for (int i = left; i < values.Length - right; i++)
                change[i] = smoothed[i + right] - smoothed[i - left];
            for (int i = 0; i < left; i++) change[i] = change[left];
            for (int i = values.Length - right; i < values.Length; i++)
                change[i] = change[values.Length - right - 1];
            float[] trend = MedianFilter(change, 2 * span, progress != null ? (Action<int>)(i => progress((values.Length + i) / 3)) : null);
            double threshold = settings.RangeThresholdMicrovolts * 1e-6;
            int quiet = Math.Max(1, (int)Math.Round(sampleRate * settings.MinimumStableDurationMilliseconds / 1000.0));
            int first = -1, last = -1, peakFirst = -1, peakLast = -1;
            double strongest = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (progress != null && (i & 1023) == 0) progress((2 * values.Length + i) / 3);
                double score = (double)change[i] - trend[i];
                double strength = Math.Abs(score);
                if (strength > threshold && Math.Abs(change[i]) > threshold && score * change[i] > 0)
                {
                    if (first >= 0 && i - last > quiet)
                    {
                        AddTransition(result, first, last, peakFirst, peakLast, left, right, smoothRadius, values.Length);
                        first = -1;
                    }
                    if (first < 0) { first = i; strongest = 0; }
                    last = i;
                    if (strength > strongest)
                    { strongest = strength; peakFirst = peakLast = i; }
                    else if (strength == strongest) peakLast = i;
                }
            }
            if (first >= 0) AddTransition(result, first, last, peakFirst, peakLast, left, right, smoothRadius, values.Length);
            if (progress != null) progress(values.Length);
            return result;
        }

        private static void AddTransition(List<Transition> result, int first, int last, int peakFirst, int peakLast,
            int left, int right, int smoothRadius, int length)
        {
            result.Add(new Transition
            {
                First = Math.Max(0, first - left - smoothRadius),
                End = Math.Min(length, last + right + smoothRadius + 1),
                Peak = (peakFirst + peakLast) / 2
            });
        }

        private static float[] MedianFilter(float[] values, int radius, Action<int> progress)
        {
            var result = new float[values.Length];
            var sorted = new List<float>(2 * radius + 1);
            for (int i = -radius; i <= radius; i++)
                sorted.Add(values[Math.Max(0, Math.Min(values.Length - 1, i))]);
            sorted.Sort();
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = sorted[radius];
                if (i + 1 < values.Length)
                {
                    sorted.RemoveAt(sorted.BinarySearch(values[Math.Max(0, i - radius)]));
                    float next = values[Math.Min(values.Length - 1, i + radius + 1)];
                    int position = sorted.BinarySearch(next);
                    sorted.Insert(position < 0 ? ~position : position, next);
                }
                if (progress != null && (i & 1023) == 0) progress(i);
            }
            return result;
        }

        private static void AppendInterval(SessionReader reader, List<HeogMarkupInterval> intervals,
            long first, long end, bool stable)
        {
            if (end <= first) return;
            intervals.Add(new HeogMarkupInterval(SampleQpc(reader, first),
                SampleQpc(reader, end - 1) + (long)Math.Round((double)reader.Header.QpcFrequency / reader.Header.SampleRate),
                stable, first, end));
        }

        private static long SampleQpc(SessionReader reader, long sample)
        {
            int low = 0, high = reader.BlockCount - 1;
            while (low < high)
            {
                int mid = low + (high - low + 1) / 2;
                if (reader.GetBlockIndexRecord(mid).FirstSampleIndex <= sample) low = mid;
                else high = mid - 1;
            }
            SessionBlockRecord block = reader.GetBlockIndexRecord(low);
            return block.QpcTimestamp + (long)Math.Round((sample - block.FirstSampleIndex) *
                (double)reader.Header.QpcFrequency / reader.Header.SampleRate);
        }

        private static void ValidateSettings(HeogMarkupSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (double.IsNaN(settings.RangeThresholdMicrovolts) ||
                double.IsInfinity(settings.RangeThresholdMicrovolts) || settings.RangeThresholdMicrovolts <= 0 ||
                settings.DetectionWindowMilliseconds < 1 || settings.DetectionWindowMilliseconds > 10000 ||
                settings.MinimumStableDurationMilliseconds < 1 || settings.MinimumStableDurationMilliseconds > 60000)
                throw new ArgumentException("Invalid HEOG markup settings.");
        }
    }
}
