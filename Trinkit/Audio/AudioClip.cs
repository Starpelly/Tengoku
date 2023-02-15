﻿using Raylib_CsLo;

namespace Trinkit.Audio
{
    /// <summary>
    /// Type of sound API.
    /// </summary>
    public enum AudioClipType
    {
        /// <summary>
        /// .OGG
        /// </summary>
        OGG,
        /// <summary>
        /// .WAV
        /// </summary>
        WAV,
        /// <summary>
        /// .MP3
        /// </summary>
        MP3
    }

    public class AudioClip : Object, IDisposable
    {
        private Music _stream { get; set; }

        private bool _loadedStream;

        public AudioClip(string location)
        {
            _stream = Raylib.LoadMusicStream(location);
            _loadedStream = true;
        }

        internal void PlayStream()
        {
            Raylib.PlayMusicStream(_stream);
        }

        internal void UpdateStream()
        {
            if (_loadedStream)
            { 
                Raylib.UpdateMusicStream(_stream);
            }
        }

        public void Dispose()
        {
            if (_loadedStream)
            {
                Raylib.UnloadMusicStream(_stream);
            }
        }
    }
}
