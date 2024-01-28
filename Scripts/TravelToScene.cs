using Godot;

namespace GGJ24.Scripts
{
	public class TravelToScene : Node
	{
		[Export] private PackedScene _scene;


		private void _on_Button_pressed()
		{
			Node newScene = _scene.InstanceOrNull<Node>();
			Node root = GetTree().Root;
			Node preRoot = this;
			while (preRoot != root && preRoot != null)
			{
				preRoot = preRoot.GetParent();
			}
			root.AddChild(newScene);
			root.RemoveChild(preRoot);
		}
	}
}
