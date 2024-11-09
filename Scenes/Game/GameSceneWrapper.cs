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
	private SubViewport viewport3;
	[Export]
	private SubViewport viewport4;
	[Export]
	private Camera2D camera1;
	[Export]
	private Camera2D camera2;
	[Export]
	private Camera2D camera3;
	[Export]
	private Camera2D camera4;
	[Export]
	private SubViewportContainer viewportContainer2;
	[Export]
	private SubViewportContainer viewportContainer3;
	[Export]
	private SubViewportContainer viewportContainer4;

	public override void _Ready()
	{
		viewport2.World2D = viewPort1.World2D;
		viewport3.World2D = viewPort1.World2D;
		viewport4.World2D = viewPort1.World2D;

		var characters = GetTree().GetNodesInGroup("characters").Cast<Character>();

		if (characters.Count() == 0) throw new Exception("No characters found in the scene");

		{
			var character1 = characters.First();
			RemoteTransform2D remoteTransform1 = new RemoteTransform2D();
			remoteTransform1.RemotePath = camera1.GetPath();
			character1.AddChild(remoteTransform1);
		}

		bool character2Exists = characters.Count() >= 2;
		bool character3Exists = characters.Count() >= 3;
		bool character4Exists = characters.Count() >= 4;

		if (character2Exists)
		{
			RemoteTransform2D remoteTransform2 = new RemoteTransform2D();
			remoteTransform2.RemotePath = camera2.GetPath();
			Character character2 = characters.ElementAt(1);
			character2.AddChild(remoteTransform2);
		}
		else
		{
			viewportContainer2.Visible = false;
		}

		if (character3Exists)
		{
			RemoteTransform2D remoteTransform3 = new RemoteTransform2D();
			remoteTransform3.RemotePath = camera3.GetPath();
			Character character3 = characters.ElementAt(2);
			character3.AddChild(remoteTransform3);
		}
		else
		{
			viewportContainer2.Visible = false;
		}

		if (character4Exists)
		{
			RemoteTransform2D remoteTransform4 = new RemoteTransform2D();
			remoteTransform4.RemotePath = camera4.GetPath();
			Character character4 = characters.ElementAt(3);
			character4.AddChild(remoteTransform4);
		}
		else
		{
			viewportContainer2.Visible = false;
		}

		camera2.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera2.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;

		camera3.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera3.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;

		camera4.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera4.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;
	}
}
