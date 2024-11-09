using Godot;
using System;
using System.Collections.Generic;

public partial class MenuScene : Node2D
{

	// I apologize for this janky code

	[Export]
	private Control playerControl;
	[Export]
	private Control playerContro2;
	[Export]
	private Control playerContro3;
	[Export]
	private Control playerContro4;
	[Export]
	private Control playerContro5;

	private List<Control> controls;

	private const int teamId1 = 1;
	private const int teamId2 = 2;

	public override void _Ready()
	{
		controls = new List<Control>{
			playerControl,
			playerContro2,
			playerContro3,
			playerContro4,
			playerContro5,
		};

		foreach (Control each in controls)
		{
			each.Visible = false;
		}

		PlayerEntered(5);
	}

	public override void _Process(double delta)
	{
		// Has any player pressed start
		foreach (var player in PlayerInput.PlayerTagByID)
		{
			// TODO Remove
			if ((int)player.Key != 5)
			{
				continue;
			}


			if (Input.IsActionPressed($"{player.Value}{PlayerInput.InputByName[InputAction.MoveLeft]}"))
			{
				SelectTeam((int)player.Key, teamId1);
			}
			if (Input.IsActionPressed($"{player.Value}{PlayerInput.InputByName[InputAction.MoveRight]}"))
			{
				SelectTeam((int)player.Key, teamId2);
			}
		}
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

		controls[idInArray].GetNode<Label>("1/Label").Visible = teamId == 1;
		controls[idInArray].GetNode<Label>("2/Label").Visible = teamId == 2;
	}
}
