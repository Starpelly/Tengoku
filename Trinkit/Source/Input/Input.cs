using Raylib_CsLo;

namespace Trinkit
{
    public class Input
    {
        /// <summary>
        /// Screen-space mouse position.
        /// </summary>
        public static Vector2 mousePosition { get { return GetMousePosition(); } set { SetMousePosition(value); } }

        /// <summary>
        /// Screen-space mouse position.
        /// </summary>
        public static Vector2 GetMousePosition()
        {
            return (Vector2)Raylib.GetMousePosition();
        }

        /// <summary>
        /// Sets the mouse position in screen-space relative to the window position.
        /// </summary>
        public static void SetMousePosition(Vector2 val)
        {
            Raylib.SetMousePosition((int)val.x, (int)val.y);
        }

        /// <summary>
        /// Returns true on the frame the user starts pressing down the specified key.
        /// </summary>
        public static bool GetKeyDown(KeyCode key)
        {
            return Raylib.IsKeyPressed((Raylib_CsLo.KeyboardKey)key);
        }

        /// <summary>
        /// Returns true on the frame the user stops pressing down the specified key.
        /// </summary>
        public static bool GetKeyUp(KeyCode key)
        {
            return Raylib.IsKeyReleased((Raylib_CsLo.KeyboardKey)key);
        }

        /// <summary>
        /// Returns true if the user is currently pressing down the specified key.
        /// </summary>
        public static bool GetKey(KeyCode key)
        {
            return Raylib.IsKeyDown((Raylib_CsLo.KeyboardKey)key);
        }

        /// <summary>
        /// Returns true on the frame the user starts pressing down the specified mouse button.
        /// </summary>
        public static bool GetMouseButtonDown(int button)
        {
            return Raylib.IsMouseButtonPressed((Raylib_CsLo.MouseButton)button);
        }

        /// <summary>
        /// Returns true on the frame the user stops pressing down the specified mouse button.
        /// </summary>
        public static bool GetMouseButtonUp(int button)
        {
            return Raylib.IsMouseButtonReleased((Raylib_CsLo.MouseButton)button);
        }

        /// <summary>
        /// Returns true if the user is currently pressing down the specified mouse button.
        /// </summary>
        public static bool GetMouseButton(int button)
        {
            return Raylib.IsMouseButtonDown((Raylib_CsLo.MouseButton)button);
        }
    }
}
