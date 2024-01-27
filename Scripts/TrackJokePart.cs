using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts
{
	public class TrackJokePart : Node2D
	{
		[Export] public float Velocity = 100;

		public JokePart ContainedJokePart;
		public float DeadZoneY;

		TrackJokePart()
		{
			ZIndex = 1;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if (ContainedJokePart == null)
			{
				return;
			}

			AddChild(ContainedJokePart);
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(float delta)
		{
			Position += Vector2.Down * Velocity * delta;

			if (Position.y > DeadZoneY)
			{
				QueueFree();
			}
		}
	}
}
