using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PolyHubNet
{
    // Separate GDI plot: never touches the EEG viewer's OpenGL context or render loop.
    public static class SsvepPerformancePlot
    {
        private static readonly Color[] Colors = { Color.RoyalBlue, Color.OrangeRed, Color.ForestGreen,
            Color.DarkViolet, Color.DarkCyan, Color.SaddleBrown, Color.DeepPink, Color.OliveDrab };
        // A standard Panel is declared by the WinForms Designer. This helper is used only at runtime.
        public static void Draw(Control surface, Graphics g, SsvepPerformance data, bool timeline, bool background, bool smooth, bool showAll = false)
        {
            int width = surface.ClientSize.Width, height = surface.ClientSize.Height;
            Font font = surface.Font;
            Rectangle client = surface.ClientRectangle;
            Color foreground = surface.ForeColor;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (data == null || width < 220 || height < 130)
            { TextRenderer.DrawText(g, "Analyze a labeled recording to display results.", font, client, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak); return; }
            var plot = PlotBounds(width, height, timeline);
            using (var grid = new Pen(Color.FromArgb(230, 233, 236)))
            using (var border = new Pen(Color.SlateGray))
            using (var text = new SolidBrush(foreground))
            {
                if (timeline && showAll) DrawUnlabeled(g, plot, data, font);
                for (int i = 0; i <= (timeline ? 8 : 5); i++)
                {
                    float y = plot.Bottom - i * plot.Height / (timeline ? 8 : 5);
                    g.DrawLine(grid, plot.Left, y, plot.Right, y);
                    string label = timeline ? (i == 0 ? "None/BG" : SsvepPerformance.Notes[i - 1].ToString()) : (i * 20).ToString() + "%";
                    g.DrawString(label, font, text, plot.Left - g.MeasureString(label, font).Width - 5, y - font.Height / 2f);
                }
                for (int i = 0; i <= 4; i++)
                {
                    float x = plot.Left + i * plot.Width / 4;
                    g.DrawLine(grid, x, plot.Top, x, plot.Bottom);
                    double start = TimelineStart(data, showAll), end = TimelineEnd(data, showAll);
                    string label = timeline ? Time(start + (end - start) * i / 4) : (i * 25).ToString() + "%";
                    g.DrawString(label, font, text, Math.Min(width - g.MeasureString(label, font).Width - 2, x - g.MeasureString(label, font).Width / 2), plot.Bottom + 5);
                }
                g.DrawRectangle(border, plot.X, plot.Y, plot.Width, plot.Height);
                if (timeline) DrawTimeline(g, plot, data, font, showAll);
                else DrawRoc(g, plot, data, background, smooth, font);
                string caption = timeline ? "Time from recording start (mm:ss.fff)" : "False positive rate (FPR)";
                g.DrawString(caption, font, text, plot.Left + (plot.Width - g.MeasureString(caption, font).Width) / 2, height - font.Height - 8);
            }
        }
        private static void DrawRoc(Graphics g, RectangleF plot, SsvepPerformance data, bool background, bool smooth, Font font)
        {
            using (var chance = new Pen(Color.Silver) { DashStyle = DashStyle.Dash }) g.DrawLine(chance, plot.Left, plot.Bottom, plot.Right, plot.Top);
            var curves = background ? data.BackgroundRoc : data.NotesRoc;
            bool any = false;
            for (int i = 0; i < 8; i++)
            {
                var roc = curves[i];
                using (var pen = new Pen(Colors[i], 2))
                using (var brush = new SolidBrush(Colors[i]))
                {
                    if (roc != null && roc.Points.Count > 1)
                    {
                        any = true;
                        var points = roc.Points.Select(p => new PointF(plot.Left + (float)p.Item1 * plot.Width, plot.Bottom - (float)p.Item2 * plot.Height)).ToArray();
                        var state = g.Save(); g.SetClip(plot);
                        if (smooth)
                        {
                            // Split at the real operating point so smoothing passes through it exactly.
                            int current = roc.CurrentPoint == null ? -1 : roc.Points.FindIndex(p =>
                                p.Item1 == roc.CurrentPoint.Item1 && p.Item2 == roc.CurrentPoint.Item2);
                            if (current > 0)
                                using (var path = SmoothPath(points.Take(current + 1).ToArray())) g.DrawPath(pen, path);
                            if (current < points.Length - 1)
                                using (var path = SmoothPath(current >= 0 ? points.Skip(current).ToArray() : points)) g.DrawPath(pen, path);
                        }
                        else g.DrawLines(pen, points);
                        g.Restore(state);
                    }
                    float y = plot.Top + i * (font.Height + 9);
                    g.DrawLine(pen, plot.Right + 12, y + 7, plot.Right + 30, y + 7);
                    g.DrawString(SsvepPerformance.Notes[i] + "  AUC " + SsvepPerformance.Value(roc != null ? roc.Auc : double.NaN), font, brush, plot.Right + 36, y);
                }
            }
            // Paint markers after every curve so subsequent lines cannot cover them.
            for (int i = 0; i < 8; i++)
            {
                var roc = curves[i];
                if (roc == null || roc.CurrentPoint == null) continue;
                float x = plot.Left + (float)roc.CurrentPoint.Item1 * plot.Width;
                float y = plot.Bottom - (float)roc.CurrentPoint.Item2 * plot.Height;
                using (var pen = new Pen(Colors[i], 2))
                {
                    g.FillEllipse(Brushes.White, x - 4, y - 4, 8, 8);
                    g.DrawEllipse(pen, x - 4, y - 4, 8, 8);
                }
            }
            if (any)
            {
                float y = plot.Top + 8 * (font.Height + 9) + 6;
                g.DrawEllipse(Pens.DimGray, plot.Right + 16, y + 3, 8, 8);
                g.DrawString("Current (θ = 0)", font, Brushes.DimGray, plot.Right + 30, y);
            }
            g.DrawString("TPR", font, Brushes.DimGray, plot.Left, 2);
            if (!any) TextRenderer.DrawText(g, background ? "No scored target/background pairs." : "No scored positive/negative pairs.", font, Rectangle.Round(plot), Color.DimGray, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }
        // Monotone cubic interpolation of distinct FPR levels, for display only.
        // Harmonic-mean slopes avoid spline overshoot; source points are never modified.
        internal static GraphicsPath SmoothPath(PointF[] points)
        {
            var path = new GraphicsPath();
            if (points.Length < 2) return path;
            var knots = new List<PointF>();
            foreach (var point in points)
            {
                if (knots.Count > 0 && point.X == knots[knots.Count - 1].X)
                    knots[knots.Count - 1] = point;
                else knots.Add(point);
            }
            // Remove repeated plateau levels before interpolation: otherwise every
            // horizontal step forces a zero tangent and the curve stays stair-shaped.
            // The endpoints (including the split at Current) remain exact.
            for (int i = knots.Count - 2; i > 0; i--)
                if (knots[i].Y == knots[i - 1].Y) knots.RemoveAt(i);
            path.AddLine(points[0], knots[0]);
            if (knots.Count < 2) return path;
            var slopes = new float[knots.Count - 1];
            for (int i = 0; i < slopes.Length; i++)
                slopes[i] = (knots[i + 1].Y - knots[i].Y) / (knots[i + 1].X - knots[i].X);
            for (int i = 0; i < slopes.Length; i++)
            {
                float left = i == 0 ? slopes[i] : Tangent(slopes[i - 1], slopes[i]);
                float right = i == slopes.Length - 1 ? slopes[i] : Tangent(slopes[i], slopes[i + 1]);
                float dx = (knots[i + 1].X - knots[i].X) / 3;
                path.AddBezier(knots[i], new PointF(knots[i].X + dx, knots[i].Y + left * dx),
                    new PointF(knots[i + 1].X - dx, knots[i + 1].Y - right * dx), knots[i + 1]);
            }
            return path;
        }
        private static float Tangent(float before, float after)
        {
            return before * after > 0 ? 2 * before * after / (before + after) : 0;
        }
        private static void DrawTimeline(Graphics g, RectangleF plot, SsvepPerformance data, Font font, bool showAll)
        {
            double start = TimelineStart(data, showAll), end = TimelineEnd(data, showAll);
            Func<double, float> x = seconds => plot.Left + (float)((seconds - start) / (end - start)) * plot.Width;
            Func<int, float> y = target => plot.Bottom - target * plot.Height / 8;
            var state = g.Save(); g.SetClip(plot);
            using (var targetPen = new Pen(Color.ForestGreen, 2))
            using (var outputPen = new Pen(Color.FromArgb(130, Color.RoyalBlue), 1))
            using (var unlabeledPen = new Pen(Color.Silver, 1))
            {
                foreach (var a in data.Attempts) g.DrawLine(targetPen, x(a.StartSeconds), y(a.Target), x(a.EndSeconds), y(a.Target));
                PointF? previous = null;
                int previousAttempt = -1;
                foreach (var d in data.Decisions)
                {
                    if (!showAll && d.Target < 0) { previous = null; continue; }
                    if (!showAll && previousAttempt != d.AttemptIndex) previous = null;
                    var point = new PointF(x(d.Seconds), y(d.Prediction));
                    if (previous.HasValue) g.DrawLine(d.Target < 0 ? unlabeledPen : outputPen, previous.Value, point);
                    Brush pointBrush = d.Target < 0 ? Brushes.Gray : d.NewSelection ? Brushes.OrangeRed : Brushes.RoyalBlue;
                    g.FillEllipse(pointBrush, point.X - 2, point.Y - 2, 4, 4);
                    previous = point;
                    previousAttempt = d.AttemptIndex;
                }
            }
            g.Restore(state);
            g.DrawString("Green: target    Blue: decoder    Orange: new selection" + (showAll ? "    Gray: unlabeled" : ""), font, Brushes.DimGray, plot.Left, 2);
        }
        private static RectangleF PlotBounds(int width, int height, bool timeline)
        { return new RectangleF(58, 22, Math.Max(20, width - (timeline ? 78 : 200)), Math.Max(20, height - 68)); }

        private static double TimelineStart(SsvepPerformance data, bool showAll)
        { return !showAll && data.Attempts.Count > 0 ? data.Attempts[0].StartSeconds : 0; }

        private static double TimelineEnd(SsvepPerformance data, bool showAll)
        {
            double end = !showAll && data.Attempts.Count > 0 ? data.Attempts.Last().EndSeconds :
                Math.Max(data.Decisions.Count > 0 ? data.Decisions.Last().Seconds : 1,
                    data.Attempts.Count > 0 ? data.Attempts.Last().EndSeconds : 1);
            return Math.Max(TimelineStart(data, showAll) + 0.001, end);
        }

        private static void DrawUnlabeled(Graphics g, RectangleF plot, SsvepPerformance data, Font font)
        {
            double end = TimelineEnd(data, true), previous = 0;
            using (var fill = new SolidBrush(Color.FromArgb(242, 244, 246)))
            {
                foreach (var a in data.Attempts.Concat(new[] { new SsvepPerformanceAttempt { StartSeconds = end, EndSeconds = end } }))
                {
                    if (a.StartSeconds > previous)
                    {
                        float left = plot.Left + (float)(previous / end) * plot.Width;
                        float width = (float)((a.StartSeconds - previous) / end) * plot.Width;
                        g.FillRectangle(fill, left, plot.Top, width, plot.Height);
                        if (width > g.MeasureString("Unlabeled", font).Width + 8)
                            g.DrawString("Unlabeled", font, Brushes.DimGray, left + 4, plot.Top + 4);
                    }
                    previous = Math.Max(previous, a.EndSeconds);
                }
            }
        }

        // Uses the same plot bounds and coordinates as DrawTimeline. No EEG rendering is involved.
        public static string TimelineHint(Control surface, SsvepPerformance data, Point location, bool showAll = false)
        {
            if (data == null || surface.ClientSize.Width < 220 || surface.ClientSize.Height < 130) return null;
            RectangleF plot = PlotBounds(surface.ClientSize.Width, surface.ClientSize.Height, true);
            if (!plot.Contains(location)) return null;
            double start = TimelineStart(data, showAll), end = TimelineEnd(data, showAll);
            double seconds = start + (location.X - plot.Left) / plot.Width * (end - start);
            SsvepPerformanceDecision nearest = null;
            double distance = 64;
            foreach (var d in data.Decisions)
            {
                if (!showAll && d.Target < 0) continue;
                double dx = plot.Left + (d.Seconds - start) / (end - start) * plot.Width - location.X;
                double dy = plot.Bottom - d.Prediction * plot.Height / 8 - location.Y;
                double squared = dx * dx + dy * dy;
                if (squared <= distance) { distance = squared; nearest = d; }
            }
            if (nearest != null)
            {
                var d = nearest;
                string target = d.Target < 0 ? "Unlabeled (excluded from target metrics)" :
                    d.Target == 0 ? "Background (label " + d.TargetLabel + ")" : "Note " + SsvepPerformance.Note(d.Target);
                return "Time: " + Time(d.Seconds) + "\nTarget: " + target +
                    "\nDecoder: " + (d.Prediction > 0 ? "Note " + SsvepPerformance.Note(d.Prediction) :
                        d.Result.IsRejected ? "EOG rejected" : "No note") +
                    "\nHeld key: " + (d.HeldNote > 0 ? SsvepPerformance.Note(d.HeldNote).ToString() : "None") +
                    (d.NewSelection ? "\nNew selection" : d.Released ? "\nKey released" : "") +
                    (d.Prediction == 0 && d.Result.CandidateClassId.HasValue && !d.Result.IsRejected ?
                        "\nNo-note count: " + d.NoNoteCount + "/" + SsvepPerformance.ReleaseConfirmationCount : "") +
                    "\nEEG window: " + Time((double)(d.Result.WindowStartQpc - data.SourceFirstQpc) / data.SourceQpcFrequency) +
                    " - " + Time(d.Seconds);
            }
            var attempt = data.Attempts.FirstOrDefault(a => seconds >= a.StartSeconds && seconds < a.EndSeconds);
            return "Time: " + Time(seconds) + "\nTarget: " + (attempt == null ? "Unlabeled (excluded from target metrics)" :
                attempt.Target == 0 ? "Background (label " + attempt.Label + ")" : "Note " + SsvepPerformance.Note(attempt.Target)) +
                (attempt == null ? "" : "\nTarget period: " + Time(attempt.StartSeconds) + " - " + Time(attempt.EndSeconds));
        }

        private static string Time(double seconds)
        {
            var ms = (long)Math.Round(Math.Max(0, seconds) * 1000);
            return (ms / 60000).ToString("00", CultureInfo.InvariantCulture) + ":" + (ms / 1000 % 60).ToString("00") + "." + (ms % 1000).ToString("000");
        }
    }
}
