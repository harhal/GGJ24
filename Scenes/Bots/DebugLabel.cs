using Godot;
using System;
using GGJ24.Scripts.Robot;

public class DebugLabel : Label
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Robot robot = GetParent<Robot>();

        Text = robot.GetFun().ToString() + "\n" + robot.LastAddedFun.ToString();
    }
}