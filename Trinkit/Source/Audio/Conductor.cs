using System.IO;

namespace Trinkit.Audio
{
    public class Conductor : AudioSource
    {
        public static readonly Conductor Instance = new Conductor();

        /// <summary>
        /// The tempo the song will start at.
        /// </summary>
        public float InitialTempo = 120.0f;

        /// <summary>
        /// Song beats per minute.
        /// This is determined by the song you're trying to sync up to.
        /// </summary>
        public float SongTempo => 60.0f / SecPerBeat;

        /// <summary>
        /// The real time in seconds per beat.
        /// </summary>
        private float SecPerBeat;

        /// <summary>
        /// Current song position, in seconds.
        /// </summary>
        public float SongPosition;

        /// <summary>
        /// Current song position, in beats.
        /// </summary>
        public float SongPositionInBeats;

        /// <summary>
        /// The offset to the first beat of the song in seconds.
        /// </summary>
        public float FirstBeatOffset;

        /// <summary>
        /// List of TempoChanges that changes the tempo using the beat it starts and length of the change.
        /// </summary>
        public List<TempoChange> tempoChanges = new();

        public override void Update()
        {
            base.Update();

            SecPerBeat = (60.0f / InitialTempo);

            {
                var dt = Trinkit.Time.deltaTime;

                SongPosition = Time;
                SongPositionInBeats = Time / SecPerBeat;
            }
        }

        /// <summary>
        /// Get the normalized position from a beat to length. (For example on Beat 3 between a startBeat of 2 and a length of 2, the position will be 0.5)
        /// </summary>
        public float GetPositionFromBeat(float startBeat, float length)
        {
            return Mathf.Normalize(SongPositionInBeats, startBeat, startBeat + length);
        }

        /// <summary>
        /// Uses the tempo changes to accurately get what the song position would be at this beat.
        /// </summary>
        public float GetSongPosFromBeat(float beat)
        {
            float counter = 0.0f;
            float lastTempoChangeBeat = 0.0f;

            for (int i = 0; i < tempoChanges.Count; i++)
            {
                var tempoChange = tempoChanges[i];

                if (tempoChange.beat > beat)
                    break;

                counter += (tempoChange.beat - lastTempoChangeBeat) * SecPerBeat;

                lastTempoChangeBeat = tempoChange.beat;
            }

            counter += (beat - lastTempoChangeBeat) * SecPerBeat;

            return counter;
        }
    }
}
