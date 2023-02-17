using System.ComponentModel;
using Raylib_CsLo;
using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Games.Spaceball
{
    public class Spaceball : IDisposable
    {
        private Texture refTex;

        public Sound HitSound;
        public Sound ShootSound;
        public Sound ShootHighSound;

        Camera3D _cam;

        private Texture _spaceballPlayerSheet0;
        private Texture _spaceballPlayerSheet1;
        private Texture _spaceballPlayerSheet2;
        private Texture _spaceballPlayerSheet3;

        public Texture TexSpaceballProps;
        private Texture _spaceballRoom;

        private Animation _playerAnim;

        private float _camPosZ = 10.0f;

        public List<Ball> Balls = new();

        public bool ZoomingEnabled = false;
        public float CameraZoomBeat = -4;
        public float CameraZoomLength = 0;
        public float CameraZoomZoom = 0;
        public float LastZoom = -80f;

        public Spaceball()
        {
            refTex = Raylib.LoadTexture("resources/sprites/games/spaceball/refff.png");

            _spaceballPlayerSheet0 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_0.png");
            _spaceballPlayerSheet1 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_1.png");
            _spaceballPlayerSheet2 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_2.png");
            _spaceballPlayerSheet3 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_3.png");

            TexSpaceballProps = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_props.png");
            _spaceballRoom = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_room.png");

            HitSound = Raylib.LoadSound("Resources/audio/sfx/games/spaceball/hit.ogg");
            ShootSound = Raylib.LoadSound("Resources/audio/sfx/games/spaceball/shoot.ogg");
            ShootHighSound = Raylib.LoadSound("Resources/audio/sfx/games/spaceball/shootHigh.ogg");

            _cam = new Camera3D();
            _cam.projection_ = CameraProjection.CAMERA_PERSPECTIVE;

            _playerAnim = new Animation(5, 20);
        }

        public void Update()
        {
            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -_camPosZ);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            if (ZoomingEnabled)
            _camPosZ = Mathf.Lerp(LastZoom, CameraZoomZoom, GameManager.Instance.Conductor.GetPositionFromBeat(CameraZoomBeat, CameraZoomLength));
            
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                _cam.position += new System.Numerics.Vector3(5, 0, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                _cam.position += new System.Numerics.Vector3(-5, 0, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                _cam.position += new System.Numerics.Vector3(0, 5, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                _cam.position += new System.Numerics.Vector3(0, -5, 0) * Raylib.GetFrameTime();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _playerAnim.Play();
            }
        }

        public void Draw()
        {
            Raylib.ClearBackground(new Trinkit.Color("000073"));
            Raylib.BeginMode3D(_cam);

            // Room
            Sprite.DrawSprite(_spaceballRoom, new Vector3(0.0f, 0.535f), 0.0f, Trinkit.Color.white, new Rectangle(), 90f);

            // Dispenser
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(-0.55f, -0.53f), 0.0f, Trinkit.Color.white,
                new Rectangle(0, 32, 32, 32), 90f);

            // Umpire
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.0f, -0.11f), 0.0f, Trinkit.Color.white,
                new Rectangle(32, 0, 32, 32), 90f);

            // Player Shadow
            Sprite.DrawSprite(TexSpaceballProps, new Vector3(0.64f, -0.61f), 0.0f, Trinkit.Color.white,
                new Rectangle(0, 128, 32, 32), 90f);

            // Player
            var playerYPos = -0.045f;

            _playerAnim.Update();

            var _playerFrame = _playerAnim.Frame;


            Sprite.DrawSprite(_spaceballPlayerSheet0, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.white, 
                new Rectangle((_spaceballPlayerSheet0.width / 5) * _playerFrame, 0, _spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet1, new Vector3(0.54f, playerYPos), 0.0f, new Trinkit.Color("63e600"),
                new Rectangle((_spaceballPlayerSheet0.width / 5) * _playerFrame, 0, _spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet2, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.black,
                new Rectangle((_spaceballPlayerSheet0.width / 5) * _playerFrame, 0, _spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(_spaceballPlayerSheet3, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.white,
                new Rectangle((_spaceballPlayerSheet0.width / 5) * _playerFrame, 0, _spaceballPlayerSheet0.width / 5, 0.0f), 90f);

            // Balls
            for (int i = 0; i < Balls.Count; i++)
            {
                Balls[i].Draw();
            }

            Raylib.EndMode3D();
        }

        public void DrawGUI()
        {
            /*Raylib.DrawTexturePro(
                refTex,
                    new Rectangle(0, 0, 280, 160),
                    new Rectangle(0, 19, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - 19),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    new Trinkit.Color(1, 1, 1, 0.5f)
                );*/

            _textAdd = 0;

            DebugText($"Rhythm Tengoku '{Environment.UserName}' Debug");
            DebugSeparator();
            DebugText($"FPS: {Raylib.GetFPS()}");
            DebugText($"Time: {Raylib.GetTime()}");
            DebugText($"Mouse: {Raylib.GetMousePosition()}");
            DebugText($"Sprites: {Trinkit.Debug.Counters.SpritesRendered}");
            DebugSeparator();
            DebugText("Game: Spaceball");
            DebugSeparator();
            DebugText($"BPM: {GameManager.Instance.Conductor.InitialTempo}");
            DebugText($"SongPos: {GameManager.Instance.Conductor.SongPosition}");
            DebugText($"SongPosBeat: {GameManager.Instance.Conductor.SongPositionInBeats}");
            DebugSeparator();

            Raylib.DrawCircle((int)Raylib.GetMousePosition().X, (int)Raylib.GetMousePosition().Y, 30, Trinkit.Color.black.ChangeAlpha(0.25f));
        }

        private int _textAdd;

        private void DebugText(string text)
        {
            Raylib.DrawText(text, 10, 29 + (20 * _textAdd), 20, Trinkit.Color.black);
            _textAdd++;
        }

        private void DebugSeparator()
        {
            DebugText($"===================================");
        }

        public void ImGui()
        {
        }

        public void Dispose()
        {
            Raylib.UnloadSound(HitSound);
            Raylib.UnloadSound(ShootSound);
            Raylib.UnloadSound(ShootHighSound);

            Raylib.UnloadTexture(refTex);

            Raylib.UnloadTexture(_spaceballPlayerSheet0);
            Raylib.UnloadTexture(_spaceballPlayerSheet1);
            Raylib.UnloadTexture(_spaceballPlayerSheet2);
            Raylib.UnloadTexture(_spaceballPlayerSheet3);
            Raylib.UnloadTexture(TexSpaceballProps);
            Raylib.UnloadTexture(_spaceballRoom);
        }

        public void Ball(float beat, bool high)
        {
            Raylib.PlaySound((high) ? ShootHighSound : ShootSound);

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
