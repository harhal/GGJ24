using Godot;

namespace GGJ24.Scripts
{
	public class MainScene : Node2D
	{
		private Hall Hall;
		private GameStateTracker GameStateTracker;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Hall = GetNode<Hall>("%Hall");
			Hall.Connect(nameof(Hall.AllRobotsHappilyFinished), this, "_on_Hall_AllRobotsHappilyFinished");

			GameStateTracker = GetNode<GameStateTracker>("%GameStateTracker");
			
		}

		private void _on_Hall_AllRobotsHappilyFinished()
		{
			GameStateTracker?.SetState(GameState.Won);
		}

		private void _on_GameStateTracker_GameStateChanged(GameState state)
		{
			foreach (Node child in GetChildren())
			{
				var gameStateTracker = child.GetNode<GameStateTracker>("%GameStateTracker");
				gameStateTracker?.SetState(state);
			}
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
