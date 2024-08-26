using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    private PackedScene _GameScene = null;

    private void Play()
    {
        GetTree().ChangeSceneToPacked( _GameScene );
    }
}
