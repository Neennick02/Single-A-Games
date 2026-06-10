using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerObject data;
    private float _currentSpeed;
    private CharacterController controller;
    private InputManager input;
    private PlayerStateMachine state;
    private PlayerLook look;

    public Vector3 Velocity;
    private Vector3 inputVelocity;    
    private Vector3 externalVelocity;  


    private bool isSprinting;
    private float _multiplier = 1f;

    private bool _switchingScene;

    public List<AudioClip> FootstepAudio;
    private float _stepTimer;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<InputManager>();
        state = GetComponent<PlayerStateMachine>();
        look = GetComponent<PlayerLook>();
    }


    /// <summary>
    /// block player input when switching scenes
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EndGame.OnSwitchScene += SwitchScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EndGame.OnSwitchScene -= SwitchScene;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
            StartScene();
    }
    //

    public void Tick()
    {
        if (state.CurrentState == PlayerStateMachine.PlayerStates.Dead || _switchingScene) return;

        Vector2 moveInput = input.onFoot.Movement.ReadValue<Vector2>();

        if (input.onFoot.Sprint.IsPressed() && moveInput.magnitude > 0.1f)
            isSprinting = true;
        else if (input.onFoot.Sprint.WasReleasedThisFrame() || moveInput.magnitude < 0.1f)
            isSprinting = false;



        Vector3 horizontalVel = controller.velocity;
        horizontalVel.y = 0f;
        look.UpdateFOV(horizontalVel.magnitude);

        PlayFootSteps(3f);
    }

    public void ProcessMove(Vector2 inputDir)
    {
        if (state.IsDead || _switchingScene) return;

        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * inputDir.y + right * inputDir.x;

        float targetSpeed = isSprinting ? data.SprintSpeed * _multiplier : data.Speed * _multiplier;

        //acceleration
        float accel = isSprinting ? data.SprintAcceleration : data.WalkAcceleration;

        //deceleration
        float decel = data.Deceleration;

        if (inputDir.magnitude > 0.1f)
            _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, accel * Time.deltaTime);
        else
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, decel * Time.deltaTime);


        inputVelocity = move * _currentSpeed;

        if (controller.isGrounded && Velocity.y < 0)
            Velocity.y = -2f;

        Velocity.y += data.Gravity * Time.deltaTime;

        Vector3 final = inputVelocity + externalVelocity + new Vector3(0, Velocity.y, 0);

        controller.Move(final * Time.deltaTime);

        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * 5f);
    }
    public void AddExternalVelocity(Vector3 vel)
    {
        externalVelocity = vel;
    }
    public void IncreaseMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }

    private void SwitchScene()
    {
        _switchingScene = true;
        controller.enabled = false;
    }
    private void StartScene()
    {
        _switchingScene = false;
        controller.enabled = true;

    }

    private void PlayFootSteps(float interval)
    {
        Vector3 vel = controller.velocity;
        vel.y = 0;

        interval /= vel.magnitude ;

        _stepTimer += Time.deltaTime;

        if (controller.isGrounded && 
            _stepTimer> interval &&
            vel.magnitude > 0.1f &&
            state.CurrentState != PlayerStateMachine.PlayerStates.Sliding)
        {
            AudioManager.Instance.PlayClips(FootstepAudio, .7f, Random.Range(0.8f, 1.2f));
            _stepTimer = 0;
        }
    }
}
