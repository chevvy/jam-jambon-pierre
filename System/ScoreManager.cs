using Godot;
using System;

public partial class ScoreManager : Node
{
	[Export] Label LabelTeam1;

	[Export] Label LabelTeam2;
	private int ScoreTeam1 = 0;
	private int ScoreTeam2 = 0;

	[Export] public int ScoreLimit = 10;

	public override void _Ready()
	{
		GameManager.Instance.PlayerGainedPellet += OnTeamGainPoint;
	}

	
	public void OnTeamGainPoint(string id, int qty)
	{
		var playerId = (int)PlayerInput.IDbyPlayerTag["p" + id];
		if (playerId <= 1)
		{
			ScoreTeam1 += 1;
		}
		else
		{
			ScoreTeam2 += 1;
		}

		UpdateLabels();
		OnScoreUpdate();
	}

	private void UpdateLabels()
	{
		LabelTeam1.Text = ScoreTeam1.ToString();
		LabelTeam2.Text = ScoreTeam2.ToString();
	}

	private void OnScoreUpdate()
	{
		if (ScoreTeam2 >= ScoreLimit)
		{
			ResetScores();
			GameManager.Instance.OnGameEnded(2);
		}

		if (ScoreTeam1 >= ScoreLimit)
		{
			ResetScores();
			GameManager.Instance.OnGameEnded(1);
		}
	}

	public void ResetScores()
	{
		ScoreTeam1 = ScoreTeam2 = 0;
		UpdateLabels();
	}
}
