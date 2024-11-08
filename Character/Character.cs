using System.Collections;
using Godot;


public partial class Character : CharacterBody2D
{
    [Export]
    public AnimationPlayer animator;

    [Export]
    private float MoveSpeed = 75.0f;
    [Export]
    private float Desceleration = 30f;
    [Export]
    private float ChargeSpeed = 40f;

    // Direction of the currently selected action
    public Vector2 currentDirection = Vector2.Zero;

    // Multipler on the charged sling
    private float chargeStrength = 1;

    public enum State
    {
        Charging,
        Moving
    }

    private State slingState = State.Moving;

    private PlayerInput _playerInput;

    private bool _canCharacterMove = false;

    public void SetupPlayer(PlayerID id)
    {
        _playerInput = new(id);
        _canCharacterMove = true;
    }

    public override void _Process(double delta)
    {
        if (_playerInput == null)
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
            currentDirection = inputVector;
        }
    }

    private Vector2 GetActiveInputVector()
    {
        return Input.GetVector(
            _playerInput.GetInputKey(InputAction.MoveLeft),
            _playerInput.GetInputKey(InputAction.MoveRight),
            _playerInput.GetInputKey(InputAction.MoveUp),
            _playerInput.GetInputKey(InputAction.MoveDown)
        ).Normalized();
    }

    private bool HasActiveInput()
    {
        return GetActiveInputVector() != Vector2.Zero;
    }

    private void GainChargeStrength(float delta)
    {
        chargeStrength += delta * ChargeSpeed;
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
        Velocity = currentDirection * MoveSpeed * chargeStrength;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
