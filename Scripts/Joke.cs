using System.Collections.Generic;
using System.Linq;
using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts
{
	public class Joke
	{
		public int MaxSequenceLength = 1;

		private bool bIsFinished = false;

		private bool bIsFailed = false;
	
		private List<JokePart> Parts = new List<JokePart>();

		public Joke(int MaxSequenceLength)
		{
			this.MaxSequenceLength = MaxSequenceLength;
		}
	
		public float Score()
		{
			return 0.0f;
		}
		
		public bool AddPart(JokePart newJokePart)
		{
			if (Parts.Count >= MaxSequenceLength)
			{
				return false;
			}

			bool bMatchByColor = Parts.Count > 0 ? Parts[Parts.Count() - 1].Color == newJokePart.Color : true;
			bool bMatchByShape = Parts.Count > 0 ? Parts[Parts.Count() - 1].Shape == newJokePart.Shape : true;

			if (!bMatchByColor && !bMatchByShape)
			{
				bIsFailed = true;
			}
			
			Parts.Add(newJokePart);
			
			if (Parts.Count >= MaxSequenceLength)
			{
				bIsFinished = true;
			}
			
			if (newJokePart.OperationType == JokePartOperationType.Punchline)
			{
				bIsFinished = true;
			}
			
			return true;
		}

		public bool IsFinished()
		{
			return bIsFinished || bIsFailed;
		}
	}
}
