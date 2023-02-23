using Newtonsoft.Json;

namespace Trinkit.Graphics
{
    public class Animator : Component
    {
        private float _clock = 0.0f;
        private int _frame = 0;
        private Animation? _currentAnimation;

        public List<Animation> Animations = new();
        public Animation CurrentAnimation => _currentAnimation!;
        public int CurrentFrame => (_currentAnimation == null) ? 0 : _frame % _currentAnimation.MaxFrames;

        public override void Update()
        {
            if (_currentAnimation != null)
            {
                _clock += Time.deltaTime;
                float secondsPerFrame = 1.0f / _currentAnimation.FPS;
                while (_clock >= secondsPerFrame)
                {
                    _clock -= secondsPerFrame;
                    _frame++;
                    if (_frame >= _currentAnimation.MaxFrames && !_currentAnimation.Loop)
                    {
                        ResetClock();
                        _currentAnimation = 
                            (_currentAnimation.Fallback != string.Empty) 
                            ? GetAnimation(_currentAnimation.Fallback)
                            : _currentAnimation;
                        return;
                    }
                }
            }
        }

        public Animation? GetAnimation(string name)
        {
            return Animations.Find(c => c.Name == name);
        }

        public void Play(string name, float normalizedTime = 0.0f)
        {
            var anim = GetAnimation(name);
            if (anim != null)
            {
                _currentAnimation = anim;
                _frame = 0; // Use normalized time in the future.
                _clock = 0.0f;
            }
            else
                throw new Exception("Animation not found!");
        }

        public void LoadAnimations(string fileLoc)
        {
            var jsonFile = File.ReadAllText(fileLoc);
            var animations = JsonConvert.DeserializeObject<List<Animation>>(jsonFile);
            if (animations != null)
            {
                Animations = animations;
                _currentAnimation = Animations[0];
            }
        }

        private void ResetClock()
        {
            _frame = 0;
            _clock = 0.0f;
        }

        public override void Dispose()
        {
        }
    }
}
