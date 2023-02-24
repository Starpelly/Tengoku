using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Games.Spaceball
{
    public class Spaceball : IDisposable
    {
        public Texture refTex;

        public Texture SpaceballPlayerSheet0;
        public Texture SpaceballPlayerSheet1;
        public Texture SpaceballPlayerSheet2;
        public Texture SpaceballPlayerSheet3;

        public Texture[] HatSprites = new Texture[]
        {
            new Texture(),
            new Texture("resources/sprites/games/spaceball/spaceball_player_hat_0.png"),
            new Texture("resources/sprites/games/spaceball/spaceball_player_hat_1.png"),
            new Texture("resources/sprites/games/spaceball/spaceball_player_hat_2.png"),
        };

        public Texture TexSpaceballProps;
        public Texture SpaceballRoom;

        public List<Ball> Balls = new();

        public bool ZoomingEnabled = false;
        public float CameraZoomBeat = -4;
        public float CameraZoomLength = 0;
        public float CameraZoomZoom = 0;
        public float LastZoom = -80f;

        public Raylib_CsLo.Sound HitSnd;
        public Raylib_CsLo.Sound ShootSnd;
        public Raylib_CsLo.Sound ShootHighSnd;

        private float _camPosZ = 10.0f;
        private int _currentCostume;
        private Animator _playerAnim;
        private Animator _dispenserAnim;
        private Animator _umpireAnim;
        private Raylib_CsLo.Camera3D _cam;

        private List<Vector3> _stars = new();
        private float _starsClock = 0.0f;
        private float _nextStarTime = 0.0f;
        private float _nextStarPeriod = 0.03f;
        private int _maxStars = 300;

        private Color[] playerColors = new Color[]
        {
            Color.white,
            "63e600".Hex2RGB(),
            Color.black
        };

        public Spaceball()
        {
            SpaceballPlayerSheet0 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_0.png");
            SpaceballPlayerSheet1 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_1.png");
            SpaceballPlayerSheet2 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_2.png");
            SpaceballPlayerSheet3 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_3.png");
            
            refTex = new Texture("resources/sprites/games/spaceball/spaceball_ref.png");

            TexSpaceballProps = new Texture("resources/sprites/games/spaceball/spaceball_props.png");
            SpaceballRoom = new Texture("resources/sprites/games/spaceball/spaceball_room_alt.png");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;

            ShootSnd = Raylib_CsLo.Raylib.LoadSound("resources/audio/sfx/games/spaceball/shoot.ogg");
            ShootHighSnd = Raylib_CsLo.Raylib.LoadSound("resources/audio/sfx/games/spaceball/shootHigh.ogg");
            HitSnd = Raylib_CsLo.Raylib.LoadSound("resources/audio/sfx/games/spaceball/hit.ogg");

            _playerAnim = new Animator("resources/animations/spaceballplayer.json");
            _dispenserAnim = new Animator("resources/animations/spaceballdispenser.json");
            _umpireAnim = new Animator("resources/animations/spaceballumpire.json");
        }

        public void Update()
        {
            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -_camPosZ);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            if (ZoomingEnabled)
                _camPosZ = Mathf.Lerp(LastZoom, CameraZoomZoom, Conductor.Instance.GetPositionFromBeat(CameraZoomBeat, CameraZoomLength));

            if (PlayerInput.GetPlayerDown())
            {
                _playerAnim.Play("SpaceballPlayerSwing");
            }

            _playerAnim.Update();
            _dispenserAnim.Update();
            _umpireAnim.Update();

            if (_starsClock > _nextStarTime)
            {
                _nextStarTime += _nextStarPeriod;

                var maxStarRangeX = 1.75f;
                var maxStarRangeXMin = 1.75f;
                var maxStarRangeY = 2.45f;
                _stars.Add(
                    new Vector3(
                        Trinkit.Random.Range(-maxStarRangeX - maxStarRangeXMin, maxStarRangeXMin + maxStarRangeX),
                        Trinkit.Random.Range(-maxStarRangeY, maxStarRangeY),
                        -6f
                        ));
                if (_stars.Count > _maxStars)
                    _stars.RemoveAt(0);

            }
            _starsClock += Time.deltaTime;
        }

        public void Draw()
        {
            Window.Clear(new Color("#000073"));
            Raylib_CsLo.Raylib.BeginMode3D(_cam);

            for (int i = 0; i < _stars.Count; i++)
            {
                Vector3 star = _stars[i];
                _stars[i] -= new Vector3(0, 0, Time.deltaTime * 30f);
                _stars[i] += new Vector3(_stars[i].x * 0.2f * Time.deltaTime, _stars[i].y * 0.2f * Time.deltaTime, 0f);
                Sprite.DrawSprite(TexSpaceballProps, star, 0.0f, Color.white, new Raylib_CsLo.Rectangle(32, 160, 32, 32), 40);
            }

            // Room
            Sprite.DrawSprite(SpaceballRoom, new Vector3(0.0f, 0.535f), 0.0f, Color.white, new Raylib_CsLo.Rectangle(), 90f);

            var dispenserFrame = 0;
            switch (_dispenserAnim.CurrentAnimation.Name)
            {
                case "SpaceballDispenserIdle":
                    dispenserFrame = 2;
                    break;
                case "SpaceballDispenserShoot":
                    dispenserFrame = _dispenserAnim.CurrentFrame + 1;
                    break;
            }

            // Dispenser
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(-0.55f, -0.53f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(32 * dispenserFrame, 64, 32, 32), 90f);

            int umpireY = 0;
            if (_umpireAnim.CurrentAnimation.Name == "SpaceballUmpireShow")
                umpireY = 32;

            // Umpire
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.0f, -0.11f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(32 * _umpireAnim.CurrentFrame, umpireY, 32, 32), 90f);

            // Player Shadow
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.64f, -0.61f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(0, 160, 32, 32), 90f);

            // Player
            var playerYPos = 0.38f;

            var playerFrame = (_playerAnim.CurrentAnimation.Name == "SpaceballPlayerSwing") ? _playerAnim.CurrentFrame + 1 : 0;

            Sprite.DrawSprite(SpaceballPlayerSheet0, new Vector3(0.54f, playerYPos), 0.0f, playerColors[0], 
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(SpaceballPlayerSheet1, new Vector3(0.54f, playerYPos), 0.0f, playerColors[1],
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(SpaceballPlayerSheet2, new Vector3(0.54f, playerYPos), 0.0f, playerColors[2],
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(SpaceballPlayerSheet3, new Vector3(0.54f, playerYPos), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);

            var hatSprite = HatSprites[_currentCostume];
            if (_currentCostume > 0)
            Sprite.DrawSprite(hatSprite, new Vector3(0.54f, playerYPos), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle((hatSprite.Width / 5) * playerFrame, 0, hatSprite.Width / 5, 0.0f), 90f);

            // Balls
            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].Draw();
            }
            Raylib_CsLo.Raylib.EndMode3D();
        }

        public void DrawGUI()
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].DrawGUI();
            }

            /*Raylib_CsLo.Raylib.DrawTexturePro(
                refTex,
                new Raylib_CsLo.Rectangle(0, 0, 280, 160),
                new Raylib_CsLo.Rectangle(0, 19, GameWindow.Width, GameWindow.Height - 19),
                new System.Numerics.Vector2(0, 0),
                0,
                new Color(1, 1, 0.5f, 0.5f));*/
        }

        public void Ball(float beat, bool high, bool riceball = false)
        {
            var newBall = new Ball(this);
            newBall.StartBeat = beat;
            newBall.High = high;
            newBall.Riceball = riceball;

            newBall.Start();
            Balls.Add(newBall);

            _dispenserAnim.Play("SpaceballDispenserShoot");
        }

        public void Zoom(float beat, float zoom, float length)
        {
            LastZoom = _camPosZ;

            ZoomingEnabled = true;
            CameraZoomBeat = beat;
            CameraZoomLength = length;
            CameraZoomZoom = zoom / 8.0f;
        }

        public void DispenserPrepare()
        {
            _dispenserAnim.Play("SpaceballDispenserPrepare");
        }

        public void Umpire(bool show)
        {
            _umpireAnim.Play((show) ? "SpaceballUmpireShow" : "SpaceballUmpireIdle");
        }

        public void Costume(int costume, string color1, string color2, string color3)
        {
            _currentCostume = costume;
            playerColors = new Color[]
            {
                color1.Hex2RGB(),
                color2.Hex2RGB(),
                color3.Hex2RGB()
            };
        }

        public void Dispose()
        {
            Raylib_CsLo.Raylib.UnloadSound(HitSnd);
            Raylib_CsLo.Raylib.UnloadSound(ShootSnd);
            Raylib_CsLo.Raylib.UnloadSound(ShootHighSnd);
        }
    }
}
