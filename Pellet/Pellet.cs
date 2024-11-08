using Godot;
using System;

public partial class Pellet : Node2D
{
	[Signal] public delegate void PelletCollectedByCharacterEventHandler(int characterId, int pelletId);

	public Area2D Area;

	public override void _Ready()
	{
		Area = GetNode<Area2D>("Area2D");
		Area.BodyEntered += OnBodyEntered;
	}

	public void OnBodyEntered(Node body)
	{
		if (body is Character character)
		{
			GD.Print("Pellet collected by a character");

			var characterId = 1;
			var pelletId = 1;
			EmitSignal(SignalName.PelletCollectedByCharacter, characterId, pelletId);

			QueueFree();
		}
	}
}
