using Godot;
using System;
using System.Collections.Generic;
using System.Timers;
using GGJ24.Scripts;
using GGJ24.Scripts.JokeParts;
using Timer = System.Timers.Timer;

class ElementWithTransition
{
	public Node2D Element;
	public Vector2 StartLocation;
	public Vector2 DesiredLocation;

	public ElementWithTransition(Node2D element, Vector2 startLocation, Vector2 desiredLocation)
	{
		Element = element;
		StartLocation = startLocation;
		DesiredLocation = desiredLocation;
	}
}

public class JokeAssembler : Node2D
{
	[Export] private NodePath BasePlacePath;
	private Vector2 BasePlace;
	[Export] private float HorizontalSpace = 100f;
	[Export] private float TransitionTime = 1f;
	[Export] private int MaxSequenceLength = 5;
	[Export] private PackedScene Tip;
	[Export] private float TipOffset = 700f;
	[Export] private float FadeoutTime = 3f;

	private Node2D BackGround;

	private Joke AssembledJoke;

	private List<ElementWithTransition> Elements;
	private float TransitionProgress = 1f;

	public static JokeAssembler StaticAssembler;
	
	[Signal] public delegate void JokePartAdded(JokePart jokePart);

	public JokeAssembler()
	{
		StaticAssembler = this;
	}

	public bool AddElement(Node2D Element)
	{
		if (AssembledJoke == null)
		{
			AssembledJoke = new Joke(MaxSequenceLength);
		}
		
		if (AssembledJoke.IsFinished())
		{
			return false;
		}
		
		Transform2D GlobalTransform = Element.GlobalTransform;
		if (Element.GetParent() != null)
		{
			Element.GetParent().RemoveChild(Element);
		}

		JokePart NewJokePart = Element as JokePart;
		if (!AssembledJoke.AddPart(NewJokePart))
		{
			return false;
		}
		
		AddChild(Element);
		Element.GlobalTransform = GlobalTransform;
		Elements.Add(new ElementWithTransition(Element, Element.Position, Element.Position));
		UpdateDesiredLocations();
		
		EmitSignal(nameof(JokePartAdded), NewJokePart);

		return true;
	}

	private void UpdateDesiredLocations()
	{
		for (int idx = 0; idx < Elements.Count; idx++)
		{
			ElementWithTransition Focus = Elements[idx];
			Focus.StartLocation = Focus.Element.Position;
			Focus.DesiredLocation = BasePlace + Vector2.Right * HorizontalSpace * ((float)idx - (float)Elements.Count / 2);
		}

		TransitionProgress = 0;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Elements = new List<ElementWithTransition>();
		BasePlace = GetNode<Node2D>(BasePlacePath).Position;
		BackGround = GetNode<Node2D>("Background");
	}

	public override void _Process(float delta)
	{
		if (TransitionProgress >= 1)
		{
			return;
		}

		TransitionProgress += Mathf.Clamp(delta / TransitionTime, 0f, 1f);
		
		for (int idx = 0; idx < Elements.Count; idx++)
		{
			ElementWithTransition Focus = Elements[idx];
			Vector2 NewPos = Focus.StartLocation;
			NewPos = NewPos.LinearInterpolate(Focus.DesiredLocation, TransitionProgress);
			Focus.Element.Position = NewPos;
		}
		
		if (TransitionProgress >= 1)
		{
			AddFinished();
		}
	}

	void AddFinished()
	{
		if (AssembledJoke.IsFinished())
		{
			PushJoke();
		}
	}

	void PushJoke()
	{
		AssembledJoke.Score();
		
		for (int idx = 0; idx < Elements.Count; idx++)
		{
			JokePartTip tip = Tip.InstanceOrNull<JokePartTip>();
			AddChild(tip);
			tip.Position = Elements[idx].DesiredLocation + Vector2.Up * TipOffset;

			JokePartOperationType partType = AssembledJoke.GetPartType(idx);

			string tipText = "";
			Godot.Color tipColor = Godot.Color.ColorN("Green");

			switch (partType)
			{
				case JokePartOperationType.AddOne:
					if (!AssembledJoke.IsFailed())
						tipText = "+1";
					break;
				case JokePartOperationType.AddTwo:
					if (!AssembledJoke.IsFailed())
						tipText = "+2";
					break;
				case JokePartOperationType.MinusOne:
				{
					tipText = "-1";
					tipColor = Godot.Color.ColorN("Red");
					break;
				}
				case JokePartOperationType.MinusTwo:
				{
					tipText = "-2";
					tipColor = Godot.Color.ColorN("Red");
					break;
				}
				case JokePartOperationType.Double:
					tipText = "x2";
					break;
				case JokePartOperationType.Opener:
					if (!AssembledJoke.IsFailed())
					{
						if (idx == 0)
						{
							tipText = "+3";
						}
						else
						{
							tipText = "+1";
							tipColor = Godot.Color.ColorN("Yellow");
						}
					}

					break;
				case JokePartOperationType.Robot:
					if (!AssembledJoke.IsFailed())
					{
						tipText = "+4";
					}
					else
					{
						tipText = "-8";
						tipColor = Godot.Color.ColorN("Red");
					}
					break;
				case JokePartOperationType.Human:
					if (!AssembledJoke.IsFailed())
						tipText = "+3";
					break;
				case JokePartOperationType.Punchline:
					tipText = "x2";
					break;
			}

			tip.SetText(tipText, tipColor, FadeoutTime);
		}

		if (AssembledJoke.IsFailed())
		{
			
			BackGround.Modulate = Godot.Color.ColorN("Red");
			
			JokePartTip tip = Tip.InstanceOrNull<JokePartTip>();
			AddChild(tip);
			tip.Position = Elements[Elements.Count - 1].DesiredLocation;

			string tipText = "";
			switch (AssembledJoke.GetFinishReason())
			{
				case FinishReason.Mismatch:
					tipText = "Missmatch";
					break;
				case FinishReason.Repeat:
					tipText = "Repeate";
					break;
				case FinishReason.Spoiled:
					tipText = "Spoiled";
					break;
			}
			
			tip.SetText(tipText, Godot.Color.ColorN("Red"), FadeoutTime);
		}
		
		float startTimeMs = Time.GetTicksMsec();
		
		Timer fadeOutTimer = new Timer(10f);
		fadeOutTimer.Elapsed += (object sender, ElapsedEventArgs e) => 
		{
			float localTime = (Time.GetTicksMsec() - startTimeMs) / 1000f;
			float progress = Mathf.InverseLerp(0, FadeoutTime, localTime);
			float alpha = Mathf.Sqrt(1f - progress);
			Godot.Color newModulate = Modulate;
			newModulate.a = alpha;
			Modulate = newModulate;
			
			if (localTime >= FadeoutTime)
			{
				(sender as Timer).Stop();
				(sender as Timer).Dispose();
				OnJokePushed();
			}
		};
		fadeOutTimer.Start();
		
		Timer pushDelay = new Timer(FadeoutTime / 2 * 1000);
		pushDelay.AutoReset = false;
		pushDelay.Elapsed += (object sender, ElapsedEventArgs e) => 
		{
			Hall.StaticHall.PushJoke(AssembledJoke);
		};
		pushDelay.Start();
	}

	void OnJokePushed()
	{
		AssembledJoke = new Joke(MaxSequenceLength);
		
		for (int idx = 0; idx < Elements.Count; idx++)
		{
			Elements[idx].Element.QueueFree();
		}
			
		Elements.Clear();

		BackGround.Modulate = Godot.Color.ColorN("White");
		Modulate = Godot.Color.ColorN("White");
	}
}
