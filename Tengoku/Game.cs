using Trinkit;

using Raylib_CsLo;
using Tengoku.Games;

using ImGuiNET;
using Tengoku.Games.Spaceball;
using Tengoku.UI;

namespace Tengoku
{
    public class Game : TrinkitApp
    {
        Spaceball spaceball;
        GameManager gameManager;
        DSGuy dsGuy;

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

            gameManager = new GameManager();
            spaceball = new Spaceball();
            gameManager.Spaceball = spaceball;
            dsGuy = new DSGuy();
        }

        public override void OnUpdate()
        {
            gameManager.Update();
            spaceball.Update();
        }

        public override void OnDraw()
        {
            Raylib.ClearBackground(Raylib.WHITE);

            Raylib.BeginTextureMode(_renderTexture);

            spaceball.Draw();
            dsGuy.DrawGUI();

            Raylib.EndTextureMode();
#if DEBUG
            var menubarHeight = 19;
#else
            var menubarHeight = 0;
#endif
            Raylib.ClearBackground(Raylib.BLACK);
            Raylib.DrawTexturePro(
                _renderTexture.texture,
                    new Rectangle(0, 0, (float)_renderTexture.texture.width, (float)-_renderTexture.texture.height),
                    new Rectangle(0, menubarHeight, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - menubarHeight),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Raylib.WHITE
                );
#if DEBUG
            spaceball.DrawGUI();
            TrinkitImGui.Begin();

            Debug.Menubar.Layout();
            spaceball.ImGui();

            TrinkitImGui.End();
#endif
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
