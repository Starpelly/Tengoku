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

        RenderTexture renderTexture;

        private float virtualRatio = 1;
        private int _screenWidth = 280;
        private int _screenHeight = 160;

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

            renderTexture = Raylib.LoadRenderTexture(_screenWidth, _screenHeight);
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
            Raylib.BeginTextureMode(renderTexture);
            Raylib.ClearBackground(new Trinkit.Color("000073"));
            Raylib.BeginMode3D(cam);


            Sprite.DrawSprite(spaceballRoom, new Rectangle(0, 0, spaceballRoom.width, spaceballRoom.height), new Vector3(0.0f, 0.48f), Vector2.one, 0.0f, Trinkit.Color.white);
            Sprite.DrawSprite(spaceballProps, new Rectangle(32*2, 0, 32, 32), new Vector3(0.0f, 0.0f), Vector2.one, 0.0f, Trinkit.Color.white);

            // Sprite.DrawSprite(refTex, new Rectangle(0, 0, refTex.width, refTex.height), new Vector3(), Vector2.one, 0.0f, Trinkit.Color.white);
            
            // Player Shadow
            Sprite.DrawSprite(spaceballProps, new Rectangle(32*0, 32*4, 32, 32), new Vector3(-0.58f, -0.55f), Vector2.one, 0.0f, Trinkit.Color.white);

            // Player body
            Sprite.DrawSprite(spaceballPlayerSheet0, new Rectangle(0, 0, spaceballPlayerSheet0.width/5, spaceballPlayerSheet0.height), new Vector3(-0.48f, -0.04f), Vector2.one, 0.0f, Trinkit.Color.white);
            Sprite.DrawSprite(spaceballPlayerSheet1, new Rectangle(0, 0, spaceballPlayerSheet1.width/5, spaceballPlayerSheet1.height), new Vector3(-0.48f, -0.04f), Vector2.one, 0.0f, new Trinkit.Color("63e600"));
            Sprite.DrawSprite(spaceballPlayerSheet2, new Rectangle(0, 0, spaceballPlayerSheet2.width/5, spaceballPlayerSheet2.height), new Vector3(-0.48f, -0.04f), Vector2.one, 0.0f, Trinkit.Color.black);
            Sprite.DrawSprite(spaceballPlayerSheet3, new Rectangle(0, 0, spaceballPlayerSheet3.width/5, spaceballPlayerSheet3.height), new Vector3(-0.48f, -0.04f), Vector2.one, 0.0f, Trinkit.Color.white);
            

            Raylib.EndMode3D();
            Raylib.EndTextureMode();

            Raylib.ClearBackground(Raylib.BLACK);

            Raylib.DrawTexturePro(
                renderTexture.texture,
                    new Rectangle(0, 0, (float)renderTexture.texture.width, (float)-renderTexture.texture.height),
		            new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
		            new System.Numerics.Vector2(0.0f, 0.0f),
		            0.0f,
		            Raylib.WHITE
	            );

            /*Raylib.DrawTexturePro(
                refTex,
                    new Rectangle(0, 0, 280, 160),
                    new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),
                    new System.Numerics.Vector2(0.0f, 0.0f),
                    0.0f,
                    new Trinkit.Color(1, 1, 1, 0.5f)
                );*/
        }

        public void Dispose()
        {
            music.OnDestroy();

            Raylib.UnloadTexture(refTex);

            Raylib.UnloadTexture(spaceballPlayerSheet0);

            Raylib.UnloadRenderTexture(renderTexture);
        }
    }
}
