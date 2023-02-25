namespace Trinkit
{
    public abstract class Scene
    {
        public abstract void Update();
        public abstract void DrawBefore();
        public abstract void Draw();
        public abstract void DrawGUI();
        public abstract void OnExit();
    }
}
