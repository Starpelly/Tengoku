using ImGuiNET;

namespace Tengoku.Debugging
{
    public class DebugView
    {
        public static void Gui()
        {
            if (ImGui.Begin("Debug"))
            {
                ImGui.Text($"FPS: {Raylib_CsLo.Raylib.GetFPS()}");
                ImGui.Text($"Sprites Rendered: {Trinkit.Debug.Counters.SpritesRendered}");
                ImGui.End();
            }
        }
    }
}
