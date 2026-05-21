using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerObject data;

    private CharacterController controller;
    private InputManager input;
    private PlayerStateMachine state;
    private PlayerLook look;

    public Vector3 Velocity;
    private bool isSprinting;
    private float _multiplier = 1f;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        state = GetComponent<PlayerStateMachine>();
        look = GetComponent<PlayerLook>();
    }

    public void Tick()
    {
        if (state.IsSliding || state.IsDead) return;

        Vector2 moveInput = input.onFoot.Movement.ReadValue<Vector2>();

        if (input.onFoot.Sprint.IsPressed() && moveInput.magnitude > 0.1f)
            isSprinting = true;
        else if (input.onFoot.Sprint.WasReleasedThisFrame() || moveInput.magnitude < 0.1f)
            isSprinting = false;



        Vector3 horizontalVel = controller.velocity;
        horizontalVel.y = 0f;
        look.UpdateFOV(horizontalVel.magnitude);
    }

    public void ProcessMove(Vector2 inputDir)
    {
        if (state.IsSliding || state.IsDead) return;

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * inputDir.y + right * inputDir.x;

        float speed = isSprinting ? data.SprintSpeed * _multiplier : data.Speed * _multiplier;

        Vector3 horizontal = move * speed;

        if (controller.isGrounded && Velocity.y < 0)
            Velocity.y = -2f;

        Velocity.y += data.Gravity * Time.deltaTime;

        controller.Move((horizontal + new Vector3(0, Velocity.y, 0)) * Time.deltaTime);
    }

    public void IncreaseMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }
}
