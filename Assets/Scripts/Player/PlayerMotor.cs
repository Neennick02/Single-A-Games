using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateMachine;

public class PlayerMotor : MonoBehaviour
{
    public PlayerMovement Movement { get; private set; }
    public PlayerSlide Slide { get; private set; }
    public PlayerDash Dash { get; private set; }

    public PlayerJump Jump { get; private set; }
    public PlayerStateMachine State { get; private set; }

    private InputManager _input;

    public List<MonoBehaviour> PlayerAbilities;
    private void OnEnable()
    {
        PlayerHealth.OnDeath += Die;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDeath -= Die;
    }
    private void Start()
    {
        Movement = GetComponent<PlayerMovement>();
        Slide = GetComponent<PlayerSlide>();
        Dash = GetComponent<PlayerDash>();
        Jump = GetComponent<PlayerJump>();
        State = GetComponent<PlayerStateMachine>();
        _input = GetComponent<InputManager>();
    }

    private void Update()
    {
        State.Tick();
        Slide.Tick();
        Dash.Tick();
        Jump.Tick();
        Movement.Tick();
    }

    public void ProcessMove(Vector2 input)
    {
        Movement.ProcessMove(input);
    }
    private void Die()
    {
        State.SetState(PlayerStates.Dead);

        StartCoroutine(Slide.ChangeHeight(0));

        for(int i = 0; i < PlayerAbilities.Count; i++ )
        {
            Destroy(PlayerAbilities[i]);
        }
        PlayerAbilities.Clear();
    }

}
