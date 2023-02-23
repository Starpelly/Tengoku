using Newtonsoft.Json;

namespace Trinkit.Graphics
{
    public class Animator : Component
    {
        private float _clock = 0.0f;
        private int _frame = 0;
        private bool _finishedAnim = false;
        private Animation? _currentAnimation;

        public List<Animation> Animations = new();
        public Animation CurrentAnimation => _currentAnimation!;
        public int CurrentFrame => (_currentAnimation == null) ? 0 : _frame % _currentAnimation.MaxFrames;

        public Animator(string fileLoc)
        {
            LoadAnimations(fileLoc);
        }

        public override void Update()
        {
            // && _clock <= _currentAnimation.MaxFrames / (float)_currentAnimation.FPS && !_finishedAnim
            if (_currentAnimation != null && !_finishedAnim)
            {
                _clock += Time.deltaTime;
                float secondsPerFrame = (float)_currentAnimation.Frames[CurrentFrame].Length / _currentAnimation.FPS;
                while (_clock >= secondsPerFrame)
                {
                    _clock -= secondsPerFrame;
                    _frame++;
                    if (_frame >= _currentAnimation.MaxFrames && !_currentAnimation.Loop)
                    {
                        _frame--;
                        if (_currentAnimation.Fallback != string.Empty)
                        {
                            ResetClock();
                            SetAnimation(_currentAnimation.Fallback);
                        }
                        else
                            _finishedAnim = true;
                        break;
                    }
                }
            }
        }

        private void SetAnimation(string name)
        {
            _currentAnimation = Animations.Find(c => c.Name == name);
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
                _finishedAnim = false;
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
