using System;
using System.Collections.Generic;
using Godot;

public enum State
{
	Moving,
	Charging
}

public partial class Character : CharacterBody2D
{
    [Signal] public delegate void CharacterPositionChangedEventHandler(int characterId, Vector2 position);
	[Signal] public delegate void CharacterWasEatenEventHandler(int characterId);

	private Timer EmitPositionTimer;

	[Export] public AnimationPlayer animator;

	[Export]
	private float MoveSpeed = 40.0f;
	[Export]
	private float Desceleration = 60f;
	[Export]
	private float ChargeSpeed = 25f;
	[Export]
	private float MaxCharge = 25f;
	[Export]
	private float ChargeInertiaRatio = 2f;
	[Export]
	private float SpeedGainPerPill = 2.5f;
	[Export]
	private float ChargeGainPerPill = 10f;

	[Export] ChargeBall chargeball;

	private List<PlayerChargeState> _players = new List<PlayerChargeState>();

	[Export] public int characterId = -1;
	[Export] public AudioStreamPlayer BuildUpSfx;
	[Export] public AudioStreamPlayer ReleaseBuildUpSfx;

	[Export] public float BounceDampening = 0.30f;

	[Export] public float MinimumBounceSpeed = 100.0f;

	[Export] public int PelletsEaten = 0;
	[Export] public int PelletsToBigBoy = 2;


	public override void _Ready()
	{
		EmitPositionTimer = GetNode<Timer>("EmitPositionTimer");
		EmitPositionTimer.WaitTime = 0.1f;
		EmitPositionTimer.Timeout += () => EmitSignal(SignalName.CharacterPositionChanged, characterId, GlobalPosition);
		EmitPositionTimer.Start();
	}

	public void SetupPlayer(PlayerID id, int characterId)
	{
		_players = new List<PlayerChargeState> {
			new(new PlayerInput(id))
		};

		this.characterId = characterId;
		
		GetParent<CharacterRoot>().Modulate = CharacterColor.CharColor[characterId - 1];
	}

	public void SetupPlayer(List<PlayerID> ids, int characterId)
	{
		_players = new List<PlayerChargeState>();
		foreach (PlayerID id in ids)
		{
			_players.Add(new(new PlayerInput(id)));
		}
		
		this.characterId = characterId;
		
		GetParent<CharacterRoot>().Modulate = CharacterColor.CharColor[characterId - 1];
	}

	public void SetupPlayer(List<PlayerInput> inputs, int characterId)
	{
		_players = new List<PlayerChargeState>();
		foreach (PlayerInput input in inputs)
		{
			_players.Add(new(input));
		}
		
		this.characterId = characterId;
		
		GetParent<CharacterRoot>().Modulate = CharacterColor.CharColor[characterId - 1];
	}

	public override void _Process(double delta)
	{
		if (_players.Count == 0)
		{
			SetupPlayer(new List<PlayerID> { PlayerID.P5 }, 0);
			return;
		}

		foreach (PlayerChargeState player in _players)
		{
			// Charging your shot
			if (HasActiveInput(player))
			{
				SelectActiveInput(player);
				if (player.SlingState == State.Moving)
				{
					player.SlingState = State.Charging;
					DisplayCharging(player);
				}
				else if (player.SlingState == State.Charging)
				{

					GainChargeStrength(player, (float)delta);
				}
			}

			// Shot has been released
			else
			{
				if (player.SlingState == State.Charging)
				{
					player.SlingState = State.Moving;
					StopCharging(player);
					ReleaseShot(player);
				}

				
			}
		}
	
	}

	private void SelectActiveInput(PlayerChargeState player)
	{
		Vector2 inputVector = GetActiveInputVector(player);
		if (inputVector != Vector2.Zero)
		{
			player.CurrentDirection = inputVector;
		}
	}

	private Vector2 GetActiveInputVector(PlayerChargeState player)
	{

		return Input.GetVector(
			player.Input.GetInputKey(InputAction.MoveRight),
			player.Input.GetInputKey(InputAction.MoveLeft),
			player.Input.GetInputKey(InputAction.MoveDown),
			player.Input.GetInputKey(InputAction.MoveUp)
		).Normalized();
	}

