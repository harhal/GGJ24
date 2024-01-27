using Godot;

namespace GGJ24.Scripts.JokeParts
{
    public partial class JokePartOperationPayload : Resource
    {
        public JokePartOperationPayload() : this(JokeOperationType.None, null, JokePartProcessPhase.None, "") {}
        public JokePartOperationPayload(JokeOperationType inType, Texture inTexture, JokePartProcessPhase inProcessPhase, string inLabelText)
        {
            Type = inType;
            Texture = inTexture;
            ProcessPhase = inProcessPhase;
            LabelText = inLabelText;
        }

        [Export] public JokeOperationType Type;
		
        [Export] public Texture Texture;

        [Export] public JokePartProcessPhase ProcessPhase;

        [Export] public string LabelText;
    }
}