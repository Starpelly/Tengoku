using Raylib_CsLo;

namespace Trinkit
{
    public abstract class TrinkitApp : IDisposable
    {
        /// <summary>
        /// The application name.
        /// </summary>
        public string Name = "";

        public TrinkitApp(string title, int width, int height)
        {
            Name = title;

            Raylib.InitWindow(width, height, Name);

            OnLoad();
        }

        public void Run()
        {
            while (!Raylib.WindowShouldClose())
            {
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
            Raylib.CloseWindow();
        }
    }
}
