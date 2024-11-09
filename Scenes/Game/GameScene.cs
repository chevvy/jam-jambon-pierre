using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameScene : Node2D
{
	[Export] public PackedScene CharacterBlueprint;
	[Export] public Marker2D[] PlayerSpawnPoint;

	private SlimeTrailsManager SlimeTrailsManager;

	public override void _Ready()
	{
		SlimeTrailsManager = GetNode<SlimeTrailsManager>("SlimeTrailsManager");

		if (PartyManager.Instance.GetParty().Count() == 0)
		{
			SpawnDefaultCharacter();
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
		var spawn = PlayerSpawnPoint[new Random().Next(0, PlayerSpawnPoint.Length)];

		character.GlobalTransform = spawn.GlobalTransform;
		character.SetupPlayer(members.Select(x => x.PlayerInput).ToList(), characterId);
		character.CharacterPositionChanged += SlimeTrailsManager.UpdateCharacterSlimeTrail;
		character.Name = $"Character{characterId}";

		AddChild(characterRoot);
	}

	public void SpawnDefaultCharacter()
	{
		SpawnCharacter(1, new List<PartyMember>());
	}
}
