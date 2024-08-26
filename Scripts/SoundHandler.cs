using Godot;
using System;

public partial class SoundHandler : Node
{
    [Export]
    private Player _Player { get; set; }  = null;

    [Export]
    private AudioStreamPlayer _Bounce { get; set; }  = null;

    [Export]
    private AudioStreamPlayer _Hit { get; set; } = null;

    public override void _Ready()
    {
        _Player ??= GetNode<Player>( ".." );
        _Player.OnMoveLeft += PlayBounce;
        _Player.OnMoveRight += PlayBounce;
        _Player.OnHitWall += PlayHit;
    }

    private void PlayBounce()
    {
        _Bounce?.Play();
    }

    private void PlayHit()
    {
        _Hit?.Play();
    }
}
