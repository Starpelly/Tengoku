using Raylib_CsLo;
using Trinkit;

namespace Tengoku.UI
{
    public class DSGuy : Component
    {
        private float _realRadius;
        private float _growSpeed = 140.0f;
        private int _radius => (int)_realRadius;

        public override void Dispose()
        {
        }

        public override void DrawGUI()
        {
            var pos = Input.mousePosition;
            // pos.x /= (GameWindow.Width / 280.0f);
            // pos.y /= (GameWindow.Height / 160.0f);

            if (PlayerInput.GetPlayer())
                _realRadius += Time.DeltaTime * _growSpeed;
            else
                _realRadius -= Time.DeltaTime * _growSpeed;

            _realRadius = Mathf.Clamp(_realRadius, 0, 16);

            if (_realRadius != 0)
            {
                Raylib.DrawCircle((int)pos.x - 4, (int)pos.y - _radius - 2 - 4, 3, Trinkit.Color.white);
                Raylib.DrawCircle((int)pos.x - 4, (int)pos.y - _radius - 2 - 4, 1, Trinkit.Color.black);
                Raylib.DrawCircle((int)pos.x + 4, (int)pos.y - _radius - 2 - 4, 3, Trinkit.Color.white);
                Raylib.DrawCircle((int)pos.x + 4, (int)pos.y - _radius - 2 - 4, 1, Trinkit.Color.black);

                Raylib.DrawCircle((int)pos.x, (int)pos.y, _radius, Trinkit.Color.black.ChangeAlpha(0.5f));
            }
        }
    }
}
