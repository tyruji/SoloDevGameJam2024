using Godot;
using System;

public partial class WallDrawer : Node2D
{
    [Export]
    private WallHandler _WallHandler = null;

    [Export]
    private Texture2D _WallTexture = null;

    [Export]
    private Texture2D _EmptyTexture = null;

    [Export]
    private Texture2D _PipeTexture1 = null;

    [Export]
    private Texture2D _PipeTexture2 = null;

    [Export]
    private Texture2D _PipeTexture3 = null;

    [Export]
    private Texture2D _PipeTexture4 = null;

    public const int WALL_PIXEL_SIZE = 64;

    public readonly Sprite2D [,] walls = new Sprite2D[ WallHandler.WALL_COLUMNS,
        WallHandler.WALL_ROWS ];

    private int _highestPosition = -480;


    public override void _Ready()
    {
        _WallHandler ??= GetNode<WallHandler>( "../WallHandler" );
        _WallHandler.OnAdvanceRows += ArrangeWalls;

        SpawnWalls();
    }


    private void SpawnWalls()
    {
        for( int x = 0; x < walls.GetLength( 0 ); ++x )
        {
            for( int y = 0; y < walls.GetLength( 1 ); ++y )
            {
                Sprite2D wall = new Sprite2D();

                wall.Texture = _EmptyTexture;

                AddChild( wall );

                wall.GlobalPosition = new Vector2( WALL_PIXEL_SIZE * ( x - 1 ), 
                    y * WALL_PIXEL_SIZE + _highestPosition );

                walls[ x, y ] = wall;
            }
        }

        for( int i = 1; i < walls.GetLength( 1 ) - 1; ++i )
        {
            UpdateWallTextures( i );
        }
    }

    private void UpdateWallTextures( int height )
    {
        Sprite2D wall = walls[ 0, height ];

        bool wall_up = _WallHandler.walls[ 0, height - 1 ] == eWallType.Empty;
        bool wall_down = _WallHandler.walls[ 0, height + 1 ] == eWallType.Empty;


        if( _WallHandler.walls[ 0, height ] == eWallType.Empty )
        {
            if( wall_up && wall_down )
            {
                wall.Texture = _PipeTexture4;
                wall.FlipH = true;
            }
            else if( wall_up )
            {
                wall.Texture = _PipeTexture2;
                wall.FlipH = true;
            }
            else if( wall_down )
            {
                wall.Texture = _PipeTexture3;
                wall.FlipH = false;
            }
        }
        else
        {
            wall.Texture = _PipeTexture1;
            wall.FlipH = false;
        }

        wall = walls[ 2, height ];

        wall_up = _WallHandler.walls[ 2, height - 1 ] == eWallType.Empty;
        wall_down = _WallHandler.walls[ 2, height + 1 ] == eWallType.Empty;

        if( _WallHandler.walls[ 2, height ] == eWallType.Empty )
        {
            if( wall_up && wall_down )
            {
                wall.Texture = _PipeTexture4;
                wall.FlipH = false;
            }
            else if( wall_up )
            {
                wall.Texture = _PipeTexture2;
                wall.FlipH = false;
            }
            else if( wall_down )
            {
                wall.Texture = _PipeTexture3;
                wall.FlipH = true;
            }
        }
        else
        {
            wall.Texture = _PipeTexture1;
            wall.FlipH = true;
        }
    }

    private void ArrangeWalls()
    {
        PushArrayPositionsDown();
        _highestPosition -= WALL_PIXEL_SIZE;

        for( int i = 0; i < walls.GetLength( 0 ); ++i )
        {
            Sprite2D wall = walls[ i, 0 ];

            eWallType wall_type = _WallHandler.walls[ i, 0 ];

            wall.Position = wall.Position * Vector2.Right + _highestPosition * Vector2.Down;
            //wall.Texture = wall_type == eWallType.Wall ? _WallTexture : _EmptyTexture;
        }

        UpdateWallTextures( 1 );
    }

    private void PushArrayPositionsDown()
    {
        for( int x = 0; x < walls.GetLength( 0 ); ++x )
        {
            Sprite2D downmost_wall = walls[ x, walls.GetLength( 1 ) - 1 ];

            for( int y = walls.GetLength( 1 ) - 1; y > 0; --y )
            {
                    // Push higher walls down.
                walls[ x, y ] = walls[ x, y - 1 ];
            }

                // Loop around, so the lowest walls appear on top.
            walls[ x, 0 ] = downmost_wall;
        }
    }
}
