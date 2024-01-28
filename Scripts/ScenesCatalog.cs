using Godot;
using System;
using Godot.Collections;

public class ScenesCatalog : Node
{
    [Export] private Array<PackedScene> _scenes;
    private static Array<PackedScene> StaticCatalog;
    [Export] private NodePath _musicPath;
    public static Node Music;

    public override void _Ready()
    {
        base._Ready();
        StaticCatalog = _scenes;
        Music = GetNode(_musicPath);
    }

    public static void MoveToScene(Node context, int sceneIdx)
    {
        context.GetTree().ChangeSceneTo(StaticCatalog[sceneIdx]);
    }
}
