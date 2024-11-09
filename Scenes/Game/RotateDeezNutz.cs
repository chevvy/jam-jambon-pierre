using Godot;
using System;

public partial class RotateDeezNutz : CharacterBody2D
{
    [Export] public double RotationSpeed = Math.PI;
    public override void _Process(double delta)
    {
        Rotate((float)RotationSpeed * (float)delta);
    }
}
