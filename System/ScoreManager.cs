using Godot;
using System;

public partial class ScoreManager : Node
{
	[Export] Label LabelTeam1;

	[Export] Label LabelTeam2;
	private int ScoreTeam1 = 0;
	private int ScoreTeam2 = 0;
	public override void _Ready()
	{
		GameManager.Instance.PlayerGainedPellet += OnTeamGainPoint;
	}
	// public void OnTeamGainPoint(string id, int qty)
	public void OnTeamGainPoint(string id, int qty)
	{
		
		var playerId = (int)PlayerInput.IDbyPlayerTag["p"+id];
		 if (playerId <= 1)
		 {
			 ScoreTeam1 += 1;
			 LabelTeam1.Text = ScoreTeam1.ToString();
		 }
		 else
		 {
			 ScoreTeam2 += 1;
			 LabelTeam1.Text = ScoreTeam2.ToString();
		 }
	}

	public override void _ExitTree()
	{
		GD.Print("Quitting score manager");
	}
}
