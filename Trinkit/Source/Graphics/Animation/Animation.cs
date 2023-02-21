namespace Trinkit.Graphics
{
    public class Animation : Object
    {
        public string Name = "NewAnimation";
        public List<Sprite> Sprites = new();

        public int FramesPerSecond = 60;
        public bool Loop;

        public int MaxFrames => Sprites.Count;
    }
}
