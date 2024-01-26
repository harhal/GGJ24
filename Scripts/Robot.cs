using System;
using Godot;
using Godot.Collections;

namespace GGJ24.Scenes
{
    public class Robot : Node2D
    {
        [Export]
        private Color RobotColor = Color.TYPE1;
    
        [Export]
        private Shape RobotShape = Shape.TYPE1;
        
        
        //TODO: rename all of this
        [Export]
        Texture FirstShapeTexture;
        
        [Export]
        Texture SecondShapeTexture;
        
        [Export]
        Texture ThirdShapeTexture;

        [Export] private Godot.Color FirstColor;
        [Export] private Godot.Color SecondColor;
        [Export] private Godot.Color ThirdColor;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            var sprite = GetChild<Sprite>(0);

            switch (RobotColor)
            {
                case Color.TYPE1:
                    sprite.Modulate = FirstColor;
                    break;
                case Color.TYPE2:
                    sprite.Modulate = SecondColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (RobotShape)
            {
                case Shape.TYPE1:
                    sprite.Texture = FirstShapeTexture;
                    break;
                
                case Shape.TYPE2:
                    sprite.Texture = SecondShapeTexture;
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
