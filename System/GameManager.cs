using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	public int WinningTeamId;

	// playerid should be used with PlayerInput classes to convert it to enum
	// Godot doesnt allow enum for signal props
	[Signal]
	public delegate void PlayerGainedPelletEventHandler(string id, int qty);

	[Signal]
	public delegate void GameEndEventHandler(string winnerId);

	public override void _Ready()
	{
		Instance = this;
		PlayerGainedPellet += (id,qty) => GD.Print("biscuits du capitaine" + id + qty);
	}

	public void OnGainedPoint(string id, int qty)
	{
		EmitSignal(SignalName.PlayerGainedPellet, id, qty);
		GD.Print("player gained point : ");
	}

	public void OnGameEnded(int winnerId)
	{
		EmitSignal(SignalName.GameEnd, winnerId);
		WinningTeamId = winnerId;
		Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Result.SceneId);
	}
}
