using Godot;
using System;

public partial class DeleteAfterSFX : AudioStreamPlayer
{
    [Export] private Timer deleteTimer;
    [Export] public Node ToDelete;

    public override void _Ready()
    {
        deleteTimer.Timeout += () => ToDelete.QueueFree();
    }

    public void PlayAndDelete()
    {
        Play();
        deleteTimer.Start();
    }
}
