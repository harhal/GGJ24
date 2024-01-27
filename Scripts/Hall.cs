using Godot;
using System;
using GGJ24.Scripts;

public class Hall : Node2D
{
	[Export] public int HallWidth = 5;
	[Export] public int HallDepth = 3;
	
	[Signal] public delegate void AllRobotsHappilyFinished();

	public const int RobotsCount = 12;
	private Robot[] _robots = new Robot[RobotsCount];

	private int _lastAddedIndex = 0;
	private int _happilyFinishedRobots = 0;

	public void Register(Robot inRobot)
	{
		_robots.SetValue(inRobot, _lastAddedIndex);
		inRobot.Connect(nameof(Robot.HappilyFinished), this, "_on_Robot_HappilyFinished");
		_lastAddedIndex++;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var children = GetChildren();
		foreach (var child in children)
		{
			if (child is Robot robot)
			{
				Register(robot);
			}
		}
	}

	private void _on_Robot_HappilyFinished(Robot robot)
	{
		_happilyFinishedRobots++;

		if (_happilyFinishedRobots >= RobotsCount)
		{
			GD.Print("ALL FINISHED");
			EmitSignal(nameof(AllRobotsHappilyFinished));
		}
	}
	
	private void _on_Button_pressed()
	{
		// Replace with function body.
		for (var i = 0; i < RobotsCount; i++)
		{
			Joke joke = new Joke(4);
			_robots[i].ReceiveJoke(joke);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}



