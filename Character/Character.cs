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
    [Signal] public delegate void CharacterPositionChangedEventHandler(int playerId, Vector2 position);

    private Timer EmitPositionTimer;

    [Export] public AnimationPlayer animator;

    [Export]
    private float MoveSpeed = 60.0f;
    [Export]
    private float Desceleration = 45f;
    [Export]
    private float ChargeSpeed = 30f;
    [Export]
    private float MaxCharge = 45f;
    [Export]
    private float ChargeInertiaRatio = 2f;

    private List<PlayerChargeState> _players = new List<PlayerChargeState>();

    public override void _Ready()
    {
        EmitPositionTimer = GetNode<Timer>("EmitPositionTimer");
        EmitPositionTimer.WaitTime = 0.1f;
        EmitPositionTimer.Timeout += () => EmitSignal(SignalName.CharacterPositionChanged, 0, GlobalPosition);
        EmitPositionTimer.Start();
    }

    public void SetupPlayer(PlayerID id)
    {
        _players = new List<PlayerChargeState> {
            new(new PlayerInput(id))
        };
    }

    public void SetupPlayer(List<PlayerID> ids)
    {
        _players = new List<PlayerChargeState>();
        foreach (PlayerID id in ids)
        {
            _players.Add(new(new PlayerInput(id)));
        }
    }

    public void SetupPlayer(List<PlayerInput> inputs)
    {
        _players = new List<PlayerChargeState>();
        foreach (PlayerInput input in inputs)
        {
            _players.Add(new(input));
        }
    }

    public override void _Process(double delta)
    {
        if (_players.Count == 0)
        {
            SetupPlayer(new List<PlayerID> { PlayerID.P5 });
            GD.PrintErr("No player input, default to computer");
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

                Vector2 velocity = Velocity;
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Desceleration);
                velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Desceleration);
                Velocity = velocity;
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
    }

    private void DisplayCharging(PlayerChargeState player)
    {
        // TODO Send message to the "ball" for the correct player
        animator.Play("grow");
    }

    private void StopCharging(PlayerChargeState player)
    {
        // TODO Send message to the "ball" for the correct player
        animator.Play("RESET");
    }

    private void ReleaseShot(PlayerChargeState player)
    {
        float adjustedChargeRatio = Math.Clamp(ChargeInertiaRatio * player.ChargeStrength / MaxCharge, 0, 1);
        Velocity = player.CurrentDirection * MoveSpeed * player.ChargeStrength * adjustedChargeRatio;
        player.ChargeStrength = 1;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
    
    // TODO use the correct playerID
    public PlayerID GetPlayerId() => PlayerID.P1;
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