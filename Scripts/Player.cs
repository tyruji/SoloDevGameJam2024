using Godot;
using System;

public partial class Player : Node
{
    [Export]
    private WallHandler _WallHandler = null;

    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnHitWallLeft;
    public event Action OnHitWallRight;
    public event Action OnHitWallUp;

        // Will be of values 0, 1 or 2
    private int _playerPos = 1;

    private const int _playerHeight = 9;

    public override void _Ready()
    {
        _WallHandler ??= GetNode<WallHandler>( "../WallHandler" );
        
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( @event.IsActionPressed( "left" ) )
        {
            MovePlayer( -1 );

            return;
        }
        if( @event.IsActionPressed( "right" ) )
        {
            MovePlayer( 1 );
        }
    }

    private void MovePlayer( int direction )
    {
        _playerPos += direction;

        if( direction < 0 && ( _playerPos < 0
            || _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall ) )
        {
            OnHitWallLeft?.Invoke();
            return;
        }

        if( direction > 0 && ( _playerPos > 2
            || _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall ) )
        {
            OnHitWallRight?.Invoke();
            return;
        }

        _WallHandler.AdvanceRow();
        
        if( _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall )
        {
            OnHitWallUp?.Invoke();
            return;
        }

        var @event = direction < 0 ? OnMoveLeft : OnMoveRight;
        @event?.Invoke();
    }
}
