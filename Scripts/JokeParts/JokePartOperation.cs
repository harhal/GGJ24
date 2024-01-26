using System;
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

	public class JokePartOperation : Node2D
	{
		[Export] protected Texture OperationTexture;

		protected JokePartProcessPhase ProcessPhase;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GD.Print("JokePartOperation ready");

			var sprite = GetChild<Sprite>(0);

			if (sprite == null)
			{
				return;
			}

			sprite.Texture = OperationTexture;
		}
		
	}
}
