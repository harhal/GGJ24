using System.Collections.Generic;
using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts
{
	public class Joke
	{
		public int MaxSequenceLength = 1;
		
		//[Export] public JokePart PartToAdd;
	
		private List<JokePart> Parts = new List<JokePart>();

		// Called when the node enters the scene tree for the first time.
		//public override void _Ready()
		//{
			/*Parts = new List<JokePart>();

			if (PartToAdd != null)
			{
				AddPart(PartToAdd);
			}*/
		//}
	
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
		
		public bool AddPart(JokePart newJokePart)
		{
			if (Parts.Count >= MaxSequenceLength)
			{
				return true;
			}
		
			//TODO: Add missmatch
			//TODO: Add punchline
			return false;
		}

		public bool IsFinished()
		{
			throw new System.NotImplementedException();
		}
	}
}
