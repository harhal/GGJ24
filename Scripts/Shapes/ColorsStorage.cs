using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts.Shapes
{
	public class ColorsStorage : Node
	{
		[Export] private List<ColorData> _colorDataList;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		
		}
		
		public ColorData GetColor(Color color)
		{
			return _colorDataList.Find(colorData => colorData != null && colorData.Enum == color);
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
