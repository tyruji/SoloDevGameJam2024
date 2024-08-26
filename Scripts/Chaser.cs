using Godot;
using System;

public partial class Chaser : Sprite2D
{
    [Export]
    private int _MaxDistanceToPlayer = 276; 

    [Export]
    private float _SecondMaxSpeed = 350f;

    [Export]
    private float _FirstMaxSpeed = 150f;

    [Export]
    private float _FirstSpeedTime = 60f;

    [Export]
    private float _SecondSpeedTime = 60f;

    private Node2D _playerPosition = null;

    private Player _player = null;

    private bool _killedPlayer = false;

    private float _timeElapsed = .0f;

    public override void _Ready()
    {
        _playerPosition = GetNode<Node2D>( "../Player/PlayerSprite" );
        _player = GetNode<Player>( "../Player" );
    }

    public override void _Process( double delta )
    {
        _timeElapsed += ( float ) delta;

        if( _killedPlayer || _player.Lost ) return;
            
            // Move Up
        float speed = GetSpeed();
        GlobalPosition += ( float ) delta * speed * Vector2.Up;

        float dist = _playerPosition.GlobalPosition.Y - GlobalPosition.Y;
        dist = Mathf.Abs( dist );

        if( dist > _MaxDistanceToPlayer ) return;

            // Kill Player;
        _player.Kill();
        _killedPlayer = true;
    }

    private float GetSpeed()
    {
        if( _timeElapsed < _FirstSpeedTime )
        {
            return FirstSpeedInterpolate( 0, _FirstMaxSpeed, _FirstSpeedTime );
        }
        else if( _timeElapsed < _SecondSpeedTime )
        {
            return SecondSpeedInterpolate();
        }
        return _SecondMaxSpeed;
    }

    private float FirstSpeedInterpolate( float start, float end, float time )
    {
        return start + ( end - start ) * ( Mathf.Log( _timeElapsed ) / Mathf.Log( time ) );
    }

    private float SecondSpeedInterpolate()
    {
        float a = ( _SecondMaxSpeed - _FirstMaxSpeed )
            / ( _SecondSpeedTime - _FirstSpeedTime );

        return _FirstMaxSpeed + a * ( _timeElapsed - _FirstSpeedTime );
    }
}
