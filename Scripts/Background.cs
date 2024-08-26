using Godot;
using System;

[Tool]
public partial class Background : Sprite2D
{
    public override void _Process( double delta )
    {
        ( Material as ShaderMaterial ).SetShaderParameter( "world_pos", GlobalPosition );
    }
}
