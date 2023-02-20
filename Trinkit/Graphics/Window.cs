using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Window
    {
        /// <summary>
        /// Clears the background of the window with (color).
        /// </summary>
        public static void Clear(Color color)
        {
            Raylib.ClearBackground(color);
        }
    }
}
