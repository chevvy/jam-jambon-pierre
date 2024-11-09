using Godot;
using System.Collections.Generic;

public partial class SlimeTrailsManager : Node
{
	[Export]
	public Material shaderMaterial;
	[Export]
	public Gradient gradient;

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
			slimeTrail.Material = shaderMaterial;
			slimeTrail.Gradient = gradient;
			slimeTrail.Modulate = CharacterColor.CharColor[i];

			SlimeTrailsList.Add(slimeTrail);

			AddChild(slimeTrail);
		}
	}

    public void UpdateCharacterSlimeTrail(int characterId, Vector2 position)
	{
		if (characterId <= 0)
		{
			GD.PrintErr($"Invalid character ID {characterId}");
		}
		SlimeTrailsList[characterId - 1].AddPoint(position);
	}
}
