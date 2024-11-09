using Godot;
using System;

public partial class ResultScreen : Node2D
{
	[Export] public Label WinText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		WinText.Text = "Équipe " + GameManager.Instance.WinningTeamId;
	}
}
