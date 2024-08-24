using Godot;
using System;

public partial class PlayerAnimator : Sprite2D
{
    [Export]
    private Player _Player = null;

    private Vector2 _targetPosition = Vector2.Zero;

    public override void _Ready()
    {
        _Player ??= GetNode<Player>( ".." );
        _Player.OnMoveLeft += MoveLeft;
        _Player.OnMoveRight += MoveRight;
        _Player.OnHitWallLeft += HitWallLeft;
        _Player.OnHitWallRight += HitWallRight;

        _targetPosition = Position;
    }

    private void MoveLeft()
    {
        var tween = CreateTween();
        Position = _targetPosition;

        _targetPosition += Vector2.Left * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .2f );

        _targetPosition += Vector2.Up * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .2f );
    }
    private void MoveRight()
    {
        var tween = CreateTween();
        Position = _targetPosition;

        _targetPosition += Vector2.Right * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .2f );

        _targetPosition += Vector2.Up * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .2f );
    }

    private void HitWallLeft()
    {
        var tween = CreateTween().SetParallel();
        Position = _targetPosition;

        _targetPosition += .5f * WallDrawer.WALL_PIXEL_SIZE * Vector2.Left;
        tween.TweenProperty( this, "position", _targetPosition, .2f );
        tween.TweenProperty( this, "scale", 1.2f * Vector2.Down, .2f );
    }
    private void HitWallRight()
    {
        var tween = CreateTween().SetParallel();
        Position = _targetPosition;

        _targetPosition += .5f * WallDrawer.WALL_PIXEL_SIZE * Vector2.Right;
        tween.TweenProperty( this, "position", _targetPosition, .2f );
        tween.TweenProperty( this, "scale", 1.2f * Vector2.Down, .2f );
    }
}
