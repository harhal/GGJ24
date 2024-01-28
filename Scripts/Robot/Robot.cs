using GGJ24.Scripts.Data;
using GGJ24.Scripts.JokeParts;
using Godot;
using Godot.Collections;

namespace GGJ24.Scripts.Robot
{
    public enum FunLevel
    {
        None,
        Low,
        Normal,
        Completed
    }
    
    public class Robot : Node2D
    {
        [Export] private Color _robotColor = Color.None;
        [Export] private Shape _robotShape = Shape.None;
        [Export] private float _startingFun = 0.7f;
        [Export] private float _lowFunMargin = 0.2f;
        [Export] private Dictionary<int, float> _boredomLevelsToFunDebuff;

        [Signal]
        public delegate void NewBoredomLevelReached(Robot robot);

        private FunLevel _funLevel = FunLevel.None;

        //Clamped to [0,1]
        private float _fun;

        [Export] private float _maxFunToScale = 60;

        public float GetFun()
        {
            return _fun;
        }

        [Signal]
        public delegate void FunLevelChanged(Robot robot, FunLevel previousFunLevel, FunLevel newFunLevel);

        private int _boredom = 0;

        private bool _isPlaying = true;

        private AnimatedSprite _mainSprite;
        private AnimatedSprite _glareSprite;
        private Sprite _reactionSprite;

        private float _fadeoutStartTime = 0.2f;
        [Export] private float _reactionFadeoutTimeMsec = 1400;

        public float LastAddedFun = 0.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _fun = _startingFun;

            _mainSprite = GetNode<AnimatedSprite>("%MainSprite");
            _glareSprite = GetNode<AnimatedSprite>("%Glare");
            _reactionSprite = GetNode<Sprite>("%Reaction");

            var colorsStorage = GetNode<ColorsStorage>("%ColorsStorage");
            if (_mainSprite != null && colorsStorage != null)
            {
                _mainSprite.Modulate = colorsStorage.GetColor(_robotColor).Color;
            }

            JokeAssembler assembler = JokeAssembler.StaticAssembler;

            assembler.Connect(nameof(JokeAssembler.JokePartAdded), this, "_on_joke_part_added");
        }

        private void _on_joke_part_added(JokePart part)
        {
            if (part.Color == _robotColor || part.Shape == _robotShape)
            {
                _boredom = 0;
                EmitSignal(nameof(NewBoredomLevelReached), this);

                React(0);

                return;
            }

            var currentLevel = 0;

            foreach (var pair in _boredomLevelsToFunDebuff)
            {
                if (pair.Key <= _boredom)
                {
                    currentLevel = pair.Key;
                }
                else break;
            }

            AddFun(-_boredomLevelsToFunDebuff[currentLevel]);
        }

		public float ReceiveJoke(Joke joke)
        {
            float baseScore = joke.GetTotalScore();
            LastAddedFun = 0;
            if (baseScore == 0)
            {
                return 0;
            }

            if (baseScore < 0)
            {
                LastAddedFun = baseScore / _maxFunToScale;
                AddFun(baseScore / _maxFunToScale);
                return baseScore;
            }
            
            int colorCount = joke.GetColorCount(_robotColor);
            int shapeCount = joke.GetShapeCount(_robotShape);

            if (colorCount == 0 && shapeCount == 0)
            {
                return 0;
            }
            
            float colorBonus = (Mathf.Pow(2, colorCount - 1) - 1) / Mathf.Pow(2, colorCount - 1);
            float shapeBonus = (Mathf.Pow(3, shapeCount - 1) - 1) / Mathf.Pow(3, shapeCount - 1);

            colorBonus = Mathf.Max(colorBonus, 0);
            shapeBonus = Mathf.Max(shapeBonus, 0);
            
            float finalScore = baseScore + baseScore * colorBonus + baseScore * shapeBonus;
            AddFun(finalScore / _maxFunToScale);

            LastAddedFun = finalScore / _maxFunToScale;
            
            return finalScore;
		}

        public void AddFun(float deltaFun)
        {
            if (!_isPlaying)
            {
                return;
            }

            _fun = Mathf.Clamp(_fun + deltaFun, 0, 1);
            var prevFunLevel = _funLevel;

            if (_fun <= _lowFunMargin)
            {
                _funLevel = FunLevel.Low;

                _mainSprite.Frame = 1;
                _glareSprite.Frame = 1;
            }
            // Don't even bother to ask why 0.99f, IDK either
            else if (_fun > _lowFunMargin && _fun <= 0.99f)
            {
                _funLevel = FunLevel.Normal;

                _mainSprite.Frame = 0;
                _glareSprite.Frame = 0;
            }
            else
            {
                _isPlaying = false;
                _funLevel = FunLevel.Completed;

                _mainSprite.Frame = 3;
                _mainSprite.Modulate = new Godot.Color("a4a4a4");
            
                _glareSprite.Frame = 3;
            }

            if (_funLevel != prevFunLevel)
            {
                EmitSignal(nameof(FunLevelChanged), this, prevFunLevel, _funLevel);
            }
        }

        void React(int emojiNum)
        {
            if (!_isPlaying) return;
            AnimatedSprite emoji = _reactionSprite.GetChild<AnimatedSprite>(0);
            emoji.Frame = emojiNum;

            _fadeoutStartTime = Time.GetTicksMsec();

            _reactionSprite.Modulate = new Godot.Color(1);
        }
        
        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            var timePassed = Time.GetTicksMsec() - _fadeoutStartTime;

            var passedPercent = timePassed / _reactionFadeoutTimeMsec;
            
            var currentOpacity = Mathf.Lerp(1, 0,passedPercent);
            _reactionSprite.Modulate = new Godot.Color(1, 1, 1, 1 - passedPercent);
        }


    }
}