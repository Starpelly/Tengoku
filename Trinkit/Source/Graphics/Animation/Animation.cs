using Newtonsoft.Json;

namespace Trinkit.Graphics
{
    public class Animation : Object
    {
        public string Name = "NewAnimation";
        public int FPS = 60;
        public bool Loop;
        public string Fallback = string.Empty;
        public List<Frame> Frames = new();

        [JsonIgnore]
        public int MaxFrames => Frames.Count;

        public struct Frame
        {
            public int Index { get; set; } = -1;
            public int Length { get; set; }

            public Frame(int length)
            {
                this.Length = length;
            }
        }
    }
}
