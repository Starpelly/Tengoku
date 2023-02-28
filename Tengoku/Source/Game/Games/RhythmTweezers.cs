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
        private Texture _hairappear;

        private Sound _shortAppearSnd;
        private Sound _longAppearSnd;
        private Sound _pluckSnd;

        private Raylib_CsLo.Camera3D _cam;

        private List<float> _hairs = new();

        public RhythmTweezers()
        {
            _veggiesTex = Resources.Load<Texture>("sprites/games/tweezers/veggies.png");
            _tweezersTex = Resources.Load<Texture>("sprites/games/tweezers/tweezers.png");
            _hairappear = Resources.Load<Texture>("sprites/games/tweezers/hairappear.png");

            _shortAppearSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/shortAppear.ogg");
            _longAppearSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/longAppear.ogg");
            _pluckSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/shortPluck1.ogg");

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

            var normalizedTweezers = Conductor.Instance.GetLoopPositionFromBeat(-2.5f, 8f);
            var tweezersRot = (Mathf.Lerp(0, 360f, normalizedTweezers) + 90f) * Mathf.Deg2Rad;
            var length = 132f;
            var x = Mathf.Cos(tweezersRot) * length;
            var y = Mathf.Sin(tweezersRot) * length;

            Sprite.DrawSprite(_tweezersTex,
                new Vector3(x*0.01f, (y*0.01f) + 0.6f, 0),
                Mathf.Lerp(360f, 0f, normalizedTweezers) + 180f,
                Color.white,
                Vector2.one,
                new Raylib_CsLo.Rectangle(0, 0, 48, 48),
                90.0f);

            for (int i = 0; i < _hairs.Count; i++)
            {
                var beat = _hairs[i];
                var rot = (Mathf.Lerp(0, 360f, Mathf.Normalize(beat, 0, 8f)) + -23f) * Mathf.Deg2Rad;
                var hairRotX = -Mathf.Cos(rot) * 80f;
                var hairRotY = -Mathf.Sin(rot) * 70f;

                Sprite.DrawSprite(_hairappear,
                    new Vector3(hairRotX * 0.01f, (hairRotY * 0.01f) + 0.575f, 0),
                    90.0f - (rot * Mathf.Rad2Deg),
                    Color.white,
                    Vector2.one,
                    new Vector2(0.5f, 1f),
                    new Raylib_CsLo.Rectangle(32*4, 0, 32, 32),
                    90.0f);
            }

            Raylib_CsLo.Raylib.EndMode3D();
        }

        [GameFunction("hair")]
        public void Hair()
        {
            Console.WriteLine("hair");
            _shortAppearSnd.Play();
            _hairs.Add(Conductor.Instance.SongPositionInBeats);
        }

        [GameFunction("longhair")]
        public void LongHair()
        {
            Console.WriteLine("long hair");
            _longAppearSnd.Play();
        }

        [GameFunction("pluck")]
        public void Pluck()
        {
            _hairs.RemoveAt(0);
            _pluckSnd.Play();
        }
    }
}
