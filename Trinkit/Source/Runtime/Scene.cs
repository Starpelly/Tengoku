namespace Trinkit
{
    public abstract class Scene
    {
        public readonly List<Component> SceneComponents = new List<Component>();

        public void AddComponent(Component component)
        {
            SceneComponents.Add(component);
        }

        public void ClearComponents(bool destroyOnLoadCheck = true)
        {
            for (int i = 0; i < SceneComponents.Count; i++)
            {
                var component = SceneComponents[i];

                if (destroyOnLoadCheck)
                if (component.DontDestroyOnLoad)
                    continue;

                component.Dispose();
            }
            SceneComponents.Clear();
        }

        public abstract void Start();
        public virtual void Update()
        {
        }
        public virtual void DrawBefore() { }
        public virtual void Draw() { }
        public virtual void DrawGUI() { }
        public virtual void OnExit() { }
    }
}
