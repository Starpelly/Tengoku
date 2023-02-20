using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Games.Spaceball
{
    public class Spaceball
    {
        private Texture refTex;

        Raylib_CsLo.Camera3D _cam;

        public Texture SpaceballPlayerSheet0;
        private Texture _spaceballPlayerSheet1;
        private Texture _spaceballPlayerSheet2;
        private Texture _spaceballPlayerSheet3;

        public Texture TexSpaceballProps;
        private Texture _spaceballRoom;

        private Animator _playerAnim;

        private float _camPosZ = 10.0f;

        public List<Ball> Balls = new();

        public bool ZoomingEnabled = false;
        public float CameraZoomBeat = -4;
        public float CameraZoomLength = 0;
        public float CameraZoomZoom = 0;
        public float LastZoom = -80f;

        public Spaceball()
        {
            refTex = new Texture("resources/sprites/games/spaceball/refff.png");

            SpaceballPlayerSheet0 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_0.png");
            _spaceballPlayerSheet1 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_1.png");
            _spaceballPlayerSheet2 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_2.png");
            _spaceballPlayerSheet3 = new Texture("resources/sprites/games/spaceball/spaceball_player_sheet_3.png");

            TexSpaceballProps = new Texture("resources/sprites/games/spaceball/spaceball_props.png");
            _spaceballRoom = new Texture("resources/sprites/games/spaceball/spaceball_room.png");

            _cam = new Raylib_CsLo.Camera3D();
            _cam.projection_ = Raylib_CsLo.CameraProjection.CAMERA_PERSPECTIVE;

            _playerAnim = new Animator();
        }

        public void Update()
        {
            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -_camPosZ);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            if (ZoomingEnabled)
                _camPosZ = Mathf.Lerp(LastZoom, CameraZoomZoom, GameManager.Instance.Conductor.GetPositionFromBeat(CameraZoomBeat, CameraZoomLength));

            if (PlayerInput.GetPlayerDown())
            {
                // _playerAnim.Play();
            }
        }

        public void Draw()
        {
            Window.Clear(new Color("#000073"));
            Raylib_CsLo.Raylib.BeginMode3D(_cam);

            // Room
            Sprite.DrawSprite(_spaceballRoom, new Vector3(0.0f, 0.535f), 0.0f, Color.white, new Raylib_CsLo.Rectangle(), 90f);

            // Dispenser
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(-0.55f, -0.53f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(0, 32, 32, 32), 90f);

            // Umpire
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.0f, -0.11f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(32, 0, 32, 32), 90f);

            // Player Shadow
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.64f, -0.61f), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle(0, 128, 32, 32), 90f);

            // Player
            var playerYPos = -0.045f;

            _playerAnim.Update();

            var _playerFrame = _playerAnim.Frame;


            Sprite.DrawSprite(SpaceballPlayerSheet0, new Vector3(0.54f, playerYPos), 0.0f, Color.white, 
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * _playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet1, new Vector3(0.54f, playerYPos), 0.0f, new Color("63e600"),
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * _playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet2, new Vector3(0.54f, playerYPos), 0.0f, Color.black,
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * _playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet3, new Vector3(0.54f, playerYPos), 0.0f, Color.white,
                new Raylib_CsLo.Rectangle((SpaceballPlayerSheet0.Width / 5) * _playerFrame, 0, SpaceballPlayerSheet0.Width / 5, 0.0f), 90f);

            // Balls
            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].Draw();
            }

            Raylib_CsLo.Raylib.EndMode3D();
        }

        public void Ball(float beat, bool high)
        {
            var newBall = new Ball();
            newBall.Spaceball = this;
            newBall.StartBeat = beat;
            newBall.High = high;
            Balls.Add(newBall);
        }

        public void Zoom(float beat, float zoom, float length)
        {
            LastZoom = _camPosZ;

            ZoomingEnabled = true;
            CameraZoomBeat = beat;
            CameraZoomLength = length;
            CameraZoomZoom = zoom / 8.0f;
        }
    }
}
