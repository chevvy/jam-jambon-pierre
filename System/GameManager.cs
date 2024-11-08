using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance;

	// playerid should be used with PlayerInput classes to convert it to enum
	// Godot doesnt allow enum for signal props
	[Signal] public delegate void OnPlayerGainedPointEventHandler(string playerId);
	public override void _EnterTree()
	{
		GD.Print("GameManager entering tree");
		base._EnterTree();
		if (Instance != null)
			QueueFree();
		else
			Instance = this;
	}

	public override void _Ready()
	{
		OnPlayerGainedPoint += OnGainedPoint;
	}

	private void OnGainedPoint(string id)
	{
		GD.Print("player gained point : " + id);
	}

	public void PlayerGainsPoint(PlayerID id)
	{	
		EmitSignal(SignalName.OnPlayerGainedPoint, PlayerInput.PlayerTagByID[id]);
	}
}
