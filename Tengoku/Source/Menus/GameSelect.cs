using Trinkit;
using Trinkit.Graphics;

namespace Tengoku.Menus
{
    public class GameSelect : Scene
    {
        private float SceneClock;

        public float sat = 1f;
        private Color color1;
        private Color color2;


        private Texture _gameIcons;
        private Texture _extraIcons;
        private Texture _square;

        private RenderTexture _bgTexture;

        private List<Vector3> _squares = new();
        private float _squaresClock = 0.0f;
        private float _nextSquareTime = 0.0f;
        private float _nextSquarePeriod = 0.03f;
        private int _maxSquares = 300;

        private Raylib_CsLo.Camera3D _cam;

        public GameSelect()
        {
            _bgTexture = new RenderTexture(1, 32);
            _gameIcons = new Texture("resources/sprites/gameselect/gameicons.png");
            _extraIcons = new Texture("resources/sprites/gameselect/extras.png");
            _square = new Texture("resources/sprites/square.png");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;

            color1 = Color.white;
        }

        public override void Update()
        {
            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -10.0f);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            SceneClock += Time.deltaTime;


            color1 = ShiftHueBy(color1, ((SceneClock > 0.4f) ? 0.195f : 0.8f) * Time.deltaTime);
            if (SceneClock < 0.02f) return;
            color2 = ShiftHueBy(color2, 0.195f * Time.deltaTime);

            if (_squaresClock > _nextSquareTime)
            {
                _nextSquareTime += _nextSquarePeriod;

                _squares.Add(
                    new Vector3(
                        8f,
                        Trinkit.Random.Range(-2f, 2f),
                        Trinkit.Random.Range(15f, -6f)
                        ));
                if (_squares.Count > _maxSquares)
                    _squares.RemoveAt(0);

            }
            _squaresClock += Time.deltaTime;
        }

        public override void DrawBefore()
        {
            
            _bgTexture.Begin();

            Raylib_CsLo.Raylib.DrawRectangleGradientV(0, 0, Game.ViewWidth, 32, color2, color1);
            Raylib_CsLo.Raylib.DrawRectangle(0, 0, Game.ViewWidth, 32, Color.Lerp(Color.white, Color.transparentWhite, SceneClock / 1.25f));

            _bgTexture.End();
        }

        public override void Draw()
        {
            Window.Clear(Color.black);
            
            Raylib_CsLo.Raylib.DrawTexturePro(
                _bgTexture.texture,
                        new Raylib_CsLo.Rectangle(0, 0, (float)_bgTexture.texture.width, (float)-_bgTexture.texture.height),
                        new Raylib_CsLo.Rectangle(0, 0, Game.ViewWidth, Game.ViewHeight),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    Color.white
                );

            Raylib_CsLo.Raylib.BeginMode3D(_cam);

            for (int i = 0; i < _squares.Count; i++)
            {
                _squares[i] += new Vector3(-2.25f * Time.deltaTime, 0, 0);
                Sprite.DrawSprite(_square, _squares[i], _squaresClock * -660f, new Color(1, 1, 1, 0.35f), new Raylib_CsLo.Rectangle(), 90.0f);
            }

            for (int i = 0; i < 6; i++)
                DrawGame(i);

            Raylib_CsLo.Raylib.EndMode3D();

            var descriptionPanelY = 29;
            Raylib_CsLo.Raylib.DrawRectangleLines(180, descriptionPanelY, 96, 97, Color.black);
            Raylib_CsLo.Raylib.DrawRectangle(181, descriptionPanelY+1, 94, 15, "2030a8".Hex2RGB());
            Raylib_CsLo.Raylib.DrawRectangle(181, descriptionPanelY+16, 94, 80, Color.white);
        }

        private void DrawGame(int i)
        {
            var xPos = -0.4f;
            var yPos = 0.1f;
            if (i == 0)
            {
                Sprite.DrawSprite(_gameIcons, new Vector3(xPos, (0.24f * i) - yPos, 0), 0, Color.white, new Raylib_CsLo.Rectangle(24*i, 0, 24, 24), 90.0f);
            }
            else
            {
                Sprite.DrawSprite(_extraIcons, new Vector3(xPos, (0.24f * i) - yPos, 0), 0, Color.white, new Raylib_CsLo.Rectangle(24*2, 24*2, 24, 24), 90.0f);
            }
        }


        private Color ShiftHueBy(Color color, float amount)
        {
            // convert from RGB to HSV
            Color.RGBToHSV(color, out float hue, out float sat, out float val);

            // shift hue by amount
            hue += amount;
            sat = this.sat;
            val = 1f;

            // convert back to RGB and return the color
            return Color.HSVToRGB(hue, sat, val);
        }

        public override void DrawGUI()
        {
        }
    }
}
