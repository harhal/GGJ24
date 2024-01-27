using Godot;
using System;
using GGJ24.Scripts;

public class Hall : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export] public int HallWidth = 5;
	[Export] public int HallDepth = 3;

	private Robot[,] _robots = new Robot[5,3];

	public void Register(Robot inRobot)
	{
		_robots[inRobot.seatRow, inRobot.seat] = inRobot;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (var child in GetChildren())
		{
			if (child is Robot robot)
			{
				Register(robot);
			}
		}
	}
	
	private void _on_Button_pressed()
	{
		// Replace with function body.
		foreach (var robot in _robots)
		{
			if(robot == null || !robot.Visible) continue;
			
			Joke joke = new Joke();
			robot.ReceiveJoke(joke);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}



