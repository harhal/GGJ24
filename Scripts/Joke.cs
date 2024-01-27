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
			float TotalScore = 0f;
			
			//PreCycle
			for (int idx = 1; idx < Parts.Count; idx++)
			{
				switch (Parts[idx].OperationType)
				{
					case JokePartOperationType.CopyPrevious:
					{
						Parts[idx].OperationType = Parts[idx - 1].OperationType;
						break;
					}
						
					case JokePartOperationType.RemovePrevious:
					{
						Parts[idx - 1].OperationType = JokePartOperationType.None;
						break;
					}
						
					case JokePartOperationType.ChekhovGun:
					{
						if (Parts[Parts.Count - 1].OperationType != JokePartOperationType.Punchline)
						{
							Parts[idx].OperationType = JokePartOperationType.Spoiler;
						}
						break;
					}
				}
			}

			float Multiplyer = 1;
			//MultiplyersCycle
			for (int idx = 1; idx < Parts.Count; idx++)
			{
				switch (Parts[idx].OperationType)
				{
					case JokePartOperationType.Punchline:
					{
						Multiplyer *= 2;
						break;
					}

					case JokePartOperationType.ChekhovGun:
					{
						Multiplyer *= 2;
						break;
					}

					case JokePartOperationType.Spoiler:
					{
						bIsFailed = true;
						break;
					}
				}
			}

			//MainCycle
			for (int idx = 1; idx < Parts.Count; idx++)
			{
				switch (Parts[idx].OperationType)
				{
					case JokePartOperationType.Human:
					{
						if (CringeLevelScript.StaticCringe != null)
						{
							CringeLevelScript.StaticCringe.AddCringe(5);
						}

						if (!bIsFailed)
						{
							TotalScore += 3;
						}
						break;
					}

					case JokePartOperationType.Opener:
					{
						if (!bIsFailed)
						{
							TotalScore += (idx == 0) ? 3 : 1;
						}
						break;
					}

					case JokePartOperationType.AddOne:
					{
						if (!bIsFailed)
						{
							TotalScore += 1;
						}
						break;
					}

					case JokePartOperationType.MinusOne:
					{
						TotalScore -= 1;
						break;
					}

					case JokePartOperationType.MinusTwo:
					{
						TotalScore -= 2;
						break;
					}

					case JokePartOperationType.Robot:
					{
						if (!bIsFailed)
						{
							TotalScore += 4;
						}
						else
						{
							TotalScore -= 8;
						}
						break;
					}
				}
			}

			TotalScore *= Multiplyer;
			return TotalScore;
		}
		
		public bool AddPart(JokePart newJokePart)
		{
			if (Parts.Count >= MaxSequenceLength)
			{
				return false;
			}

			JokePart lastPart = Parts.Count > 0 ? Parts[Parts.Count() - 1] : newJokePart;
			
			Parts.Add(newJokePart);

			bool bMatchByColor = lastPart.Color == newJokePart.Color;
			bool bMatchByShape = lastPart.Shape == newJokePart.Shape;

			bool bJoker = lastPart.OperationType == JokePartOperationType.Joker ||
			              newJokePart.OperationType == JokePartOperationType.Joker;

			if (!bMatchByColor && !bMatchByShape && !bJoker)
			{
				bIsFailed = true;
				newJokePart.OperationType = JokePartOperationType.Spoiler;
			}
			
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

		public JokePartOperationType GetPartType(int idx)
		{
			if (idx < 0 || idx >= Parts.Count)
			{
				return JokePartOperationType.None;
			}

			return Parts[idx].OperationType;
		}

		public bool IsFailed()
		{
			return bIsFailed;
		}
	}
}
