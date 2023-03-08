using Trinkit;
using Trinkit.Graphics;

namespace Tengoku.Scenes
{
    public class MenuScene : Scene
    {
        private Texture _buttons;

        public override void Start()
        {
            _buttons = Resources.Load<Texture>("sprites/mainmenu/buttons.png");
        }

        public override void Draw()
        {
            Window.Clear("#f8b868".Hex2RGB());
            float ratio =  (Game.ViewWidth / 280.0f);
            Raylib_CsLo.Raylib.DrawTexturePro(_buttons, new Raylib_CsLo.Rectangle(0, 0, 220, 25), new Raylib_CsLo.Rectangle(100 * ratio, 100 * ratio, 220* ratio, 25*ratio), new System.Numerics.Vector2(0, 0), 0, Color.white);
        }
    }
}
