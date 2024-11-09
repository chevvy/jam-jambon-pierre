using Godot;
using System.Collections.Generic;

public partial class SlimeTrailsManager : Node
{
	public List<Line2D> SlimeTrailsList { get; private set; }

	public override void _Ready()
	{
		SlimeTrailsList = new List<Line2D>();
	}

    public void UpdateCharacterSlimeTrail(int characterId, Vector2 position)
	{
		if (characterId >= SlimeTrailsList.Count)
			SlimeTrailsList.Add(new Line2D());

		SlimeTrailsList[characterId].AddPoint(position);
	}
}
