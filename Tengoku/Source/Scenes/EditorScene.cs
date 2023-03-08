using Trinkit;

namespace Tengoku.Scenes
{
    public class EditorScene : Scene
    {
        public override void Start()
        {

        }

        public override void Draw()
        {
            var song = 1;
            Raylib_CsLo.Raylib.DrawRectangle((int)20*song, 40, 4, 40, Color.green);
        }
    }
}
