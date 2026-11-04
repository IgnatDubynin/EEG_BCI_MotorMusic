using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PolyHubNet
{
    public sealed class SsvepPerformanceDecision
    {
        public SsvepAnalysisResult Result;
        public double Seconds;
        public int? TargetLabel;
        public int Target = -1, Prediction, AttemptIndex = -1;
        public bool NewSelection, Released;
        public int HeldNote, NoNoteCount;
    }

    public sealed class SsvepPerformanceAttempt
    {
        public int Label, Target, Windows, FirstSelection, FirstCorrect;
        public long StartSample, EndSample;
        public double StartSeconds, EndSeconds, FirstSelectionSeconds = double.NaN, FirstCorrectSeconds = double.NaN;
        public double Duration { get { return Math.Max(0, EndSeconds - StartSeconds); } }
        public double SelectionTime { get { return double.IsNaN(FirstSelectionSeconds) ? Duration : FirstSelectionSeconds; } }
    }

    public sealed class SsvepRoc
    {
        public int Positives, Negatives;
        public double Auc = double.NaN;
        // Same finite-score population as ROC; live acceptance is strictly greater than zero.
        public Tuple<double, double> CurrentPoint;
        public readonly List<Tuple<double, double>> Points = new List<Tuple<double, double>>();
        public static SsvepRoc Compute(double[] scores, bool[] positive)
        {
            if (scores.Length != positive.Length) throw new ArgumentException("ROC input length mismatch.");
            var roc = new SsvepRoc();
            int[] order = Enumerable.Range(0, scores.Length).Where(i => SsvepPerformance.Finite(scores[i]))
                .OrderByDescending(i => scores[i]).ToArray();
            roc.Positives = order.Count(i => positive[i]); roc.Negatives = order.Length - roc.Positives;
            if (roc.Positives == 0 || roc.Negatives == 0) return roc;
            roc.CurrentPoint = Tuple.Create(
                (double)order.Count(i => !positive[i] && scores[i] > 0.0) / roc.Negatives,
                (double)order.Count(i => positive[i] && scores[i] > 0.0) / roc.Positives);
            roc.Points.Add(Tuple.Create(0.0, 0.0));
            int tp = 0, fp = 0;
            for (int first = 0; first < order.Length;)
            {
                int end = first;
                while (end < order.Length && scores[order[end]] == scores[order[first]])
                { if (positive[order[end]]) tp++; else fp++; end++; }
                roc.Points.Add(Tuple.Create((double)fp / roc.Negatives, (double)tp / roc.Positives));
                first = end;
            }
            roc.Auc = 0;
            for (int i = 1; i < roc.Points.Count; i++)
                roc.Auc += (roc.Points[i].Item1 - roc.Points[i - 1].Item1) *
                    (roc.Points[i].Item2 + roc.Points[i - 1].Item2) / 2;
            return roc;
        }
    }

    public sealed class SsvepPerformance
    {
        public const int ReleaseConfirmationCount = 3;
        private int _noNoteCount;
        public static readonly int[] Notes = { 41, 43, 45, 47, 48, 50, 52, 53 };
        private static readonly CultureInfo C = CultureInfo.InvariantCulture;
        public SsvepModel Model;
        public string SourcePath, ModelPath, LogError;
        public long SourceQpcFrequency, SourceFirstQpc, SourceTotalSamples, SourceCommittedBytes;
        public int SourceSampleRate;
        public string[] SourceChannels;
        public HeogMarkupSettings HeogSettings;
        public EogRejectionProfileSettings EogSettings;
        public List<HeogMarkupInterval> Markup;
        public readonly List<SsvepPerformanceDecision> Decisions = new List<SsvepPerformanceDecision>();
        public readonly List<SsvepPerformanceAttempt> Attempts = new List<SsvepPerformanceAttempt>();
        public readonly List<long> GapSamples = new List<long>();
        public int[,] Confusion = new int[9, 9];
        public SsvepRoc[] NotesRoc = new SsvepRoc[8], BackgroundRoc = new SsvepRoc[8];
        public int ScoredDecisions;
        public double EstimatedItr = double.NaN, FirstSelectionAccuracy = double.NaN, MeanSelectionSeconds = double.NaN;
        public double MacroAuc { get { return NotesRoc.All(r => r != null && Finite(r.Auc)) ? NotesRoc.Average(r => r.Auc) : double.NaN; } }
        public int AnnotatedDecisions { get { return Decisions.Count(d => d.Target >= 0); } }
        public int CorrectDecisions { get { return Decisions.Count(d => d.Target >= 0 && d.Target == d.Prediction); } }
        public static bool Finite(double n) { return !double.IsNaN(n) && !double.IsInfinity(n); }
        public static int Note(int index) { return index > 0 ? Notes[index - 1] : 0; }
        public static int TargetIndex(int label)
        {
            if (label >= 9 && label <= 14) return 0;
            int i = Array.IndexOf(Notes, label); return i >= 0 ? i + 1 : -1;
        }
        public static string Value(double n, string format = "0.000") { return Finite(n) ? n.ToString(format, C) : "—"; }
        public static string Json<T>(T value)
        {
            using (var stream = new MemoryStream())
            {
                new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
                    { MaxItemsInObjectGraph = int.MaxValue }).WriteObject(stream, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public static T Snapshot<T>(T value)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Json(value))))
                return (T)new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
                    { MaxItemsInObjectGraph = int.MaxValue }).ReadObject(stream);
        }

        public static SsvepPerformance Run(string path, SsvepModel model, string modelPath,
            List<HeogMarkupInterval> markup, HeogMarkupSettings heog, EogRejectionProfileSettings eog,
            IProgress<int> progress)
        {
            model.Validate();
            if (model.ClassCount != 8 || model.ClassIds.Distinct().Count() != 8 || model.ClassIds.Any(id => id < 1201 || id > 1208))
                throw new InvalidOperationException("Performance requires note classes 1201..1208.");
            var run = new SsvepPerformance { SourcePath = path, Model = model, ModelPath = modelPath,
                Markup = markup, HeogSettings = heog, EogSettings = eog };
            using (var reader = new SessionReader())
            {
                reader.Open(path);
                if (reader.BlockCount == 0) throw new InvalidOperationException("The recording is empty.");
                if (reader.Header.SampleRate != model.SampleRate)
                    throw new InvalidOperationException("Model sample rate does not match recording.");
                if (model.ChannelNames.Any(n => EogRejection.FindChannel(reader.ChannelNames, n) < 0))
                    throw new InvalidOperationException("A model EEG channel is missing from the recording.");
                var rejection = new EogRejection(eog); rejection.ValidateChannels(reader.ChannelNames);
                run.SourceQpcFrequency = reader.Header.QpcFrequency; run.SourceFirstQpc = reader.GetFirstSourceQpc();
                run.SourceSampleRate = reader.Header.SampleRate; run.SourceTotalSamples = reader.Header.TotalSamplesPerChannel;
                run.SourceCommittedBytes = reader.Header.CommittedBytes; run.SourceChannels = reader.ChannelNames;
                run.CreateAttempts(reader);
                double frequency = reader.Header.QpcFrequency;
                if (frequency <= 0) throw new InvalidDataException("Invalid source timestamp frequency.");
                long expectedSample = 0, previousSequence = 0;
                using (var processor = new SsvepProcessor("Offline SSVEP", 2, reader.Header.QpcFrequency))
                {
                    var source = new SignalStream("Offline source"); source.SetChannelNames(reader.ChannelNames);
                    processor.SetModel(model); processor.SetEogRejection(rejection); processor.AttachInput(source); processor.Reset();
                    int attemptIndex = 0, heldNote = 0;
                    processor.AnalysisReady += result => run.RecordDecision(result, ref attemptIndex, ref heldNote);
                    int reported = -1;
                    for (int index = 0; index < reader.BlockCount; index++)
                    {
                        var record = reader.GetBlockIndexRecord(index);
                        if (record.FirstSampleIndex != expectedSample)
                            throw new InvalidDataException("Non-contiguous recording sample indices.");
                        if (index > 0 && record.Sequence != previousSequence + 1) run.GapSamples.Add(record.FirstSampleIndex);
                        previousSequence = record.Sequence; expectedSample += record.SampleCount;
                        processor.AnalyzeRecordedBlock(reader.ReadBlock(index));
                        int percent = (int)(90L * (index + 1) / reader.BlockCount);
                        if (percent != reported) { reported = percent; if (progress != null) progress.Report(percent); }
                    }
                }
            }
            run.CalculateMetrics();
            if (progress != null) progress.Report(100);
            return run;
        }

        internal void RecordDecision(SsvepAnalysisResult result, ref int attemptIndex, ref int heldNote)
        {
            long lastSample = result.WindowEndSampleIndexExclusive - 1;
            while (attemptIndex < Attempts.Count && lastSample >= Attempts[attemptIndex].EndSample) attemptIndex++;
            var decision = new SsvepPerformanceDecision { Result = result,
                Seconds = (double)(result.QpcTimestamp - SourceFirstQpc) / SourceQpcFrequency,
                Prediction = result.PredictedClassId.HasValue ? result.PredictedClassId.Value - 1200 : 0 };
            if (decision.Prediction > 0)
            {
                _noNoteCount = 0;
                decision.NewSelection = decision.Prediction != heldNote;
                heldNote = decision.Prediction;
            }
            else if (!result.IsRejected && result.CandidateClassId.HasValue)
            {
                if (_noNoteCount < ReleaseConfirmationCount) _noNoteCount++;
                if (_noNoteCount == ReleaseConfirmationCount)
                { decision.Released = heldNote > 0; heldNote = 0; }
            }
            decision.HeldNote = heldNote;
            decision.NoNoteCount = _noNoteCount;
            if (attemptIndex < Attempts.Count && lastSample >= Attempts[attemptIndex].StartSample)
            {
                var attempt = Attempts[attemptIndex];
                decision.AttemptIndex = attemptIndex; decision.TargetLabel = attempt.Label; decision.Target = attempt.Target;
                attempt.Windows++;
                if (decision.NewSelection)
                {
                    double latency = Math.Max(0, decision.Seconds - attempt.StartSeconds);
                    if (attempt.FirstSelection == 0)
                    { attempt.FirstSelection = Note(decision.Prediction); attempt.FirstSelectionSeconds = latency; }
                    if (attempt.Target > 0 && decision.Prediction == attempt.Target && attempt.FirstCorrect == 0)
                    { attempt.FirstCorrect = Note(decision.Prediction); attempt.FirstCorrectSeconds = latency; }
                }
            }
            Decisions.Add(decision);
        }

        private void CreateAttempts(SessionReader reader)
        {
            long end = 0;
            foreach (var interval in Markup ?? new List<HeogMarkupInterval>())
            {
                if (interval.StartSampleIndex < end || interval.EndSampleIndex <= interval.StartSampleIndex ||
                    interval.EndSampleIndex > reader.Header.TotalSamplesPerChannel)
                    throw new InvalidDataException("Invalid markup sample boundaries. Re-mark HEOG.");
                end = interval.EndSampleIndex;
                if (!interval.IsStable || interval.TargetLabel <= 0) continue;
                int target = TargetIndex(interval.TargetLabel);
                if (target < 0) throw new InvalidDataException("Unknown target label: " + interval.TargetLabel);
                Attempts.Add(new SsvepPerformanceAttempt { Label = interval.TargetLabel, Target = target,
                    StartSample = interval.StartSampleIndex, EndSample = interval.EndSampleIndex,
                    StartSeconds = (double)(interval.StartQpc - reader.GetFirstSourceQpc()) / reader.Header.QpcFrequency,
                    EndSeconds = (double)(interval.EndQpc - reader.GetFirstSourceQpc()) / reader.Header.QpcFrequency });
            }
            if (Attempts.Count == 0) throw new InvalidOperationException("Mark HEOG and assign target labels before analysis.");
        }

        internal void CalculateMetrics()
        {
            Confusion = new int[9, 9];
            foreach (var d in Decisions.Where(d => d.Target >= 0)) Confusion[d.Target, d.Prediction]++;
            var scored = Decisions.Where(d => d.Target >= 0 && d.Result.ClassAcceptanceMargins.Length == 8 &&
                d.Result.ClassAcceptanceMargins.All(Finite)).ToArray();
            ScoredDecisions = scored.Length;
            for (int note = 1; note <= 8; note++)
            {
                int column = Array.IndexOf(Model.ClassIds, note + 1200);
                var notes = scored.Where(d => d.Target > 0).ToArray();
                var background = scored.Where(d => d.Target == 0 || d.Target == note).ToArray();
                NotesRoc[note - 1] = SsvepRoc.Compute(notes.Select(d => d.Result.ClassAcceptanceMargins[column]).ToArray(), notes.Select(d => d.Target == note).ToArray());
                BackgroundRoc[note - 1] = SsvepRoc.Compute(background.Select(d => d.Result.ClassAcceptanceMargins[column]).ToArray(), background.Select(d => d.Target == note).ToArray());
            }
            var attempts = Attempts.Where(a => a.Target > 0).ToArray();
            if (attempts.Length == 0) return;
            FirstSelectionAccuracy = (double)attempts.Count(a => a.FirstSelection == Note(a.Target)) / attempts.Length;
            MeanSelectionSeconds = attempts.Average(a => a.SelectionTime);
            EstimatedItr = EstimateItr(FirstSelectionAccuracy, MeanSelectionSeconds);
        }

        internal static double EstimateItr(double p, double seconds)
        {
            if (!Finite(p) || !Finite(seconds) || seconds <= 0 || p < 0 || p > 1) return double.NaN;
            if (p <= 1.0 / 8) return 0;
            double bits = 3 + p * Math.Log(p, 2) + (p == 1 ? 0 : (1 - p) * Math.Log((1 - p) / 7, 2));
            return 60 / seconds * bits;
        }

        public static string LogPath(string path) { return Path.ChangeExtension(path, ".analysis.csv"); }
        private static string Cell(string text) { return "\"" + (text ?? "").Replace("\"", "\"\"").Replace("\r", " ").Replace("\n", " ") + "\""; }
        private static string Number(double n) { return Finite(n) ? n.ToString("R", C) : ""; }
        public void SaveLog()
        {
            string path = LogPath(SourcePath), temporary = path + ".tmp." + Guid.NewGuid().ToString("N");
            try
            {
                using (var writer = new StreamWriter(temporary, false, new UTF8Encoding(true)))
                {
                    writer.WriteLine("# PolyHub performance CSV v2; UTF-8; comma-separated; # lines contain metadata and summaries");
                    writer.WriteLine("# Source," + Cell(Path.GetFileName(SourcePath)));
                    writer.WriteLine("# SourceFormat," + SourceSampleRate + "," + SourceTotalSamples + "," + SourceCommittedBytes + "," + SourceQpcFrequency + "," + SourceFirstQpc);
                    writer.WriteLine("# SourceChannelsJson," + Cell(Json(SourceChannels)));
                    writer.WriteLine("# CreatedUtc," + DateTime.UtcNow.ToString("o", C));
                    writer.WriteLine("# ModelPath," + Cell(ModelPath)); writer.WriteLine("# ModelJson," + Cell(Json(Model)));
                    writer.WriteLine("# HEOGSettingsJson," + Cell(Json(HeogSettings))); writer.WriteLine("# EOGSettingsJson," + Cell(Json(EogSettings)));
                    writer.WriteLine("# MarkupJson," + Cell(Json(Markup)));
                    writer.WriteLine("# ClassMap,1201=41,1202=43,1203=45,1204=47,1205=48,1206=50,1207=52,1208=53; squares 9..14=background; output 0=no command");
                    writer.WriteLine("# WindowRule,Continuous existing SsvepProcessor from recording start; original blocks/sequences; model windows; no fixation reset or padding; CSV rows contain selected decisions only");
                    writer.WriteLine("# TargetRule,Last EEG sample must be inside a labeled stable HEOG interval [start; end); gaps and unassigned intervals excluded from metrics and CSV; EEG window may start before the interval");
                    writer.WriteLine("# SelectionRule,Release held key after " + ReleaseConfirmationCount + " consecutive classified no-command results; accepted note resets release count; rejected/unprocessed windows do not count; repeated held note is not a new selection; first selection and first correct selection are separate");
                    writer.WriteLine("# AUCMethod,Binary one-note versus seven others and separately versus background; actual finite signed margins; tied scores grouped; trapezoidal ROC area; no score imputation; overlapping windows are not independent trials");
                    writer.WriteLine("# ITRMethod,Estimated Wolpaw 8-target bits/min; first NEW selection per note attempt; omissions counted as errors with full attempt time; below-chance floored to 0; time starts at fixation and excludes gaze-shift and GUI/IPC latency; assumptions of uniform targets/errors and memoryless trials are approximate");
                    writer.WriteLine("# References,https://scikit-learn.org/stable/modules/generated/sklearn.metrics.roc_curve.html; https://pmc.ncbi.nlm.nih.gov/articles/PMC4185283/");
                    foreach (long sample in GapSamples) writer.WriteLine("# SourceGapSample," + sample);
                    writer.Write("StartSample,EndSampleExclusive,StartSeconds,EndSeconds,Attempt,TargetLabel,TargetNote,CandidateClassId,CandidateNote,PredictedClassId,PredictedNote,NewSelection,Correct,AcceptanceDistanceRatio,BackgroundLogOdds,NoCommandReason");
                    foreach (int id in Model.ClassIds) writer.Write(",Margin_" + Note(id - 1200));
                    for (int j = 0; j < Model.FeatureCount; j++) writer.Write(",Feature_" + (j + 1));
                    writer.WriteLine();
                    // Use the actual source timestamps, not elapsed CPU time or a reconstructed window grid.
                    long origin = SourceFirstQpc;
                    foreach (var d in Decisions.Where(d => d.Target >= 0))
                    {
                        var r = d.Result;
                        var row = new List<string> { r.WindowStartSampleIndex.ToString(), r.WindowEndSampleIndexExclusive.ToString(),
                            Number((r.WindowStartQpc - origin) / (double)SourceQpcFrequency), Number(d.Seconds),
                            d.AttemptIndex >= 0 ? (d.AttemptIndex + 1).ToString() : "", d.TargetLabel.HasValue ? d.TargetLabel.Value.ToString() : "",
                            d.Target >= 0 ? Note(d.Target).ToString() : "", r.CandidateClassId.HasValue ? r.CandidateClassId.Value.ToString() : "",
                            r.CandidateClassId.HasValue ? Note(r.CandidateClassId.Value - 1200).ToString() : "", r.PredictedClassId.HasValue ? r.PredictedClassId.Value.ToString() : "",
                            Note(d.Prediction).ToString(), d.NewSelection ? "1" : "0", d.Target >= 0 ? (d.Target == d.Prediction ? "1" : "0") : "",
                            Number(r.AcceptanceDistanceRatio), Number(r.BackgroundLogOdds), Cell(r.IsRejected ? r.EogRejection.Reason : r.HasPrediction ? "" : "Not accepted") };
                        for (int k = 0; k < 8; k++) row.Add(k < r.ClassAcceptanceMargins.Length ? Number(r.ClassAcceptanceMargins[k]) : "");
                        for (int k = 0; k < Model.FeatureCount; k++) row.Add(k < r.Features.Length ? Number(r.Features[k]) : "");
                        writer.WriteLine(string.Join(",", row));
                    }
                    writer.WriteLine("# ConfusionColumns,Target,NoCommand,41,43,45,47,48,50,52,53");
                    for (int target = 0; target <= 8; target++)
                    { writer.Write("# Confusion," + Note(target)); for (int j = 0; j <= 8; j++) writer.Write("," + Confusion[target, j]); writer.WriteLine(); }
                    writer.WriteLine("# AttemptColumns,Index,TargetLabel,StartSeconds,EndSeconds,Windows,FirstSelection,FirstSelectionSeconds,FirstCorrect,FirstCorrectSeconds");
                    for (int i = 0; i < Attempts.Count; i++)
                    { var a = Attempts[i]; writer.WriteLine("# Attempt," + (i + 1) + "," + a.Label + "," + Number(a.StartSeconds) + "," + Number(a.EndSeconds) + "," + a.Windows + "," + a.FirstSelection + "," + Number(a.FirstSelectionSeconds) + "," + a.FirstCorrect + "," + Number(a.FirstCorrectSeconds)); }
                    writer.WriteLine("# ROCColumns,Note,Comparison,PositiveWindows,NegativeWindows,AUC,FPR,TPR");
                    for (int i = 0; i < 8; i++) foreach (bool background in new[] { false, true })
                    {
                        var roc = background ? BackgroundRoc[i] : NotesRoc[i];
                        string prefix = "# ROC," + Notes[i] + "," + (background ? "Background" : "OtherNotes") + "," + roc.Positives + "," + roc.Negatives + "," + Number(roc.Auc);
                        if (roc.Points.Count == 0) writer.WriteLine(prefix + ",,");
                        foreach (var point in roc.Points) writer.WriteLine(prefix + "," + Number(point.Item1) + "," + Number(point.Item2));
                    }
                    writer.WriteLine("# Summary,Decisions," + Decisions.Count + ",Annotated," + AnnotatedDecisions + ",ScoredForAUC," + ScoredDecisions + ",CorrectDecisions," + CorrectDecisions + ",MacroAUCNotes," + Number(MacroAuc) + ",FirstSelectionAccuracy," + Number(FirstSelectionAccuracy) + ",MeanSelectionSeconds," + Number(MeanSelectionSeconds) + ",EstimatedITRBitsPerMinute," + Number(EstimatedItr));
                }
                if (File.Exists(path)) File.Replace(temporary, path, null); else File.Move(temporary, path);
            }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }

    }
}
