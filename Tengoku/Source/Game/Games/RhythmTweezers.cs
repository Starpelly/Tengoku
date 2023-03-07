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
        private Sound _nextSnd;
        private Sound _pluckSnd;

        private Animator _hairAnim;

        private Raylib_CsLo.Camera3D _cam;

        private List<Hair> _hairs = new();
        private int _hairIndex;

        private float _pluckTime;

        private float _intervalBeat = 0.0f;

        private class Hair
        {
            public float Beat;
            public bool IsLong;
            public Animator Anim;
            public bool IsPlucked;
            public float PluckedTime;
            public Vector2 Position;
            public float HairRot;

            public Hair(float beat, bool isLong, Animator anim)
            {
                this.Beat = beat;
                this.IsLong = isLong;
                this.Anim = anim;
            }
        }

        public RhythmTweezers()
        {
            _veggiesTex = Resources.Load<Texture>("sprites/games/tweezers/veggies.png");
            _tweezersTex = Resources.Load<Texture>("sprites/games/tweezers/tweezers.png");
            _hairappear = Resources.Load<Texture>("sprites/games/tweezers/hairappear.png");

            _shortAppearSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/shortAppear.ogg");
            _longAppearSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/longAppear.ogg");
            _pluckSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/shortPluck1.ogg");
            _nextSnd = Resources.Load<Sound>("audio/sfx/games/rhythmTweezers/register.ogg");

            _hairAnim = new Animator("resources/animations/games/tweezers/rhythmtweezershair.json");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;
        }

        public override void Update()
        {
            _cam.fovy = 10f;
            _cam.position = new System.Numerics.Vector3(0.0f, Mathf.Lerp(-0.03f, 0.0f, Mathf.Normalize(Conductor.Instance.SongPosition, _pluckTime, _pluckTime + 0.05f)), -10.0f);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            if (PlayerInput.GetPlayerDown())
            {
                _pluckSnd.Play();

                _hairs[_hairIndex].IsPlucked = true;
                _hairs[_hairIndex].PluckedTime = Conductor.Instance.SongPosition;
                _hairIndex++;

                _pluckTime = Conductor.Instance.SongPosition;
            }
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
                new Raylib_CsLo.Rectangle(240*0, 160*0, 240, 160),
                90.0f);

            bool _isPlucking = (Conductor.Instance.SongPosition < _pluckTime + 0.2f);

            var normalizedTweezers = Conductor.Instance.GetLoopPositionFromBeat(-2.5f, 8f);
            var tweezersRot = (Mathf.Lerp(0, 360f, normalizedTweezers) + 90f) * Mathf.Deg2Rad;
            var tweezersNormalized = Mathf.Normalize(Conductor.Instance.SongPosition, _pluckTime, _pluckTime + 0.2f);
            var length = Mathf.Lerp(122f, 132f, tweezersNormalized);
            var x = Mathf.Cos(tweezersRot) * length;
            var y = Mathf.Sin(tweezersRot) * length;

            Sprite.DrawSprite(_tweezersTex,
                new Vector3(x*0.01f, (y*0.01f) + 0.6f, 0),
                Mathf.Lerp(360f, 0f, normalizedTweezers) + 180f,
                Color.white,
                Vector2.one,
                new Raylib_CsLo.Rectangle((_isPlucking) ? 48 : 0, 0, 48, 48),
                90.0f);

            for (int i = 0; i < _hairs.Count; i++)
            {
                var hair = _hairs[i];
                var beat = hair.Beat - _intervalBeat + 1;

                hair.Anim.Update();

                if (hair.IsPlucked)
                {
                    var hairLength = Mathf.Lerp(70f, 93f, tweezersNormalized);
                    var hairX = (Mathf.Cos(tweezersRot) * (hairLength)) * 0.01f;
                    var hairY = ((Mathf.Sin(tweezersRot) * (hairLength)) * 0.01f) + 0.6f;

                    var originY = 1.0f;
                    if (Conductor.Instance.SongPosition > hair.PluckedTime + 0.2f)
                    {
                        hair.Position -= new Vector2(0, 1.26f * Time.DeltaTime * (Conductor.Instance.SongPosition - hair.PluckedTime) * 2f);
                        hair.HairRot += 355f * Time.DeltaTime;
                        originY = 0.5f;
                    }
                    else
                    {
                        hair.Position = new Vector2(hairX, hairY);
                        hair.HairRot = Mathf.Lerp(360f, 0f, normalizedTweezers) + 180f;
                    }
                    Sprite.DrawSprite(_hairappear,
                        new Vector3(hair.Position.x, hair.Position.y, 0),
                        hair.HairRot,
                        Color.white,
                        Vector2.one,
                        new Vector2(0.5f, originY),
                        new Raylib_CsLo.Rectangle(32 * 3, 0, 32, 32),
                        90.0f);
                }
                else
                {
                    var rot = (Mathf.Lerp(0, 360f, Mathf.Normalize(beat, 0, 8f)) + -23f) * Mathf.Deg2Rad;
                    var hairRotX = -Mathf.Cos(rot) * 80f;
                    var hairRotY = -Mathf.Sin(rot) * 70f;

                    var hairFrame = hair.Anim.CurrentIndex;

                    Sprite.DrawSprite(_hairappear,
                        new Vector3(hairRotX * 0.01f, (hairRotY * 0.01f) + 0.575f, 0),
                        90.0f - (rot * Mathf.Rad2Deg),
                        Color.white,
                        Vector2.one,
                        new Vector2(0.5f, 1f),
                        new Raylib_CsLo.Rectangle(32 * hairFrame, 0, 32, 32),
                        90.0f);
                }
            }

            Raylib_CsLo.Raylib.EndMode3D();
        }

        [GameFunction("hair", new GameFunction.ParamType[] { GameFunction.ParamType.COMMAND_BEAT })]
        public void SpawnHair(float beat)
        {
            Console.WriteLine("hair");
            _shortAppearSnd.Play();

            var hairObj = new Hair(beat, false, new Animator("resources/animations/games/tweezers/rhythmtweezershair.json"));
            hairObj.Anim.Play("RhythmTweezersHairAppear");
            _hairs.Add(hairObj);
        }

        [GameFunction("long_hair")]
        public void SpawnLongHair()
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

        [GameFunction("start_interval", new GameFunction.ParamType[] { GameFunction.ParamType.COMMAND_BEAT })]
        public void StartInterval(float beat)
        {
            _intervalBeat = beat;
        }

        [GameFunction("next")]
        public void Next()
        {
            _nextSnd.Play();
        }
    }
}
