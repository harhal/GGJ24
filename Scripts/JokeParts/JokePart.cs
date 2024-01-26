using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts
{
	public enum JokePartProcessPhase
	{
		BeforeAllMain,
		DuringMain,
		AfterAllMain,
		Specific
	}

	public class JokePart : Node2D
	{
		[Export] public Color Color;
		[Export] public Shape Shape;
		[Export] public JokePartProcessPhase ProcessPhase;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public virtual bool Match(JokePart other)
		{
			return true;
		}
		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
