using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameScene : Node2D
{
	[Export] public PackedScene CharacterBlueprint;
	[Export] public Marker2D[] PlayerSpawnPoint;

	public static int MaxCharacterCount = 4;

	private SlimeTrailsManager SlimeTrailsManager;

	public override void _Ready()
	{
		SlimeTrailsManager = GetNode<SlimeTrailsManager>("SlimeTrailsManager");

		if (PartyManager.Instance.GetParty().Length == 0)
		{
			SpawnDefaultCharacter();
			return;
		}

		var party = PartyManager.Instance.GetParty();
		var teams = party.GroupBy(x => x.TeamNumber);

		foreach (var team in teams)
		{
			var teamId = team.Key;
			var members = team;

			SpawnCharacter(teamId, members);
		}
	}

	public void SpawnCharacter(int characterId, IEnumerable<PartyMember> members)
	{
		var characterRoot = CharacterBlueprint.Instantiate<CharacterRoot>();
		var character = characterRoot.Character;
		var spawn = PlayerSpawnPoint[characterId - 1];

		characterRoot.GlobalTransform = spawn.GlobalTransform;
		character.SetupPlayer(members.Select(x => x.PlayerInput).ToList(), characterId);
		character.CharacterPositionChanged += SlimeTrailsManager.UpdateCharacterSlimeTrail;
		character.CharacterWasEaten += OnCharacterWasEaten;
		character.Name = $"Character{characterId}";

		AddChild(characterRoot);
	}

	public void OnCharacterWasEaten(int characterId)
	{
		var characters = GetTree().GetNodesInGroup("characters").Cast<Character>();

		var eaten = characters.First(c => c.characterId == characterId);

		var remaining = characters.Where(c => c.characterId != characterId).ToArray();
		
		if (remaining.Length == 1) {
			Signals.Instance.EmitSignal(
				Signals.SignalName.LastCharacterStandingInGame,
				remaining[0].characterId
			);
		}

		eaten.GetParent().QueueFree();
	}

	public void SpawnDefaultCharacter()
	{
		SpawnCharacter(1, new List<PartyMember>());
	}
}
