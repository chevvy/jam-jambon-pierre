using Godot;
using System;

public partial class RotateDeezNutz : CharacterBody2D
{
    [Export] public double RotationSpeed = Math.PI;
    [Export] public bool invertDirection = false;
    
    public override void _Process(double delta)
    {
        var adjustedSpeedDirection = invertDirection ? -RotationSpeed : RotationSpeed;
        Rotate((float)adjustedSpeedDirection * (float)delta);
    }
}
