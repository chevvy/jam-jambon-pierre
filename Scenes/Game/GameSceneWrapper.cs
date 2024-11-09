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
	[Export]
	private SubViewportContainer extraScreenContainer;

	private const string PlayerPath = "HB/VPC/VC/GameScene/CharacterRoot/Character";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		viewport2.World2D = viewPort1.World2D;

		Character character1 = GetNode<Character>(PlayerPath + "1");

		// Sanity check
		if (character1 == null)
		{
			GD.PrintErr("Character not found");
		}
		bool character2Exists = HasNode(PlayerPath + "2");


		RemoteTransform2D remoteTransform1 = new RemoteTransform2D();
		remoteTransform1.RemotePath = camera1.GetPath();
		character1.AddChild(remoteTransform1);

		if (character2Exists)
		{
			RemoteTransform2D remoteTransform2 = new RemoteTransform2D();
			remoteTransform2.RemotePath = camera2.GetPath();
			Character character2 = GetNode<Character>(PlayerPath + "2");
			character2.AddChild(remoteTransform2);
		}

		camera2.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera2.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;

		if (!character2Exists)
		{
			HideSecondScreen();
		}
	}

	private void HideSecondScreen()
	{
		extraScreenContainer.Visible = false;
	}
}
