using Tengoku.Games.Spaceball;
using Trinkit;

namespace Tengoku.Scenes
{
    public class GameScene : Scene
    {
        public GameManager GameManager { get; private set; }

        public Spaceball Spaceball { get; set; }

        public GameScene()
        {
            GameManager = new GameManager();
            Spaceball = new Spaceball();
        }

        public override void Update()
        {
            GameManager.Update();
            Spaceball.Update();
        }

        public override void DrawBefore()
        {
        }

        public override void Draw()
        {
            Spaceball.Draw();
        }

        public override void DrawGUI()
        {
            Spaceball.DrawGUI();
        }

        public override void OnExit()
        {
            Spaceball.Dispose();
        }
    }
}
