using System.Collections.Generic;
using Godot;

namespace GGJ24.Scripts.JokeParts
{
	public class JokePartOperationFactory : Node
	{
		[Export] public PackedScene JokePartOperationTemplate;
		[Export] public List<JokePartOperationPayload> JokePartOperationsSetup;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
		}

		public JokePartOperation Create(JokePartOperationType type)
		{
			if (!JokePartOperationsSetup.Exists(operationPayload => operationPayload != null && operationPayload.Type == type))
			{
				return null;
			}

			JokePartOperation operation = JokePartOperationTemplate.InstanceOrNull<JokePartOperation>();
			if (operation == null)
			{
				return null;
			}

			operation.Setup(JokePartOperationsSetup.Find(operationPayload => operationPayload.Type == type));
			return operation;
		}

	}
}
