using Trinkit;

using Raylib_CsLo;
using Tengoku.Games;

using ImGuiNET;

namespace Tengoku
{
    public class Game : TrinkitApp
    {
        Spaceball spaceball;

        public Game(string title, int width, int height) : base(title, width, height)
        {
        }

        public override void OnLoad()
        {
            Raylib.InitAudioDevice();

            TrinkitImGui.Setup(false);

            spaceball = new Spaceball();
        }

        public override void OnUpdate()
        {
            spaceball.Update();
        }

        public override void OnDraw()
        {
            Raylib.ClearBackground(Raylib.WHITE);

            spaceball.Draw();

            TrinkitImGui.Begin();
            
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

            spaceball.ImGui();

            TrinkitImGui.End();
        }

        public override void OnQuit()
        {
            spaceball.Dispose();

            TrinkitImGui.Shutdown();
            Raylib.CloseAudioDevice();
        }
    }
}
