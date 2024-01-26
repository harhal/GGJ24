using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts
{
	public class Joke : Node2D
	{
		//[Export] public JokePart PartToAdd;
	
		//private List<JokePart> Parts;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			/*Parts = new List<JokePart>();

			if (PartToAdd != null)
			{
				AddPart(PartToAdd);
			}*/
		}
	
		public float Score()
		{
			return 0.0f;
		}

		/*public bool AddPart(JokePart part)
		{
			GD.Print("Added part!");
			
			
			Parts.Add(part);

			return false;
		}*/

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
