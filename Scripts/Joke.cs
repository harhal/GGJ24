using System;
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

		private Dictionary<Color, int> ColorsMap = new Dictionary<Color, int>();
		private Dictionary<Shape, int> ShapesMap = new Dictionary<Shape, int>();
		
		private float TotalScore = 0f;

		public Joke(int MaxSequenceLength)
		{
			this.MaxSequenceLength = MaxSequenceLength;
		}
	
		public void Score()
		{
			TotalScore = 0f;

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
		}

		public float GetTotalScore()
		{
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

			if (newJokePart.OperationType == JokePartOperationType.Joker)
			{
				foreach (Color color in Enum.GetValues(typeof(Color)))
				{
					AddColor(color);
				}
				foreach (Shape shape in Enum.GetValues(typeof(Shape)))
				{
					AddShape(shape);
				}
			}
			else
			{
				AddColor(newJokePart.Color);
				AddShape(newJokePart.Shape);
			}

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
				if (part != newPart && part.Operation.Texture == newPart.Operation.Texture)
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

		void AddColor(Color color)
		{
			/*if (ColorsMap.ContainsKey(color))
			{
				ColorsMap[color]++;
			}
			else
			{
				ColorsMap.Add(color, 1);
			}*/
			                                        
			ColorsMap[color] = GetColorCount(color) + 1; 
		}

		void AddShape(Shape shape)
		{                                  
			ShapesMap[shape] = GetShapeCount(shape) + 1;      
		}
		public int GetColorCount(Color color)
		{
			ColorsMap.TryGetValue(color, out int result);
			return result;
		}

		public int GetShapeCount(Shape shape)
		{
			ShapesMap.TryGetValue(shape, out int result);
			return result;
		}
	}
}
