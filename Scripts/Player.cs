using Godot;
using System;

public partial class Player : Node
{
    [Export]
    private WallHandler _WallHandler = null;

    public bool Lost { get; private set; } = false;

    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnHitWall;

        // Will be of values 0, 1 or 2
    private int _playerPos = 1;


    private const int _playerHeight = 9;

    public override void _Ready()
    {
        _WallHandler ??= GetNode<WallHandler>( "../WallHandler" );
        
    }

    public override void _UnhandledInput( InputEvent @event )
    {
        if( Lost ) return;

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

    public void Kill()
    {
        Lost = true;
        OnHitWall?.Invoke();
    } 

    private void MovePlayer( int direction )
    {
        _playerPos += direction;

        if( direction < 0 && ( _playerPos < 0
            || _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall ) )
        {
            Kill();
            return;
        }

        if( direction > 0 && ( _playerPos > 2
            || _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall ) )
        {
            Kill();
            return;
        }

        _WallHandler.AdvanceRow();
        
        if( _WallHandler.walls[ _playerPos, _playerHeight ] == eWallType.Wall )
        {
            Kill();
            return;
        }

        var @event = direction < 0 ? OnMoveLeft : OnMoveRight;
        @event?.Invoke();
    }
}
