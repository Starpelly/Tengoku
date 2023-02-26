using Trinkit;
using Trinkit.Graphics;

namespace Tengoku.Scenes
{
    public class SplashscreenScene : Scene
    {
        private Texture _trinkitLogo;

        public SplashscreenScene()
        {
            _trinkitLogo = Resources.Load<Texture>("sprites/trinkitbanner.png");
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw()
        {
            Window.Clear(Color.trinkit);
            Raylib_CsLo.Raylib.DrawTexture(_trinkitLogo, 
                Game.ViewWidth / 2 - (_trinkitLogo.Width / 2),
                Game.ViewHeight / 2 - (_trinkitLogo.Height / 2),
                Color.white);
        }
    }
}
