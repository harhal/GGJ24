using Godot;
using System;

public class CringeLevelScript : Node
{
    [Export] private float MinCringe = 0;

    [Export] private float MaxCringe = 100;

    [Export] private float CurrentCringe = 0;
    
    [Export] private float CringeFaloffPerSec = -1;

    private TextureProgress ProgressBar;
    
    public static CringeLevelScript StaticCringe;

    public void AddCringe(float AddCringe)
    {
        CurrentCringe = Mathf.Clamp(CurrentCringe + AddCringe, MinCringe, MaxCringe);

        UpdateCringeProgress();

        if (CurrentCringe >= MaxCringe)
        {
            DieFromCringe();
        }
    }

    private void UpdateCringeProgress()
    {
        if (ProgressBar != null)
        {
            ProgressBar.Value = CurrentCringe;
        }
    }

    public CringeLevelScript()
    {
        StaticCringe = this;
    }

    public override void _Ready()
    {
        base._Ready();
        ProgressBar = GetChild<TextureProgress>(0);
        if (ProgressBar != null)
        {
            ProgressBar.MinValue = MinCringe;
            ProgressBar.MaxValue = MaxCringe;
            ProgressBar.Value = 0;
        }
    }

    public override void _Process(float delta)
    {
        AddCringe(CringeFaloffPerSec * delta);
    }

    public void DieFromCringe()
    {
        throw new NotImplementedException();
    }
}
