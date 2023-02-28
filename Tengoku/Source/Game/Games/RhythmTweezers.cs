using Trinkit;
using Trinkit.Graphics;

using Tickscript;
using Trinkit.Audio;

namespace Tengoku.Games
{
    [GameEngine("tweezers")]
    public class RhythmTweezers : Minigame
    {
        private Texture _veggiesTex;
        private Texture _tweezersTex;

        private Raylib_CsLo.Camera3D _cam;

        public RhythmTweezers()
        {
            _veggiesTex = Resources.Load<Texture>("sprites/games/tweezers/veggies.png");
            _tweezersTex = Resources.Load<Texture>("sprites/games/tweezers/tweezers.png");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;
        }

        public override void Update()
        {
            _cam.fovy = 10f;
            _cam.position = new System.Numerics.Vector3(0.0f, -0.012f, -10.0f);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
        }

        public override void Draw()
        {
            Window.Clear(Color.white);

            Raylib_CsLo.Raylib.BeginMode3D(_cam);

            Sprite.DrawSprite(_veggiesTex,
                new Vector3(),
                0,
                Color.white,
                Vector2.one,
                new Raylib_CsLo.Rectangle(0, 0, 240, 160),
                90.0f);

            var tweezersRot = Mathf.Lerp(0, Mathf.PI * 2f, Conductor.Instance.GetLoopPositionFromBeat(0, 10f));
            var length = 120f;
            var x = Mathf.Cos(tweezersRot) * length;
            var y = Mathf.Sin(tweezersRot) * length;

            Sprite.DrawSprite(_tweezersTex,
                new Vector3(x*0.01f, (y*0.01f) + 0.6f, 0),
                Mathf.Lerp(360f, 0f, Conductor.Instance.GetLoopPositionFromBeat(0, 10f)) - 90f,
                Color.white,
                Vector2.one,
                new Raylib_CsLo.Rectangle(0, 0, 48, 48),
                90.0f);

            Raylib_CsLo.Raylib.EndMode3D();
        }

        [GameFunction("hair")]
        public void Hair()
        {
            Console.WriteLine("hair");
        }
    }
}
