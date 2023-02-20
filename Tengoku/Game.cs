using Trinkit;

using Raylib_CsLo;
using Tengoku.Games;

using ImGuiNET;
using Tengoku.Games.Spaceball;
using Tengoku.UI;
using Tengoku.Debugging;

namespace Tengoku
{
    public class Game : TrinkitApp
    {
        public static Game? Instance { get; private set; }

        public Spaceball spaceball;
        GameManager gameManager;
        DSGuy dsGuy;

        private RenderTexture _renderTexture;
        private int _screenWidth = 280;
        private int _screenHeight = 160;

        public static RenderTexture RenderTexture => Instance!._renderTexture;

        public Game(string title, int width, int height, bool resizable = false) : base(title, width, height, resizable)
        {
            Instance = this;

            _renderTexture = Raylib.LoadRenderTexture(_screenWidth, _screenHeight);
        }

        public override void OnLoad()
        {
            Raylib.InitAudioDevice();

            TrinkitImGui.Setup(true);

            ImGui.GetIO().ConfigFlags |=
                  ImGuiConfigFlags.DockingEnable
                | ImGuiConfigFlags.ViewportsEnable
                | ImGuiConfigFlags.NavEnableKeyboard;

            ImGui.GetStyle().WindowRounding = 8f;

            gameManager = new GameManager();
            spaceball = new Spaceball();
            gameManager.Spaceball = spaceball;
            dsGuy = new DSGuy();
        }

        public override void OnUpdate()
        {
            // gameManager.Update();
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
            Raylib.ClearBackground(new Trinkit.Color("#1f1f1f"));
#if RELEASE
            Raylib.DrawTexturePro(
                _renderTexture.texture,
                    new Rectangle(0, 0, (float)_renderTexture.texture.width, (float)-_renderTexture.texture.height),
                    new Rectangle(0, menubarHeight, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - menubarHeight),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Raylib.WHITE
                );
#endif
#if DEBUG
            spaceball.DrawGUI();

            TrinkitImGui.Begin();

            Dockspace();
            spaceball.ImGui();

            GameView.Gui();
            Menubar.Layout();
            AnimationEditor.Gui();
            ConsoleView.Gui();

            TrinkitImGui.End();
#endif
        }

        private void Dockspace()
        {
            ImGui.PushStyleColor(ImGuiCol.WindowBg, (uint)Trinkit.Color.transparent);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0, 0));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0f, 0), ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f);
            ImGuiWindowFlags dockSpaceFlags = ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse |
                                              ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;

            bool p_open = true;
            ImGui.Begin("Dockspace", ref p_open, dockSpaceFlags);
            ImGui.PopStyleVar(2);

            ImGui.DockSpace(ImGui.GetID("Dockspace"), new System.Numerics.Vector2(0, 0), ImGuiDockNodeFlags.PassthruCentralNode);
            ImGui.PopStyleVar();
            ImGui.PopStyleColor();
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
