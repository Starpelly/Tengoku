using Raylib_CsLo;

namespace Trinkit
{
    public class Time
    {
        /// <summary>
        /// The time in seconds since the last frame.
        /// The time in seconds it took to complete the last frame.
        /// </summary>
        public static float deltaTime => GetDeltaTime();

        /// <summary>
        /// The time in seconds since the current scene started.
        /// </summary>
        public static float Clock { get; set; }

        private static float GetDeltaTime()
        {
            return Raylib.GetFrameTime();
        }
    }
}
