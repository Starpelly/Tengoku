using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class AudioSource : Component
    {
        public AudioClip? Clip;

        public float Time => (Clip != null) ? (float)Clip.GetTime() : 0.0f;
        public float Volume = 1.0f;
        public float Pitch = 1.0f;

        public bool IsPlaying { get; set; }

        public void Play()
        {
            if (Clip != null)
                Clip.PlayStream();
        }

        public override void Update()
        {
            if (Clip != null)
            {
                Clip.UpdateStream();
                Clip.SetVolume(Volume);
            }
            // Clip.SetPitch(Pitch);
        }

        public override void Dispose()
        {
            if (Clip != null)
                Clip.Dispose();
        }
    }
}
