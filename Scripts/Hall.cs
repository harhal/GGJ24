using System.Timers;
using Godot;
using GGJ24.Scripts;
using GGJ24.Scripts.Robot;

public class Hall : Node2D
{
	[Export] public int HallWidth = 5;
	[Export] public int HallDepth = 3;

	[Export] private int _lowFunRobotsToLose = 9;

	[Signal]
	public delegate void AllRobotsCompletedFun();

	[Signal]
	public delegate void MostRobotsGotLowFun();

	private const int RobotsCount = 12;
	private Robot[] _robots = new Robot[RobotsCount];

	private int _lastAddedIndex = 0;
	private int _completedFunRobots = 0;
	private int _lowFunRobots = 0;

	public static Hall StaticHall;
	[Export] private PackedScene Tip;
	[Export] private float TipOffset = 100;

	public void Register(Robot inRobot)
	{
		_robots.SetValue(inRobot, _lastAddedIndex);
		inRobot.Connect(nameof(Robot.FunLevelChanged), this, "_on_Robot_FunLevelChanged");
		_lastAddedIndex++;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StaticHall = this;
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

	public void PushJoke(Joke assembledJoke)
	{
		foreach (Robot robot in _robots)
		{
			if (robot != null)
			{
				float Value = robot.ReceiveJoke(assembledJoke);
				if (Value != 0)
				{
					const float FadeOutTime = 1f;
					JokePartTip tip = Tip.InstanceOrNull<JokePartTip>();
					AddChild(tip);
					tip.Position = robot.Position + TipOffset * Vector2.Up;
					Godot.Color tipColor = Value > 0 ? Godot.Color.ColorN("Green") : Godot.Color.ColorN("Red");
					tip.SetText(Value.ToString("0."), tipColor, FadeOutTime);

					float startTimeMs = Time.GetTicksMsec();
					
					System.Timers.Timer fadeOutTimer = new System.Timers.Timer();
					fadeOutTimer.Interval = 10f;
					fadeOutTimer.Elapsed += (sender, args) =>
					{
						if (tip != null)
						{
							return;
						}
						
						float localTime = (Time.GetTicksMsec() - startTimeMs) / 1000f;
						float progress = Mathf.InverseLerp(0, FadeOutTime, localTime);
						float alpha = Mathf.Sqrt(1f - progress);
						Godot.Color newModulate = tip.Modulate;
						newModulate.a = alpha;
						tip.Modulate = newModulate;

						if (localTime >= FadeOutTime)
						{
							(sender as System.Timers.Timer).Stop();
							(sender as System.Timers.Timer).Dispose();
							tip.QueueFree();
							tip = null;
						}
					};
					fadeOutTimer.Start();
				}
			}
		}
	}
}

