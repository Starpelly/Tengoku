using System.ComponentModel;
using Raylib_CsLo;
using Trinkit;
using Trinkit.Audio;
using Trinkit.Graphics;

namespace Tengoku.Games
{
    public class Spaceball : IDisposable
    {
        private Texture refTex;

        private AudioSource music;
        private Sound _hitSound;

        Camera3D _cam;

        private Texture _spaceballPlayerSheet0;
        private Texture _spaceballPlayerSheet1;
        private Texture _spaceballPlayerSheet2;
        private Texture _spaceballPlayerSheet3;

        private Texture _spaceballProps;
        private Texture _spaceballRoom;

        private Animation _playerAnim;

        private float _camPosZ = 10.0f;

        public Spaceball()
        {
            refTex = Raylib.LoadTexture("resources/sprites/games/spaceball/refff.png");

            _spaceballPlayerSheet0 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_0.png");
            _spaceballPlayerSheet1 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_1.png");
            _spaceballPlayerSheet2 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_2.png");
            _spaceballPlayerSheet3 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_3.png");

            _spaceballProps = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_props.png");
            _spaceballRoom = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_room.png");

            music = new AudioSource();
            music.Clip = Resources.Load<AudioClip>("audio/music/remix1.ogg");
            music.Play();

            _hitSound = Raylib.LoadSound("Resources/audio/sfx/games/spaceball/hit.ogg");

            _cam = new Camera3D();
            _cam.projection_ = CameraProjection.CAMERA_PERSPECTIVE;

            _playerAnim = new Animation(5, 20);
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

        public void Update()
        {
            _cam.fovy = 10.125f;
            _cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -_camPosZ);
            _cam.target = new System.Numerics.Vector3(_cam.position.X, _cam.position.Y, 0.0f);
            _cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);

            // _camPosZ = Mathf.Lerp(10.0f, 200.0f, Mathf.Normalize(music.Time, 0, 10));
            
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
                Raylib.PlaySound(_hitSound);
            }

            music.Update();
        }

        public void Draw()
        {
            Raylib.ClearBackground(new Trinkit.Color("000073"));
            Raylib.BeginMode3D(_cam);

            // Room
            Sprite.DrawSprite(_spaceballRoom, new Vector3(0.0f, 0.535f), 0.0f, Trinkit.Color.white, new Rectangle(), 90f);

            // Dispenser
            Sprite.DrawSprite(_spaceballProps, new Vector3(-0.55f, -0.53f), 0.0f, Trinkit.Color.white,
                new Rectangle(0, 32, 32, 32), 90f);

            // Umpire
            Sprite.DrawSprite(_spaceballProps, new Vector3(0.0f, -0.11f), 0.0f, Trinkit.Color.white,
                new Rectangle(32, 0, 32, 32), 90f);

            // Player Shadow
            Sprite.DrawSprite(_spaceballProps, new Vector3(0.64f, -0.61f), 0.0f, Trinkit.Color.white,
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

            var addPos = 0.77f;
            var addPosY = 1.25f;
            var normalizedBallTime = Mathf.Repeat((float)music.Time, 1f);
            var ballRot = normalizedBallTime * 240f;

            // Ball
            Sprite.DrawSprite(_spaceballProps,
                GetPointOnBezierCurve(
                    new Vector3(-0.55f, -0.43f),
                    new Vector3(-0.55f, -0.53f + addPosY),
                    new Vector3(-0.55f + addPos, -0.53f + addPosY),
                    new Vector3(-0.55f + addPos, -0.62f),
                    normalizedBallTime
                    ),
                ballRot, Trinkit.Color.white,
                new Rectangle(0, 32 * 2, 32, 32), 90f);

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
            Raylib.DrawFPS(10, 29);
            Raylib.DrawCircle((int)Raylib.GetMousePosition().X, (int)Raylib.GetMousePosition().Y, 30, Trinkit.Color.black.ChangeAlpha(0.25f));
        }

        public void ImGui()
        {
        }

        public void Dispose()
        {
            music.OnDestroy();

            Raylib.UnloadSound(_hitSound);

            Raylib.UnloadTexture(refTex);

            Raylib.UnloadTexture(_spaceballPlayerSheet0);
            Raylib.UnloadTexture(_spaceballPlayerSheet1);
            Raylib.UnloadTexture(_spaceballPlayerSheet2);
            Raylib.UnloadTexture(_spaceballPlayerSheet3);
            Raylib.UnloadTexture(_spaceballProps);
            Raylib.UnloadTexture(_spaceballRoom);
        }
    }
}
