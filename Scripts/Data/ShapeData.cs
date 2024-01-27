using Godot;

namespace GGJ24.Scripts.Data
{
	public partial class ShapeData : Resource
	{
		[Export] public Shape Shape;
		[Export] public Texture JokePartShapeTexture;
		[Export] public Vector2 JokePartOperationTextureOffset; // different shapes want to adjust operation texture differently
	}
}
