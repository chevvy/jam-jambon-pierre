using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameScene : Node2D
{
	[Export] public PackedScene Character;
	[Export] public Marker2D[] PlayerSpawnPoint;

	public override void _Ready()
	{
		SpawnCharacters();
	}

	public void SpawnCharacters()
	{
		if (Character.Instantiate() is CharacterRoot characterRootTeam1)
		{
			var character = characterRootTeam1.Character;
			// setup creature 1
			characterRootTeam1.GlobalTransform = PlayerSpawnPoint[0].GlobalTransform;
			var creature1List = new ArraySegment<PlayerInput>(PartyManager.Instance.Party, 0, 2).Where(x => x != null);
			character.SetupPlayer(creature1List.ToList());
			AddChild(characterRootTeam1);
		}

		// if we dont have a second player
		if (PartyManager.Instance.Party[2] == null)
		{
			return;
		}
		if (Character.Instantiate() is CharacterRoot characterRootTeam2)
		{
			var character = characterRootTeam2.Character;
			// setup creature 2
			characterRootTeam2.GlobalTransform = PlayerSpawnPoint[1].GlobalTransform;
			var creature2List = new ArraySegment<PlayerInput>(PartyManager.Instance.Party, 2, 2).Where(x => x != null);
			character.SetupPlayer(creature2List.ToList());
			AddChild(characterRootTeam2);
		}
	}
}
