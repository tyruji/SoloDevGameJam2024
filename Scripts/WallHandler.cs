using Godot;
using System;

public partial class WallHandler : Node2D
{
    public const int WALL_COLUMNS = 3;
    public const int WALL_ROWS = 17;
    public const int MAX_WALL_LENGTH = 6;

    public readonly eWallType [,] walls = new eWallType[ WALL_COLUMNS, WALL_ROWS ];

    public event Action OnAdvanceRows;

    public override void _Ready()
    {
        for( int i = 0; i < walls.GetLength( 1 ) / 2; ++i )
        {
            PushRowsDown();
        }
    }

    public void AdvanceRow()
    {
        PushRowsDown();
        OnAdvanceRows?.Invoke();
    }

    private void PushRowsDown()
    {
        MoveRowsDown();

            // Clear the first row.
        walls[ 0, 0 ] = walls[ 1, 0 ] = walls[ 2, 0 ] = eWallType.Empty;

        int countL;
        for( countL = 0; countL < MAX_WALL_LENGTH; ++countL )
        {
            if( walls[ 0, countL + 1 ] != eWallType.Wall ) break;
        }

        int countR;
        for( countR = 0; countR < MAX_WALL_LENGTH; ++countR )
        {
            if( walls[ 2, countR + 1 ] != eWallType.Wall ) break;
        }

        //GD.Print( "countL: ", countL, ", countR: ", countR );

            // If there are no previous walls on both sides, spawn at random.
        if( countL == 0 && 0 == countR )
        {
            walls[ GD.RandRange( 0, 1 ) * 2, 0 ] = eWallType.Wall;
            return;
        }

            // If reached max wall count on side, create on the other.
        if( countL == MAX_WALL_LENGTH )
        {
            walls[ 2, 0 ] = eWallType.Wall;
            return;
        }
        if( countR == MAX_WALL_LENGTH )
        {
            walls[ 0, 0 ] = eWallType.Wall;
            return;
        }

            // If there is uneven number of walls on one side,
            // we must place another or else its not solvable.
        if( ( countL + 1 ) % 2 == 0 )
        {
            walls[ 0, 0 ] = eWallType.Wall;
            return;
        }
        if( ( countR + 1 ) % 2 == 0 )
        {
            walls[ 2, 0 ] = eWallType.Wall;
            return;
        }


            // If there are even number of walls, randomly place the walls.
        bool randomChance = GD.Randf() < .5f;
        if( countL % 2 == 0 && randomChance )
        {
            walls[ 0, 0 ] = eWallType.Wall;
            return;
        }
        walls[ 2, 0 ] = eWallType.Wall;

    }

    private void MoveRowsDown()
    {
        for( int i = walls.GetLength( 1 ) - 1; i > 0; --i )
        {
            walls[ 0, i ] = walls[ 0, i - 1 ];
            walls[ 1, i ] = walls[ 1, i - 1 ];
            walls[ 2, i ] = walls[ 2, i - 1 ];
        }
    }
}

public enum eWallType 
{
    Empty,
    Wall
}