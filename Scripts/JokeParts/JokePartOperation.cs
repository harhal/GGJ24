using System;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public enum JokePartOperationType
	{
		None,
		AddOne,
		MinusOne,
		MinusTwo,
		Opener,
		Robot,
		Human,
		RemovePrevious,
		CopyPrevious,
		ChekhovGun,
		Spoiler,
		Punchline,
		Joker,
		Max
	}
	
	public enum JokePartProcessPhase
	{
		None,
		BeforeAllMain,
		DuringMain,
		AfterAllMain,
		Specific
	}

	public class JokePartOperation : Node2D
	{
		// setup from payload:
		public JokePartOperationType Type;
		protected Texture Texture;
		public JokePartProcessPhase ProcessPhase;
		
		// state:
		public Vector2 TextureOffset;

		public static JokePartOperationType GetRandomJokePartOperationType()
		{
			var random = new Random();
			return (JokePartOperationType) random.Next(1, (int)JokePartOperationType.Max);
		}

		public void Setup(JokePartOperationPayload payload)
		{
			Type = payload.Type;
	
			Texture = payload.Texture;
			ProcessPhase = payload.ProcessPhase;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var sprite = GetNode<Sprite>("%Sprite");
			if (sprite != null)
			{
				Ready_SetupSprite(sprite);
			}
		}

		protected void Ready_SetupSprite(Sprite sprite)
		{
			sprite.Translate(TextureOffset);
			sprite.Texture = Texture;
		}
		
	}
}
