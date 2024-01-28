using Godot;
using System;
using System.Timers;

public class JokePartTip : Node2D
{
    private Label Label;

    //[Export] private float LifeTime = 0.5f;
    private float StartTimeMs;

    public override void _Ready()
    {
        Label = GetNode<Label>("Label");
        StartTimeMs = Time.GetTicksMsec();
    }

    public void SetText(string SetText, Godot.Color Color, float LifeTime)
    {
        Modulate = Color;
        Label.Text = SetText;
		
        System.Timers.Timer delay = new System.Timers.Timer(LifeTime * 1000);
        delay.AutoReset = true;
        delay.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
            QueueFree();
        };
        delay.Start();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        
        /*float localTime = (Time.GetTicksMsec() - StartTimeMs) / 1000f;
        float progress = Mathf.InverseLerp(0, LifeTime, localTime);
        float alpha = Mathf.Sqrt(1f - progress);
        Godot.Color newModulate = Modulate;
        newModulate.a = alpha;
        Modulate = newModulate;

        if (localTime >= LifeTime)
        {
            QueueFree();
        }*/
    }
}
