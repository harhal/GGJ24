using GGJ24.Scripts.Data;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public class JokePart : Node2D
	{
		public Color Color;
		public Shape Shape;
		public JokePartOperationType OperationType;
		public bool bIsFree = true;
		
		public JokePartOperation Operation;

		public void Setup(Color inColor, Shape inShape, JokePartOperationType inOperationType)
		{
			Color = inColor;
			Shape = inShape;
			OperationType = inOperationType;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			var jokePartOperationFactory = GetNode<JokePartOperationFactory>("%JokePartOperationFactory");
			var shapesStorage = GetNode<ShapesStorage>("%ShapesStorage");
			var colorsStorage = GetNode<ColorsStorage>("%ColorsStorage");
			var shapeSprite = GetNode<Sprite>("%ShapeSprite");
			if (jokePartOperationFactory == null || shapesStorage == null || shapeSprite == null)
			{
				return;
			}

			var shapeData = shapesStorage.GetShape(Shape);
			var colorData = colorsStorage.GetColor(Color);
			if (shapeData == null || colorData == null)
			{
				return;
			}

			shapeSprite.Texture = shapeData.JokePartShapeTexture;
			shapeSprite.Modulate = colorData.Color;

			Operation = jokePartOperationFactory.Create(OperationType);
			if (Operation != null)
			{
				Operation.TextureOffset = shapeData.JokePartOperationTextureOffset;
				AddChild(Operation);
			}
		}
		
		private void _on_Area2D_input_event(object viewport, object @event, int shape_idx)
		{
			InputEventMouseButton MouseButtonInput = @event as InputEventMouseButton;
			if (MouseButtonInput != null)
			{
				SendToJoke();
			}

			InputEventScreenTouch ScreenTouchInput = @event as InputEventScreenTouch;
			if (ScreenTouchInput != null)
			{
				SendToJoke();
			}
		}

		private void SendToJoke()
		{
			if (!bIsFree)
			{
				return;
			}
			
			if (JokeAssembler.StaticAssembler != null)
			{
				bIsFree = !JokeAssembler.StaticAssembler.AddElement(this);
			}
		}
	}
}
