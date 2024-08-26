using Godot;
using System;

public partial class CameraHandler : Camera2D
{
    [Export]
    private Node2D _PlayerFollow = null;

    [Export]
    private float _Dampening = 8.0f;

    public override void _Ready()
    {
        _PlayerFollow ??= GetNode<Node2D>( "../Player/PlayerSprite" );
    }

    public override void _Process(double delta)
    {
        var new_pos = Position * Vector2.Right + _PlayerFollow.Position * Vector2.Down;

        Position = Position.Lerp( new_pos, ( float ) delta * _Dampening );
    }
}
