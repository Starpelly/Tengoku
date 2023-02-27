using Trinkit;
using Trinkit.Graphics;

using Tickscript;

namespace Tengoku.Games
{
    [GameEngine("tweezers")]
    public class RhythmTweezers : Minigame
    {
        public override void Draw()
        {
            Window.Clear(Color.white);
        }

        [GameFunction("hair")]
        public void Hair()
        {
            Console.WriteLine("hair");
        }
    }
}