	private bool HasActiveInput(PlayerChargeState player)
	{
		return GetActiveInputVector(player) != Vector2.Zero;
	}

	private void GainChargeStrength(PlayerChargeState player, float delta)
	{
		player.ChargeStrength += delta * ChargeSpeed;
		player.ChargeStrength = Math.Clamp(player.ChargeStrength, 1, MaxCharge);
		chargeball.SetChargeDirection(player.CurrentDirection, Math.Clamp(player.ChargeStrength/MaxCharge,0,1),(int)player.Input.Id);
	}

	private void DisplayCharging(PlayerChargeState player)
	{
		animator.Play("grow");
		if (!BuildUpSfx.Playing)
		{
			BuildUpSfx.Play();
		}
	}

	private void StopCharging(PlayerChargeState player)
	{
		
		chargeball.ResetPosition((int)player.Input.Id);
		animator.Play("RESET");
		BuildUpSfx.Stop();
	}

	private void ReleaseShot(PlayerChargeState player)
	{
		float adjustedChargeRatio = Math.Clamp(ChargeInertiaRatio * player.ChargeStrength / MaxCharge, 0, 1);
		Velocity = player.CurrentDirection * MoveSpeed * player.ChargeStrength * adjustedChargeRatio;
		player.ChargeStrength = 1;
		ReleaseBuildUpSfx.Play();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;
		velocity.X = Mathf.MoveToward(Velocity.X, 0, Desceleration);
		velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Desceleration);
		Velocity = velocity;
		
		HandleCollisions(MoveAndCollide(Velocity * (float)delta));
	}

	public void HandleCollisions(KinematicCollision2D collision)
	{
		if (collision == null) return;

		var other = collision.GetCollider();

		if 		(other is Character) HandleCollision((Character)other, collision);
		else if (other is RotateDeezNutz) HandleCollision((RotateDeezNutz)other, collision);
		else if (other is StaticBody2D) HandleCollision((StaticBody2D)other, collision);
	}

	public void HandleCollision(Character other, KinematicCollision2D collision)
	{
		if (IsBigBoy() && !other.IsBigBoy())
		{
			other.GetEatenBy(this);
		}
		else
		{
			Velocity = Velocity.Bounce(collision.GetNormal()) * BounceDampening;
		}
	}	

	public bool IsBigBoy() {
		return PelletsEaten >= PelletsToBigBoy;
	}

	public void GetEatenBy(Character other)
	{
		GD.Print($"Emitting character was eaten signal: {characterId}");

		EmitSignal(SignalName.CharacterWasEaten, characterId);
	}

	public void HandleCollision(RotateDeezNutz deezNutz, KinematicCollision2D collision)
	{
		Velocity = Velocity.Bounce(collision.GetNormal()) * BounceDampening;
	}

	public void HandleCollision(StaticBody2D staticBody, KinematicCollision2D collision)
	{
		Velocity = Velocity.Bounce(collision.GetNormal()) * BounceDampening;

		if (Velocity.Length() < MinimumBounceSpeed)
		{
			Velocity = Velocity.Normalized() * MinimumBounceSpeed;
		}
	}

	public void HandlePelletAcquired(PelletType color)
	{
		if (color == PelletType.RED)
		{
			ChargeSpeed += ChargeGainPerPill;
		}
		else if (color == PelletType.BLUE)
		{
			MoveSpeed += SpeedGainPerPill;
		}

		PelletsEaten++;		
	}
}

public class PlayerChargeState
{
	// Direction of the currently selected action
	public Vector2 CurrentDirection { get; set; } = Vector2.Zero;

	// Multipler on the charged sling
	public float ChargeStrength { get; set; } = 1f;

	public State SlingState { get; set; } = State.Moving;

	public readonly PlayerInput Input;

	public PlayerChargeState(PlayerInput _input)
	{
		Input = _input;
	}
}
