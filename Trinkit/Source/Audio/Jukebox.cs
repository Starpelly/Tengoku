using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class Jukebox
    {
        private static List<Sound> Sounds = new List<Sound>();

        public static void PlayOneShot(string location)
        {
            var snd = new Sound(location);
        }

        public static void CloseJukebox()
        {
            for (int i = 0; i < Sounds.Count; i++)
            {
                Sounds[i].Dispose();
            }
        }
    }
}
