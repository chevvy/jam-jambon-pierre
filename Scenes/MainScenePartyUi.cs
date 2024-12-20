using Godot;
using System;

public partial class MainScenePartyUi : Label
{
	private string[] PartyEntries;

	public override void _Ready()
	{
		PartyEntries = new string[5];

		Signals.Instance.PlayerJoinedParty += OnPlayerJoinedParty;
	}

	public void OnPlayerJoinedParty(int partySlot, int playerId)
	{
		PartyEntries[partySlot] = $"Player {playerId} ({partySlot})";
		Redraw();
	}

	public void Redraw()
	{
		Text = string.Join("\n", PartyEntries);
	}
}
