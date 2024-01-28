using System;
using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
    public partial class JokePartOperationPayload : Resource
    {
        [Export] public JokePartOperationType Type;
		
        [Export] public List<Texture> EmojiTextures;

        // [Export] public JokePartProcessPhase ProcessPhase;

        public Texture GetRandomEmojiTexture()
        {
            if (EmojiTextures.Count == 0)
            {
                return null;
            }

            var random = new Random();
            return EmojiTextures[random.Next(0, EmojiTextures.Count)];
        }
    }
}