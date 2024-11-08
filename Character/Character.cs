using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using Godot;


public partial class Character : CharacterBody2D
{
    [Export]
    public AnimationPlayer animator;

    [Export]
    private float MoveSpeed = 60.0f;
    [Export]
    private float Desceleration = 45f;
    [Export]
    private float ChargeSpeed = 10f;
    [Export]
    private float MaxCharge = 15f;

    public enum State
    {
        Charging,
        Moving
    }

    private State slingState = State.Moving;

    private List<PlayerChargeState> _players = new List<PlayerChargeState>();

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
            SetupPlayer(PlayerID.P5);
            GD.PrintErr("No player input, default to computer");
            return;
        }

        // Charging your shot
        if (HasActiveInput())
        {
            SelectActiveInput();
            if (slingState == State.Moving)
            {
                slingState = State.Charging;
                DisplayCharging();
            }
            else if (slingState == State.Charging)
            {
                GainChargeStrength((float)delta);
            }
        }

        // Shot has been released
        else
        {
            if (slingState == State.Charging)
            {
                slingState = State.Moving;
                StopCharging();
                ReleaseShot();
            }

            Vector2 velocity = Velocity;
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Desceleration);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Desceleration);
            Velocity = velocity;
        }
    }

    private void SelectActiveInput()
    {
        Vector2 inputVector = GetActiveInputVector();
        if (inputVector != Vector2.Zero)
        {
            _players[0].CurrentDirection = inputVector;
        }
    }

    private Vector2 GetActiveInputVector()
    {
        return Input.GetVector(
            _players[0].Input.GetInputKey(InputAction.MoveLeft),
            _players[0].Input.GetInputKey(InputAction.MoveRight),
            _players[0].Input.GetInputKey(InputAction.MoveUp),
            _players[0].Input.GetInputKey(InputAction.MoveDown)
        ).Normalized();
    }

    private bool HasActiveInput()
    {
        return GetActiveInputVector() != Vector2.Zero;
    }

    private void GainChargeStrength(float delta)
    {
        _players[0].ChargeStrength += Math.Clamp(delta * ChargeSpeed, 1f, MaxCharge);
    }

    private void DisplayCharging()
    {
        animator.Play("grow");
    }

    private void StopCharging()
    {
        animator.Play("RESET");
    }

    private void ReleaseShot()
    {
        Velocity = _players[0].CurrentDirection * MoveSpeed * _players[0].ChargeStrength;
        _players[0].ChargeStrength = 1;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}

public class PlayerChargeState
{
    // Direction of the currently selected action
    public Vector2 CurrentDirection { get; set; } = Vector2.Zero;

    // Multipler on the charged sling
    public float ChargeStrength { get; set; } = 1f;

    public readonly PlayerInput Input;

    public PlayerChargeState(PlayerInput _input)
    {
        Input = _input;
    }
}