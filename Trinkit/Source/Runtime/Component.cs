namespace Trinkit
{
    public abstract class Component : Object
    {
        public Component()
        {
            if (TrinkitApp.Instance.CurrentScene != null)
                TrinkitApp.Instance.CurrentScene?.AddComponent(this);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        public virtual void Start() { }
        /// <summary>
        /// Update is called every frame, if the Behaviour is enabled.
        /// </summary>
        public virtual void Update() { }
        /// <summary>
        /// Draw is called every draw call and draws in world-space, if the Behaviour is enabled.
        /// </summary>
        public virtual void Draw() { }
        /// <summary>
        /// DrawGUI is called every draw call and draws in screen-space, if the Behaviour is enabled.
        /// </summary>
        public virtual void DrawGUI() { }

        public abstract void Dispose();
    }
}
