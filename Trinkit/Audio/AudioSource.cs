﻿using Raylib_CsLo;

namespace Trinkit.Audio
{
    public class AudioSource : Behavior
    {
        public AudioClip Clip;

        public float Time => (float)Clip.GetTime();
        public float Volume = 0.0f;

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

        public override void OnDestroy()
        {
            Clip.Dispose();
        }
    }
}
