using ImGuiNET;
using Tengoku.Scenes;

namespace Tengoku.Debugging
{
    public class Menubar
    {
        public static void Layout()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Edit"))
                {
                    ImGui.EndMenu();
                }
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
                    if (ImGui.MenuItem("Splashscreen")) { Game.LoadScene<SplashscreenScene>(); }
                    if (ImGui.MenuItem("Menu")) { Game.LoadScene<MenuScene>(); }
                    if (ImGui.MenuItem("GameSelect")) { Game.LoadScene<GameSelect>(); }
                    if (ImGui.MenuItem("Game")) { Game.LoadScene<GameScene>(); }
                    if (ImGui.MenuItem("Editor")) { Game.LoadScene<EditorScene>(); }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Games"))
                {
                    if (ImGui.BeginMenu("Extras")) 
                    {
                        ImGui.EndMenu();
                    }
                    /*ImGui.Separator();
                    if (ImGui.MenuItem("Karate Man")) { }
                    if (ImGui.MenuItem("Rhythm Tweezers")) { }
                    if (ImGui.MenuItem("Marching Orders")) { }
                    if (ImGui.MenuItem("Spaceball")) { }
                    if (ImGui.MenuItem("The Clappy Trio")) { }
                    if (ImGui.MenuItem("Remix 1")) { }*/
                    ImGui.EndMenu();
                }

                ImGui.EndMainMenuBar();
            }
        }
    }
}
