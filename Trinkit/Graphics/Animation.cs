namespace Trinkit.Graphics
{
    public class Animation : Behavior
    {
        public int MaxFrames;
        public int Frame;
        public int FramesPerSecond = 60;

        private bool _isPlaying = false;
        private bool _loop = false;
        private int _frame = 0;
        private float _timer = 0.0f;

        public Animation(int maxFrames, int FPS)
        {
            MaxFrames = maxFrames;
            FramesPerSecond = FPS;
        }

        public override void Update()
        {
            if (_isPlaying)
            {
                if (_frame > MaxFrames && !_loop)
                {
                    Reset();
                    _isPlaying = false;
                    return;
                }

                _timer += Time.deltaTime;

                if (_timer > 1f / FramesPerSecond)
                {
                    _timer = 0.0f;
                    _frame += 1;
                }

                Frame = _frame % MaxFrames;
            }
        }

        public void Play()
        {
            Reset();
            _isPlaying = true;
            Frame = 1;
            _frame = 1;
        }

        private void Reset()
        {
            Frame = 0;
            _frame = 0;
        }
    }
}
