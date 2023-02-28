using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class Sound : Component
    {
        private Raylib_CsLo.Sound _internSound { get; set; }

        public Sound(string location)
        {
            _internSound = Raylib.LoadSound(location);
        }

        public void Play()
        {
            Raylib.PlaySound(_internSound);
        }

        public override void Dispose()
        {
            Raylib.UnloadSound(_internSound);
        }
    }
}
