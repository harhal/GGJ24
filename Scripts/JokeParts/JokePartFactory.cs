using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public class JokePartFactory : Node
	{
		[Export] public PackedScene JokePartTemplate;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			AddChild(Create(Color.TYPE1, Shape.Triangle, JokeOperationType.AddOne));
		}

		public JokePart Create(Color inColor, Shape inShape, JokeOperationType inOperationType)
		{
			var jokePart = JokePartTemplate.InstanceOrNull<JokePart>();
			if (jokePart == null)
			{
				return null;
			}
			
			jokePart.Setup(inColor, inShape, inOperationType);
			return jokePart;
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
