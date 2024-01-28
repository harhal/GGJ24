using System;
using Godot;
using Godot.Collections;

namespace GGJ24.Scripts
{
    public class BooPlayer : AudioStreamPlayer2D
    {
        [Export] private Array<AudioStream> _audioSamples;
		
        RandomNumberGenerator randGen = new RandomNumberGenerator();

        public override void _Ready()
        {
            JokeAssembler.StaticAssembler.OnJokeFailedDelegate += () =>
            {
                if (_audioSamples.Count > 0)
                {
                    Stream = _audioSamples[randGen.RandiRange(0, _audioSamples.Count - 1)];
                    Play();
                }
            };
        }
    }
}
