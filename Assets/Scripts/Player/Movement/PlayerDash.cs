using Unity.Cinemachine;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public PlayerObject Data;
    private CharacterController controller;
    private InputManager input;
    private PlayerMovement movement;
    private PlayerStateMachine state;

    private Vector3 dashVelocity;
    private float dashTimer;

    private bool _started;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        movement = GetComponent<PlayerMovement>();
        state = GetComponent<PlayerStateMachine>();
    }

    public void Tick()
    {
        if (state.IsDashing)
        {
            if (!_started)
            {
                StartDash();
                _started = true;
            }
            DashUpdate();
        }
        else
        {
                _started = false;
        }
    }

    private void StartDash()
    {
        Vector3 dir = controller.velocity;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.1f)
            dir = transform.forward;

        dir.Normalize();

        dashVelocity = dir * Data.DashInitialBoost;
        dashTimer = 0f;
    }

    private void DashUpdate()
    {
        dashTimer += Time.deltaTime;

        dashVelocity += dashVelocity.normalized * Data.DashAcceleration * Time.deltaTime;

        dashVelocity = Vector3.ClampMagnitude(dashVelocity, Data.MaxDashSpeed);

        Vector3 dir = movement.Velocity;
        dir.y = 0;

        if (dir.sqrMagnitude < 0.1f)
            dir = transform.forward;

        dir.Normalize();
        movement.AddExternalVelocity(dashVelocity);
    }
}
