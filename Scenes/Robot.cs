using System;
using Godot;

namespace GGJ24.Scenes
{
    public class Robot : Node2D
    {
        [Export]
        private Color RobotColor = Color.TYPE1;
    
        [Export]
        private Shape RobotShape = Shape.TYPE1;
    
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            var sprite = GetChild<Sprite>(0);

            switch (RobotColor)
            {
                case Color.TYPE1:
                    sprite.Modulate = new Godot.Color(0.2f, 0.1f, 1);
                    break;
                case Color.TYPE2:
                    sprite.Modulate = new Godot.Color(0.6f, 0.1f, 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
