using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts.Track
{
	public class TrackJokePart : Node2D
	{
		public AudioStreamPlayer2D SpawnAudioPlayer;

		public GameStateTracker GameStateTracker;

		public float Velocity;
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

			GameStateTracker.Connect(nameof(GameStateTracker.GameStateChanged), this, "_on_GameStateTracker_GameStateChanged");

			AddChild(ContainedJokePart);
			
			SpawnAudioPlayer = GetNode<AudioStreamPlayer2D>("%SpawnAudioPlayer");
			SpawnAudioPlayer.Play();
		}
		
		private void _on_GameStateTracker_GameStateChanged(GameState state)
		{
			if (state == GameState.Running)
			{
				return;
			}

			SetProcess(false);
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
