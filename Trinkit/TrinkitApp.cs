using Raylib_CsLo;
using Trinkit.Debug;

namespace Trinkit
{
    public abstract class TrinkitApp : IDisposable
    {
        /// <summary>
        /// The application name.
        /// </summary>
        public string Name = "";

        private Image _icon;

        public TrinkitApp(string title, int width, int height)
        {
            Name = title;

            Raylib.SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT | ConfigFlags.FLAG_WINDOW_ALWAYS_RUN);
            Raylib.SetTraceLogLevel((int)rlTraceLogLevel.RL_LOG_ERROR);
            Raylib.InitWindow(width, height, Name);

            _icon = Raylib.LoadImage("Resources/icon.png");
            Raylib.SetWindowIcon(_icon);


            OnLoad();
        }

        public void Run()
        {
            while (!Raylib.WindowShouldClose())
            {
                Counters.Reset();

                OnUpdate();
                Raylib.BeginDrawing();
                OnDraw();
                Raylib.EndDrawing();
            }
        }

        public abstract void OnLoad();
        public abstract void OnUpdate();
        public abstract void OnDraw();
        public abstract void OnQuit();

        public void Dispose()
        {
            OnQuit();

            Raylib.UnloadImage(_icon);
            Raylib.CloseWindow();
        }
    }
}
