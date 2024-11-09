using Godot;
using System;
using System.Linq;

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

	public override void _Ready()
	{
		viewport2.World2D = viewPort1.World2D;

		var characters = GetTree().GetNodesInGroup("characters").Cast<Character>();

		if (characters.Count() == 0) throw new Exception("No characters found in the scene");

		var character1 = characters.First();

		RemoteTransform2D remoteTransform1 = new RemoteTransform2D();
		remoteTransform1.RemotePath = camera1.GetPath();
		character1.AddChild(remoteTransform1);

		bool character2Exists = characters.Count() > 1;
		if (character2Exists)
		{
			RemoteTransform2D remoteTransform2 = new RemoteTransform2D();
			remoteTransform2.RemotePath = camera2.GetPath();
			Character character2 = characters.ElementAt(1);
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
