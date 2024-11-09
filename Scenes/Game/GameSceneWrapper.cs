using Godot;
using System;

public partial class GameSceneWrapper : Node2D
{
	[Export]
	private SubViewport viewPort1;
	[Export]
	private SubViewport viewport2;
	[Export]
	private Camera2D camera1;
	[Export]
	private Camera2D camera2;

	private const string PlayerPath = "HB/VPC/VC/GameScene/CharacterRoot/Character";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		viewport2.World2D = viewPort1.World2D;

		Character character = GetNode<Character>(PlayerPath);
		if (character == null)
		{
			GD.PrintErr("Character not found");
		}
		RemoteTransform2D remoteTransform1 = new RemoteTransform2D();
		RemoteTransform2D remoteTransform2 = new RemoteTransform2D();
		remoteTransform1.RemotePath = camera1.GetPath();
		remoteTransform2.RemotePath = camera2.GetPath();
		character.AddChild(remoteTransform2);
		character.AddChild(remoteTransform1);

		camera2.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera2.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;
	}
}
