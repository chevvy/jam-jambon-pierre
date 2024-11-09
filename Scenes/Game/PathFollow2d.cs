using Godot;
using System;

public partial class PathFollow2d : PathFollow2D
{
    public float Speed = 60;  // Speed of the obstacle

    public override void _Process(double delta)
    {
        // Move along the path by adjusting the offset based on speed and delta time
        Progress += Speed * (float)delta;
    }
}