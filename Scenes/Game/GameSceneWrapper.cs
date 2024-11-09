using Godot;
using System;
using System.Collections.Generic;
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
	[Export]
	private Control firstSplit;
	[Export]
	private Control secondSplit;
	[Export]
	private Control thirdSplit;

	public override void _Ready()
	{
		viewport2.World2D = viewPort1.World2D;
		viewport3.World2D = viewPort1.World2D;
		viewport4.World2D = viewPort1.World2D;

		var characters = GetTree().GetNodesInGroup("characters").Cast<Character>();

		if (characters.Count() == 0) throw new Exception("No characters found in the scene");

		// Bad sort - don't hurt me
		{
			var character1Query = characters.Where(e => e.Name == "Character1");
			var character2Query = characters.Where(e => e.Name == "Character2");
			var character3Query = characters.Where(e => e.Name == "Character3");
			var character4Query = characters.Where(e => e.Name == "Character4");
			characters = character1Query
							.Concat(character2Query)
							.Concat(character3Query)
							.Concat(character4Query);
		}

		GD.Print($"We have {characters.Count()} players");

		if (characters.Count() >= 1)
		{
			GD.Print($"First player is {characters.First()}");

			var character1 = characters.ElementAt(0);
			RemoteTransform2D remoteTransform1 = new RemoteTransform2D();
			remoteTransform1.RemotePath = camera1.GetPath();
			character1.AddChild(remoteTransform1);
		}

		if (characters.Count() >= 2)
		{
			RemoteTransform2D remoteTransform2 = new RemoteTransform2D();
			remoteTransform2.RemotePath = camera2.GetPath();
			Character character2 = characters.ElementAt(1);
			character2.AddChild(remoteTransform2);
			firstSplit.Visible = true;
		}
		else
		{
			viewportContainer2.Visible = false;
		}

		if (characters.Count() >= 3)
		{
			RemoteTransform2D remoteTransform3 = new RemoteTransform2D();
			remoteTransform3.RemotePath = camera3.GetPath();
			Character character3 = characters.ElementAt(2);
			character3.AddChild(remoteTransform3);
			secondSplit.Visible = true;
		}
		else
		{
			viewportContainer3.Visible = false;
		}

		if (characters.Count() >= 4)
		{
			RemoteTransform2D remoteTransform4 = new RemoteTransform2D();
			remoteTransform4.RemotePath = camera4.GetPath();
			Character character4 = characters.ElementAt(3);
			character4.AddChild(remoteTransform4);
			thirdSplit.Visible = true;
		}
		else
		{
			viewportContainer4.Visible = false;
		}

		camera2.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera2.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;

		camera3.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera3.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;

		camera4.PositionSmoothingEnabled = camera1.PositionSmoothingEnabled;
		camera4.PositionSmoothingSpeed = camera1.PositionSmoothingSpeed;
	}
}
