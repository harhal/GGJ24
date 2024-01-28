using GGJ24.Scripts.JokeParts;
using Godot;
using Godot.Collections;

namespace GGJ24.Scripts
{
	public class JokeLiner : AudioStreamPlayer2D
	{
		[Export] private Array<AudioStream> _audioSamples;
		private Array<AudioStream> _queue = new Array<AudioStream>();
		
		RandomNumberGenerator randGen = new RandomNumberGenerator();

		public override void _Ready()
		{
			base._Ready();
			
			JokeAssembler.StaticAssembler.Connect(nameof(JokeAssembler.JokePartAdded), this, "_on_JokeAssembler_JokePartAdded");
		}

		private void _on_JokeAssembler_JokePartAdded(JokePart jokePart)
		{
			PushIntoQueue(_audioSamples[randGen.RandiRange(0, _audioSamples.Count - 1)]);
		}

		private void PushIntoQueue(AudioStream audioSample)
		{
			if (Stream == null || !Playing)
			{
				Stream = audioSample;
				Play();
			}
			else
			{
				_queue.Add(audioSample);
			}
		}

		private void _on_JokeLiner_finished()
		{
			if (_queue.Count <= 0)
			{
				return;
			}
			
			Stream = _queue[0];
			_queue.RemoveAt(0);
			
			Play();
		}
	}
}
