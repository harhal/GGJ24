using Godot;
using System;
using GGJ24.Scenes;

public class Hall : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export] public int HallWidth = 5;
	[Export] public int HallDepth = 3;

	private Robot[,] _robots;

	public void Register(Robot inRobot)
	{
		//_robots[0, 0] = 1;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//robots
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
