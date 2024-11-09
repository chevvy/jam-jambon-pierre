using Godot;
using System;

public partial class ScoreManager : Node
{
	[Export] Label LabelTeam1;

	[Export] Label LabelTeam2;
    public int ScoreTeam1 = 0;
    public int ScoreTeam2 = 0;
    public int ScoreTeam3 = 0;
    public int ScoreTeam4 = 0;

    [Export] public int ScoreLimit = 10;

    public override void _Ready()
    {
        GameManager.Instance.PlayerGainedPellet += OnTeamGainPoint;

        Signals.Instance.LastCharacterStandingInGame += OnLastCharacterStanding;
    }

    
    public void OnTeamGainPoint(int teamId, int qty)
    {
        if (teamId <= 1)
        {
            ScoreTeam1 += qty;
        }
        else
        {
            ScoreTeam2 += qty;
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

    private void OnLastCharacterStanding(int teamId)
    {
        ResetScores();
        GameManager.Instance.OnGameEnded(teamId);
    }

	public void ResetScores()
	{
		ScoreTeam1 = ScoreTeam2 = 0;
		UpdateLabels();
	}
}

