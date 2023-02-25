using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Scenes
{
    public class GameSelect : Scene
    {
        private float SceneClock;

        public float sat = 1f;
        private Color color1;
        private Color color2;

        private int currentGameColumn;
        private int currentGameRow;

        private Texture _gameIcons;
        private Texture _extraIcons;
        private Texture _square;
        private Texture _selection;

        private List<Vector3> _squares = new();
        private float _squaresClock = 0.0f;
        private float _nextSquareTime = 0.0f;
        private float _nextSquarePeriod = 0.03f;
        private int _maxSquares = 300;

        private Raylib_CsLo.Camera3D _cam;

        private AudioSource music;

        public GameSelect()
        {
            _gameIcons = new Texture("resources/sprites/gameselect/gameicons.png");
            _extraIcons = new Texture("resources/sprites/gameselect/extras.png");
            _selection = new Texture("resources/sprites/gameselect/selection.png");
            _square = new Texture("resources/sprites/square.png");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;

            music = new AudioSource();
            music.Clip = Resources.Load<AudioClip>("audio/music/remix1.ogg");
            music.Play();
        }

        public override void Update()
        {
            music.Update();

            if (Input.GetKeyDown(KeyCode.Up))
            {
                currentGameRow -= 1;
            }
            if (Input.GetKeyDown(KeyCode.Down))
            {
                currentGameRow += 1;
            }
            if (Input.GetKeyDown(KeyCode.Left))
            {
                currentGameColumn -= 1;
            }
            if (Input.GetKeyDown(KeyCode.Right))
            {
                currentGameColumn += 1;
            }
            currentGameColumn = Mathf.Clamp(currentGameColumn, 0, 7);
            currentGameRow = Mathf.Clamp(currentGameRow, -5, 0);

            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(Mathf.Lerp(_cam.position.X, -currentGameColumn * (0.24f + 0.2f) - 0.2f, Time.deltaTime * 20f), Mathf.Lerp(_cam.position.Y, -currentGameRow * 0.26f, Time.deltaTime * 20f), -10.0f);
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
                        Trinkit.Random.Range(-4f, 2f),
                        Trinkit.Random.Range(15f, -6f)
                        ));
                if (_squares.Count > _maxSquares)
                    _squares.RemoveAt(0);

            }
            _squaresClock += Time.deltaTime;
        }

        public override void DrawBefore()
        {
            Window.Clear(Color.black);

            Raylib_CsLo.Raylib.DrawRectangleGradientV(0, 0, Game.ViewWidth, Game.ViewHeight, color2, color1);
            Raylib_CsLo.Raylib.DrawRectangle(0, 0, Game.ViewWidth, Game.ViewHeight, Color.Lerp(Color.white, Color.transparentWhite, SceneClock / 1.25f));
        }

        public override void Draw()
        {
            Raylib_CsLo.Raylib.BeginMode3D(_cam);

            for (int i = 0; i < _squares.Count; i++)
            {
                _squares[i] += new Vector3(-2.25f * Time.deltaTime, 0, 0);
                Sprite.DrawSprite(_square, _squares[i], _squaresClock * -660f, new Color(1, 1, 1, 0.35f), new Raylib_CsLo.Rectangle(), 90.0f);
            }

            for (int column = 0; column < 8; column++)
            {
                for (int row = 0; row < 6; row++)
                    DrawGame(column, row);
            }

            Sprite.DrawSprite(_selection, new Vector3(currentGameColumn * (0.24f + 0.2f), currentGameRow * -0.26f), 0, Color.white, new Raylib_CsLo.Rectangle(0, 26*3, 26, 26), 90.0f);

            Raylib_CsLo.Raylib.EndMode3D();

            var descriptionPanelY = (int)(29 * Game.AspectRatio);
            Raylib_CsLo.Raylib.DrawRectangle((int)(180 * Game.AspectRatio), descriptionPanelY, (int)(96 * Game.AspectRatio), (int)(97 * Game.AspectRatio), Color.black);
            Raylib_CsLo.Raylib.DrawRectangle((int)(181 * Game.AspectRatio), descriptionPanelY+ (int)(1 * Game.AspectRatio), (int)(94 * Game.AspectRatio), (int)(15 * Game.AspectRatio), "2030a8".Hex2RGB());
            Raylib_CsLo.Raylib.DrawRectangle((int)(181 * Game.AspectRatio), descriptionPanelY+ (int)(16 * Game.AspectRatio), (int)(94 * Game.AspectRatio), (int)(80 * Game.AspectRatio), Color.white);
        }

        private void DrawGame(int column, int row)
        {
            Sprite.DrawSprite(_gameIcons, new Vector3(((0.24f + 0.2f) * column), (0.26f * row), 0), 0, Color.white, new Raylib_CsLo.Rectangle(24 * row, 24 * column, 24, 24), 90.0f);
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

        public override void OnExit()
        {
        }
    }
}
