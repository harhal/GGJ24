using System;
using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts.Track
{
	public class Track : Node2D
	{
		[Export] public PackedScene TrackItemTemplate;
		[Export] public float TrackPadVelocity = 100;
		[Export] public float MinSpawnDelay = 2.0f;
		[Export] public float MaxSpawnDelay = 4.0f;
		
		public TrackStart TrackStart;
		public Position2D DeadZone;
		public Sprite TrackPad;
		public JokePartFactory JokePartFactory;
		public Timer SpawnTimer;
		public GameStateTracker GameStateTracker;

		private float _yAdvancement = 0;
		private Vector2 _trackPadStartPosition;
		private const float TrackPadLengthClampPoint = 1141;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			TrackStart = GetNode<TrackStart>("%TrackStart");
			DeadZone = GetNode<Position2D>("%DeadZone");
			JokePartFactory = GetNode<JokePartFactory>("%JokePartFactory");
			TrackPad = GetNode<Sprite>("%TrackPad");
			_trackPadStartPosition = TrackPad.Position;

			SpawnTimer = GetNode<Timer>("%SpawnTimer");
			SpawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");

			GameStateTracker = GetNode<GameStateTracker>("%GameStateTracker");
			GameStateTracker.Connect(nameof(GameStateTracker.GameStateChanged), this,
				"_on_GameStateTracker_GameStateChanged");

			StartSpawnTimer();
		}

		private double GetSpawnDelay()
		{
			var random = new Random();
			return MinSpawnDelay + random.NextDouble() * (MaxSpawnDelay - MinSpawnDelay);
		}

		private void StartSpawnTimer()
		{
			SpawnTimer.Start((float) GetSpawnDelay());
		}

		public void _on_SpawnTimer_timeout()
		{
			SpawnNewItem();
			StartSpawnTimer();
		}

		private void SpawnNewItem()
		{
			var trackItem = TrackItemTemplate.InstanceOrNull<TrackJokePart>();

			trackItem.GameStateTracker = GameStateTracker;

			trackItem.Position = TrackStart.GetStartPosition();
			trackItem.ContainedJokePart = JokePartFactory.CreateRandom();
			trackItem.DeadZoneY = DeadZone.Position.y;

			AddChild(trackItem);
		}

		private void _on_GameStateTracker_GameStateChanged(GameState state)
		{
			if (state == GameState.Running)
			{
				return;
			}

			SetProcess(false);
			SpawnTimer.Stop();
		}

		public override void _Process(float delta)
		{
			_yAdvancement += delta * TrackPadVelocity;
			_yAdvancement %= TrackPadLengthClampPoint;

			TrackPad.Position = new Vector2(_trackPadStartPosition.x, _trackPadStartPosition.y + _yAdvancement);

		}
	}
}
