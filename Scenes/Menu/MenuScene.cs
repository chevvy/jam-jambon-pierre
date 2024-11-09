using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MenuScene : CanvasLayer
{

	// I apologize for this janky code

	[Export]
	private Control playerControl1;
	[Export]
	private Control playerControl2;
	[Export]
	private Control playerControl3;
	[Export]
	private Control playerControl4;

	private List<Control> controls;

	private Dictionary<int, int> playerIdToTeam = new Dictionary<int, int>();

	public override void _Ready()
	{
		controls = new List<Control>{
			playerControl1,
			playerControl2,
			playerControl3,
			playerControl4,
		};

		controls[0].Modulate = CharacterColor.CharColor[0];
		controls[1].Modulate = CharacterColor.CharColor[1];
		controls[2].Modulate = CharacterColor.CharColor[2];
		controls[3].Modulate = CharacterColor.CharColor[3];

		foreach (Control each in controls)
		{
			each.Visible = false;
		}

		foreach (var player in PlayerInput.PlayerTagByID)
		{
			SetupPlayerTeam((int)player.Key);
		}

		Signals.Instance.PlayerJoinedParty += OnPlayerJoinedParty;
	}

	public void OnPlayerJoinedParty(int partySlot, int playerId)
	{
		GD.Print($"Player {playerId} with slot {partySlot} is being shown");
		controls[partySlot].Visible = true;
	}

	public override void _Process(double delta)
	{
		// Has any player pressed start
		foreach (var player in PlayerInput.PlayerTagByID)
		{
			// Jump means start :)
			if (Input.IsActionJustPressed($"{player.Value}{PlayerInput.InputByName[InputAction.Jump]}"))
			{
				StartGame();
				AudioManager.Instance.PlayStart();
			}
		}
	}

	public List<int> GetPlayersWithTeam(int teamId)
	{
		return playerIdToTeam.Where(each => each.Value == teamId).Select(each => each.Key).ToList();
	}

	public void SetupPlayerTeam(int playerId)
	{
		int team = playerId > GameScene.MaxCharacterCount ? 2 : playerId;
		SelectTeam(playerId, team);
	}

	public void SelectTeam(int playerId, int teamId)
	{
		playerIdToTeam.Remove(playerId);
		playerIdToTeam.Add(playerId, teamId);
	}

	private void StartGame()
	{
		for (int i = 0; i < PartyManager.Instance.GetParty().Count(); i++)
		{
			PartyMember each = PartyManager.Instance.GetParty()[i];
			int playerId = (int)each.PlayerId;
			int teamId = playerIdToTeam[playerId];
			each.TeamNumber = teamId;
		}

		GD.Print("Game start requested");
		Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Game.SceneId);
	}
}
