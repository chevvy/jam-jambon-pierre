using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;
	public int WinningTeamId = 0;

	public bool IsGameFinished()
	{
		return WinningTeamId != 0;
	}

	[Signal] public delegate void PlayerGainedPelletEventHandler(int characterId, int qty);

	[Signal] public delegate void GameEndEventHandler(string winnerId);

	public override void _Ready()
	{
		Instance = this;
	}

	public void OnGainedPoint(int characterId, int qty)
	{
		EmitSignal(SignalName.PlayerGainedPellet, characterId, qty);
	}

	public void OnGameEnded(int winnerId)
	{
		EmitSignal(SignalName.GameEnd, winnerId);
		WinningTeamId = winnerId;
		Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Result.SceneId);
		AudioManager.Instance.PlayEnd();
	}
}
