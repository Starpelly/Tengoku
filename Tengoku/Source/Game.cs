#define HD

using Trinkit;

using Tengoku.Games.Spaceball;
using Tengoku.UI;
using Tengoku.Debugging;

using ImGuiNET;
using Tengoku.Discord;
using Trinkit.Graphics;
using Trinkit.Localization;
using Newtonsoft.Json;


namespace Tengoku
{
    public class Game : TrinkitApp
    {
        public static new Game Instance { get; private set; } = null!;

        public Scene scene;

        private RenderTexture _renderTexture;
        public static RenderTexture RenderTexture => Instance!._renderTexture;

#if HD
        private int _screenWidth => GameWindow.Width;
        private int _screenHeight => GameWindow.Height - 19;
#else

        private int _screenWidth = 280;
        private int _screenHeight = 160;
#endif

        public static float AspectRatio => (Instance._screenWidth / 280.0f);

        public static int ViewWidth => Instance._screenWidth;
        public static int ViewHeight => Instance._screenHeight;

        public static Vector2 ViewMousePosition => Input.mousePosition / new Vector2(GameWindow.Width / 280.0f, GameWindow.Height / 160.0f);

        private DiscordRichPresence richPresence;

        public Game(string title, int width, int height, bool resizable = false) : base(title, width, height, resizable)
        {
            Instance = this;

            _renderTexture = new RenderTexture(_screenWidth, _screenHeight);

            Raylib_CsLo.Raylib.InitAudioDevice();

            TrinkitImGui.Setup(true);

            ImGui.GetIO().ConfigFlags |=
                  ImGuiConfigFlags.DockingEnable
                | ImGuiConfigFlags.ViewportsEnable
                | ImGuiConfigFlags.NavEnableKeyboard;

            ImGui.GetStyle().WindowRounding = 8f;

            richPresence = new DiscordRichPresence();

            scene = new Menus.GameSelect();
        }

        public override void OnUpdate()
        {
            scene.Update();
        }

        public override void OnDraw()
        {
            Window.Clear(Color.black);

            scene.DrawBefore();

            _renderTexture.Begin();

            scene.Draw();

            _renderTexture.End();

            // Raylib_CsLo.Raylib.BeginShaderMode(shader);
            Raylib_CsLo.Raylib.DrawTexturePro(
                _renderTexture.texture,
                    new Raylib_CsLo.Rectangle(0, 0, (float)_renderTexture.texture.width, (float)-_renderTexture.texture.height),
                    new Raylib_CsLo.Rectangle(0, 19, GameWindow.Width, GameWindow.Height - 19),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Color.white
                );
            // Raylib_CsLo.Raylib.EndShaderMode();

            scene.DrawGUI();

#if RELEASE
#else
            GuiLayer();
#endif
        }

        private void GuiLayer()
        {
            TrinkitImGui.Begin();

            Dockspace();
            Menubar.Layout();
            DebugView.Gui();
            // GameView.Gui();
            // LocalizerView.Gui();

            TrinkitImGui.End();
        }

        private void Dockspace()
        {
            ImGui.PushStyleColor(ImGuiCol.WindowBg, (uint)Trinkit.Color.transparent);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0, 0));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0f, 0), ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(GameWindow.Width, GameWindow.Height));
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
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Dispose();
            }

            richPresence.Dispose();

            TrinkitImGui.Shutdown();
            Raylib_CsLo.Raylib.CloseAudioDevice();
        }
    }
}
