using System.Numerics;
using ImGuiNET;

namespace Tengoku.Debugging
{
    public class GameView
    {
        public static void Gui()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0, 0));
            if (ImGui.Begin("Game (Paused)###GameView", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                var windowSize = GetLargestSizeForViewport();
                var windowPos = GetCenteredPositionForViewport(windowSize);

                ImGui.SetCursorPos(new Vector2(windowPos.X + 8, windowPos.Y + 8));

                ImGui.Image(new IntPtr(Game.RenderTexture.texture.id), windowSize - new Vector2(16, 16), new Vector2(0, 1), new Vector2(1, 0));
                ImGui.End();
            }
            ImGui.PopStyleVar();
        }

        private static Vector2 GetLargestSizeForViewport()
        {
            Vector2 windowSize = new Vector2();
            windowSize = ImGui.GetContentRegionAvail();
            windowSize.X -= ImGui.GetScrollX();
            windowSize.Y -= ImGui.GetScrollY();

            float aspectWidth = windowSize.X;
            float aspectHeight = (aspectWidth / TargetAspectRatio());
            if (aspectHeight > windowSize.Y)
            {
                aspectHeight = windowSize.Y;
                aspectWidth = aspectHeight * TargetAspectRatio();
            }

            return new Vector2(aspectWidth, aspectHeight);
        }

        private static Vector2 GetCenteredPositionForViewport(Vector2 aspectSize)
        {
            Vector2 windowSize = new Vector2();
            windowSize = ImGui.GetContentRegionAvail();
            windowSize.X -= ImGui.GetScrollX();
            windowSize.Y -= ImGui.GetScrollY();

            float viewportX = (windowSize.X / 2.0f) - (aspectSize.X / 2.0f);
            float viewportY = (windowSize.Y / 2.0f) - (aspectSize.Y / 2.0f);

            return new Vector2(viewportX + ImGui.GetCursorPosX(), viewportY + ImGui.GetCursorPosY());
        }

        public static float TargetAspectRatio()
        {
            return 16.0f / 9.0f;
        }
    }
}
