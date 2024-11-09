using Godot;
using System.Collections.Generic;

public partial class SlimeTrailsManager : Node
{
	public List<Line2D> SlimeTrailsList { get; private set; }

	public override void _Ready()
	{
		SlimeTrailsList = new List<Line2D>(GameScene.MaxCharacterCount);

		for (int i = 0; i < GameScene.MaxCharacterCount; i++)
		{
            var slimeTrail = new Line2D
            {
                DefaultColor = CharacterColor.CharColor[i],
                Width = 75f,
                Name = $"SlimeTrail{i}"
            };

            SlimeTrailsList.Add(slimeTrail);

			AddChild(slimeTrail);
		}
	}

    public void UpdateCharacterSlimeTrail(int characterId, Vector2 position)
	{
		SlimeTrailsList[characterId].AddPoint(position);
	}
}
