using Godot;
using System;

public partial class StartScene : Node2D
{
    public override void _Process(double delta)
    {
        // Has any player pressed start
        foreach (var playerId in PlayerInput.PlayerTagByID.Values)
        {
            if (Input.IsActionJustPressed($"{playerId}{PlayerInput.InputByName[InputAction.Jump]}"))
            {
                Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Menu.SceneId);
            }
        }
    }
}
