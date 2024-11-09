using Godot;
using System;

public partial class ResultScreen : Node2D
{
	[Export] public Label WinText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WinText.Text = "Équipe " + GameManager.Instance.WinningTeamId;
	}

	public override void _Process(double delta)
	{
		// Has any player pressed start
		foreach (var playerId in PlayerInput.PlayerTagByID.Values)
		{
			if (Input.IsActionJustPressed($"{playerId}{PlayerInput.InputByName[InputAction.Start]}"))
			{
				// Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Game.SceneId);
				Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Credit.SceneId);
			}
		}
	}
}
