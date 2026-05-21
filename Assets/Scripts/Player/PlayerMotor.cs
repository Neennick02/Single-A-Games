using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public PlayerSlide Slide { get; private set; }
    public PlayerJump Jump { get; private set; }
    public PlayerStateMachine State { get; private set; }

    private InputManager _input;

    private void Start()
    {
        Movement = GetComponent<PlayerMovement>();
        Slide = GetComponent<PlayerSlide>();
        Jump = GetComponent<PlayerJump>();
        State = GetComponent<PlayerStateMachine>();
        _input = GetComponent<InputManager>();
    }

    private void Update()
    {
        State.Tick();
        Slide.Tick();
        Jump.Tick();
        Movement.Tick();
    }

    public void ProcessMove(Vector2 input)
    {
        Movement.ProcessMove(input);
    }
}
