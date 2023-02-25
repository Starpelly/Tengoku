using ImGuiNET;

namespace Tengoku.Debugging
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
                    /*if (ImGui.MenuItem("Splashscreen")) { }
                    if (ImGui.MenuItem("Title")) { }
                    if (ImGui.MenuItem("Menu")) { }
                    if (ImGui.MenuItem("GameSelect")) { }*/
                    if (ImGui.MenuItem("Game")) { }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Games"))
                {
                    if (ImGui.BeginMenu("Extras")) 
                    {
                        if (ImGui.MenuItem("Spaceball"))
                            GameManager.Instance.LoadScript(@"Resources\levels\spaceball.tks");
                        if (ImGui.MenuItem("Remix 1 Test"))
                            GameManager.Instance.LoadScript(@"Resources\levels\remix1.tks");
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
