using System;
using Godot;

namespace GGJ24.Scripts
{
	public class RuleHint : Sprite
	{
		public float JokeBlockTime;

		private float TimeLeft;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public void Trigger()
		{
			Modulate = Colors.Red;
			TimeLeft = JokeBlockTime;
			SetProcess(true);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			TimeLeft -= delta;
			if (TimeLeft <= 0.0f)
			{
				SetProcess(false);
				Modulate = Colors.White;
				return;
			}

			if (Math.Floor(TimeLeft * 10) % 3 == 0)
			{
				Modulate = Colors.Red;
			}
			else
			{
				Modulate = Colors.White;
			}
		}
	}
}
