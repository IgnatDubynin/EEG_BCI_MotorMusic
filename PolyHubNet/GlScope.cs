using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace PolyHubNet
{
    public sealed class GlScope : IDisposable
    {
        private readonly GLControl _glCtrl;
        private readonly object _sync = new object();
        private readonly List<Tuple<long, long>> _rejectedIntervals = new List<Tuple<long, long>>();
        private long[] _previewHeogMarkers = new long[0];
        private long[] _playbackHeogMarkers = new long[0];
        private Tuple<long, long, bool>[] _previewAnalysisIntervals = new Tuple<long, long, bool>[0];
        private Tuple<long, long, bool>[] _playbackAnalysisIntervals = new Tuple<long, long, bool>[0];
        private Tuple<long, long, HeogMarkupInterval>[] _previewHeogEpochs = new Tuple<long, long, HeogMarkupInterval>[0];
        private Tuple<long, long, HeogMarkupInterval>[] _playbackHeogEpochs = new Tuple<long, long, HeogMarkupInterval>[0];
        private readonly List<Tuple<Rectangle, HeogMarkupInterval>> _heogEpochLabels = new List<Tuple<Rectangle, HeogMarkupInterval>>();
        private readonly List<int> _heogLabelRowEnds = new List<int>();
        private int _heogLabelTexture;
        private const int HeogLabelWidth = 26, HeogLabelHeight = 20;
        private long _playbackMarkupSampleOffset;

        private readonly Action _invalidateOnUiThreadAction;
        private int _uiRefreshPending;
        private int _renderTickPending;

        private SignalStream _inputStream;

        private bool _glInitialized;
        private bool _freezeTimeline;

        private int _channelsCount;
        private int _sampleRate;
        private int _samplesPerChannel;

        private int _timeBaseSeconds = 5;
        private float _scaleVolts = 100e-6f;

        private long _qpcFreq;

        private long _totalReceivedSamples;
        private long _lastDataEndQpc;
        private long _lastDataEndSample;

        // -------------------------------------------------
        // Live page shift / screen position
        // -------------------------------------------------
        private float _pageShiftPx;
        private long _visualPageSeq;
        private float _pageDxPx;

        // -------------------------------------------------
        // Pending blocks for direct drawing
        // -------------------------------------------------
        private struct PendingBlock
        {
            public float[] Samples;
            public int SamplesCount;
            public long FirstAbsoluteSample;
            public int ChannelsCount;
        }

        private List<PendingBlock> _pendingBlocks = new List<PendingBlock>(8);
        private List<PendingBlock> _pendingBlocksUi = new List<PendingBlock>(8);

        // -------------------------------------------------
        // Pending event markers
        // -------------------------------------------------
        private struct PendingEvent
        {
            public long AbsoluteSample;
            public int ClassId;
            public int State; // 1=Start, 2=End
        }

        private List<PendingEvent> _pendingEvents = new List<PendingEvent>(8);
        private List<PendingEvent> _pendingEventsUi = new List<PendingEvent>(8);
        private readonly List<PendingEvent> _drawnEvents = new List<PendingEvent>();

        // -------------------------------------------------
        // Page drawing state
        // -------------------------------------------------
        private struct PrevPoint
        {
            public bool HasValue;
            public float X;
            public float Y;
            public long AbsoluteSample;
        }

        private PrevPoint[] _prevPoints;

        private int[] _signalTex = new int[2];
        private int[] _signalFbo = new int[2];
        private long[] _signalSeq = new long[] { long.MinValue, long.MinValue };

        private int _pageW;
        private int _pageH;

        // -------------------------------------------------
        // Overlay
        // -------------------------------------------------
        private int _overlayTextureId;
        private Bitmap _overlayBitmap;
        private bool _overlayDirty = true;

        private const float PlotLeft = 70.0f;
        private const float PlotTop = 10.0f;
        private const float PlotRight = 10.0f;
        private const float PlotBottom = 24.0f;

        [DllImport("kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long value);

        private int _uiTimerPeriodMs = 5;

        // -------------------------------------------------
        // Static page mode
        // -------------------------------------------------
        private bool _staticPageMode;
        private int _staticPageSamplesCount;
        private int _staticViewRightPageSample;

        // -------------------------------------------------
        // Channel selection / focused view
        // -------------------------------------------------
        private bool[] _selectedChannels = new bool[0];
        private int[] _visibleChannels = new int[0];
        private int[] _channelToVisibleIndex = new int[0];
        private bool _showSelectedOnly;


        public GlScope(GLControl glCtrl)
        {
            if (glCtrl == null)
                throw new ArgumentNullException("glCtrl");

            _glCtrl = glCtrl;
            _glCtrl.Paint += glCtrl_Paint;
            _glCtrl.Resize += glCtrl_Resize;
            _glCtrl.MouseClick += glCtrl_MouseClick;
            _glCtrl.MouseDoubleClick += glCtrl_MouseDoubleClick;

            _invalidateOnUiThreadAction = InvalidateOnUiThread;

            QueryPerformanceFrequency(out _qpcFreq);
        }

        public void SetUiTimerPeriodMs(int uiTimerPeriodMs)
        {
            if (uiTimerPeriodMs <= 0)
                uiTimerPeriodMs = 8;

            _uiTimerPeriodMs = uiTimerPeriodMs;
            UpdatePageDxPx();
        }

        // -------------------------------------------------
        // Public API
        // -------------------------------------------------
        public void AttachInput(SignalStream input)
        {
            DetachInput();

            _inputStream = input;
            if (_inputStream != null)
            {
                _inputStream.BlockPublished += InputStream_BlockPublished;

                _inputStream.ChannelNamesChanged += InputStream_ChannelNamesChanged;
            }
        }

        public void DetachInput()
        {
            if (_inputStream != null)
            {
                _inputStream.BlockPublished -= InputStream_BlockPublished;
                _inputStream.ChannelNamesChanged -= InputStream_ChannelNamesChanged;
            }

            _inputStream = null;
        }

        public void Configure(int channelsCount, int sampleRate, int timeBaseSeconds, float scaleVolts)
        {
            lock (_sync)
            {
                _freezeTimeline = false;
                _playbackHeogMarkers = new long[0];
                _playbackHeogEpochs = new Tuple<long, long, HeogMarkupInterval>[0];
                _heogEpochLabels.Clear();
                _playbackAnalysisIntervals = new Tuple<long, long, bool>[0];
                _playbackMarkupSampleOffset = 0;

                _channelsCount = channelsCount > 0 ? channelsCount : 0;
                _sampleRate = sampleRate > 0 ? sampleRate : 250;
                _timeBaseSeconds = timeBaseSeconds > 0 ? timeBaseSeconds : 5;
                _scaleVolts = scaleVolts > 0.0f ? scaleVolts : 100e-6f;

                _samplesPerChannel = _sampleRate * _timeBaseSeconds;
                if (_samplesPerChannel < 100)
                    _samplesPerChannel = 100;

                _totalReceivedSamples = 0;
                _lastDataEndQpc = 0;
                _lastDataEndSample = 0;

                _pageShiftPx = 0.0f;
                _visualPageSeq = 0;

                ClearPendingBlocks_NoLock();
                ClearPendingEvents_NoLock();
                ClearSignalHistory_NoLock();
                ResetChannelSelection_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        public void SetTimeBaseSeconds(int timeBaseSeconds)
        {
            int newTimeBase = timeBaseSeconds > 0 ? timeBaseSeconds : 5;

            lock (_sync)
            {
                if (newTimeBase == _timeBaseSeconds)
                    return;

                _timeBaseSeconds = newTimeBase;

                _samplesPerChannel = _sampleRate * _timeBaseSeconds;
                if (_samplesPerChannel < 100)
                    _samplesPerChannel = 100;

                RebasePlaybackMarkup_NoLock(_totalReceivedSamples);
                _totalReceivedSamples = 0;
                _lastDataEndQpc = 0;
                _lastDataEndSample = 0;

                _pageShiftPx = 0.0f;
                _visualPageSeq = 0;

                ClearPendingBlocks_NoLock();
                ClearPendingEvents_NoLock();
                ClearSignalHistory_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        public void SetScaleVolts(float scaleVolts)
        {
            lock (_sync)
            {
                _scaleVolts = scaleVolts > 0.0f ? scaleVolts : 100e-6f;

                _signalPagesNeedRedraw = true;
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        public void OnLiveRenderClockTick()
        {
            if (_staticPageMode)
                return;

            Interlocked.Increment(ref _renderTickPending);
            EnqueueInvalidate();
        }

        public void AddEventMarker(long qpcTimestamp, int classId, int state)
        {
            lock (_sync)
            {
                if (_sampleRate <= 0 || _qpcFreq <= 0 || _lastDataEndQpc <= 0)
                    return;

                double markerSample =
                    _lastDataEndSample -
                    ((double)(_lastDataEndQpc - qpcTimestamp) * _sampleRate / _qpcFreq);

                long absoluteSample = (long)Math.Round(markerSample);
                if (absoluteSample < 0)
                    absoluteSample = 0;

                _pendingEvents.Add(new PendingEvent
                {
                    AbsoluteSample = absoluteSample,
                    ClassId = classId,
                    State = state
                });
            }

            RequestImmediateInvalidate();
        }

        public void AddRejectedInterval(long startQpc, long endQpc)
        {
            lock (_sync)
            {
                if (_sampleRate <= 0 || _qpcFreq <= 0 || _lastDataEndQpc <= 0) return;
                long first = (long)Math.Round(_lastDataEndSample -
                    (double)(_lastDataEndQpc - startQpc) * _sampleRate / _qpcFreq);
                long last = (long)Math.Round(_lastDataEndSample -
                    (double)(_lastDataEndQpc - endQpc) * _sampleRate / _qpcFreq);
                AddRejectedSamples_NoLock(Math.Max(0, first), Math.Max(0, last));
            }
            RequestImmediateInvalidate();
        }

        // Gaze-shift lines use sample coordinates and do not create recording events.
        public void SetPreviewHeogMarkers(List<long> markers)
        {
            lock (_sync)
                _previewHeogMarkers = markers != null ? markers.ToArray() : new long[0];
            RequestImmediateInvalidate();
        }

        public void SetPlaybackHeogMarkers(List<long> markers)
        {
            lock (_sync)
                _playbackHeogMarkers = ShiftHeogMarkers(markers != null ? markers.ToArray() :
                    new long[0], _playbackMarkupSampleOffset);
            RequestImmediateInvalidate();
        }

        public void SetPreviewAnalysisIntervals(List<Tuple<long, long, bool>> intervals)
        {
            lock (_sync) _previewAnalysisIntervals = MergeAnalysisIntervals(intervals);
            RequestImmediateInvalidate();
        }

        public void SetPlaybackAnalysisIntervals(List<Tuple<long, long, bool>> intervals)
        {
            lock (_sync) _playbackAnalysisIntervals = ShiftMarkupIntervals(MergeAnalysisIntervals(intervals), _playbackMarkupSampleOffset);
            RequestImmediateInvalidate();
        }

        // Whole fixation spans, with a reference to the independently editable annotation.
        public void SetPreviewHeogEpochs(List<Tuple<long, long, HeogMarkupInterval>> epochs)
        {
            lock (_sync)
            {
                _previewHeogEpochs = epochs != null ? epochs.ToArray() : new Tuple<long, long, HeogMarkupInterval>[0];
                _previewAnalysisIntervals = EpochSpans(_previewHeogEpochs);
                _heogEpochLabels.Clear();
            }
            RequestImmediateInvalidate();
        }

        public void SetPlaybackHeogEpochs(List<Tuple<long, long, HeogMarkupInterval>> epochs)
        {
            lock (_sync)
            {
                _playbackHeogEpochs = ShiftHeogEpochs(epochs != null ? epochs.ToArray() :
                    new Tuple<long, long, HeogMarkupInterval>[0], _playbackMarkupSampleOffset);
                _playbackAnalysisIntervals = EpochSpans(_playbackHeogEpochs);
                _heogEpochLabels.Clear();
            }
            RequestImmediateInvalidate();
        }

        private static Tuple<long, long, bool>[] EpochSpans(Tuple<long, long, HeogMarkupInterval>[] epochs)
        {
            var spans = new List<Tuple<long, long, bool>>();
            foreach (var epoch in epochs)
                if (epoch.Item3.TargetLabel >= 0) spans.Add(Tuple.Create(epoch.Item1, epoch.Item2, true));
            return MergeAnalysisIntervals(spans);
        }

        // Hit-test the same snapped sample position used for a red line in static preview.
        private long? _timeCursorSample;
        private string _timeCursorText;

        public bool TryGetPreviewTimeSample(Point point, out long sample)
        {
            sample = -1;
            var plot = GetPlotRectangle();
            if (!_staticPageMode || !plot.Contains(point) || _pageW <= 1 || _staticPageSamplesCount <= 1) return false;
            sample = (long)Math.Round((point.X - plot.Right + GetPreviewStaticPageX(_staticViewRightPageSample)) *
                (_staticPageSamplesCount - 1) / (_pageW - 1.0));
            return sample >= 0 && sample < _staticPageSamplesCount;
        }

        public void SetPreviewTimeCursor(long? sample, string text)
        {
            _timeCursorSample = sample;
            _timeCursorText = text;
            RequestImmediateInvalidate();
        }

        private void DrawTimeCursor()
        {
            if (!_staticPageMode || !_timeCursorSample.HasValue || string.IsNullOrEmpty(_timeCursorText)) return;
            var plot = GetPlotRectangle();
            float x = (float)Math.Round(GetHeogSampleScreenX(_timeCursorSample.Value, plot));
            if (x < plot.Left || x > plot.Right) return;
            GL.Disable(EnableCap.Texture2D);
            GL.Color3(Color.White);
            GL.LineWidth(1f);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(x, plot.Top); GL.Vertex2(x, plot.Bottom);
            GL.End();
            DrawEventLabelCore(_timeCursorText, x - 4, plot.Bottom - 20, 0, _glCtrl.Width, Color.White);
            GL.Enable(EnableCap.Texture2D);
        }

        public bool TryGetPreviewHeogMarker(Point point, out long sample)
        {
            sample = -1;
            RectangleF plot = GetPlotRectangle();
            if (!_staticPageMode || !plot.Contains(point) || _staticPageSamplesCount <= 1 || _pageW <= 1) return false;
            long[] markers;
            lock (_sync) markers = _previewHeogMarkers;
            float nearest = 5.0f;
            foreach (long marker in markers)
            {
                if (marker < 0 || marker >= _staticPageSamplesCount) continue;
                float x = (float)Math.Floor(GetHeogSampleScreenX(marker, plot)) + 0.5f;
                float distance = Math.Abs(point.X - x);
                if (distance <= nearest) { nearest = distance; sample = marker; }
            }
            return sample >= 0;
        }

        public bool TryGetHeogEpochLabel(Point point, out HeogMarkupInterval epoch, out Rectangle bounds)
        {
            foreach (var label in _heogEpochLabels)
                if (label.Item1.Contains(point))
                { epoch = label.Item2; bounds = label.Item1; return true; }
            epoch = null;
            bounds = Rectangle.Empty;
            return false;
        }

        // Includes unnumbered fixations before the current start, so it can be moved earlier.
        public bool TryGetHeogEpochAt(Point point, out HeogMarkupInterval epoch)
        {
            epoch = null;
            RectangleF plot = GetPlotRectangle();
            if (!plot.Contains(point)) return false;
            Tuple<long, long, HeogMarkupInterval>[] epochs;
            long availableEnd;
            lock (_sync)
            {
                epochs = _staticPageMode ? _previewHeogEpochs : _playbackHeogEpochs;
                availableEnd = _staticPageMode ? _staticPageSamplesCount : _totalReceivedSamples;
            }
            if ((_staticPageMode ? _staticPageSamplesCount : _samplesPerChannel) <= 1) return false;
            float sampleWidth = (_pageW - 1.0f) /
                (_staticPageMode ? _staticPageSamplesCount - 1 : _samplesPerChannel - 1);
            foreach (var item in epochs)
            {
                long last = Math.Min(item.Item2, availableEnd);
                if (last <= item.Item1) continue;
                if (point.X >= GetHeogSampleScreenX(item.Item1, plot) &&
                    point.X < GetHeogSampleScreenX(last - 1, plot) + sampleWidth)
                { epoch = item.Item3; return true; }
            }
            return false;
        }

        private static Tuple<long, long, bool>[] MergeAnalysisIntervals(List<Tuple<long, long, bool>> intervals)
        {
            var merged = new List<Tuple<long, long, bool>>();
            if (intervals == null) return merged.ToArray();
            foreach (var interval in intervals)
            {
                if (interval.Item2 <= interval.Item1) continue;
                if (merged.Count > 0 && interval.Item1 <= merged[merged.Count - 1].Item2)
                {
                    var previous = merged[merged.Count - 1];
                    merged[merged.Count - 1] = Tuple.Create(previous.Item1, Math.Max(previous.Item2, interval.Item2), true);
                }
                else merged.Add(Tuple.Create(interval.Item1, interval.Item2, true));
            }
            return merged.ToArray();
        }

        private void DrawAnalysisIntervals(long pageSequence, float pageLeft, RectangleF plot)
        {
            Tuple<long, long, bool>[] intervals;
            long availableEnd;
            lock (_sync)
            {
                intervals = _staticPageMode ? _previewAnalysisIntervals : _playbackAnalysisIntervals;
                availableEnd = _staticPageMode ? _staticPageSamplesCount : _totalReceivedSamples;
            }
            if (intervals.Length == 0 || pageSequence < 0) return;
            long pageFirst = _staticPageMode ? 0 : pageSequence * _samplesPerChannel;
            long pageEnd = Math.Min(availableEnd, pageFirst +
                (_staticPageMode ? _staticPageSamplesCount : _samplesPerChannel));
            if (pageEnd <= pageFirst) return;
            bool blendWasEnabled = GL.IsEnabled(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            float sampleWidth = (_pageW - 1.0f) /
                (_staticPageMode ? _staticPageSamplesCount - 1 : _samplesPerChannel - 1);
            foreach (var interval in intervals)
            {
                if (interval.Item2 <= pageFirst) continue;
                if (interval.Item1 >= pageEnd) break;
                long first = Math.Max(pageFirst, interval.Item1);
                long last = Math.Min(pageEnd, interval.Item2);
                if (last <= first) continue;
                float x1 = pageLeft + (_staticPageMode ? GetPreviewStaticPageX(first) : GetPageX(first));
                float x2 = Math.Min(pageLeft + _pageW - 1.0f,
                    pageLeft + (_staticPageMode ? GetPreviewStaticPageX(last - 1) : GetPageX(last - 1)) + sampleWidth);
                GL.Color4(0.15f, 0.8f, 0.35f, 0.13f);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(x1, plot.Top); GL.Vertex2(x2, plot.Top);
                GL.Vertex2(x2, plot.Bottom); GL.Vertex2(x1, plot.Bottom);
                GL.End();
            }
            if (!blendWasEnabled) GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
        }

        private void DrawHeogMarkers(long pageSequence, float pageLeft, RectangleF plot)
        {
            long[] markers;
            long availableEnd;
            lock (_sync)
            {
                markers = _staticPageMode ? _previewHeogMarkers : _playbackHeogMarkers;
                availableEnd = _staticPageMode ? _staticPageSamplesCount : _totalReceivedSamples;
            }
            if (markers.Length == 0 || pageSequence < 0) return;
            long pageFirst = _staticPageMode ? 0 : pageSequence * _samplesPerChannel;
            long pageEnd = Math.Min(availableEnd, pageFirst +
                (_staticPageMode ? _staticPageSamplesCount : _samplesPerChannel));
            bool smoothWasEnabled = GL.IsEnabled(EnableCap.LineSmooth);
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(1.0f, 0.2f, 0.2f, 1.0f);
            GL.Begin(PrimitiveType.Lines);
            foreach (long sample in markers)
            {
                if (sample < pageFirst) continue;
                if (sample >= pageEnd) break;
                float x = pageLeft + (_staticPageMode ? GetPreviewStaticPageX(sample) : GetPageX(sample));
                x = (float)Math.Floor(x) + 0.5f;
                GL.Vertex2(x, plot.Top); GL.Vertex2(x, plot.Bottom);
            }
            GL.End();
            if (smoothWasEnabled) GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Texture2D);
        }

        public void AddPreviewRejectedInterval(long first, long last)
        {
            lock (_sync)
                AddRejectedSamples_NoLock(Math.Max(0, first), Math.Min(_staticPageSamplesCount - 1, last));
            RequestImmediateInvalidate();
        }

        private void AddRejectedSamples_NoLock(long first, long last)
        {
            if (last < first) return;
            for (int i = _rejectedIntervals.Count - 1; i >= 0; i--)
            {
                var range = _rejectedIntervals[i];
                if (range.Item2 + 1 < first || range.Item1 > last + 1) continue;
                first = Math.Min(first, range.Item1); last = Math.Max(last, range.Item2);
                _rejectedIntervals.RemoveAt(i);
            }
            _rejectedIntervals.Add(Tuple.Create(first, last));
            if (!_staticPageMode)
                _rejectedIntervals.RemoveAll(r => r.Item2 < _totalReceivedSamples - 2L * _samplesPerChannel);
        }

        private void DrawRejectedIntervals(long pageSequence, float pageLeft, RectangleF plot)
        {
            Tuple<long, long>[] ranges;
            lock (_sync) ranges = _rejectedIntervals.ToArray();
            if (ranges.Length == 0 || pageSequence < 0) return;
            long pageFirst = _staticPageMode ? 0 : pageSequence * _samplesPerChannel;
            long pageLast = pageFirst + (_staticPageMode ? _staticPageSamplesCount : _samplesPerChannel) - 1;
            bool blendWasEnabled = GL.IsEnabled(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            foreach (var range in ranges)
            {
                long first = Math.Max(pageFirst, range.Item1), last = Math.Min(pageLast, range.Item2);
                if (last < first) continue;
                float x1 = pageLeft + (_staticPageMode ? GetPreviewStaticPageX(first) : GetPageX(first));
                float x2 = pageLeft + (_staticPageMode ? GetPreviewStaticPageX(last) : GetPageX(last)) + 1.0f;
                GL.Color4(0.0f, 0.0f, 0.0f, 0.65f);
                GL.Begin(PrimitiveType.Quads);
                GL.Vertex2(x1, plot.Top); GL.Vertex2(x2, plot.Top);
                GL.Vertex2(x2, plot.Bottom); GL.Vertex2(x1, plot.Bottom);
                GL.End();
                if (range.Item2 <= pageLast)
                    DrawEventLabelCore("Rejected", x2 - 8.0f, plot.Top + 18.0f, plot.Left, plot.Right, Color.Gray);
            }
            // Markers already drawn into the signal page must stay bright and
            // keep their numeric labels above the rejection overlay.
            PendingEvent[] markers;
            lock (_sync) markers = _drawnEvents.ToArray();
            foreach (PendingEvent marker in markers)
            {
                if (marker.AbsoluteSample < pageFirst || marker.AbsoluteSample > pageLast) continue;
                float localX = _staticPageMode ? GetPreviewStaticPageX(marker.AbsoluteSample) : GetPageX(marker.AbsoluteSample);
                DrawEventMarkerCore(pageLeft + (float)Math.Round(localX), plot.Top, plot.Bottom,
                    Math.Max(plot.Left, pageLeft), Math.Min(plot.Right, pageLeft + _pageW), marker.ClassId, marker.State);
            }
            if (!blendWasEnabled) GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.Texture2D);
        }

        public void ClearEventMarkers()
        {
            lock (_sync)
            {
                ClearPendingEvents_NoLock();
            }

            RequestImmediateInvalidate();
        }

        public void SetFreezeTimeline(bool freeze)
        {
            _freezeTimeline = freeze;
        }

        public void ResetFrozenViewToLatest()
        {
            _freezeTimeline = false;
            RequestImmediateInvalidate();
        }

        public void RefreshNow()
        {
            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        public void EnterPreviewStaticPageMode(int channelsCount, int sampleRate, int timeBaseSeconds, float scaleVolts)
        {
            lock (_sync)
            {
                _staticPageMode = true;

                _channelsCount = channelsCount > 0 ? channelsCount : 0;
                _sampleRate = sampleRate > 0 ? sampleRate : 250;
                _timeBaseSeconds = timeBaseSeconds > 0 ? timeBaseSeconds : 5;
                _scaleVolts = scaleVolts > 0.0f ? scaleVolts : 100e-6f;

                _samplesPerChannel = _sampleRate * _timeBaseSeconds;
                if (_samplesPerChannel < 100)
                    _samplesPerChannel = 100;

                _pageShiftPx = 0.0f;
                _visualPageSeq = 0;

                _staticPageSamplesCount = _samplesPerChannel;
                _staticViewRightPageSample = 0;

                ClearPendingBlocks_NoLock();
                ClearPendingEvents_NoLock();
                ClearSignalHistory_NoLock();
                NormalizeChannelSelection_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
        }

        public void ExitPreviewStaticPageMode()
        {
            lock (_sync)
            {
                _staticPageMode = false;
                _staticPageSamplesCount = 0;
                _staticViewRightPageSample = 0;

                _pageShiftPx = 0.0f;
                _visualPageSeq = 0;

                ClearPendingBlocks_NoLock();
                ClearPendingEvents_NoLock();
                ClearSignalHistory_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
        }

        public void SetPreviewStaticRightPageSample(int viewRightPageSample)
        {
            lock (_sync)
            {
                if (viewRightPageSample < 0)
                    viewRightPageSample = 0;

                if (viewRightPageSample > _staticPageSamplesCount - 1)
                    viewRightPageSample = _staticPageSamplesCount - 1;

                _staticViewRightPageSample = viewRightPageSample;
            }
        }

        public void BeginPreviewStaticPage(int pageSamplesCount, int rightPageSample)
        {
            if (pageSamplesCount <= 0)
                pageSamplesCount = 1;

            lock (_sync)
            {
                _staticPageMode = true;
                _staticPageSamplesCount = pageSamplesCount;
                ClearPendingEvents_NoLock();
                ClearSignalHistory_NoLock();

                if (rightPageSample < 0)
                    rightPageSample = 0;

                if (rightPageSample > _staticPageSamplesCount - 1)
                    rightPageSample = _staticPageSamplesCount - 1;

                _staticViewRightPageSample = rightPageSample;

                _pageShiftPx = 0.0f;
                _visualPageSeq = 0;
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();

                if (_signalFbo[0] != 0)
                    ClearSignalPage(0);

                if (_signalFbo[1] != 0)
                    ClearSignalPage(1);
            }

            EnsurePrevPoints();
            ResetPrevPoints();
        }

        private float GetPreviewStaticPageX(long pageSample)
        {
            if (_staticPageSamplesCount <= 1 || _pageW <= 1)
                return 0.0f;

            return (float)pageSample * (_pageW - 1) / (float)(_staticPageSamplesCount - 1);
        }

        private int GetPreviewStaticPageWidthPx(RectangleF rect)
        {
            int plotWidthPx = Math.Max(1, (int)Math.Ceiling(rect.Width));

            if (!_staticPageMode || _staticPageSamplesCount <= 1 || _samplesPerChannel <= 1)
                return plotWidthPx;

            int staticWidthPx = 1 + (int)Math.Ceiling(
                (double)(_staticPageSamplesCount - 1) * (plotWidthPx - 1) / (_samplesPerChannel - 1));

            if (staticWidthPx < plotWidthPx)
                staticWidthPx = plotWidthPx;

            return staticWidthPx;
        }

        public void DrawPreviewStaticBlock(float[] blockChannelMajor, int channelsCount, int sampleCount, long firstPageSample)
        {
            if (!_staticPageMode || blockChannelMajor == null || channelsCount <= 0 || sampleCount <= 0 ||
                blockChannelMajor.Length < (long)channelsCount * sampleCount) return;
            lock (_sync)
            {
                _staticSignalBlocks.Add(CopySignalBlock(blockChannelMajor, channelsCount, sampleCount, firstPageSample));
                _signalPagesNeedRedraw = true;
            }
        }

        private void RenderPreviewStaticBlock(float[] blockChannelMajor, int channelsCount, int sampleCount, long firstPageSample)
        {
            if (blockChannelMajor == null || blockChannelMajor.Length == 0)
                return;

            if (!_staticPageMode)
                return;

            if (channelsCount <= 0 || sampleCount <= 0 || _staticPageSamplesCount <= 1)
                return;

            if (_signalFbo[0] == 0)
                return;

            EnsurePrevPoints();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[0]);
            SetupPageViewport();
            GL.Disable(EnableCap.Texture2D);

            try
            {
                for (int s = 0; s < sampleCount; s++)
                {
                    long pageSample = firstPageSample + s;
                    if (pageSample < 0 || pageSample >= _staticPageSamplesCount)
                        continue;

                    float x2 = GetPreviewStaticPageX(pageSample);

                    for (int ch = 0; ch < channelsCount; ch++)
                    {
                        int srcIndex = ch * sampleCount + s;
                        float value = blockChannelMajor[srcIndex];

                        float y2;
                        if (!TryGetPageY(ch, channelsCount, value, out y2))
                        {
                            _prevPoints[ch].HasValue = false;
                            continue;
                        }

                        PrevPoint prev = _prevPoints[ch];

                        if (prev.HasValue && prev.AbsoluteSample == pageSample - 1)
                        {
                            GL.LineWidth(GetChannelLineWidth(ch));
                            GL.Color3(GetChannelColor(ch));
                            GL.Begin(PrimitiveType.Lines);
                            GL.Vertex2(prev.X, prev.Y);
                            GL.Vertex2(x2, y2);
                            GL.End();
                        }

                        prev.HasValue = true;
                        prev.X = x2;
                        prev.Y = y2;
                        prev.AbsoluteSample = pageSample;
                        _prevPoints[ch] = prev;
                    }
                }
            }
            finally
            {
                GL.LineWidth(1.0f);
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                SetupViewport();
                GL.Enable(EnableCap.Texture2D);
            }

            _signalSeq[0] = 0;
        }

        public void AddPreviewStaticEvent(long pageSample, int classId, int state)
        {
            if (!_staticPageMode)
                return;

            if (pageSample < 0 || pageSample >= _staticPageSamplesCount)
                return;

            lock (_sync)
            {
                _drawnEvents.Add(new PendingEvent { AbsoluteSample = pageSample, ClassId = classId, State = state });
                _signalPagesNeedRedraw = true;
            }
        }

        private void InputStream_ChannelNamesChanged()
        {
            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        // -------------------------------------------------
        // Input stream
        // -------------------------------------------------
        private void InputStream_BlockPublished(SignalBlock block)
        {
            AppendStreamingBlockChannelMajor(block);
        }

        public void AppendStreamingBlockChannelMajor(SignalBlock block)
        {
            if (block == null || block.Data == null)
                return;

            if (_staticPageMode)
                return;

            lock (_sync)
            {
                if (_channelsCount != block.ChannelCount || _sampleRate != block.SampleRate)
                {
                    _channelsCount = block.ChannelCount;
                    _sampleRate = block.SampleRate;

                    _samplesPerChannel = _sampleRate * _timeBaseSeconds;
                    if (_samplesPerChannel < 100)
                        _samplesPerChannel = 100;

                    _totalReceivedSamples = 0;
                    _lastDataEndQpc = 0;
                    _lastDataEndSample = 0;

                    _pageShiftPx = 0.0f;
                    _visualPageSeq = 0;

                    ClearPendingBlocks_NoLock();
                    ClearPendingEvents_NoLock();
                    ClearSignalHistory_NoLock();
                    ResetChannelSelection_NoLock();

                    InvalidateOverlay();
                }

                if (_freezeTimeline)
                {
                    _totalReceivedSamples += block.SampleCount;

                    long blockDurationQpc = (long)((double)block.SampleCount * _qpcFreq / _sampleRate);
                    _lastDataEndQpc = block.QpcTimestamp + blockDurationQpc;
                    _lastDataEndSample = _totalReceivedSamples - 1;
                    return;
                }

                _pendingBlocks.Add(new PendingBlock
                {
                    Samples = block.Data,
                    SamplesCount = block.SampleCount,
                    FirstAbsoluteSample = _totalReceivedSamples,
                    ChannelsCount = block.ChannelCount
                });

                _totalReceivedSamples += block.SampleCount;

                long blockDurationQpc2 = (long)((double)block.SampleCount * _qpcFreq / _sampleRate);
                _lastDataEndQpc = block.QpcTimestamp + blockDurationQpc2;
                _lastDataEndSample = _totalReceivedSamples - 1;
            }
        }

        // -------------------------------------------------
        // UI invalidation
        // -------------------------------------------------
        private void InvalidateOnUiThread()
        {
            try
            {
                if (!_glCtrl.IsDisposed && _glCtrl.IsHandleCreated)
                    _glCtrl.Refresh();   // сразу Paint на UI-потоке
            }
            finally
            {
                Interlocked.Exchange(ref _uiRefreshPending, 0);
            }
        }

        private void EnqueueInvalidate()
        {
            if (_glCtrl.IsDisposed || !_glCtrl.IsHandleCreated)
                return;

            if (Interlocked.Exchange(ref _uiRefreshPending, 1) != 0)
                return;

            try
            {
                if (_glCtrl.InvokeRequired)
                    _glCtrl.Invoke(_invalidateOnUiThreadAction);
                else
                    _invalidateOnUiThreadAction();
            }
            catch
            {
                Interlocked.Exchange(ref _uiRefreshPending, 0);
            }
        }

        private void RequestImmediateInvalidate()
        {
            EnqueueInvalidate();
        }

        // -------------------------------------------------
        // GL init / viewport
        // -------------------------------------------------
        private void EnsureGlInitialized()
        {
            if (_glInitialized)
                return;

            if (!_glCtrl.IsHandleCreated)
                return;

            _glCtrl.MakeCurrent();

            GL.ClearColor(Color.Black);
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Disable(EnableCap.CullFace);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Disable(EnableCap.PointSmooth);

            GL.LineWidth(1.0f);
            GL.PointSize(1.0f);

            SetupViewport();

            _glInitialized = true;
        }

        private void SetupViewport()
        {
            int w = _glCtrl.Width > 0 ? _glCtrl.Width : 1;
            int h = _glCtrl.Height > 0 ? _glCtrl.Height : 1;

            GL.Viewport(0, 0, w, h);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, w, h, 0.0, -1.0, 1.0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void SetupPageViewport()
        {
            int w = _pageW > 0 ? _pageW : 1;
            int h = _pageH > 0 ? _pageH : 1;

            GL.Viewport(0, 0, w, h);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, w, h, 0.0, -1.0, 1.0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        // -------------------------------------------------
        // Paint / resize
        // -------------------------------------------------
        private void glCtrl_Paint(object sender, PaintEventArgs e)
        {
            EnsureGlInitialized();
            if (!_glInitialized) return;
            _glCtrl.MakeCurrent();

            int pendingLiveTicks = Interlocked.Exchange(ref _renderTickPending, 0);

            DrawLiveFrameFromClock(pendingLiveTicks);
            RenderFrame();
        }

        private void glCtrl_Resize(object sender, EventArgs e)
        {
            // Paint recreates GPU pages and restores their samples/markers from
            // the CPU cache. Never discard pending data on a viewport resize.
            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        private void glCtrl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            RectangleF rect = GetPlotRectangle();

            // Single click selects only from the channel-name column.
            if (e.X < 0 || e.X >= rect.Left || e.Y < rect.Top || e.Y >= rect.Bottom)
                return;

            lock (_sync)
            {
                if (_visibleChannels == null || _visibleChannels.Length == 0)
                    return;

                float bandHeight = rect.Height / _visibleChannels.Length;
                int visibleIndex = (int)((e.Y - rect.Top) / bandHeight);

                if (visibleIndex < 0 || visibleIndex >= _visibleChannels.Length)
                    return;

                int channelIndex = _visibleChannels[visibleIndex];
                if (channelIndex < 0 || channelIndex >= _selectedChannels.Length)
                    return;

                _selectedChannels[channelIndex] = !_selectedChannels[channelIndex];

                if (_showSelectedOnly)
                {
                    if (GetSelectedChannelCount_NoLock() == 0)
                        _showSelectedOnly = false;
                }

                RebuildVisibleChannels_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        private void glCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            RectangleF rect = GetPlotRectangle();

            HeogMarkupInterval epoch;
            Rectangle labelBounds;
            if (TryGetHeogEpochLabel(e.Location, out epoch, out labelBounds)) return;

            // Double click switches the plot itself between all and selected channels.
            if (e.X < rect.Left || e.X >= rect.Right || e.Y < rect.Top || e.Y >= rect.Bottom)
                return;

            lock (_sync)
            {
                if (GetSelectedChannelCount_NoLock() == 0)
                    return;

                _showSelectedOnly = !_showSelectedOnly;
                RebuildVisibleChannels_NoLock();
            }

            if (_glInitialized)
            {
                _glCtrl.MakeCurrent();
                RecreateSignalPages();
            }

            InvalidateOverlay();
            RequestImmediateInvalidate();
        }

        // -------------------------------------------------
        // UI draw delegate target
        // -------------------------------------------------
        private void DrawLiveFrameFromClock(int pendingLiveTicks)
        {
            EnsureSignalPages();

            if (_staticPageMode)
                return;

            RenderPendingLiveBlocksToPages();
            RenderPendingLiveEventsToPages();

            if (!_freezeTimeline && pendingLiveTicks > 0)
                AdvanceLivePageShift(pendingLiveTicks);

            ClampLivePageShiftToLatestData();
        }

        private void RenderPendingLiveBlocksToPages()
        {
            SwapPendingBlocks();

            if (_pendingBlocksUi.Count == 0)
                return;

            for (int i = 0; i < _pendingBlocksUi.Count; i++)
            {
                PendingBlock block = _pendingBlocksUi[i];
                CacheRenderedSignalBlock(block);

                RenderLiveBlockToPages(
                    block.Samples,
                    block.ChannelsCount,
                    block.SamplesCount,
                    block.FirstAbsoluteSample);
            }

            _pendingBlocksUi.Clear();
        }

        private void RenderPendingLiveEventsToPages()
        {
            SwapPendingEvents();

            if (_pendingEventsUi.Count == 0)
                return;

            for (int i = 0; i < _pendingEventsUi.Count; i++)
            {
                PendingEvent evt = _pendingEventsUi[i];
                RenderLiveEventToPages(evt.AbsoluteSample, evt.ClassId, evt.State);
            }

            _pendingEventsUi.Clear();
        }

        private void RenderLiveBlockToPages(float[] blockChannelMajor, int channelsCount, int sampleCount, long firstAbsoluteSample)
        {
            if (blockChannelMajor == null || blockChannelMajor.Length == 0)
                return;

            if (channelsCount <= 0 || sampleCount <= 0 || _samplesPerChannel <= 1)
                return;

            int boundSlot = -1;

            try
            {
                for (int s = 0; s < sampleCount; s++)
                {
                    long abs = firstAbsoluteSample + s;
                    long seq = abs / _samplesPerChannel;
                    int slot = (int)(seq & 1L);

                    if (_signalSeq[slot] != seq)
                    {
                        ClearSignalPage(slot);
                        _signalSeq[slot] = seq;
                    }

                    if (boundSlot != slot)
                    {
                        if (boundSlot != -1)
                            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

                        GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[slot]);
                        SetupPageViewport();
                        GL.Disable(EnableCap.Texture2D);

                        boundSlot = slot;
                    }

                    float x2 = GetPageX(abs);

                    for (int ch = 0; ch < channelsCount; ch++)
                    {
                        int srcIndex = ch * sampleCount + s;
                        float value = blockChannelMajor[srcIndex];

                        float y2;
                        if (!TryGetPageY(ch, channelsCount, value, out y2))
                        {
                            _prevPoints[ch].HasValue = false;
                            continue;
                        }

                        PrevPoint prev = _prevPoints[ch];

                        if (prev.HasValue && prev.AbsoluteSample == abs - 1)
                        {
                            long prevSeq = prev.AbsoluteSample / _samplesPerChannel;

                            if (prevSeq == seq)
                            {
                                GL.LineWidth(GetChannelLineWidth(ch));
                                GL.Color3(GetChannelColor(ch));
                                GL.Begin(PrimitiveType.Lines);
                                GL.Vertex2(prev.X, prev.Y);
                                GL.Vertex2(x2, y2);
                                GL.End();
                            }
                            else if (prevSeq == seq - 1)
                            {
                                int prevSlot = (int)(prevSeq & 1L);

                                if (_signalSeq[prevSlot] == prevSeq)
                                {
                                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[prevSlot]);
                                    SetupPageViewport();
                                    GL.Disable(EnableCap.Texture2D);

                                    GL.LineWidth(GetChannelLineWidth(ch));
                                    GL.Color3(GetChannelColor(ch));
                                    GL.Begin(PrimitiveType.Lines);
                                    GL.Vertex2(prev.X, prev.Y);
                                    GL.Vertex2(_pageW - 1.0f, y2);
                                    GL.End();

                                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[slot]);
                                    SetupPageViewport();
                                    GL.Disable(EnableCap.Texture2D);

                                    boundSlot = slot;
                                }
                            }
                        }

                        prev.HasValue = true;
                        prev.X = x2;
                        prev.Y = y2;
                        prev.AbsoluteSample = abs;
                        _prevPoints[ch] = prev;
                    }
                }
            }
            finally
            {
                GL.LineWidth(1.0f);

                if (boundSlot != -1)
                    GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

                SetupViewport();
                GL.Enable(EnableCap.Texture2D);
            }
        }

        private void RenderLiveEventToPages(long absoluteSample, int classId, int state)
        {
            if (_samplesPerChannel <= 1 || _pageW <= 1)
                return;

            long seq = absoluteSample / _samplesPerChannel;
            int slot = (int)(seq & 1L);

            if (_signalSeq[slot] != seq)
                return;

            float x = GetPageX(absoluteSample);
            DrawEventMarkerToSignalPage(slot, x, classId, state);
            lock (_sync)
            {
                _drawnEvents.RemoveAll(evt => evt.AbsoluteSample < (seq - 1) * _samplesPerChannel);
                _drawnEvents.Add(new PendingEvent { AbsoluteSample = absoluteSample, ClassId = classId, State = state });
            }
        }

        private void DrawEventMarkerToSignalPage(int slot, float x, int classId, int state)
        {
            if (slot < 0 || slot >= _signalFbo.Length)
                return;

            if (_signalFbo[slot] == 0 || _pageW <= 1 || _pageH <= 1)
                return;

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[slot]);
            SetupPageViewport();
            DrawEventMarkerCore((float)Math.Round(x), 0.0f, _pageH, 0.0f, _pageW, classId, state);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            SetupViewport();
            GL.Enable(EnableCap.Texture2D);
        }

        private static void DrawEventMarkerCore(float x, float top, float bottom,
            float clipLeft, float clipRight, int classId, int state)
        {
            if (clipRight <= clipLeft) return;
            Color color = state == 1 ? Color.Yellow : Color.OrangeRed;
            float lineLeft = Math.Max(clipLeft, x - 0.5f);
            float lineRight = Math.Min(clipRight, x + 1.5f);
            if (lineRight <= lineLeft) return;
            GL.Disable(EnableCap.Texture2D);
            GL.Color3(color);
            GL.Begin(PrimitiveType.Quads);
            GL.Vertex2(lineLeft, top); GL.Vertex2(lineRight, top);
            GL.Vertex2(lineRight, bottom); GL.Vertex2(lineLeft, bottom);
            GL.End();
            DrawEventLabelCore(classId.ToString(), lineLeft - 8.0f, top + 2.0f, clipLeft, clipRight, color);
        }

        internal static void DrawEventLabelCore(
            string text,
            float rightAnchor,
            float top,
            float clipLeft,
            float clipRight,
            Color color)
        {
            if (string.IsNullOrEmpty(text))
                return;

            using (Bitmap bmp = new Bitmap(text == "Rejected" ? 72 : text.Length > 3 ? 112 : 40, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font("Consolas", 10.0f, FontStyle.Bold, GraphicsUnit.Point))
            using (Brush brush = new SolidBrush(color))
            using (StringFormat sf = new StringFormat())
            {
                g.Clear(Color.Transparent);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                g.DrawString(text, font, brush, new RectangleF(0, 0, bmp.Width, bmp.Height), sf);

                int tex = 0;

                try
                {
                    tex = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, tex);

                    BitmapData data = bmp.LockBits(
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    try
                    {
                        GL.TexImage2D(
                            TextureTarget.Texture2D,
                            0,
                            PixelInternalFormat.Rgba,
                            bmp.Width,
                            bmp.Height,
                            0,
                            OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                            PixelType.UnsignedByte,
                            data.Scan0);
                    }
                    finally
                    {
                        bmp.UnlockBits(data);
                    }

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

                    float w = bmp.Width;
                    float h = bmp.Height;

                    float right = (float)Math.Round(rightAnchor);
                    float left = right - w;

                    if (left < clipLeft)
                    {
                        left = clipLeft;
                        right = left + w;
                    }

                    if (right > clipRight)
                    {
                        right = clipRight;
                        left = right - w;

                        if (left < clipLeft)
                            left = clipLeft;
                    }

                    top = (float)Math.Round(top);
                    float bottom = top + h;

                    GL.Enable(EnableCap.Texture2D);
                    GL.BindTexture(TextureTarget.Texture2D, tex);
                    GL.Color4(1f, 1f, 1f, 1f);

                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0f, 0f); GL.Vertex2(left, top);
                    GL.TexCoord2(1f, 0f); GL.Vertex2(right, top);
                    GL.TexCoord2(1f, 1f); GL.Vertex2(right, bottom);
                    GL.TexCoord2(0f, 1f); GL.Vertex2(left, bottom);
                    GL.End();

                    GL.BindTexture(TextureTarget.Texture2D, 0);
                    GL.Disable(EnableCap.Texture2D);
                }
                finally
                {
                    if (tex != 0)
                        GL.DeleteTexture(tex);
                }
            }
        }

        private long GetLiveRightEdgeSample()
        {
            if (_samplesPerChannel <= 1 || _pageW <= 1)
                return 0;

            float pageSpanPx = _pageW - 1.0f;
            if (pageSpanPx <= 0.0f)
                return 0;

            double localSample =
                (double)_pageShiftPx * (_samplesPerChannel - 1) / pageSpanPx;

            long rightLocalSample = (long)Math.Round(localSample);

            if (rightLocalSample < 0)
                rightLocalSample = 0;
            if (rightLocalSample > _samplesPerChannel - 1)
                rightLocalSample = _samplesPerChannel - 1;

            return _visualPageSeq * (long)_samplesPerChannel + rightLocalSample;
        }

        private void SetLiveRightEdgeSample(long absoluteSample)
        {
            if (_samplesPerChannel <= 1 || _pageW <= 1)
            {
                _visualPageSeq = 0;
                _pageShiftPx = 0.0f;
                return;
            }

            if (absoluteSample < 0)
                absoluteSample = 0;

            long seq = absoluteSample / _samplesPerChannel;
            long localSample = absoluteSample % _samplesPerChannel;

            float pageSpanPx = _pageW - 1.0f;
            float shiftPx = (float)((double)localSample * pageSpanPx / (_samplesPerChannel - 1));

            if (shiftPx >= pageSpanPx)
            {
                shiftPx -= pageSpanPx;
                seq++;
            }

            _visualPageSeq = seq;
            _pageShiftPx = shiftPx;
        }

        private void ClampLivePageShiftToLatestData()
        {
            long lastDataEndSample;

            lock (_sync)
            {
                if (_totalReceivedSamples <= 0)
                    return;

                lastDataEndSample = _lastDataEndSample;
            }

            long liveRightEdgeSample = GetLiveRightEdgeSample();
            long deltaSamples = liveRightEdgeSample - lastDataEndSample;

            // Допуск в несколько сэмплов, чтобы не было мелкой дрожи от постоянного
            // микроподравнивания. Примерно около 2 пикселей по оси X.
            long syncToleranceSamples = 0;
            if (_pageW > 1 && _samplesPerChannel > 1)
            {
                syncToleranceSamples = (long)Math.Ceiling(
                    2.0 * (_samplesPerChannel - 1) / (_pageW - 1.0));
            }

            if (syncToleranceSamples < 1)
                syncToleranceSamples = 1;

            // Экран убежал вперёд относительно последних данных.
            if (deltaSamples > syncToleranceSamples)
            {
                SetLiveRightEdgeSample(lastDataEndSample);
                return;
            }

            // Экран отстал от последних данных.
            if (deltaSamples < -syncToleranceSamples)
            {
                SetLiveRightEdgeSample(lastDataEndSample);
            }
        }

        private void AdvanceLivePageShift(int ticksCount)
        {
            if (ticksCount <= 0)
                return;

            float pageSpanPx = _pageW > 1 ? (_pageW - 1.0f) : 1.0f;

            _pageShiftPx += _pageDxPx * ticksCount;

            while (_pageShiftPx >= pageSpanPx)
            {
                _pageShiftPx -= pageSpanPx;
                _visualPageSeq++;
            }
        }

        // -------------------------------------------------
        // Channel selection helpers
        // -------------------------------------------------
        private void ResetChannelSelection_NoLock()
        {
            _selectedChannels = new bool[Math.Max(_channelsCount, 0)];
            _showSelectedOnly = false;
            RebuildVisibleChannels_NoLock();
        }

        private void NormalizeChannelSelection_NoLock()
        {
            int count = Math.Max(_channelsCount, 0);

            if (_selectedChannels == null || _selectedChannels.Length != count)
            {
                bool[] normalized = new bool[count];

                if (_selectedChannels != null)
                    Array.Copy(_selectedChannels, normalized, Math.Min(_selectedChannels.Length, normalized.Length));

                _selectedChannels = normalized;
            }

            if (_showSelectedOnly && GetSelectedChannelCount_NoLock() == 0)
                _showSelectedOnly = false;

            RebuildVisibleChannels_NoLock();
        }

        private void RebuildVisibleChannels_NoLock()
        {
            int channelsCount = Math.Max(_channelsCount, 0);
            int selectedCount = GetSelectedChannelCount_NoLock();
            bool selectedOnly = _showSelectedOnly && selectedCount > 0;

            int visibleCount = selectedOnly ? selectedCount : channelsCount;
            int[] visible = new int[visibleCount];
            int[] map = new int[channelsCount];

            for (int i = 0; i < map.Length; i++)
                map[i] = -1;

            int visibleIndex = 0;

            for (int ch = 0; ch < channelsCount; ch++)
            {
                if (selectedOnly && !IsChannelSelected_NoLock(ch))
                    continue;

                visible[visibleIndex] = ch;
                map[ch] = visibleIndex;
                visibleIndex++;
            }

            _visibleChannels = visible;
            _channelToVisibleIndex = map;
        }

        private int GetSelectedChannelCount_NoLock()
        {
            int count = 0;

            if (_selectedChannels == null)
                return 0;

            for (int i = 0; i < _selectedChannels.Length; i++)
            {
                if (_selectedChannels[i])
                    count++;
            }

            return count;
        }

        private bool IsChannelSelected(int channelIndex)
        {
            bool[] selected = _selectedChannels;

            return selected != null &&
                   channelIndex >= 0 &&
                   channelIndex < selected.Length &&
                   selected[channelIndex];
        }

        private bool IsChannelSelected_NoLock(int channelIndex)
        {
            return _selectedChannels != null &&
                   channelIndex >= 0 &&
                   channelIndex < _selectedChannels.Length &&
                   _selectedChannels[channelIndex];
        }

        private float GetChannelLineWidth(int channelIndex)
        {
            return !_showSelectedOnly && IsChannelSelected(channelIndex)
                ? 3.5f
                : 1.0f;
        }

        private bool TryGetPageY(int channelIndex, int channelsCount, float valueVolts, out float y)
        {
            y = 0.0f;

            if (channelsCount <= 0)
                return false;

            int visibleIndex = channelIndex;
            int visibleCount = channelsCount;

            int[] map = _channelToVisibleIndex;
            int[] visible = _visibleChannels;

            if (channelsCount == _channelsCount &&
                map != null && map.Length == channelsCount &&
                visible != null && visible.Length > 0)
            {
                if (channelIndex < 0 || channelIndex >= map.Length)
                    return false;

                visibleIndex = map[channelIndex];
                visibleCount = visible.Length;

                if (visibleIndex < 0)
                    return false;
            }

            float bandHeight = (float)_pageH / visibleCount;

            float scaleVolts = _scaleVolts;
            if (scaleVolts <= 0.0f)
                scaleVolts = 100e-6f;

            float pixelsPerVolt = (bandHeight * 0.35f) / scaleVolts;
            float yBase = (visibleIndex + 0.5f) * bandHeight;

            y = yBase - valueVolts * pixelsPerVolt;
            return true;
        }

        // -------------------------------------------------
        // Pending queues helpers
        // -------------------------------------------------
        private void SwapPendingBlocks()
        {
            lock (_sync)
            {
                List<PendingBlock> tmp = _pendingBlocksUi;
                _pendingBlocksUi = _pendingBlocks;
                _pendingBlocks = tmp;
            }
        }

        private void SwapPendingEvents()
        {
            lock (_sync)
            {
                List<PendingEvent> tmp = _pendingEventsUi;
                _pendingEventsUi = _pendingEvents;
                _pendingEvents = tmp;
            }
        }

        private void ClearPendingBlocks()
        {
            lock (_sync)
            {
                ClearPendingBlocks_NoLock();
            }
        }

        private void ClearPendingEvents()
        {
            lock (_sync)
            {
                ClearPendingEvents_NoLock();
            }
        }

        private void ClearPendingBlocks_NoLock()
        {
            _pendingBlocks.Clear();
            _pendingBlocksUi.Clear();
        }

        private void ClearPendingEvents_NoLock()
        {
            _rejectedIntervals.Clear();
            _drawnEvents.Clear();
            _pendingEvents.Clear();
            _pendingEventsUi.Clear();
        }

        private void UpdatePageDxPx()
        {
            float pageSpanPx = _pageW > 1 ? (_pageW - 1.0f) : 1.0f;

            float ticksPerScreen = (_timeBaseSeconds * 1000.0f) / _uiTimerPeriodMs;
            if (ticksPerScreen < 1.0f)
                ticksPerScreen = 1.0f;

            _pageDxPx = pageSpanPx / ticksPerScreen;
        }

        // -------------------------------------------------
        // Signal pages
        // -------------------------------------------------
        private void DestroySignalPages()
        {
            if (!_glInitialized)
                return;

            for (int i = 0; i < 2; i++)
            {
                if (_signalFbo[i] != 0)
                {
                    GL.DeleteFramebuffer(_signalFbo[i]);
                    _signalFbo[i] = 0;
                }

                if (_signalTex[i] != 0)
                {
                    GL.DeleteTexture(_signalTex[i]);
                    _signalTex[i] = 0;
                }

                _signalSeq[i] = long.MinValue;
            }

            _pageW = 0;
            _pageH = 0;

            ResetPrevPoints();
        }

        private void ClearSignalPage(int slot)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[slot]);
            SetupPageViewport();

            GL.Disable(EnableCap.Texture2D);
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            SetupViewport();
            GL.ClearColor(Color.Black);

            _signalSeq[slot] = long.MinValue;
        }

        private void CreateSignalPage(int slot)
        {
            _signalTex[slot] = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, _signalTex[slot]);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 0);

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba8,
                _pageW,
                _pageH,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Rgba,
                PixelType.UnsignedByte,
                IntPtr.Zero);

            _signalFbo[slot] = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _signalFbo[slot]);

            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                _signalTex[slot],
                0);

            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            GL.ReadBuffer(ReadBufferMode.ColorAttachment0);

            FramebufferErrorCode status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);

            if (status != FramebufferErrorCode.FramebufferComplete)
                throw new InvalidOperationException("Signal page FBO is incomplete: " + status.ToString());

            ClearSignalPage(slot);
        }

        private void RecreateSignalPages()
        {
            RectangleF rect = GetPlotRectangle();

            int newPageW = GetPreviewStaticPageWidthPx(rect);
            int newPageH = Math.Max(1, (int)Math.Ceiling(rect.Height));

            float oldSpan = _pageW > 1 ? _pageW - 1.0f : 1.0f;
            float pageFraction = _pageShiftPx / oldSpan;
            DestroySignalPages();

            _pageW = newPageW;
            _pageH = newPageH;
            _pageShiftPx = pageFraction * Math.Max(1.0f, _pageW - 1.0f);
            _signalPagesNeedRedraw = true;

            for (int i = 0; i < 2; i++)
                CreateSignalPage(i);

            EnsurePrevPoints();
            ResetPrevPoints();
            UpdatePageDxPx();
        }

        private int GetCurrentPageWidthPx(RectangleF rect)
        {
            if (_staticPageMode)
                return GetPreviewStaticPageWidthPx(rect);

            return Math.Max(1, (int)Math.Ceiling(rect.Width));
        }

        private void EnsureSignalPages()
        {
            EnsureGlInitialized();
            if (!_glInitialized) return;
            RectangleF rect = GetPlotRectangle();
            int w = GetCurrentPageWidthPx(rect);
            int h = Math.Max(1, (int)Math.Ceiling(rect.Height));

            bool needRecreate =
                _signalTex[0] == 0 || _signalTex[1] == 0 ||
                _signalFbo[0] == 0 || _signalFbo[1] == 0 ||
                _pageW != w || _pageH != h;

            if (needRecreate)
                RecreateSignalPages();
            RedrawCachedSignalPages();
        }

        private void EnsurePrevPoints()
        {
            int channelsCount = _channelsCount;
            if (channelsCount <= 0)
                channelsCount = 1;

            if (_prevPoints != null && _prevPoints.Length == channelsCount)
                return;

            _prevPoints = new PrevPoint[channelsCount];
            ResetPrevPoints();
        }

        private void ResetPrevPoints()
        {
            if (_prevPoints == null)
                return;

            for (int i = 0; i < _prevPoints.Length; i++)
            {
                _prevPoints[i].HasValue = false;
                _prevPoints[i].X = 0.0f;
                _prevPoints[i].Y = 0.0f;
                _prevPoints[i].AbsoluteSample = long.MinValue;
            }
        }

        private float GetPageX(long absoluteSample)
        {
            if (_samplesPerChannel <= 1 || _pageW <= 1)
                return 0.0f;

            long local = absoluteSample % _samplesPerChannel;
            if (local < 0)
                local += _samplesPerChannel;

            return (float)local * (_pageW - 1) / (float)(_samplesPerChannel - 1);
        }

        // -------------------------------------------------
        // Frame render
        // -------------------------------------------------
        private void RenderFrame()
        {
            if (!_glInitialized)
                EnsureGlInitialized();

            if (!_glInitialized || _glCtrl.IsDisposed || !_glCtrl.IsHandleCreated)
                return;

            _glCtrl.MakeCurrent();

            EnsureSignalPages();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            SetupViewport();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            DrawSignalPages();
            DrawOverlayTexture();
            DrawHeogMarkerOverlay();
            DrawHeogEpochLabels();
            DrawTimeCursor();

            _glCtrl.SwapBuffers();
        }

        private void BeginPlotScissor(RectangleF rect)
        {
            int x = Math.Max((int)Math.Floor(rect.Left), 0);
            int yTop = Math.Max((int)Math.Floor(rect.Top), 0);
            int width = Math.Max((int)Math.Ceiling(rect.Width), 1);
            int height = Math.Max((int)Math.Ceiling(rect.Height), 1);

            int glY = _glCtrl.Height - (yTop + height);
            if (glY < 0)
                glY = 0;

            GL.Enable(EnableCap.ScissorTest);
            GL.Scissor(x, glY, width, height);
        }

        private void EndPlotScissor()
        {
            GL.Disable(EnableCap.ScissorTest);
        }

        private void DrawSignalPage(long pageSequence, float left, float top)
        {
            if (pageSequence < 0)
                return;

            int slot = (int)(pageSequence & 1L);
            if (_signalTex[slot] == 0 || _signalSeq[slot] != pageSequence)
                return;

            float right = left + _pageW;
            float bottom = top + _pageH;

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _signalTex[slot]);
            GL.Color4(1f, 1f, 1f, 1f);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0f, 1f); GL.Vertex2(left, top);
            GL.TexCoord2(1f, 1f); GL.Vertex2(right, top);
            GL.TexCoord2(1f, 0f); GL.Vertex2(right, bottom);
            GL.TexCoord2(0f, 0f); GL.Vertex2(left, bottom);
            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void DrawSignalPages()
        {
            if (_signalTex[0] == 0 || _pageW <= 1)
                return;

            RectangleF rect = GetPlotRectangle();

            BeginPlotScissor(rect);

            if (_staticPageMode)
            {
                float pageSpanPx = _pageW > 1 ? (_pageW - 1.0f) : 1.0f;

                float rightOffsetPx = 0.0f;
                if (_staticPageSamplesCount > 1)
                {
                    rightOffsetPx =
                        (float)_staticViewRightPageSample * pageSpanPx /
                        (float)(_staticPageSamplesCount - 1);
                }

                float pageLeft = rect.Right - rightOffsetPx;

                DrawSignalPage(0, pageLeft, rect.Top);
                DrawAnalysisIntervals(0, pageLeft, rect);
                DrawRejectedIntervals(0, pageLeft, rect);
            }
            else
            {
                if (_signalTex[1] != 0)
                {
                    float pageSpanPx = _pageW - 1.0f;

                    float currentLeft = rect.Right - _pageShiftPx;
                    float previousLeft = currentLeft - pageSpanPx;

                    DrawSignalPage(_visualPageSeq - 1, previousLeft, rect.Top);
                    DrawSignalPage(_visualPageSeq, currentLeft, rect.Top);
                    DrawAnalysisIntervals(_visualPageSeq - 1, previousLeft, rect);
                    DrawAnalysisIntervals(_visualPageSeq, currentLeft, rect);
                    DrawRejectedIntervals(_visualPageSeq - 1, previousLeft, rect);
                    DrawRejectedIntervals(_visualPageSeq, currentLeft, rect);
                }
            }

            EndPlotScissor();
        }

        // Draw gaze markers after the grid so a second tick cannot hide a red line.
        private void DrawHeogMarkerOverlay()
        {
            RectangleF rect = GetPlotRectangle();
            BeginPlotScissor(rect);
            if (_staticPageMode)
            {
                float offset = _staticPageSamplesCount > 1
                    ? (float)_staticViewRightPageSample * (_pageW - 1.0f) / (_staticPageSamplesCount - 1)
                    : 0.0f;
                DrawHeogMarkers(0, rect.Right - offset, rect);
            }
            else
            {
                float left = rect.Right - _pageShiftPx;
                DrawHeogMarkers(_visualPageSeq - 1, left - (_pageW - 1.0f), rect);
                DrawHeogMarkers(_visualPageSeq, left, rect);
            }
            EndPlotScissor();
        }

        private void DrawHeogEpochLabels()
        {
            _heogEpochLabels.Clear();
            RectangleF plot = GetPlotRectangle();
            Tuple<long, long, HeogMarkupInterval>[] epochs;
            long availableEnd;
            lock (_sync)
            {
                epochs = _staticPageMode ? _previewHeogEpochs : _playbackHeogEpochs;
                availableEnd = _staticPageMode ? _staticPageSamplesCount : _totalReceivedSamples;
            }
            if (epochs.Length == 0 || _pageW <= 1 ||
                (_staticPageMode ? _staticPageSamplesCount : _samplesPerChannel) <= 1 ||
                plot.Width < HeogLabelWidth || plot.Height < HeogLabelHeight + 22) return;
            float sampleWidth = (_pageW - 1.0f) /
                (_staticPageMode ? _staticPageSamplesCount - 1 : _samplesPerChannel - 1);
            _heogLabelRowEnds.Clear();
            foreach (var epoch in epochs)
            {
                if (epoch.Item3.TargetLabel < 0) continue;
                long last = Math.Min(epoch.Item2, availableEnd);
                if (last <= epoch.Item1) continue;
                float left = Math.Max(plot.Left, GetHeogSampleScreenX(epoch.Item1, plot));
                float right = Math.Min(plot.Right, GetHeogSampleScreenX(last - 1, plot) + sampleWidth);
                if (right <= left) continue;
                int x = (int)Math.Round(Math.Max(plot.Left, Math.Min(plot.Right - HeogLabelWidth,
                    (left + right - HeogLabelWidth) / 2)));
                // Stagger nearby labels; the underlying fixation boundaries stay exact.
                int row = 0;
                while (row < _heogLabelRowEnds.Count && x < _heogLabelRowEnds[row] + 2) row++;
                int top = (int)plot.Top + 22 + row * (HeogLabelHeight + 2);
                if (top + HeogLabelHeight > plot.Bottom) continue;
                if (row == _heogLabelRowEnds.Count) _heogLabelRowEnds.Add(x + HeogLabelWidth);
                else _heogLabelRowEnds[row] = x + HeogLabelWidth;
                var bounds = new Rectangle(x, top, HeogLabelWidth, HeogLabelHeight);
                _heogEpochLabels.Add(Tuple.Create(bounds, epoch.Item3));
            }
            if (_heogEpochLabels.Count == 0) return;
            EnsureHeogLabelTexture();
            BeginPlotScissor(plot);
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _heogLabelTexture);
            GL.Color4(1f, 1f, 1f, 1f);
            GL.Begin(PrimitiveType.Quads);
            foreach (var label in _heogEpochLabels)
            {
                Rectangle box = label.Item1;
                int number = Math.Max(0, Math.Min(HeogMarkup.MaximumTargetLabel, label.Item2.TargetLabel));
                float u1 = number / (float)(HeogMarkup.MaximumTargetLabel + 1);
                float u2 = (number + 1) / (float)(HeogMarkup.MaximumTargetLabel + 1);
                GL.TexCoord2(u1, 0f); GL.Vertex2(box.Left, box.Top);
                GL.TexCoord2(u2, 0f); GL.Vertex2(box.Right, box.Top);
                GL.TexCoord2(u2, 1f); GL.Vertex2(box.Right, box.Bottom);
                GL.TexCoord2(u1, 1f); GL.Vertex2(box.Left, box.Bottom);
            }
            GL.End();
            GL.BindTexture(TextureTarget.Texture2D, 0);
            EndPlotScissor();
        }

        private float GetHeogSampleScreenX(long sample, RectangleF plot)
        {
            if (_staticPageMode)
                return plot.Right - GetPreviewStaticPageX(_staticViewRightPageSample) + GetPreviewStaticPageX(sample);
            long sequence = sample / _samplesPerChannel;
            return plot.Right - _pageShiftPx + (sequence - _visualPageSeq) * (_pageW - 1.0f) + GetPageX(sample);
        }

        // One small atlas is reused on every frame; editing a number allocates no GL resources.
        private void EnsureHeogLabelTexture()
        {
            if (_heogLabelTexture != 0) return;
            using (var bitmap = new Bitmap((HeogMarkup.MaximumTargetLabel + 1) * HeogLabelWidth, HeogLabelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (var font = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var text = new SolidBrush(Color.FromArgb(25, 100, 50)))
            using (var border = new Pen(Color.FromArgb(115, 175, 130)))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.FromArgb(225, 243, 229));
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                for (int number = 0; number <= HeogMarkup.MaximumTargetLabel; number++)
                {
                    var box = new Rectangle(number * HeogLabelWidth, 0, HeogLabelWidth - 1, HeogLabelHeight - 1);
                    graphics.DrawRectangle(border, box);
                    graphics.DrawString(number.ToString(), font, text, box, format);
                }
                _heogLabelTexture = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, _heogLabelTexture);
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                try
                {
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0,
                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                }
                finally { bitmap.UnlockBits(data); }
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        private RectangleF GetPlotRectangle()
        {
            float width = _glCtrl.Width - PlotLeft - PlotRight;
            float height = _glCtrl.Height - PlotTop - PlotBottom;

            if (width < 10.0f) width = 10.0f;
            if (height < 10.0f) height = 10.0f;

            return new RectangleF(PlotLeft, PlotTop, width, height);
        }

        // -------------------------------------------------
        // Overlay
        // -------------------------------------------------
        private void InvalidateOverlay()
        {
            _overlayDirty = true;
        }

        private Bitmap BuildOverlayBitmap()
        {
            int w = Math.Max(_glCtrl.Width, 1);
            int h = Math.Max(_glCtrl.Height, 1);

            Bitmap bmp = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            using (Font font = new Font("Segoe UI", 8.0f, FontStyle.Regular, GraphicsUnit.Point))
            using (Font selectedFont = new Font("Segoe UI", 8.0f, FontStyle.Bold, GraphicsUnit.Point))
            //using (Pen borderPen = new Pen(Color.FromArgb(70, 70, 70), 1.0f))
            using (Pen borderPen = new Pen(Color.FromArgb(45, 45, 45), 1.0f))
            //using (Pen gridPen = new Pen(Color.FromArgb(45, 45, 45), 1.0f))
            using (Pen gridPen = new Pen(Color.FromArgb(22, 22, 22), 1.0f))
            //using (Pen baselinePen = new Pen(Color.FromArgb(35, 35, 35), 1.0f))
            using (Pen baselinePen = new Pen(Color.FromArgb(18, 18, 18), 1.0f))
            using (Brush brushChannels = new SolidBrush(Color.LightGray))
            using (Brush brushSelectedChannels = new SolidBrush(Color.White))
            using (Brush brushTime = new SolidBrush(Color.Gray))
            using (StringFormat sfCenter = new StringFormat())
            {
                g.Clear(Color.Transparent);

                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                sfCenter.Alignment = StringAlignment.Center;
                sfCenter.LineAlignment = StringAlignment.Near;

                RectangleF rect = GetPlotRectangle();

                g.DrawRectangle(borderPen, rect.X, rect.Y, rect.Width, rect.Height);

                for (int sec = 0; sec <= _timeBaseSeconds; sec++)
                {
                    float x = rect.Left + rect.Width * sec / _timeBaseSeconds;
                    g.DrawLine(gridPen, x, rect.Top, x, rect.Bottom);
                }

                int[] visibleChannels = _visibleChannels;

                if (visibleChannels != null && visibleChannels.Length > 0)
                {
                    float bandHeight = rect.Height / visibleChannels.Length;

                    for (int visibleIndex = 0; visibleIndex < visibleChannels.Length; visibleIndex++)
                    {
                        int ch = visibleChannels[visibleIndex];
                        float y = rect.Top + bandHeight * (visibleIndex + 0.5f);
                        g.DrawLine(baselinePen, rect.Left, y, rect.Right, y);

                        string channelName = "Ch " + (ch + 1).ToString();

                        if (_inputStream != null &&
                            ch < _inputStream.ChannelNames.Length &&
                            !string.IsNullOrWhiteSpace(
                                _inputStream.ChannelNames[ch]))
                        {
                            channelName = _inputStream.ChannelNames[ch];
                        }

                        bool selected = IsChannelSelected(ch);
                        Font channelFont = selected ? selectedFont : font;
                        Brush channelBrush = selected ? brushSelectedChannels : brushChannels;

                        float yBase = rect.Top + (visibleIndex + 0.5f) * bandHeight;

                        g.DrawString(
                            channelName,
                            channelFont,
                            channelBrush,
                            4.0f,
                            (float)Math.Round(yBase - channelFont.Height * 0.5f));
                    }
                }

                for (int sec2 = 0; sec2 <= _timeBaseSeconds; sec2++)
                {
                    float x2 = rect.Left + rect.Width * sec2 / _timeBaseSeconds;
                    string text = (_timeBaseSeconds - sec2).ToString();

                    RectangleF textRect = new RectangleF(
                        x2 - 20.0f,
                        rect.Bottom + 2.0f,
                        40.0f,
                        font.Height + 2.0f);

                    g.DrawString(text, font, brushTime, textRect, sfCenter);
                }

                string scaleText = "Scale";
                g.DrawString(
                    scaleText,
                    font,
                    brushTime,
                    (float)Math.Round(rect.Right - 40.0f),
                    2.0f);
            }

            return bmp;
        }

        private void RebuildOverlayTexture()
        {
            if (!_glInitialized)
                return;

            if (_overlayBitmap != null)
            {
                _overlayBitmap.Dispose();
                _overlayBitmap = null;
            }

            _overlayBitmap = BuildOverlayBitmap();

            if (_overlayTextureId == 0)
                _overlayTextureId = GL.GenTexture();

            GL.BindTexture(TextureTarget.Texture2D, _overlayTextureId);

            BitmapData data = _overlayBitmap.LockBits(
                new Rectangle(0, 0, _overlayBitmap.Width, _overlayBitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            try
            {
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    _overlayBitmap.Width,
                    _overlayBitmap.Height,
                    0,
                    OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }
            finally
            {
                _overlayBitmap.UnlockBits(data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            _overlayDirty = false;
        }

        private void DrawOverlayTexture()
        {
            if (_overlayDirty || _overlayTextureId == 0)
                RebuildOverlayTexture();

            if (_overlayTextureId == 0)
                return;

            int w = Math.Max(_glCtrl.Width, 1);
            int h = Math.Max(_glCtrl.Height, 1);

            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, _overlayTextureId);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);

            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(w, 0.0f);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(w, h);
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(0.0f, h);

            GL.End();

            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        internal static Color GetChannelColor(int ch)
        {
            Color[] colors = new Color[]
            {
                Color.Lime,
                Color.Cyan,
                Color.Yellow,
                Color.Orange,
                Color.Magenta,
                Color.DeepSkyBlue,
                Color.LightGreen,
                Color.White
            };

            return colors[ch % colors.Length];
        }

        // -------------------------------------------------
        // Dispose
        // -------------------------------------------------
        public void Dispose()
        {
            DetachInput();

            _glCtrl.Paint -= glCtrl_Paint;
            _glCtrl.Resize -= glCtrl_Resize;
            _glCtrl.MouseClick -= glCtrl_MouseClick;
            _glCtrl.MouseDoubleClick -= glCtrl_MouseDoubleClick;

            if (_overlayTextureId != 0 && _glInitialized)
            {
                try
                {
                    _glCtrl.MakeCurrent();
                    GL.DeleteTexture(_overlayTextureId);
                    if (_heogLabelTexture != 0) GL.DeleteTexture(_heogLabelTexture);
                    _heogLabelTexture = 0;
                    _overlayTextureId = 0;
                }
                catch
                {
                }
            }

            if (_overlayBitmap != null)
            {
                _overlayBitmap.Dispose();
                _overlayBitmap = null;
            }

            if (_glInitialized)
            {
                try
                {
                    _glCtrl.MakeCurrent();
                    DestroySignalPages();
                }
                catch
                {
                }
            }
        }

        // GPU pages can be destroyed by resize/context initialization. Keep their
        // source samples so changing the viewport never requires new EEG input.
        private readonly Queue<PendingBlock> _renderedSignalBlocks = new Queue<PendingBlock>();
        private readonly List<PendingBlock> _staticSignalBlocks = new List<PendingBlock>();
        private bool _signalPagesNeedRedraw = true;

        private static PendingBlock CopySignalBlock(float[] data, int channels, int count,
            long firstSample, int skip = 0)
        {
            int retained = count - skip;
            var samples = new float[channels * retained];
            for (int channel = 0; channel < channels; channel++)
                Array.Copy(data, channel * count + skip, samples, channel * retained, retained);
            return new PendingBlock
            {
                Samples = samples, ChannelsCount = channels, SamplesCount = retained,
                FirstAbsoluteSample = firstSample + skip
            };
        }

        private void CacheRenderedSignalBlock(PendingBlock block)
        {
            lock (_sync)
            {
                long end = block.FirstAbsoluteSample + block.SamplesCount;
                long firstRetained = Math.Max(0, (end - 1) / _samplesPerChannel - 1) * _samplesPerChannel;
                while (_renderedSignalBlocks.Count > 0)
                {
                    PendingBlock first = _renderedSignalBlocks.Peek();
                    if (first.FirstAbsoluteSample + first.SamplesCount > firstRetained) break;
                    _renderedSignalBlocks.Dequeue();
                }
                int skip = (int)Math.Max(0, firstRetained - block.FirstAbsoluteSample);
                _renderedSignalBlocks.Enqueue(CopySignalBlock(block.Samples, block.ChannelsCount,
                    block.SamplesCount, block.FirstAbsoluteSample, skip));
            }
        }

        private void RebasePlaybackMarkup_NoLock(long firstSample)
        {
            if (firstSample <= 0) return;
            _playbackMarkupSampleOffset += firstSample;
            _playbackHeogMarkers = ShiftHeogMarkers(_playbackHeogMarkers, firstSample);
            _playbackAnalysisIntervals = ShiftMarkupIntervals(_playbackAnalysisIntervals, firstSample);
            _playbackHeogEpochs = ShiftHeogEpochs(_playbackHeogEpochs, firstSample);
        }

        private static Tuple<long, long, HeogMarkupInterval>[] ShiftHeogEpochs(
            Tuple<long, long, HeogMarkupInterval>[] epochs, long firstSample)
        {
            var remaining = new List<Tuple<long, long, HeogMarkupInterval>>();
            foreach (var epoch in epochs)
                if (epoch.Item2 > firstSample)
                    remaining.Add(Tuple.Create(Math.Max(0, epoch.Item1 - firstSample), epoch.Item2 - firstSample, epoch.Item3));
            return remaining.ToArray();
        }

        private static long[] ShiftHeogMarkers(long[] markers, long firstSample)
        {
            var remaining = new List<long>();
            foreach (long sample in markers)
                if (sample >= firstSample) remaining.Add(sample - firstSample);
            return remaining.ToArray();
        }

        private static Tuple<long, long, bool>[] ShiftMarkupIntervals(Tuple<long, long, bool>[] intervals, long firstSample)
        {
            var remaining = new List<Tuple<long, long, bool>>();
            foreach (var interval in intervals)
                if (interval.Item2 > firstSample)
                    remaining.Add(Tuple.Create(Math.Max(0, interval.Item1 - firstSample),
                        interval.Item2 - firstSample, interval.Item3));
            return remaining.ToArray();
        }

        private void ClearSignalHistory_NoLock()
        {
            _renderedSignalBlocks.Clear();
            _staticSignalBlocks.Clear();
            _signalPagesNeedRedraw = true;
        }

        private void RedrawCachedSignalPages()
        {
            PendingBlock[] blocks;
            PendingEvent[] events;
            lock (_sync)
            {
                if (!_signalPagesNeedRedraw) return;
                _signalPagesNeedRedraw = false;
                blocks = _staticPageMode ? _staticSignalBlocks.ToArray() : _renderedSignalBlocks.ToArray();
                events = _drawnEvents.ToArray();
            }
            ResetPrevPoints();
            foreach (PendingBlock block in blocks)
            {
                if (_staticPageMode)
                    RenderPreviewStaticBlock(block.Samples, block.ChannelsCount,
                        block.SamplesCount, block.FirstAbsoluteSample);
                else
                    RenderLiveBlockToPages(block.Samples, block.ChannelsCount,
                        block.SamplesCount, block.FirstAbsoluteSample);
            }
            foreach (PendingEvent marker in events)
            {
                if (_staticPageMode)
                    DrawEventMarkerToSignalPage(0, GetPreviewStaticPageX(marker.AbsoluteSample), marker.ClassId, marker.State);
                else
                {
                    long sequence = marker.AbsoluteSample / _samplesPerChannel;
                    int slot = (int)(sequence & 1);
                    if (_signalSeq[slot] == sequence)
                        DrawEventMarkerToSignalPage(slot, GetPageX(marker.AbsoluteSample), marker.ClassId, marker.State);
                }
            }
        }
    }
}
