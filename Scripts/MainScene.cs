using Godot;

namespace GGJ24.Scripts
{
	public class MainScene : Node2D
	{
		private Hall _hall;
		private GameStateTracker _gameStateTracker;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_hall = GetNode<Hall>("%Hall");
			_hall.Connect(nameof(Hall.AllRobotsCompletedFun), this, "_on_Hall_AllRobotsCompletedFun");

			_gameStateTracker = GetNode<GameStateTracker>("%GameStateTracker");
		}

		private void _on_Hall_AllRobotsCompletedFun()
		{
			_gameStateTracker?.SetState(GameState.Won);
		}

	}
}
