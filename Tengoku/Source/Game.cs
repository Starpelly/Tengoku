// #define HD

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

        public static Vector2 ViewMousePosition => Input.mousePosition / new Vector2(ViewWidth / 280.0f, ViewHeight / 160.0f);

#if HD
        public static int ViewWidth => Window.Width;
        public static int ViewHeight => Window.Height;
#else
        public static int ViewWidth => 280;
        public static int ViewHeight => 160;
#endif

        private DiscordRichPresence? _richPresence;

        private RenderTexture? _gameRenderTexture;
        public static RenderTexture? RenderTexture => Instance._gameRenderTexture;

        public Dictionary<string, Language> Languages { get; set; }

        private bool _isPlaying { get; set; } = false;
        public static bool IsPlaying { get { return Instance._isPlaying; } set { Instance._isPlaying = value; } }
        
        public Game(string title, int width, int height, bool resizable = false) : base(title, width, height, resizable)
        {
            Instance = this;

            ImGuiLayer.Setup();

            Languages = new Dictionary<string, Language>()
            {
                { "eng", new Language() }
            };

            _gameRenderTexture = new RenderTexture(ViewWidth, ViewHeight);

            _richPresence = new DiscordRichPresence();

            LoadScene<GameScene>();
        }

        public override void OnStart()
        {
        }

        public override void OnUpdate()
        {
            if (_isPlaying)
            {
                Time.Clock += Time.DeltaTime;
                CurrentScene?.Update();
            }
        }

        public override void OnDraw()
        {
            Window.Clear(Color.black);

            if (_isPlaying)
            {
                _gameRenderTexture?.Begin();

                CurrentScene?.DrawBefore();
                CurrentScene?.Draw();
                CurrentScene?.DrawGUI();

                _gameRenderTexture?.End();
            }


            /*
            float ratio = ((float)Window.Width / (float)Window.Height);
            var resolutionWidth = Mathf.Round(160 * ratio);
            
            // Raylib_CsLo.Raylib.BeginShaderMode(shader);
            if (_gameRenderTexture != null)
            Raylib_CsLo.Raylib.DrawTexturePro(
                _gameRenderTexture.texture,
                    new Raylib_CsLo.Rectangle(0, 0, (float)_gameRenderTexture.texture.width, (float)-_gameRenderTexture.texture.height),
                    new Raylib_CsLo.Rectangle(0, 0, Window.Width, Window.Height - 0),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Color.white
                );
            // Raylib_CsLo.Raylib.EndShaderMode();
            */
#if RELEASE
#else
            ImGuiLayer.Draw();
#endif
        }

        public override void OnQuit()
        {
            _richPresence?.Dispose();

            TrinkitImGui.Shutdown();
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
            Instance.CurrentScene.Start();

            Time.Clock = 0.0f;
        }
    }
}
