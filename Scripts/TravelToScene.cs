using Godot;

namespace GGJ24.Scripts
{
	public class TravelToScene : Node
	{
		[Export] private int _sceneIdx = 0;


		private void _on_Button_pressed()
		{
			ScenesCatalog.MoveToScene(this, _sceneIdx);
		}
	}
}
