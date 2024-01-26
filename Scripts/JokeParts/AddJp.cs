using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public class AddJp : JokePart
	{
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			ProcessPhase = JokePartProcessPhase.DuringMain;
		}

		public override bool Match(JokePart other)
		{
			return true;
		}
	}
}
