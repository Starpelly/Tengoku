using Trinkit;
using Trinkit.Audio;

using Tengoku.Scenes;

namespace Tengoku
{
    public class JudgementManager
    {
        public static float GetDelta(float targetBeats, float currentSecond)
        {
            float num = GameScene.Conductor!.GetSongPosFromBeat((float)targetBeats);
            var max = Mathf.Max(currentSecond, num);
            var min = Mathf.Min(currentSecond, num);
            return (float)(currentSecond - num);
        }
    }
}
