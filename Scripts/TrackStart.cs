using Godot;
using System;

public class TrackStart : Position2D
{
	[Export] private int _horizontalSpawnDeviation = 100;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public Vector2 GetStartPosition()
	{
		var random = new Random();
		var xDeviation = random.Next(-_horizontalSpawnDeviation, _horizontalSpawnDeviation);

		return new Vector2(Position.x + xDeviation, Position.y);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
