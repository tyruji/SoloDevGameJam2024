using Godot;
using System;

public partial class PlayerAnimator : Sprite2D
{
    [Export]
    private Player _Player = null;

    [Export]
    private AnimationPlayer _AnimationPlayer = null;

    [Export]
    private float _AnimationDuration = .2f;

    [Export]
    private Tween.TransitionType _TransitionType = Tween.TransitionType.Elastic;

    [Export]
    private Tween.EaseType _EaseType = Tween.EaseType.InOut;

    private Vector2 _targetPosition = Vector2.Zero;

    private GpuParticles2D _particles = null;

    public override void _Ready()
    {
        _particles = GetNode<GpuParticles2D>( "GPUParticles2D" );
        _AnimationPlayer ??= GetNode<AnimationPlayer>( "../AnimationPlayer" );

        _Player ??= GetNode<Player>( ".." );
        _Player.OnMoveLeft += MoveLeft;
        _Player.OnMoveRight += MoveRight;
        _Player.OnHitWall += HitWall;

        _targetPosition = Position;
    }

    private void MoveLeft()
    {
        var tween = CreateTween()
            .SetTrans( _TransitionType )
            .SetEase( _EaseType );
        
        Position = _targetPosition;

        _AnimationPlayer.Play( "bounce_left" );
        var time = _AnimationPlayer.CurrentAnimationLength / _AnimationPlayer.SpeedScale;

        _targetPosition += Vector2.Left * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .5f * time );

        _targetPosition += Vector2.Up * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .5f * time );

        tween.TweenCallback( Callable.From( () => { _AnimationPlayer.Play( "idle" ); } ) );
    }
    private void MoveRight()
    {
        var tween = CreateTween()
            .SetTrans( _TransitionType )
            .SetEase( _EaseType );

        Position = _targetPosition;

        _AnimationPlayer.Play( "bounce_right" );
        var time = _AnimationPlayer.CurrentAnimationLength / _AnimationPlayer.SpeedScale;

        _targetPosition += Vector2.Right * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .5f * time );

        _targetPosition += Vector2.Up * WallDrawer.WALL_PIXEL_SIZE;
        tween.TweenProperty( this, "position", _targetPosition, .5f * time );

        tween.TweenCallback( Callable.From( () => { _AnimationPlayer.Play( "idle" ); } ) );
    }

    private void HitWall()
    {
        _particles.Emitting = true;

            // hide the sprite
        SelfModulate = new Color( 0,0,0,0 );

            // Disable the trail
        GetNode<GpuParticles2D>( "trail" ).Visible = false;
    }

    // private void HitWallRight()
    // {
    //     var tween = CreateTween()
    //         .SetParallel()
    //         .SetTrans( _TransitionType )
    //         .SetEase( _EaseType );
        
    //     Position = _targetPosition;

    //     _targetPosition += .5f * WallDrawer.WALL_PIXEL_SIZE * Vector2.Right;
    //     tween.TweenProperty( this, "position", _targetPosition, _AnimationDuration );
    //     tween.TweenProperty( this, "scale", 1.2f * Vector2.Down, _AnimationDuration );
    // }
}
