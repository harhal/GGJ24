using System.Timers;
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
		[Export] private int _winScreenIdx = 2;
		[Export] private int _looseScreenIdx = 3;
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

				PropagateStateToSiblings();

				if (GameState == GameState.Running)
				{
					return;
				}
				
				System.Timers.Timer closeDelay = new System.Timers.Timer();
				closeDelay.Interval = 1f;
				closeDelay.AutoReset = false;
				closeDelay.Elapsed += (sender, args) =>
				{
					ScenesCatalog.MoveToScene(this, GameState == GameState.Won ? _winScreenIdx : _looseScreenIdx);
				};
				closeDelay.Start();
			}
		}

		private void PropagateStateToSiblings()
		{
			var parent = GetParent<Node>();
			if (parent == null)
			{
				return;
			}
			
			foreach (Node sibling in parent.GetChildren())
			{
				var gameStateTracker = sibling.GetNodeOrNull<GameStateTracker>("%GameStateTracker");
				gameStateTracker?.SetState(GameState);
			}
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
