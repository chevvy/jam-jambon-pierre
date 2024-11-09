using Godot;
using System;

/**
Should only define signals which are used throughout multiple scenes.
If you KNOW the signal will only be used within a specific scene, then define it within that scene as you usually would.

To DEFINE a signal, add its definition in this file:
	[Signal] public delegate void GameSceneEndStateReachedEventHandler(bool wasSuccess);

TO EMIT a signal, use the following code:
	Signals.Instance.EmitSignal(Signals.SignalName.GameSceneEndStateReached, ...args);

TO CONNECT to a signal:
	Signals.Instance.GameSceneEndStateReached += OnGameSceneEndStateReached;

TO DISCONNECT from a signal:
	Signals.Instance.GameSceneEndStateReached -= OnGameSceneEndStateReached;
*/

public partial class Signals : Node
{
	[Signal] public delegate void SceneRequestedEventHandler(int sceneId);

	[Signal] public delegate void PlayerJoinedPartyEventHandler(int partySlot, int playerId);

	public static Signals Instance { get; private set; }
	public override void _EnterTree() => Instance = this;
}
