using Trinkit;

using Raylib_CsLo;
using Tengoku.Games;

using ImGuiNET;

namespace Tengoku
{
    public class Game : TrinkitApp
    {
        Spaceball spaceball;

        private RenderTexture _renderTexture;
        private int _screenWidth = 280;
        private int _screenHeight = 160;

        public Game(string title, int width, int height) : base(title, width, height)
        {
            _renderTexture = Raylib.LoadRenderTexture(_screenWidth, _screenHeight);
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

            Raylib.BeginTextureMode(_renderTexture);

            spaceball.Draw();

            Raylib.EndTextureMode();
            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.DrawTexturePro(
                _renderTexture.texture,
                    new Rectangle(0, 0, (float)_renderTexture.texture.width, (float)-_renderTexture.texture.height),
                    new Rectangle(0, 19, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - 19),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Raylib.WHITE
                );

            spaceball.DrawGUI();

            TrinkitImGui.Begin();

            Debug.Menubar.Layout();

            spaceball.ImGui();

            TrinkitImGui.End();
        }

        public override void OnQuit()
        {
            spaceball.Dispose();

            Raylib.UnloadRenderTexture(_renderTexture);

            TrinkitImGui.Shutdown();
            Raylib.CloseAudioDevice();
        }
    }
}
