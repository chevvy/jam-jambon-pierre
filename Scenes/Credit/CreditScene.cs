using Godot;
using System;

public partial class CreditScene : Node2D
{
    public override void _Process(double delta)
    {
        // Has any player pressed start
        foreach (var playerId in PlayerInput.PlayerTagByID.Values)
        {
            if (Input.IsActionJustPressed($"{playerId}{PlayerInput.InputByName[InputAction.Start]}"))
            {
                Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Game.SceneId);
            }
        }
    }
}
