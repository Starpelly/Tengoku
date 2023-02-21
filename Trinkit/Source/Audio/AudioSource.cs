using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class AudioSource : Component
    {
        public AudioClip Clip;

        public float Time => (float)Clip.GetTime();
        public float Volume = 1.0f;

        public bool IsPlaying { get; set; }

        public void Play()
        {
            Clip.PlayStream();
        }

        public override void Update()
        {
            Clip.UpdateStream();
            Clip.SetVolume(Volume);
        }

        public override void Dispose()
        {
            Clip.Dispose();
        }
    }
}
