using Godot;
using System;
using System.Collections.Generic;

public partial class MainScene : Node
{
	private Dictionary<int, PackedScene> SceneStore;
	[Export]
	private Node SceneAnchorNode;
	[Export]
	private Camera2D camera;
	[Export]
	private ScoreManager scoreManager;

	public override void _Ready()
	{
		SceneStore = new Dictionary<int, PackedScene> {
			{ Scenes.Menu.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Menu.ScenePath) ?? throw new Exception($"{Scenes.Menu.ScenePath} not found") },
			{ Scenes.Game.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Game.ScenePath) ?? throw new Exception($"{Scenes.Game.ScenePath} not found") },
			{ Scenes.Credit.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Credit.ScenePath) ?? throw new Exception($"{Scenes.Credit.ScenePath} not found") },
			{ Scenes.Result.SceneId, ResourceLoader.Load<PackedScene>(Scenes.Result.ScenePath) ?? throw new Exception($"{Scenes.Result.ScenePath} not found") }
		};

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
		scoreManager.ResetScores();

		if (SceneAnchorNode.GetChildCount() > 0)
			SceneAnchorNode.RemoveChild(SceneAnchorNode.GetChild(0));
		
		SceneAnchorNode.AddChild(SceneStore[sceneId].Instantiate());

		// On the 'game' we potentially have split-screen. Hide regular camera
		camera.Enabled = sceneId != Scenes.Game.SceneId;
	}
	
	public void OnSceneRequested(int sceneId)
	{
		CallDeferred("LoadScene", sceneId);
	}
}
