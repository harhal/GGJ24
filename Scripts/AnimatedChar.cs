using Godot;

namespace GGJ24.Scripts
{
    public class AnimatedChar : AnimatedSprite
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        private Timer _timer;

        [Export] private float _returnToIdleTime = 5;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            //TODO: sub to joke part being added and start special talk animation
            
            _timer = new Timer();
            AddChild(_timer);
            _timer.Connect("timeout", this, "_on_timer_timeout");
            _timer.OneShot = true;
            _timer.WaitTime = _returnToIdleTime;
            _timer.Start();
        }

        public void _on_timer_timeout()
        {
            Animation = "idle";
            GD.Print("Timeout!");
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
