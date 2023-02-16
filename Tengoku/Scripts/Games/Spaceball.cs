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

        Camera3D cam;

        private Texture spaceballPlayerSheet0;
        private Texture spaceballPlayerSheet1;
        private Texture spaceballPlayerSheet2;
        private Texture spaceballPlayerSheet3;

        private Texture spaceballProps;
        private Texture spaceballRoom;

        public Spaceball()
        {
            refTex = Raylib.LoadTexture("resources/sprites/games/spaceball/refff.png");

            spaceballPlayerSheet0 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_0.png");
            spaceballPlayerSheet1 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_1.png");
            spaceballPlayerSheet2 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_2.png");
            spaceballPlayerSheet3 = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_player_sheet_3.png");

            spaceballProps = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_props.png");
            spaceballRoom = Raylib.LoadTexture("resources/sprites/games/spaceball/spaceball_room.png");

            music = new AudioSource();
            music.Clip = Resources.Load<AudioClip>("audio/music/remix1.ogg");
            // music.Play();

            cam = new Camera3D();
            cam.projection_ = CameraProjection.CAMERA_PERSPECTIVE;
        }

        public void Update()
        {
            cam.fovy = 10.125f;
            cam.position = new System.Numerics.Vector3(0.0f, 0.0f, -10.0f);
            cam.target = new System.Numerics.Vector3(cam.position.X, cam.position.Y, 0.0f);
            cam.up = new System.Numerics.Vector3(0.0f, 1.0f, 0.0f);
            
            if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                cam.position += new System.Numerics.Vector3(5, 0, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                cam.position += new System.Numerics.Vector3(-5, 0, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                cam.position += new System.Numerics.Vector3(0, 5, 0) * Raylib.GetFrameTime();
            if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                cam.position += new System.Numerics.Vector3(0, -5, 0) * Raylib.GetFrameTime();


            music.Update();
        }

        public void Draw()
        {
            Raylib.ClearBackground(new Trinkit.Color("000073"));
            Raylib.BeginMode3D(cam);

            // Room
            Sprite.DrawSprite(spaceballRoom, new Vector3(0.0f, 0.535f), 0.0f, Trinkit.Color.white, new Rectangle(), 90f);

            // Dispenser
            Sprite.DrawSprite(spaceballProps, new Vector3(-0.55f, -0.53f), 0.0f, Trinkit.Color.white,
                new Rectangle(0, 32, 32, 32), 90f);

            // Umpire
            Sprite.DrawSprite(spaceballProps, new Vector3(0.0f, -0.11f), 0.0f, Trinkit.Color.white,
                new Rectangle(32, 0, 32, 32), 90f);

            // Player Shadow
            Sprite.DrawSprite(spaceballProps, new Vector3(0.64f, -0.61f), 0.0f, Trinkit.Color.white,
                new Rectangle(0, 128, 32, 32), 90f);

            // Player
            var playerFrame = 0;
            var playerYPos = -0.045f;

            Sprite.DrawSprite(spaceballPlayerSheet0, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.white, 
                new Rectangle((spaceballPlayerSheet0.width / 5) * playerFrame, 0, spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(spaceballPlayerSheet1, new Vector3(0.54f, playerYPos), 0.0f, new Trinkit.Color("63e600"),
                new Rectangle((spaceballPlayerSheet0.width / 5) * playerFrame, 0, spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(spaceballPlayerSheet2, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.black,
                new Rectangle((spaceballPlayerSheet0.width / 5) * playerFrame, 0, spaceballPlayerSheet0.width / 5, 0.0f), 90f);
            Sprite.DrawSprite(spaceballPlayerSheet3, new Vector3(0.54f, playerYPos), 0.0f, Trinkit.Color.white,
                new Rectangle((spaceballPlayerSheet0.width / 5) * playerFrame, 0, spaceballPlayerSheet0.width / 5, 0.0f), 90f);

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
            Raylib.DrawCircle((int)Raylib.GetMousePosition().X, (int)Raylib.GetMousePosition().Y, 10, Trinkit.Color.red);
        }

        public void ImGui()
        {
            if (ImGuiNET.ImGui.Begin("Spaceball Debug"))
            {
                ImGuiNET.ImGui.End();
            }
        }

        public void Dispose()
        {
            music.OnDestroy();

            Raylib.UnloadTexture(refTex);

            Raylib.UnloadTexture(spaceballPlayerSheet0);
            Raylib.UnloadTexture(spaceballPlayerSheet1);
            Raylib.UnloadTexture(spaceballPlayerSheet2);
            Raylib.UnloadTexture(spaceballPlayerSheet3);
            Raylib.UnloadTexture(spaceballProps);
            Raylib.UnloadTexture(spaceballRoom);
        }
    }
}
