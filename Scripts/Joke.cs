using System.Collections.Generic;
using System.Linq;
using GGJ24.Scripts.JokeParts;
using Godot;

public enum FinishReason
{
	InAssemble,
	Full,
	Punchline,
	Mismatch,
	Repeat,
	Spoiled
}

namespace GGJ24.Scripts
{
	public class Joke
	{
		public int MaxSequenceLength = 1;

		private FinishReason _finishReason = FinishReason.InAssemble;
	
		private List<JokePart> Parts = new List<JokePart>();
		
		public Joke(int MaxSequenceLength)
		{
			this.MaxSequenceLength = MaxSequenceLength;
		}
	
		public float Score()
		{
			float TotalScore = 0f;

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

					case JokePartOperationType.Double:
					{
						Multiplyer *= 2;
						break;
					}
				}
			}

			//Success cycle
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

						if (!IsFailed())
						{
							TotalScore += 3;
						}
						break;
					}

					case JokePartOperationType.Opener:
					{
						if (!IsFailed())
						{
							TotalScore += (idx == 0) ? 3 : 1;
						}
						break;
					}

					case JokePartOperationType.AddOne:
					{
						if (!IsFailed())
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
						if (!IsFailed())
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

			if (Parts.Count >= MaxSequenceLength)
			{
				_finishReason = FinishReason.Full;
			}
			
			if (newJokePart.OperationType == JokePartOperationType.Punchline)
			{
				_finishReason = FinishReason.Punchline;
			}

			bool bMatchByColor = lastPart.Color == newJokePart.Color;
			bool bMatchByShape = lastPart.Shape == newJokePart.Shape;

			bool bJoker = lastPart.OperationType == JokePartOperationType.Joker ||
			              newJokePart.OperationType == JokePartOperationType.Joker;

			if (!bMatchByColor && !bMatchByShape && !bJoker)
			{
				_finishReason = FinishReason.Mismatch;
			}

			if (IsRepeat(newJokePart))
			{
				_finishReason = FinishReason.Repeat;
			}

			if (newJokePart.OperationType == JokePartOperationType.Spoiler)
			{
				_finishReason = FinishReason.Spoiled;
			}
			
			return true;
		}

		bool IsRepeat(JokePart newPart)
		{
			foreach (JokePart part in Parts)
			{
				if (part != newPart && part.OperationType == newPart.OperationType)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsFinished()
		{
			return _finishReason != FinishReason.InAssemble;
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
			return _finishReason != FinishReason.InAssemble && _finishReason != FinishReason.Full && _finishReason != FinishReason.Punchline;
		}

		public FinishReason GetFinishReason()
		{
			return _finishReason;
		}
	}
}
