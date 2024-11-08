using Godot;
using System;

public partial class CharacterRoot : Node2D
{
	[Export]
	public Character Character { get; set; }

	public override void _Ready()
	{
		if (Character == null)
		{
			GD.PrintErr("Character is null");
		}
	}
}
