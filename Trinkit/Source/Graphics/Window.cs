using Raylib_CsLo;

namespace Trinkit.Graphics
{
    public class Window
    {
        public static int Width => Raylib.GetScreenWidth();
        public static int Height => Raylib.GetScreenHeight();

        /// <summary>
        /// Clears the background of the window with (color).
        /// </summary>
        public static void Clear(Color color)
        {
            Raylib.ClearBackground(color);
        }
    }
}
