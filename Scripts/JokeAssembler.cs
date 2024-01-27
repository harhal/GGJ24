using Godot;
using System;
using System.Collections.Generic;
using GGJ24.Scripts;
using GGJ24.Scripts.JokeParts;

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

	private Joke AssembledJoke;

	private bool IsLocked = false;

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
		if (IsLocked)
		{
			return false;
		}
		
		Transform2D GlobalTransform = Element.GlobalTransform;
		if (Element.GetParent() != null)
		{
			Element.GetParent().RemoveChild(Element);
		}

		if (AssembledJoke == null)
		{
			AssembledJoke = new Joke(MaxSequenceLength);
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

		if (AssembledJoke.IsFinished())
		{
			IsLocked = true;
		}
		
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
		OnJokePushed();
	}

	void OnJokePushed()
	{
		for (int idx = 0; idx < Elements.Count; idx++)
		{
			ElementWithTransition Focus = Elements[idx];
			Elements[idx].Element.QueueFree();
		}
			
		Elements.Clear();
		AssembledJoke = new Joke(MaxSequenceLength);
		IsLocked = false;
	}
}
