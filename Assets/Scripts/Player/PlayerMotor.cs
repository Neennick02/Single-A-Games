using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static PlayerStateMachine;

public class PlayerMotor : MonoBehaviour
{
    public static PlayerMotor Instance { get; private set; }
    public PlayerObject Data;
    public PlayerMovement Movement { get; private set; }
    public PlayerSlide Slide { get; private set; }
    public PlayerDash Dash { get; private set; }

    public PlayerJump Jump { get; private set; }
    public PlayerStateMachine State { get; private set; }

    private InputManager _input;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        PlayerHealth.OnDeath += Die;
        GameManager.OnGameStart += StartGame;
    }

    private void Start()
    {
        Movement = GetComponent<PlayerMovement>();
        Dash = GetComponent<PlayerDash>();
        Jump = GetComponent<PlayerJump>();
        State = GetComponent<PlayerStateMachine>();
        _input = GetComponent<InputManager>();
        Slide = GetComponent<PlayerSlide>();
    }

    private void OnDisable()
    {
        PlayerHealth.OnDeath -= Die;
        GameManager.OnGameStart -= StartGame;
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

        //disable player attack
        Destroy(GetComponent<PlayerAttack>());
    }
    
    private void StartGame()
    {
        State.SetState(PlayerStates.Locomotion);
        StartCoroutine(Slide.ChangeHeight(Data.DefaultHeight));
    }
}
