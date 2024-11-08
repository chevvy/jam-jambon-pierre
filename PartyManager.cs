using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PartyManager: Node
{
	public int MaxPartySize = 4;
	public int MaxPlayerIdCount = 5;
	public PlayerInput[] Party;

    public override void _EnterTree()
    {
		Party = new PlayerInput[MaxPartySize];
    }

    public override void _Input(InputEvent @event)
	{
		for (int pid = 1; pid <= MaxPlayerIdCount; pid++)
		{
			if (@event.IsActionReleased($"p{pid}_start")) 
			{
				AddPlayerToParty(pid);
			}
		}
	}

	public void AddPlayerToParty(int playerId)
	{
		if (Array.Exists(Party, p => p != null && p.Id == (PlayerID)playerId)) return;

		var emptyPartySlotIndex = Array.FindIndex(Party, p => p == null);

		if (emptyPartySlotIndex < 0) return;

		Party[emptyPartySlotIndex] = new PlayerInput((PlayerID)playerId);

		Signals.Instance.EmitSignal(Signals.SignalName.PlayerJoinedParty, emptyPartySlotIndex, playerId);

		GD.Print($"Player {playerId} joined at party slot {emptyPartySlotIndex}");
	}

	public static PartyManager Instance {get; private set;}
    public override void _Ready() => Instance = this;
}
