namespace Trinkit.Audio
{
    public class Conductor : AudioSource
    {
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
    }
}
