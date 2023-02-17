using Raylib_CsLo;
using Trinkit;
using Trinkit.Graphics;

namespace Tengoku.Games.Spaceball
{
    public class Ball : Behavior
    {
        public Spaceball Spaceball { get; set; }

        public float StartBeat;
        public bool High;

        private float _hitBeat;
        private Vector3 _hitPos;
        private float randomEndPosX;
        private bool _hit;
        private Vector3 _lastPos;
        private float _lastRot;

        public override void Update()
        {
        }

        public override void Draw()
        {
            if (!_hit)
            {
                var length = (High) ? 2 : 1;
                var normalizedPitchAnim = GameManager.Instance.Conductor.GetPositionFromBeat(StartBeat, length + 0.15f);

                if (normalizedPitchAnim > 1.0f)
                {

                }
                else
                {

                    var addPos = 0.77f;
                    var addPosY = (High) ? 2.5f : 1.35f;
                    var ballRot = normalizedPitchAnim * 440f;

                    if (PlayerInput.GetPlayerDown())
                    {
                        _hitBeat = GameManager.Instance.Conductor.SongPositionInBeats;
                        _hit = true;
                        _hitPos = _lastPos;
                        _lastRot = ballRot;
                        Raylib.PlaySound(Spaceball.HitSound);
                    }


                    _lastPos = GetPointOnBezierCurve(
                            new Vector3(-0.55f, -0.43f),
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
            }
            else
            {
                var nba = GameManager.Instance.Conductor.GetPositionFromBeat(_hitBeat, 14);
                Sprite.DrawSprite(Spaceball.TexSpaceballProps,
                    Vector3.Lerp(_hitPos, new Vector3(0f, 0, -1300f), nba),
                    _lastRot * nba * 12f, Trinkit.Color.white,
                    new Rectangle(0, 32 * 2, 32, 32), 90f);
            }
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

    }
}
