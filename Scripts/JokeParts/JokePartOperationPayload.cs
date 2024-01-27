using Godot;

namespace GGJ24.Scripts.JokeParts
{
    public partial class JokePartOperationPayload : Resource
    {
        public JokePartOperationPayload() : this(JokePartOperationType.None, null, JokePartProcessPhase.None) {}
        public JokePartOperationPayload(JokePartOperationType inType, Texture inTexture, JokePartProcessPhase inProcessPhase)
        {
            Type = inType;
            Texture = inTexture;
            ProcessPhase = inProcessPhase;
        }

        [Export] public JokePartOperationType Type;
		
        [Export] public Texture Texture;

        [Export] public JokePartProcessPhase ProcessPhase;
    }
}