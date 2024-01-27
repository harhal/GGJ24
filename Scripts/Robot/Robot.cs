using System;
using GGJ24.Scripts.JokeParts;
using GGJ24.Scripts.Shapes;
using Godot;
using Godot.Collections;

namespace GGJ24.Scripts
{
	public class Robot : Node2D
	{
		[Export] private Color _robotColor = Color.None;
		[Export] private Shape _robotShape = Shape.None;
		[Export] private float _startingFun = 0.7f;
		[Export] private float _lowFunMargin = 0.2f;
		[Export] private Dictionary<int, float> _boredomLevelsToFunDebuff;

		[Signal] public delegate void NewBoredomLevelReached(Robot robot);

		//Clamped to [0,1]
		private float _fun;
		public float GetFun()
		{
			return _fun;
		}

		[Signal] public delegate void LowFunReached(Robot robot);
		private bool _bLowFunSignaled = false;

		private int _boredom = 0;

		private bool _isPlaying = true;
		[Signal] public delegate void HappilyFinished(Robot robot);

		private AnimatedSprite _mainSprite;
		private AnimatedSprite _glareSprite;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_fun = _startingFun;

			_mainSprite = GetNode<AnimatedSprite>("%MainSprite");
			_glareSprite = GetNode<AnimatedSprite>("%Glare");
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

		public bool ReceiveJoke(Joke joke)
		{
			//foreach (var VARIABLE in joke.Parts)
			{
				//joke
			}
			
			GD.Print("Haha!");
			//AddFun(0.1f);
			return false;
		}

		public void AddFun(float deltaFun)
		{
			if (!_isPlaying)
			{
				return;
			}
			
			_fun = Mathf.Clamp(_fun + deltaFun, 0, 1);

			if (_fun <= _lowFunMargin)
			{
				if (!_bLowFunSignaled)
				{
					EmitSignal(nameof(LowFunReached), this);
					_bLowFunSignaled = true;
				}

				_mainSprite.Frame = 1;
				_glareSprite.Frame = 1;

				return;
			}
			
			if (_fun > _lowFunMargin && _fun <= 1)
			{
				_bLowFunSignaled = false;
				_mainSprite.Frame = 0;
				_glareSprite.Frame = 0;
				return;
			}
			
			_mainSprite.Frame = 3;
			_glareSprite.Frame = 3;

			FinishPlayingHappily();
		}

		private void FinishPlayingHappily()
		{
			_isPlaying = false;
			_mainSprite.Modulate = new Godot.Color(1,1,1,1);
			EmitSignal(nameof(HappilyFinished), this);
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
