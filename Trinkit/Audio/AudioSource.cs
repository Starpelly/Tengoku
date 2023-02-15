using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class AudioSource : Behavior
    {
        public AudioClip Clip;

        public void Play()
        {
            Clip.PlayStream();
        }

        public override void Update()
        {
            Clip.UpdateStream();
        }

        public override void OnDestroy()
        {
            Clip.Dispose();
        }
    }
}
