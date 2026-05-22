using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerStates
    {
        Locomotion,
        Jumping,
        Sliding,
        Dashing,
        Dead
    }

    public PlayerStates CurrentState { get; private set; } = PlayerStates.Locomotion;
    public PlayerJump Jump { get; private set; }
    private bool wasGrounded;

    public bool IsSliding => CurrentState == PlayerStates.Sliding;
    public bool IsDashing => CurrentState == PlayerStates.Dashing;

    public bool IsJumping => CurrentState == PlayerStates.Jumping;
    public bool IsDead => CurrentState == PlayerStates.Dead;

    public string StateName;

    public void SetState(PlayerStates newState)
    {
        CurrentState = newState;
    }

    private void Start()
    {
        Jump = GetComponent<PlayerJump>();
    }
    public void Tick()
    {
        bool grounded = Jump.GroundCheck();

        if ((IsJumping || IsDashing )&&
            wasGrounded == false && grounded == true)
        {
            SetState(PlayerStates.Locomotion);
        }

        wasGrounded = grounded;
        StateName = CurrentState.ToString();
    }
}
