using GGJ24.Scripts;
using GGJ24.Scripts.Robot;
using Godot;

namespace GGJ24.Scenes
{
    public class RobotLamp : Sprite
    {
        private Robot _parent;

        [Export] private Gradient _gradient;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _parent = GetParent<Robot>();
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(float delta)
        {
            Modulate = new Godot.Color(_gradient.Interpolate(_parent.GetFun()));
        }
    }
}