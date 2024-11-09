using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameScene : Node2D
{
	[Export] public PackedScene CharacterBlueprint;
	[Export] public Marker2D[] PlayerSpawnPoint;

	private SlimeTrailsManager SlimeTrails;

	public override void _Ready()
	{
		var party = PartyManager.Instance.Party;
		var teams = party.Select((value, index) => new { value, index }).GroupBy(x => x.index / 2, x => x.value);

		foreach (var team in teams)
		{
			var teamId = team.Key;
			var members = team.Where(x => x != null).Cast<PartyMember>();

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
		character.CharacterPositionChanged += SlimeTrails.UpdateCharacterSlimeTrail;
		character.Name = $"Character{characterId}";

		AddChild(characterRoot);
	}
}
