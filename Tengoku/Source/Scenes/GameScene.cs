using Tengoku.Games;
using Tengoku.Games.Spaceball;
using Trinkit;
using Trinkit.Audio;

namespace Tengoku.Scenes
{
    public class GameScene : Scene
    {
        public GameManager GameManager { get; private set; }
        public Conductor PConductor { get; private set; }

        public Minigame CurrentMinigame { get; set; }

        public static GameScene Instance { get; private set; }
        public static Conductor Conductor => Instance.PConductor;

        public GameScene()
        {
            Instance = this;
        }

        public override void Start()
        {
            PConductor = new Conductor();
            GameManager = new GameManager();
            CurrentMinigame = new Spaceball();
        }

        public override void Update()
        {
            GameManager.Update();
            PConductor.Update();
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
