using System;
using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts
{
	public class Track : Node2D
	{
		[Export] public PackedScene TrackItemTemplate;
		[Export] public float TrackPadVelocity = 100;
		[Export] public float MinSpawnDelay = 2.0f;
		[Export] public float MaxSpawnDelay = 4.0f;
		
		public TrackStart TrackStart;
		public Sprite TrackPad;
		public JokePartFactory JokePartFactory;
		public Timer SpawnTimer;

		private float _yAdvancement = 0;
		private Vector2 _trackPadStartPosition;
		private const float TrackPadLengthClampPoint = 1141;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			TrackStart = GetNode<TrackStart>("%TrackStart");
			JokePartFactory = GetNode<JokePartFactory>("%JokePartFactory");
			TrackPad = GetNode<Sprite>("%TrackPad");
			_trackPadStartPosition = TrackPad.Position;

			SpawnTimer = GetNode<Timer>("%SpawnTimer");
			SpawnTimer.Connect("timeout", this, "_on_SpawnTimer_timeout");

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
			Vector2 startPosition = TrackStart.GetStartPosition();

			var trackItem = TrackItemTemplate.InstanceOrNull<TrackJokePart>();
			trackItem.Position = startPosition;
			trackItem.ContainedJokePart = JokePartFactory.CreateRandom();

			AddChild(trackItem);
		}

		public override void _Process(float delta)
		{
			_yAdvancement += delta * TrackPadVelocity;
			_yAdvancement %= TrackPadLengthClampPoint;

			TrackPad.Position = new Vector2(_trackPadStartPosition.x, _trackPadStartPosition.y + _yAdvancement);

		}
	}
}
