using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private PlayerObject data;

    private CharacterController controller;
    private InputManager input;
    private PlayerMovement movement;
    private PlayerStateMachine state;

    private bool wasGrounded;
    private float coyoteTime = 0.15f;
    private float coyoteTimer;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        movement = GetComponent<PlayerMovement>();
        state = GetComponent<PlayerStateMachine>();
    }
    
    public void Tick()
    {

        // Update coyote timer
        if (controller.isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        // Jump input
        if (input.onFoot.Jump.triggered)
            TryJump();

        wasGrounded = controller.isGrounded;
    }

    private void TryJump()
    {
        // Allow jump if grounded OR within coyote time
        if (coyoteTimer <= 0f || state.CurrentState == PlayerStateMachine.PlayerStates.Dead) return;

        Vector3 vel = movement.Velocity;
        vel.y = Mathf.Sqrt(data.JumpHeight * -2f * data.Gravity);
        movement.Velocity = vel;

        state.SetState(PlayerStateMachine.PlayerStates.Jumping);

        // Consume coyote time
        coyoteTimer = 0f;
    }

    public bool GroundCheck()
    {
        return controller.isGrounded;
    }
}
