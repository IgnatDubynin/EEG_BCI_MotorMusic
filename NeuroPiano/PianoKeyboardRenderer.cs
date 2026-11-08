using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace NeuroPiano
{
    public sealed class PianoKeyboardRenderer
    {
        // sRGB gray matching the mean luminance of the 0.75 + 0.25 * sin(phase) stimulus.
        private const float FlickerMaskGray = 198.0f / 255.0f;

        public void SetupViewport(int width, int height)
        {
            if (width < 1)
                width = 1;

            if (height < 1)
                height = 1;

            GL.Viewport(0, 0, width, height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, width, height, 0.0, -1.0, 1.0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.CullFace);
            GL.ClearColor(0.050f, 0.050f, 0.055f, 1.0f);
        }

        public void Render(PianoKeyboardLayout layout, int pressedMidiNumber, SSVEPStimulusEngine ssvepStimulusEngine, int activeStimulusVisibleIndex,
            int attentionCueVisibleIndex, int backgroundCueVisibleIndex, bool attentionCueIsPeripheral = false)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (layout == null)
                return;

            bool whitePressed = false;
            for (int i = 0; i < layout.WhiteKeys.Count; i++)
            {
                PianoKey key = layout.WhiteKeys[i];
                bool pressed = key.MidiNumber == pressedMidiNumber;
                whitePressed |= pressed;

                DrawWhiteKey(
                    key,
                    i,
                    pressed,
                    ssvepStimulusEngine,
                    activeStimulusVisibleIndex);
            }

            for (int i = 0; i < layout.BlackKeys.Count; i++)
            {
                PianoKey key = layout.BlackKeys[i];
                DrawBlackKey(key, pressedMidiNumber, whitePressed);
            }

            if (attentionCueVisibleIndex >= 0 &&
                attentionCueVisibleIndex < layout.WhiteKeys.Count)
            {
                PianoKey cueKey = layout.WhiteKeys[attentionCueVisibleIndex];
                RectangleF cueBounds = cueKey.MidiNumber == pressedMidiNumber
                    ? cueKey.PressedCueBounds
                    : cueKey.CueBounds;
                DrawTargetCueBorder(cueBounds, attentionCueIsPeripheral);
            }

            DrawBorder(layout.Bounds, PianoKeyboardLayout.KeyboardBorderWidth, 0.10f, 0.10f, 0.10f);

            for (int i = 0; i < layout.BackgroundTargets.Count; i++)
                DrawBorder(layout.BackgroundTargets[i], 1.0f, 1.0f, 1.0f, 1.0f);

            if (backgroundCueVisibleIndex >= 0 &&
                backgroundCueVisibleIndex < layout.BackgroundTargets.Count)
            {
                DrawTargetCueBorder(layout.GetBackgroundCueBounds(backgroundCueVisibleIndex));
            }
        }

        private static void DrawWhiteKey(PianoKey key, int visibleIndex, bool pressed, SSVEPStimulusEngine ssvepStimulusEngine,
            int activeStimulusVisibleIndex)
        {
            RectangleF r = key.Bounds;

            bool stimulusEnabled = ssvepStimulusEngine != null &&
                (activeStimulusVisibleIndex == -1 || activeStimulusVisibleIndex == visibleIndex);
            float brightness = stimulusEnabled
                ? (float)ssvepStimulusEngine.GetBrightness(visibleIndex)
                : 1.0f;

            GL.Color4(brightness, brightness, brightness, 1.0f);
            DrawKeyProjection(key, pressed, 0.65f, 0.65f);

            if (stimulusEnabled)
            {
                GL.Color4(FlickerMaskGray, FlickerMaskGray, FlickerMaskGray, 1.0f);
                DrawFlickerMask(pressed ? key.PressedLeftFlickerMask : key.LeftFlickerMask);
                DrawFlickerMask(pressed ? key.PressedRightFlickerMask : key.RightFlickerMask);
            }

            DrawBorder(r, 1.25f, 0.12f, 0.12f, 0.12f);
        }

        private static void DrawFlickerMask(RectangleF mask)
        {
            if (!mask.IsEmpty)
                FillRect(mask);
        }

        private static void DrawBlackKey(PianoKey key, int pressedMidiNumber, bool whitePressed)
        {
            const float frontBrightness = 0.035f;
            const float frontBlue = 0.040f;
            const float borderBrightness = 0.42f;
            RectangleF r = key.Bounds;

            GL.Color4(0.16f, 0.16f, 0.16f, 1.0f);
            if (!whitePressed || Math.Abs(key.MidiNumber - pressedMidiNumber) != 1)
            {
                DrawKeyProjection(key, key.MidiNumber == pressedMidiNumber, frontBrightness, frontBlue);
                DrawBorder(r, 1.25f, borderBrightness, borderBrightness, 0.43f);
                return;
            }

            bool extendLeft = pressedMidiNumber < key.MidiNumber;
            float splitX = extendLeft ? key.LeftSplitX : key.RightSplitX;

            FillRect(key.TopBounds);
            GL.Color4(frontBrightness, frontBrightness, frontBlue, 1.0f);
            DrawSteppedShape(key.FrontBounds, splitX, extendLeft, key.ExtendedFrontBottom, PrimitiveType.TriangleFan);

            GL.LineWidth(1.25f);
            GL.Color4(borderBrightness, borderBrightness, 0.43f, 1.0f);
            DrawSteppedShape(r, splitX, extendLeft, key.ExtendedFrontBottom, PrimitiveType.LineLoop);
        }

        private static void DrawSteppedShape(RectangleF r, float splitX, bool extendLeft,
            float extensionBottom, PrimitiveType primitive)
        {
            float extendedEdge = extendLeft ? r.Left : r.Right;
            float otherEdge = extendLeft ? r.Right : r.Left;
            bool stepped = splitX != otherEdge;

            GL.Begin(primitive);
            if (stepped)
            {
                // Start the fan at the inner corner of the step.
                GL.Vertex2(splitX, r.Bottom);
                GL.Vertex2(splitX, extensionBottom);
            }

            GL.Vertex2(extendedEdge, extensionBottom);
            GL.Vertex2(extendedEdge, r.Top);
            GL.Vertex2(otherEdge, r.Top);
            GL.Vertex2(otherEdge, stepped ? r.Bottom : extensionBottom);
            GL.End();
        }

        private static void DrawKeyProjection(PianoKey key, bool pressed, float frontBrightness, float frontBlue)
        {
            if (pressed)
            {
                FillRect(key.Bounds);
                return;
            }

            FillRect(key.TopBounds);
            GL.Color4(frontBrightness, frontBrightness, frontBlue, 1.0f);
            FillRect(key.FrontBounds);
        }

        private static void DrawTargetCueBorder(RectangleF r, bool peripheral = false)
        {
            if (peripheral) DrawBorder(r, 4.0f, 0.10f, 0.85f, 0.10f);
            else DrawBorder(r, 4.0f, 0.95f, 0.20f, 0.10f);
        }

        private static void FillRect(RectangleF r, PrimitiveType primitive = PrimitiveType.Quads)
        {
            GL.Begin(primitive);
            GL.Vertex2(r.Left, r.Top);
            GL.Vertex2(r.Right, r.Top);
            GL.Vertex2(r.Right, r.Bottom);
            GL.Vertex2(r.Left, r.Bottom);
            GL.End();
        }

        private static void DrawBorder(RectangleF r, float width, float red, float green, float blue)
        {
            GL.LineWidth(width);
            GL.Color4(red, green, blue, 1.0f);
            FillRect(r, PrimitiveType.LineLoop);
        }
    }
}
