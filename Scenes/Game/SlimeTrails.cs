using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SlimeTrails : Node
{
	public List<Line2D> SlimeTrailsList;

	public override void _Ready()
	{
		SlimeTrailsList = new List<Line2D>(PartyManager.Instance.MaxPartySize);

		for (int i = 0; i < PartyManager.Instance.Party.Count(); i++)
		{
			var line2d = new Line2D();
			line2d.Width = 75;
			line2d.DefaultColor = PlayerColors.PlayerColor[i];
			SlimeTrailsList.Add(line2d);
			AddChild(line2d);
		}
	}

    public void UpdateCharacterSlimeTrail(int playerId, Vector2 positon)
	{
		var line2d = SlimeTrailsList[playerId];

		line2d.AddPoint(positon);
	}
}
