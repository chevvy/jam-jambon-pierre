using Godot;
using System;
using System.Linq;

public class PartyMember
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
	public PartyMember?[] Party;

    public override void _EnterTree()
    {
		Party = new PartyMember?[MaxPartySize];
    }

    public override void _Input(InputEvent @event)
	{
		for (int pid = 1; pid <= MaxPlayerIdCount; pid++)
		{
			if (@event.IsActionReleased($"p{pid}_start")) 
			{
				GD.Print($"Starting for player {pid}");
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

		int partySlotId = (int)playerId - 1;
		Party[partySlotId] = new PartyMember
		{
			PlayerInput = new PlayerInput(playerId),
			PlayerId = playerId,
			TeamNumber = (int)playerId == 5 ? 2 : (int)playerId
		};

		GD.Print($"Player {playerId} joined with team {Party[partySlotId].TeamNumber}");
		Signals.Instance.EmitSignal(Signals.SignalName.PlayerJoinedParty, partySlotId, (int)playerId);
	}

	public static PartyManager Instance {get; private set;}
    public override void _Ready() => Instance = this;
}
