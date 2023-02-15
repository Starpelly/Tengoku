using Trinkit;

using Raylib_CsLo;
using Tengoku.Games;

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
        }

        public override void OnQuit()
        {
            spaceball.Dispose();

            Raylib.CloseAudioDevice();
        }
    }
}
