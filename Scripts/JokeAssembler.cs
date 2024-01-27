using Godot;
using System;
using System.Collections.Generic;

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

    private List<ElementWithTransition> Elements;
    private float TransitionProgress;
    
    

    public void AddElement(Node2D Element)
    {
        Vector2 GlobalPosition = Element.GlobalPosition;
        if (Element.GetParent() != null)
        {
            Element.GetParent().RemoveChild(Element);
        }

        AddChild(Element);
        Element.GlobalPosition = GlobalPosition;
        Elements.Add(new ElementWithTransition(Element, Element.Position, Element.Position));
        UpdateDesiredLocations();
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
    }
}
