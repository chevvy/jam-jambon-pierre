using Godot;
using System;
using System.Collections.Generic;

public partial class MainScene : Node
{
	private Dictionary<int, PackedScene> SceneStore;
	private Node SceneAnchorNode;

	public override void _Ready()
	{
		SceneStore = new Dictionary<int, PackedScene> {
			{ Scenes.Menu.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Menu.ScenePath) ?? throw new Exception($"{Scenes.Menu.ScenePath} not found") },
			{ Scenes.Game.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Game.ScenePath) ?? throw new Exception($"{Scenes.Game.ScenePath} not found") },
			{ Scenes.Credit.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Credit.ScenePath) ?? throw new Exception($"{Scenes.Credit.ScenePath} not found") }
		};

		SceneAnchorNode = GetNode<Node>("SceneAnchor") ?? throw new Exception("SceneAnchor (Node) not found");

		Signals.Instance.SceneRequested += OnSceneRequested;
		Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Menu.SceneId);
	}

	public override void _Input(InputEvent @event)
	{
		if 		(Input.IsActionJustPressed("load_scene_0")) Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Menu.SceneId);
		else if (Input.IsActionJustPressed("load_scene_1")) Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Game.SceneId);
		else if (Input.IsActionJustPressed("load_scene_2")) Signals.Instance.EmitSignal(Signals.SignalName.SceneRequested, Scenes.Credit.SceneId);
	}

	public void LoadScene(int sceneId)
	{
		if (SceneAnchorNode.GetChildCount() > 0)
			SceneAnchorNode.RemoveChild(SceneAnchorNode.GetChild(0));
		
		SceneAnchorNode.AddChild(SceneStore[sceneId].Instantiate());
	}
	
	public void OnSceneRequested(int sceneId)
	{
		LoadScene(sceneId);
	}
}
