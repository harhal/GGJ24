using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts.Data
{
	public class ShapesStorage : Node
	{
		[Export] private List<ShapeData> _shapeDataList = new List<ShapeData>();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public ShapeData GetShape(Shape shape)
		{
			return _shapeDataList.Find(shapeData => shapeData != null && shapeData.Shape == shape);
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
