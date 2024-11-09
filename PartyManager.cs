using Godot;
using System;
using System.Linq;

public struct PartyMember
{
	public PlayerInput PlayerInput;
	public PlayerID PlayerId;
	public int PartySlot;
	public int TeamNumber;

	public static int UndecidedTeamNumber = -1;
}

public partial class PartyManager: Node
{
	public int MaxPartySize = 5;
	public int MaxPlayerIdCount = 5;
	private PartyMember?[] Party;

    public override void _EnterTree()
    {
		Party = new PartyMember?[MaxPartySize];
    }

    public override void _Input(InputEvent @event)
	{
		if (Party.Length >= MaxPartySize) return;

		for (int pid = 1; pid <= MaxPlayerIdCount; pid++)
		{
			if (@event.IsActionReleased($"p{pid}_start")) 
			{
				AddPlayerToParty((PlayerID)pid);
			}
		}
	}

	public bool IsPartyReady() 
	{
		return Array.TrueForAll(Party, p => p != null && p?.TeamNumber != PartyMember.UndecidedTeamNumber);
	}

	public int GetPartyCount() => Array.FindAll(Party, p => p != null).Length;

	public PartyMember[] GetParty() => Array.FindAll(Party, p => p != null).Cast<PartyMember>().ToArray();

	public void AddPlayerToParty(PlayerID playerId)
	{
		if (Array.Exists(Party, p => p != null && p?.PlayerId == playerId)) return;

		var partySlotIndex = Array.FindIndex(Party, p => p == null);

		if (partySlotIndex < 0) return;

		Party[partySlotIndex] = new PartyMember {
			PlayerInput = new PlayerInput(playerId),
			PlayerId = playerId,
			PartySlot = partySlotIndex,
			TeamNumber = PartyMember.UndecidedTeamNumber
		};

		Signals.Instance.EmitSignal(Signals.SignalName.PlayerJoinedParty, partySlotIndex, (int)playerId);

		GD.Print($"Player {playerId} joined at party slot {partySlotIndex}");
	}

	public static PartyManager Instance {get; private set;}
    public override void _Ready() => Instance = this;
}
