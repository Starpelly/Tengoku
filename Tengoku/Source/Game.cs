#define HD

using Trinkit;

using Tengoku.UI;
using Tengoku.Debugging;
using Tengoku.Discord;
using Trinkit.Graphics;
using Trinkit.Localization;
using Tengoku.Scenes;

using ImGuiNET;
using Trinkit.Audio;
using System.Runtime.InteropServices;

namespace Tengoku
{
    public class Game : TrinkitApp
    {
        public static new Game Instance { get; private set; } = null!;

        public static float AspectRatio => (ViewWidth / 280.0f);

        public static Vector2 ViewMousePosition => Input.mousePosition / new Vector2(GameWindow.Width / 280.0f, GameWindow.Height / 160.0f);

#if HD
        public static int ViewWidth => 1280;
        public static int ViewHeight => 720;
#else
        public static int ViewWidth => 280;
        public static int ViewHeight => 160;
#endif

        private DiscordRichPresence? _richPresence;

        private RenderTexture? _gameRenderTexture;
        public static RenderTexture? RenderTexture => Instance._gameRenderTexture;

        public Dictionary<string, Language> Languages { get; set; }
        
        public Game(string title, int width, int height, bool resizable = false) : base(title, width, height, resizable)
        {
            Instance = this;

            Languages = new Dictionary<string, Language>()
            {
                { "eng", new Language() }
            };

            _gameRenderTexture = new RenderTexture(ViewWidth, ViewHeight);

            TrinkitImGui.Setup(true);

            ImGui.GetIO().Fonts.Clear();
            ImGui.GetIO().Fonts.AddFontFromFileTTF("Resources/fonts/Questrial-Regular.ttf", 14);
            LoadIconFont("Resources/fonts/fa-solid-900.ttf", 13, (FontIcon.IconMin, FontIcon.IconMax));
            TrinkitImGui.ReloadFonts();

            ImGui.GetIO().ConfigFlags |=
                  ImGuiConfigFlags.DockingEnable
                | ImGuiConfigFlags.ViewportsEnable
                | ImGuiConfigFlags.NavEnableKeyboard;

            ImGui.GetStyle().WindowRounding = 8f;
            DefaultTheme();

            _richPresence = new DiscordRichPresence();

            LoadScene<SplashscreenScene>();
        }

        public static unsafe ImFontPtr LoadIconFont(string name, int size, (ushort, ushort) range)
        {
            ImFontConfigPtr configuration = ImGuiNative.ImFontConfig_ImFontConfig();

            configuration.MergeMode = true;
            // configuration.PixelSnapH = true;
            configuration.GlyphMinAdvanceX = 24;

            string path = name;

            GCHandle rangeHandle = GCHandle.Alloc(new ushort[]
            {
        range.Item1,
        range.Item2,
        0
            }, GCHandleType.Pinned);

            try
            {
                return ImGui.GetIO().Fonts.AddFontFromFileTTF(path, size, configuration, rangeHandle.AddrOfPinnedObject());
            }
            finally
            {
                configuration.Destroy();

                if (rangeHandle.IsAllocated)
                {
                    rangeHandle.Free();
                }
            }
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
            CurrentScene?.Update();
        }

        public override void OnDraw()
        {
            Window.Clear(Color.black);

            _gameRenderTexture?.Begin();

            CurrentScene?.DrawBefore();
            CurrentScene?.Draw();
            CurrentScene?.DrawGUI();

            _gameRenderTexture?.End();

            /*
            // Raylib_CsLo.Raylib.BeginShaderMode(shader);
            Raylib_CsLo.Raylib.DrawTexturePro(
                _debugRenderTexture.texture,
                    new Raylib_CsLo.Rectangle(0, 0, (float)_debugRenderTexture.texture.width, (float)-_debugRenderTexture.texture.height),
                    new Raylib_CsLo.Rectangle(0, 19, GameWindow.Width, GameWindow.Height - 19),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Color.white
                );
            // Raylib_CsLo.Raylib.EndShaderMode();
            */

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
            GameView.Gui();
            // LocalizerView.Gui();

            TrinkitImGui.End();
        }

