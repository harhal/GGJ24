using Godot;
using System;
using GGJ24.Scripts;
using GGJ24.Scripts.Robot;

public class MockeryScript : Node2D
{
	[Export] private float LifeTime = 3f;
	[Export] private float LeftTime;
	[Export] private float DamageAtTheEnd = 15f;
	[Export] private float FunPerClick = 1f;
	private CringeLevelScript Cringe;
	public Robot OwnerRobot;
	[Export] private NodePath ProgressBarNode;

	private TextureProgress ProgressBar;
	
	public override void _Ready()
	{
		base._Ready();
		Cringe = CringeLevelScript.StaticCringe;

		if (ProgressBarNode != null)
		{
			ProgressBar = GetNode<TextureProgress>(ProgressBarNode);
			if (ProgressBar != null)
			{
				ProgressBar.MinValue = 0;
				ProgressBar.MaxValue = 100;
				ProgressBar.Value = 100;
			}
		}

		LeftTime = LifeTime;
	}
	
	public override void _Process(float delta)
	{
		if (LeftTime <= 0)
		{
			return;
		}
		
		LeftTime -= delta;
		
		if (ProgressBar != null)
		{
			ProgressBar.Value = 100 - (LeftTime / LifeTime) * 100;
		}

		if (LeftTime <= 0)
		{
			if (Cringe != null)
			{
				Cringe.AddCringe(DamageAtTheEnd);
				return;
			}
			
			Destory();
		}
	}

	private void _on_Area2D_input_event(object viewport, object @event, int shape_idx)
	{
		if (OwnerRobot != null)
		{
			OwnerRobot.AddFun(FunPerClick);
		}

		Destory();
	}

	private void Destory()
	{
		//QueueFree();
	}
}
