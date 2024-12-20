using Godot;
using System;

public enum PelletType
{
	RED, BLUE
}

public partial class Pellet : Node2D
{
	[Signal] public delegate void PelletCollectedByCharacterEventHandler(int characterId, int pelletId);

	[Export]
	private PelletType _pelletType = PelletType.RED;

	[Export] private DeleteAfterSFX MangeSfx;
	[Export] private Sprite2D _spriteToHide;
	[Export] private CollisionShape2D lol;

	public Area2D Area;

	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Area.BodyEntered += OnBodyEntered;
		
		// TODO we now have assets, see what we do with this
		// if (_pelletType == PelletType.RED)
		// {
		// 	Modulate = Colors.Red;
		// }
		// else if (_pelletType == PelletType.BLUE)
		// {
		// 	Modulate = Colors.Yellow;
		// }
	}

	public void OnBodyEntered(Node body)
	{
		if (body is Character character)
		{
			GD.Print("Pellet collected by a character");

			var characterId = 1;
			var pelletId = 1;

			character.HandlePelletAcquired(_pelletType);

			EmitSignal(SignalName.PelletCollectedByCharacter, characterId, pelletId);
			
			GameManager.Instance.OnGainedPoint(character.characterId, pelletId);
			MangeSfx.PlayAndDelete();
			lol.QueueFree();
			_spriteToHide.Hide();
		}
	}
}
