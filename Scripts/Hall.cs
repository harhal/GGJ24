using Godot;
using GGJ24.Scripts;
using GGJ24.Scripts.Robot;

public class Hall : Node2D
{
	[Export] public int HallWidth = 5;
	[Export] public int HallDepth = 3;
	
	[Export] private int _lowFunRobotsToLose = 9;
	
	[Signal] public delegate void AllRobotsCompletedFun();
	[Signal] public delegate void MostRobotsGotLowFun();

	private const int RobotsCount = 12;
	private Robot[] _robots = new Robot[RobotsCount];

	private int _lastAddedIndex = 0;
	private int _completedFunRobots = 0;
	private int _lowFunRobots = 0;

	public void Register(Robot inRobot)
	{
		_robots.SetValue(inRobot, _lastAddedIndex);
		inRobot.Connect(nameof(Robot.FunLevelChanged), this, "_on_Robot_FunLevelChanged");
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

	private void _on_Robot_FunLevelChanged(Robot robot, FunLevel previousFunLevel, FunLevel newFunLevel)
	{
		if (newFunLevel == FunLevel.Completed)
		{
			_completedFunRobots++;

			if (_completedFunRobots >= RobotsCount)
			{
				GD.Print("ALL ROBOTS COMPLETED FUN");
				EmitSignal(nameof(AllRobotsCompletedFun));
			}
		}

		if (newFunLevel == FunLevel.Low)
		{
			_lowFunRobots++;
		}
		
		if (previousFunLevel == FunLevel.Low)
		{
			_lowFunRobots--;
		}

		if (_lowFunRobots >= _lowFunRobotsToLose)
		{
			GD.Print("MOST ROBOTS GOT LOW FUN");
			EmitSignal(nameof(MostRobotsGotLowFun));
		}
	}
	
	private void _on_SubButton_pressed()
	{
		for (var i = 0; i < RobotsCount; i++)
		{
			_robots[i].AddFun(-0.1f);
		}
	}

	private void _on_AddButton_pressed()
	{
		for (var i = 0; i < RobotsCount; i++)
		{
			_robots[i].AddFun(0.1f);
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

