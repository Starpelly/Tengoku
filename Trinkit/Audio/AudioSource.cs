using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class AudioSource : Behavior
    {
        public AudioClip Clip;

        public double Time => Clip.GetTime();
        public float Volume;

        public void Play()
        {
            Clip.PlayStream();
        }

        public override void Update()
        {
            Clip.UpdateStream();
            Clip.SetVolume(Volume);
        }

        public override void OnDestroy()
        {
            Clip.Dispose();
        }
    }
}
