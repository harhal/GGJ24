using Godot;
using System;
using System.Timers;

public class JokePartTip : Node2D
{
    private Label Label;

    private float _lifeTime = 0.5f;
    private float _startTimeMs;

    public override void _Ready()
    {
        Label = GetNode<Label>("Label");
        _startTimeMs = Time.GetTicksMsec();
    }

    public void SetText(string SetText, Godot.Color Color, float LifeTime)
    {
        Modulate = Color;
        Label.Text = SetText;
        _lifeTime = LifeTime;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (Time.GetTicksMsec() - _startTimeMs >= _lifeTime * 1000)
        {
            QueueFree();
        }
    }
}
