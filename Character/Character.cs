using Godot;


public partial class Character : CharacterBody2D
{
    [Export]
    public AnimationPlayer animator;

    public const float Speed = 25.0f;
    public const float Desceleration = 15f;

    // Direction currently being moved towards
    private InputAction? actionDirection = null;

    // Direction of the currently selected action
    public Vector2 currentDirection = Vector2.Zero;

    // Strength vector of the slingshot
    private Vector2 chargeVector = Vector2.Zero;

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

        ChooseActiveInput();

        if (actionDirection.HasValue)
        {
            if (Input.IsActionPressed(_playerInput.GetInputKey(actionDirection.Value)))
            {
                if (slingState == State.Moving)
                {
                    slingState = State.Charging;
                    DisplayCharging();
                }
                else if (slingState == State.Charging)
                {
                    GainChargeStrength();
                }
            }

            if (Input.IsActionJustReleased(_playerInput.GetInputKey(actionDirection.Value)))
            {
                if (slingState == State.Charging)
                {
                    slingState = State.Moving;
                    StopCharging();
                    ReleaseShot();
                }
            }
        }
        else
        {
            Vector2 velocity = Velocity;
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Desceleration);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Desceleration);
            Velocity = velocity;
        }
    }

    private void ChooseActiveInput()
    {
        if (!actionDirection.HasValue)
        {
            if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.MoveDown)))
            {
                actionDirection = InputAction.MoveDown;
            }
            if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.MoveUp)))
            {
                actionDirection = InputAction.MoveUp;
            }
            if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.MoveLeft)))
            {
                actionDirection = InputAction.MoveLeft;
            }
            if (Input.IsActionJustPressed(_playerInput.GetInputKey(InputAction.MoveRight)))
            {
                actionDirection = InputAction.MoveRight;
            }
        }
    }

    private void GainChargeStrength()
    {
        if (actionDirection == InputAction.MoveDown)
        {
            currentDirection = Vector2.Down;
        }
        if (actionDirection == InputAction.MoveUp)
        {
            currentDirection = Vector2.Up;
        }
        if (actionDirection == InputAction.MoveLeft)
        {
            currentDirection = Vector2.Left;
        }
        if (actionDirection == InputAction.MoveRight)
        {
            currentDirection = Vector2.Right;
        }
        chargeVector += currentDirection * Speed;
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
        Velocity = chargeVector;
        chargeVector = Vector2.Zero;
        actionDirection = null;
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }
}
