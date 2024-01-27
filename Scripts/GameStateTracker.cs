using Godot;

namespace GGJ24.Scripts
{
	public enum GameState
	{
		Running,
		Won,
		Lost
	}

	public class GameStateTracker : Node
	{
		[Signal] public delegate void GameStateChanged(GameState state);
	
		public GameState GameState = GameState.Running;
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public void SetState(GameState state)
		{
			if (GameState != state)
			{
				GameState = state;
				EmitSignal(nameof(GameStateChanged), GameState);
			}
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
