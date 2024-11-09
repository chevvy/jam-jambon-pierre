using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MenuScene : Control
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
	[Export]
	private Control playerControl5;

	private List<Control> controls;

	private Dictionary<int, int> playerIdToTeam = new Dictionary<int, int>();

	public override void _Ready()
	{
		controls = new List<Control>{
			playerControl1,
			playerControl2,
			playerControl3,
			playerControl4,
			playerControl5,
		};

		foreach (Control each in controls)
		{
			each.Visible = true;
		}

		foreach (var player in PlayerInput.PlayerTagByID)
		{
			PlayerEntered((int)player.Key);
		}
	}

	public override void _Process(double delta)
	{
		// Has any player pressed start
		foreach (var player in PlayerInput.PlayerTagByID)
		{
			if (Input.IsActionPressed($"{player.Value}{PlayerInput.InputByName[InputAction.MoveLeft]}"))
			{
				SelectTeam((int)player.Key, (int)player.Key);
			}
			if (Input.IsActionJustPressed($"{player.Value}{PlayerInput.InputByName[InputAction.MoveRight]}"))
			{
				SelectTeam((int)player.Key, (int)player.Key);
			}

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

	public void PlayerEntered(int playerId)
	{
		int idInArray = playerId - 1;
		controls[idInArray].Visible = true;

		if (playerId <= GameScene.MaxCharacterCount)
		{
			// Default team is 1
			SelectTeam(playerId, playerId);
		}
		else
		{
			GD.Print("Keyboard defaulting to team 2");
			SelectTeam(playerId, 2);
		}
	}

	public void SelectTeam(int playerId, int teamId)
	{
		int idInArray = playerId - 1;

		playerIdToTeam.Remove(playerId);
		playerIdToTeam.Add(playerId, teamId);

		controls[idInArray].GetNode<Label>("1/Label").Visible = teamId == 1;
		controls[idInArray].GetNode<Label>("2/Label").Visible = teamId == 2;
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
