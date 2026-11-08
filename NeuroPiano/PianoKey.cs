using System.Drawing;

namespace NeuroPiano
{
    public sealed class PianoKey
    {
        public int MidiNumber { get; set; }
        public string NoteName { get; set; }
        public bool IsBlack { get; set; }
        public RectangleF Bounds { get; set; }

        internal RectangleF TopBounds;
        internal RectangleF FrontBounds;
        internal RectangleF CueBounds;
        internal RectangleF PressedCueBounds;
        internal float ExtendedFrontBottom;
        internal float LeftSplitX;
        internal float RightSplitX;
        internal RectangleF LeftFlickerMask;
        internal RectangleF RightFlickerMask;
        internal RectangleF PressedLeftFlickerMask;
        internal RectangleF PressedRightFlickerMask;
    }
}
