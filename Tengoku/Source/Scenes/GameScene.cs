using Tengoku.Games;
using Tengoku.Games.Spaceball;
using Trinkit;
using Trinkit.Audio;

namespace Tengoku.Scenes
{
    public class GameScene : Scene
    {
        public GameManager GameManager { get; private set; }

        public Minigame CurrentMinigame { get; set; }

        public override void Start()
        {
            GameManager = new GameManager();
            CurrentMinigame = new Spaceball();
        }

        public override void Update()
        {
            GameManager.Update();
            CurrentMinigame.Update();
        }

        public override void DrawBefore()
        {
        }

        public override void Draw()
        {
            CurrentMinigame.Draw();
        }

        public override void DrawGUI()
        {
            // CurrentMinigame.DrawGUI();
        }

        public override void OnExit()
        {
            CurrentMinigame.Dispose();
        }
    }
}
