namespace Trinkit.Graphics
{
    public class Animator : Component
    {
        public List<Animation> Animations = new();

        private Animation CurrentAnimation;

        public Animation? GetAnimation(string name)
        {
            return Animations.Find(c => c.Name == name);
        }

        public void Play(string name, float normalizedTime = 0.0f)
        {
            var anim = GetAnimation(name);
            if (anim != null)
                CurrentAnimation = anim;
            throw new Exception("Animation not found!");
        }

        public int Frame;
        public bool ResetOnEnd;

        private bool _isPlaying = false;
        private int _frame = 0;
        private float _timer = 0.0f;

        public override void Update()
        {
            if (_isPlaying)
            {
                if (_frame > CurrentAnimation.MaxFrames && !CurrentAnimation.Loop)
                {
                    if (ResetOnEnd)
                    {
                        Reset();
                    }
                    _isPlaying = false;
                    return;
                }

                _timer += Time.deltaTime;

                if (_timer > 1f / CurrentAnimation.FramesPerSecond)
                {
                    _timer = 0.0f;
                    _frame += 1;
                }

                Frame = _frame % CurrentAnimation.MaxFrames;
            }
        }

        private void Reset()
        {
            Frame = 0;
            _frame = 0;
        }

        public override void Dispose()
        {
        }
    }
}
