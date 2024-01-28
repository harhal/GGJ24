using Godot;

namespace GGJ24.Scripts.Menu
{
    public class ExitApp : Node
    {
        private void _on_Button2_pressed()
        {
            GetTree().Quit();
        }
    }
}
