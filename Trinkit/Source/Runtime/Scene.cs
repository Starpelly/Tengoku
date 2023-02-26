namespace Trinkit
{
    public class Scene
    {
        public readonly List<Component> SceneComponents = new List<Component>();

        public void AddComponent(Component component)
        {
            SceneComponents.Add(component);
        }

        public void ClearComponents()
        {
            for (int i = 0; i < SceneComponents.Count; i++)
            {
                SceneComponents[i].Dispose();
            }
            SceneComponents.Clear();
        }

        public float Clock { get; private set; }

        public virtual void Update()
        {
            Clock += Time.deltaTime;
        }
        public virtual void DrawBefore() { }
        public virtual void Draw() { }
        public virtual void DrawGUI() { }
        public virtual void OnExit() { }
    }
}