        private void Dockspace()
        {
            ImGui.PushStyleColor(ImGuiCol.WindowBg, (uint)Trinkit.Color.transparent);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(0, 0));
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(0f, 0), ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(Window.Width, Window.Height));
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

        public static void DefaultTheme()
        {
            int is3D = 0;

            var style = ImGui.GetStyle();

            style.AntiAliasedLinesUseTex = false;

            style.PopupRounding = 0;
            style.WindowPadding = new System.Numerics.Vector2(4, 4);
            style.FramePadding = new System.Numerics.Vector2(6, 4);
            style.ItemSpacing = new System.Numerics.Vector2(6, 2);
            style.ScrollbarSize = 16;
            style.WindowBorderSize = 1;
            style.ChildBorderSize = 0;
            style.PopupBorderSize = 1;
            style.FrameBorderSize = is3D;
            style.WindowRounding = 0;
            style.ChildRounding = 0;
            style.FrameRounding = 4;
            style.ScrollbarRounding = 2;
            style.GrabRounding = 3;
            style.TabBorderSize = is3D;
            style.TabRounding = 4;
            style.Colors[(int)ImGuiCol.WindowBg] = new System.Numerics.Vector4(0, 0, 0, 0.1f);
            style.WindowMenuButtonPosition = ImGuiDir.None;


            var colors = style.Colors;
            colors[(int)ImGuiCol.Text] = new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int)ImGuiCol.TextDisabled] = new System.Numerics.Vector4(0.40f, 0.40f, 0.40f, 1.00f);
            colors[(int)ImGuiCol.ChildBg] = (System.Numerics.Vector4)new Trinkit.Color("181718");
            colors[(int)ImGuiCol.WindowBg] = (System.Numerics.Vector4)new Trinkit.Color("242424");
            colors[(int)ImGuiCol.PopupBg] = (System.Numerics.Vector4)new Trinkit.Color("181718");
            colors[(int)ImGuiCol.Border] = (System.Numerics.Vector4)new Trinkit.Color("353435");
            colors[(int)ImGuiCol.BorderShadow] = new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.06f);
            colors[(int)ImGuiCol.FrameBg] = new System.Numerics.Vector4(0.42f, 0.42f, 0.42f, 0.54f);
            colors[(int)ImGuiCol.FrameBgHovered] = new System.Numerics.Vector4(0.42f, 0.42f, 0.42f, 0.40f);
            colors[(int)ImGuiCol.FrameBgActive] = new System.Numerics.Vector4(0.56f, 0.56f, 0.56f, 0.67f);
            colors[(int)ImGuiCol.TitleBg] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.TitleBgActive] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.TitleBgCollapsed] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.MenuBarBg] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.ScrollbarBg] = (System.Numerics.Vector4)new Trinkit.Color("1a1a1a");
            colors[(int)ImGuiCol.ScrollbarGrab] = new System.Numerics.Vector4(0.41f, 0.41f, 0.41f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabHovered] = new System.Numerics.Vector4(0.52f, 0.52f, 0.52f, 1.00f);
            colors[(int)ImGuiCol.ScrollbarGrabActive] = new System.Numerics.Vector4(0.76f, 0.76f, 0.76f, 1.00f);
            colors[(int)ImGuiCol.CheckMark] = new System.Numerics.Vector4(0.65f, 0.65f, 0.65f, 1.00f);
            colors[(int)ImGuiCol.SliderGrab] = new System.Numerics.Vector4(0.52f, 0.52f, 0.52f, 1.00f);
            colors[(int)ImGuiCol.SliderGrabActive] = new System.Numerics.Vector4(0.64f, 0.64f, 0.64f, 1.00f);
            colors[(int)ImGuiCol.Button] = (System.Numerics.Vector4)new Trinkit.Color("353435");
            colors[(int)ImGuiCol.ButtonHovered] = new System.Numerics.Vector4(0.52f, 0.52f, 0.52f, 0.59f);
            colors[(int)ImGuiCol.ButtonActive] = new System.Numerics.Vector4(0.76f, 0.76f, 0.76f, 1.00f);
            colors[(int)ImGuiCol.Header] = (System.Numerics.Vector4)new Trinkit.Color("016a5a");
            colors[(int)ImGuiCol.HeaderHovered] = (System.Numerics.Vector4)Trinkit.Color.ChangeAlpha(new Trinkit.Color("068676"), 0.35f);
            colors[(int)ImGuiCol.HeaderActive] = (System.Numerics.Vector4)Trinkit.Color.ChangeAlpha(new Trinkit.Color("02b594"), 0.55f);
            colors[(int)ImGuiCol.Separator] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.SeparatorHovered] = (System.Numerics.Vector4)new Trinkit.Color("02b594");
            colors[(int)ImGuiCol.SeparatorActive] = new System.Numerics.Vector4(0.702f, 0.671f, 0.600f, 0.674f);
            colors[(int)ImGuiCol.ResizeGrip] = new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.25f);
            colors[(int)ImGuiCol.ResizeGripHovered] = new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.67f);
            colors[(int)ImGuiCol.ResizeGripActive] = new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.95f);
            colors[(int)ImGuiCol.PlotLines] = new System.Numerics.Vector4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int)ImGuiCol.PlotLinesHovered] = new System.Numerics.Vector4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogram] = new System.Numerics.Vector4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.PlotHistogramHovered] = new System.Numerics.Vector4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int)ImGuiCol.TextSelectedBg] = new System.Numerics.Vector4(0.73f, 0.73f, 0.73f, 0.35f);
            colors[(int)ImGuiCol.ModalWindowDimBg] = new System.Numerics.Vector4(0.80f, 0.80f, 0.80f, 0.35f);
            colors[(int)ImGuiCol.DragDropTarget] = new System.Numerics.Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int)ImGuiCol.NavHighlight] = new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 1.00f);
            colors[(int)ImGuiCol.NavWindowingHighlight] = new System.Numerics.Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int)ImGuiCol.NavWindowingDimBg] = new System.Numerics.Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int)ImGuiCol.DockingEmptyBg] = new System.Numerics.Vector4(0.38f, 0.38f, 0.38f, 1.00f);
            colors[(int)ImGuiCol.Tab] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.TabHovered] = (System.Numerics.Vector4)new Trinkit.Color("242424");
            colors[(int)ImGuiCol.TabActive] = (System.Numerics.Vector4)new Trinkit.Color("242424");
            colors[(int)ImGuiCol.TabUnfocused] = (System.Numerics.Vector4)new Trinkit.Color("151515");
            colors[(int)ImGuiCol.TabUnfocusedActive] = (System.Numerics.Vector4)new Trinkit.Color("242424");
            colors[(int)ImGuiCol.DockingPreview] = (System.Numerics.Vector4)new Trinkit.Color("02b594");
        }


        public override void OnQuit()
        {
            CurrentScene?.ClearComponents();

            _richPresence?.Dispose();

            TrinkitImGui.Shutdown();
            Raylib_CsLo.Raylib.CloseAudioDevice();
        }

        public static void LoadScene<T>() where T : Scene
        {
            if (Instance.CurrentScene != null)
            {
                if (Instance.CurrentScene.GetType() == typeof(T))
                    return;

                Instance.CurrentScene.OnExit();

                Instance.CurrentScene?.ClearComponents();
            }

            var sceneObj = Activator.CreateInstance(typeof(T)) as T;
            if (sceneObj == null) throw new Exception("Scene not found!");

            Instance.CurrentScene = sceneObj;
        }
    }
}
