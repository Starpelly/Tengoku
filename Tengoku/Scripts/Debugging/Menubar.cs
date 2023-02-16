using ImGuiNET;

namespace Tengoku.Debug
{
    public class Menubar
    {
        public static void Layout()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Resolution")) { }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Audio"))
                {
                    if (ImGui.MenuItem("Latency")) { }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Scenes"))
                {
                    if (ImGui.MenuItem("Splashscreen")) { }
                    if (ImGui.MenuItem("Title")) { }
                    if (ImGui.MenuItem("Menu")) { }
                    if (ImGui.MenuItem("GameSelect")) { }
                    if (ImGui.MenuItem("Game")) { }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Games"))
                {
                    if (ImGui.MenuItem("Load Custom...")) { }
                    ImGui.Separator();
                    if (ImGui.MenuItem("Karate Man")) { }
                    if (ImGui.MenuItem("Rhythm Tweezers")) { }
                    if (ImGui.MenuItem("Marching Orders")) { }
                    if (ImGui.MenuItem("Spaceball")) { }
                    if (ImGui.MenuItem("The Clappy Trio")) { }
                    if (ImGui.MenuItem("Remix 1")) { }
                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }
        }
    }
}
