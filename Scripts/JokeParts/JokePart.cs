using GGJ24.Scripts.Shapes;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public class JokePart : Node2D
	{
		public Color Color;
		public Shape Shape;
		public JokeOperationType OperationType;
		
		public JokePartOperation Operation;

		public void Setup(Color inColor, Shape inShape, JokeOperationType inOperationType)
		{
			Color = inColor;
			Shape = inShape;
			OperationType = inOperationType;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var jokePartOperationFactory = GetNode<JokePartOperationFactory>("%JokePartOperationFactory");
			if (jokePartOperationFactory != null)
			{
				Operation = jokePartOperationFactory.Create(OperationType);
				AddChild(Operation);
			}

			var shapesStorage = GetNode<ShapesStorage>("%ShapesStorage");
			var shapeSprite = GetNode<Sprite>("%ShapeSprite");
			if (shapesStorage != null && shapeSprite != null)
			{
				var shapeData = shapesStorage.GetShape(Shape);
				shapeSprite.Texture = shapeData.Texture;
				shapeSprite.Scale = new Vector2(2, 2);
			}
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
