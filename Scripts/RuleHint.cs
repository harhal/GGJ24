using System;
using Godot;

namespace GGJ24.Scripts
{
	public class RuleHint : Sprite
	{
		[Export] private float TriggerTime = 1;

		private float TimeLeft;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public void Trigger()
		{
			Modulate = Colors.Red;
			TimeLeft = TriggerTime;
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

			GD.Print(TimeLeft * 10);
			if (Math.Floor(TimeLeft * 10) % 2 == 0)
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
