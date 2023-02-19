using ImGuiNET;

namespace Tengoku.Debugging
{
    public class ConsoleView
    {
        public static void Gui()
        {
            if (ImGui.Begin("Console"))
            {
                ImGui.End();
            }
        }
    }
}
