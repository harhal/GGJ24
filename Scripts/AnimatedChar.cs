using System;
using GGJ24.Scripts.JokeParts;
using Godot;

namespace GGJ24.Scripts
{
    public class AnimatedChar : AnimatedSprite
    {
        private Timer _returnToIdleTimer;

        [Export] private float _returnToIdleTime = 3.5f;

        private int _lastFrameIndex = 0;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _returnToIdleTimer = new Timer();
            AddChild(_returnToIdleTimer);
            _returnToIdleTimer.OneShot = true;
            _returnToIdleTimer.WaitTime = _returnToIdleTime;
            _returnToIdleTimer.Connect("timeout", this, "_on_timer_timeout");
            
            JokeAssembler assembler = JokeAssembler.StaticAssembler;
            
            assembler.Connect(nameof(JokeAssembler.JokePartAdded), this, "_on_joke_part_added");
        }

        public void _on_timer_timeout()
        {
            Animation = "idle";
            Playing = true;
        }

        private void _on_joke_part_added(JokePart part)
        {
            var rng = new RandomNumberGenerator();
            rng.Randomize();
            Animation = "action";
            Playing = false;

            //Hardcoded because I can't find the way to do this properly
            const int maxFrames = 2; //Frames.GetFrameCount("action") - 1;

            Frame = _lastFrameIndex;

            _lastFrameIndex++;
            _lastFrameIndex %= maxFrames;

            _returnToIdleTimer.Start();
        }
    }
}
