using Trinkit;
using Trinkit.Audio;

namespace Tengoku
{
    public class JudgementManager
    {
        public static float GetDelta(float targetBeats, float currentSecond)
        {
            float num = GameManager.Instance.Conductor!.GetSongPosFromBeat((float)targetBeats);
            var max = Mathf.Max(currentSecond, num);
            var min = Mathf.Min(currentSecond, num);
            return (float)(currentSecond - num);
        }
    }
}
