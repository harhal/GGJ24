using Godot;
using System;
using Godot.Collections;

public class ScenesCatalog : Node
{
	[Export] private Array<PackedScene> _scenes;
	private static Array<PackedScene> StaticCatalog;
	[Export] private NodePath _musicPath;

	public override void _Ready()
	{
		base._Ready();
		StaticCatalog = _scenes;
	}

	public static void MoveToScene(Node context, int sceneIdx)
	{
		if (context != null)
		{
			context.GetTree().ChangeSceneTo(StaticCatalog[sceneIdx]);
		}
	}
}
