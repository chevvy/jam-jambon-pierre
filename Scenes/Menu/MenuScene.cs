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

	private const int teamId1 = 1;
	private const int teamId2 = 2;

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
				SelectTeam((int)player.Key, teamId1);
			}
			if (Input.IsActionPressed($"{player.Value}{PlayerInput.InputByName[InputAction.MoveRight]}"))
			{
				SelectTeam((int)player.Key, teamId2);
			}

			// Jump means start :)
			if (Input.IsActionPressed($"{player.Value}{PlayerInput.InputByName[InputAction.Jump]}"))
			{
				StartGame();
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

		// Default team is 1
		SelectTeam(playerId, 1);
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
		GD.Print("Game start requested");
		Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Game.SceneId);
	}
}
