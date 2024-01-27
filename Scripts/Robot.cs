using System;
using Godot;
using Godot.Collections;

namespace GGJ24.Scripts
{
    public class Robot : Node2D
    {
        [Export] private Color _robotColor = Color.TYPE1;
    
        [Export] private Shape _robotShape = Shape.None;

        [Export] public uint seatRow = 0;
        [Export] public uint seat = 0;

        //Clamped to [0,1]
        private float _fun;
        public float GetFun()
        {
            return _fun;
        }

        [Export] private float _startingFun = 0.7f;
        [Export] private float _lowFunMargin = 0.2f;

        [Signal] public delegate void LowFunReached(Robot robot);
        private bool _bLowFunSignaled = false;

        private int _boredom = 0;

        [Export] private Dictionary<int, float> _boredomLevelsToFunDebuff;
        [Signal] public delegate void NewBoredomLevelReached(Robot robot);
        
        //TODO: rename all of this
        [Export]
        Texture FirstShapeTexture;
        
        [Export]
        Texture SecondShapeTexture;
        
        [Export]
        Texture ThirdShapeTexture;

        [Export] private Godot.Color FirstColor;
        [Export] private Godot.Color SecondColor;
        [Export] private Godot.Color ThirdColor;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _fun = _startingFun;
            
            var sprite = GetChild<Sprite>(0);

            switch (_robotColor)
            {
                case Color.TYPE1:
                    sprite.Modulate = FirstColor;
                    break;
                case Color.TYPE2:
                    sprite.Modulate = SecondColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch (_robotShape)
            {
                case Shape.Circle:
                    sprite.Texture = FirstShapeTexture;
                    break;
                
                case Shape.Triangle:
                    sprite.Texture = SecondShapeTexture;
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetActive(bool newActive)
        {
            Visible = newActive;
            
            //stop all logic
            //stop signals from firing
        }

        public bool ReceiveJoke(Joke joke)
        {
            //foreach (var VARIABLE in joke.Parts)
            {
                //joke
            }
            
            GD.Print("Haha!");
            AddFun(0.1f);
            return false;
        }

        //TODO: Subscribe this to JokePartAdd delegate
        private void OnJokePartAdded()
        {
            if (false /*this joke part relates to us*/)
            {
                _boredom = 0;
                EmitSignal(nameof(NewBoredomLevelReached), this);

                return;
            }

            int currentLevel = 0;

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

        public void AddFun(float deltaFun)
        {
            _fun += deltaFun;

            Mathf.Clamp(_fun, 0, 1);

            if (_fun <= _lowFunMargin && !_bLowFunSignaled)
            {
                EmitSignal(nameof(LowFunReached), this);
                _bLowFunSignaled = true;
            }
            else if (_fun > _lowFunMargin)
            {
                _bLowFunSignaled = false;
            }
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
