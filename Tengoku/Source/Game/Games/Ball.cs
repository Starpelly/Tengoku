using Raylib_CsLo;
using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Games.Spaceball
{
    public class Ball : Component
    {
        public Spaceball Spaceball { get; set; }

        public float StartBeat;
        public bool High;

        private float _hitBeat;
        private Vector3 _hitPos;
        private bool _hit;
        private Vector3 _lastPos;
        private float _lastRot;

        public Judgement State = Judgement.None;

        private float normalizedPitchAnim;

        public override void Start()
        {
            Raylib.PlaySound(High ? Spaceball.ShootHighSnd : Spaceball.ShootSnd);
        }

        public override void Draw()
        {
            if (!_hit)
            {
                var length = High ? 2 : 1;
                normalizedPitchAnim = GameManager.Instance.Conductor.GetPositionFromBeat(StartBeat, length + 0.175f);

                if (normalizedPitchAnim < 1.0f)
                {
                    var addPos = 0.77f;
                    var addPosY = High ? 2.5f : 1.35f;
                    var ballRot = normalizedPitchAnim * 240f * addPosY;

                    bool down = PlayerInput.GetPlayerDown();
                    // if (delta <= 0.035f) down = true;

                    if (down)
                    {
                        var signedDelta = JudgementManager.GetDelta(High ? StartBeat + 2 : StartBeat + 1, Conductor.Instance.SongPosition);
                        Console.WriteLine(signedDelta * 1);

                        var missRange = 0.13f;
                        var hitRange = 0.0625f;
                        var perfectRange = 0.03f;

                        if (signedDelta.IsWithin(-perfectRange, perfectRange - 0.03f))
                        {
                            State = Judgement.Perfect;
                            Console.WriteLine("Perfect");
                        }
                        else if (signedDelta.IsWithin(-hitRange, hitRange - 0.0325f))
                        {
                            State = Judgement.Hit;
                            Console.WriteLine("Hit");
                        }
                        else if (signedDelta.IsWithin(-missRange, missRange))
                        {
                            State = Judgement.Miss;
                            Console.WriteLine("Miss");
                            return;
                        }

                        if (State == Judgement.Perfect || State == Judgement.Hit)
                        {
                            Raylib.PlaySound(Spaceball.HitSnd);
                            _hitBeat = GameManager.Instance.Conductor.SongPositionInBeats;
                            _hit = true;
                            _hitPos = _lastPos;
                            _lastRot = ballRot;
                        }
                    }

                    _lastPos = GetPointOnBezierCurve(
                            new Vector3(-0.55f, -0.4f),
                            new Vector3(-0.55f + (addPos * 0.5f) - 0.2f, -0.53f + addPosY),
                            new Vector3(-0.55f + (addPos * 0.5f) + 0.1f, -0.53f + addPosY),
                            new Vector3(-0.55f + addPos, -0.62f),
                            normalizedPitchAnim
                            );
                    Sprite.DrawSprite(Spaceball.TexSpaceballProps,
                        _lastPos,
                        ballRot, Trinkit.Color.white,
                        new Rectangle(0, 32 * 2, 32, 32), 90f);
                }
                else
                {
                    Destroy();
                }
            }
            else
            {
                var nba = GameManager.Instance.Conductor.GetPositionFromBeat(_hitBeat, 14);
                Sprite.DrawSprite(Spaceball.TexSpaceballProps,
                    Vector3.Lerp(_hitPos, new Vector3(0f, 0, -1300f), nba),
                    _lastRot * nba * 12f, Trinkit.Color.white,
                    new Rectangle(0, 32 * 2, 32, 32), 90f);

                if (Conductor.Instance.SongPositionInBeats > _hitBeat + 14)
                    Destroy();
            }
        }

        public override void DrawGUI()
        {
            /*
            var hitBeat = 1f;
            var actionLength = (High) ? 2.25f : 1.25f;

            var inputWidth = 1280 - 40;
            var endWidth = inputWidth + 20;
            Raylib.DrawRectangle(20, 20+19, inputWidth, 24, "ff7c26".Hex2RGB());

            var hitRange = 0.0625f;
            var hitRangeD = hitRange - 0.0325f;
            var perfectRange = 0.03f;

            var hitNormalized = Mathf.Normalize(hitBeat, 0, actionLength);
            var hitWidth = (int)Mathf.Lerp(0f, inputWidth, hitRange + (hitRange - hitRangeD));
            Raylib.DrawRectangle(
                (int)Mathf.Lerp(20, inputWidth, hitNormalized), 
                20+19,
                hitWidth, 
                24,
                "6de23b".Hex2RGB());

            var normalizedX = (int)Mathf.Lerp(20, endWidth, normalizedPitchAnim);
            Raylib.DrawLineEx(new System.Numerics.Vector2(normalizedX, 20+19), new System.Numerics.Vector2(normalizedX, 20 + 19 + 24), 4, Trinkit.Color.black);*/
        }

        Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector3 result =
                (u3) * p0 +
                (3f * u2 * t) * p1 +
                (3f * u * t2) * p2 +
                (t3) * p3;

            return result;
        }

        public void Destroy()
        {
            Spaceball.Balls.Remove(this);
        }

        public override void Dispose()
        {
        }
    }
}
