using Godot;

namespace GGJ24.Scripts.JokeParts
{

	public class AddOneJPO : JokePartOperation
	{
		public AddOneJPO()
		{
			ProcessPhase = JokePartProcessPhase.DuringMain;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			base._Ready();
			GD.Print("AddOneJPO ready");
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
