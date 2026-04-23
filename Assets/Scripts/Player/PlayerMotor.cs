using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private PlayerObject _movementObject;
    private InputManager _input;
    private CharacterController _controller;

    public PlayerStates CurrentState;
    public enum PlayerStates
    {
        Locomotion,
        Jumping,
        Sliding,
        Dead
    }
    private Vector3 _playerVelocity;

    private bool _isGrounded;
    private bool _wasGrounded;
    private bool _isSprinting = false;

    //slide
    private bool _isSliding = false;
    private Coroutine _slideRoutine;

    [Header("Fov")]
    [SerializeField] float minFov = 60;
    [SerializeField] float maxFov = 90;
    [SerializeField] private CinemachineCamera _camera;

    [SerializeField] float fovSmoothSpeed = 8;


    [SerializeField] private Transform headTransform; 

    private void OnEnable()
    {
        PlayerHealth.OnDeath += Die;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDeath -= Die;
    }
    void Start()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<CharacterController>();
        _controller.height = _movementObject.DefaultHeight;

        CurrentState = PlayerStates.Locomotion;

        Vector3 height = new Vector3(headTransform.position.x, _movementObject.DefaultHeight, headTransform.position.z);
        headTransform.position = height;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _isGrounded = _controller.isGrounded;

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();

        if (_input.onFoot.Sprint.IsPressed() && moveInput.magnitude > 0.1f) //if moving and sprint is pressed
        {
            _isSprinting = true;
        }
        else if (_input.onFoot.Sprint.WasReleasedThisFrame() || moveInput.magnitude < 0.1f) //no movement or no sprint
        {
            _isSprinting = false;
        }
    }

    private void Update()
    {
        HandleSlideInput(); //check for slide input

        //landing detection
        if(CurrentState == PlayerStates.Jumping &&
            !_wasGrounded && _controller.isGrounded)
        {
            CurrentState = PlayerStates.Locomotion;
        }

        _wasGrounded = _controller.isGrounded;

        //increase fov with move speed
        UpdateFOV();
    }
    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        if (_isSliding) return;

        NormalMovement(input);
    }

    void NormalMovement(Vector2 input)
    {
        // Input direction
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        // flatten camera vectors
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 inputDir = forward * input.y + right * input.x;



        float speed = _isSprinting ? _movementObject.SprintSpeed : _movementObject.Speed;

        // Horizontal velocity
        Vector3 horizontal = inputDir * speed;

        // Gravity
        if (_controller.isGrounded && _playerVelocity.y < 0)
            _playerVelocity.y = -2f;

        _playerVelocity.y += _movementObject.Gravity * Time.deltaTime;

        if (CurrentState != PlayerStates.Dead)
        {
            //apply normal velocity
            Vector3 finalMove = horizontal + new Vector3(0, _playerVelocity.y, 0);
            _controller.Move(finalMove * Time.deltaTime);
        }
        else
        {
            //only apply y velocity when dead
            Vector3 finalMove = new Vector3(0, _playerVelocity.y, 0);
            _controller.Move(finalMove * Time.deltaTime);
        }
    }
    public void Jump()
    {
        if (_isGrounded)
        {
            //change state 
            CurrentState = PlayerStates.Jumping;

            //cancel current slide
            if (_slideRoutine != null)
            {
                StopCoroutine(_slideRoutine);
                _slideRoutine = null;

                //reset player height
                _controller.height = _movementObject.DefaultHeight;
                _isSliding = false;
            }

            _playerVelocity.y = Mathf.Sqrt(_movementObject.JumpHeight * -2f * _movementObject.Gravity);
        }
    }


    void HandleSlideInput()
    {
        if (_isSliding || !_controller.isGrounded) return; // ignore crouch input during slide

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();
        bool dashPressed = _input.onFoot.Dash.triggered;

        Vector3 horizontalVelocity = _controller.velocity;
        horizontalVelocity.y = 0;

        //check if player is sliding
        if (dashPressed &&
            !_isSliding &&
            horizontalVelocity.magnitude > 0.1f &&
            IsAlignedWithDirection(moveInput))
        {
            //empty coroutine reference
            if(_slideRoutine != null) StopCoroutine(_slideRoutine);

            //start new slide
            _isSliding = true;
            _slideRoutine = StartCoroutine(SlideRoutine());
        }
    }

    IEnumerator SlideRoutine()
    {
        //change state
        CurrentState = PlayerStates.Sliding;

        //make player smaller
        Vector3 height = new Vector3(headTransform.position.x, _movementObject.SlideHeight, headTransform.position.z);
        headTransform.position = height;

        //add gravity
        _playerVelocity.y = -2f;

        float slideTimer = 0f;
        float duration = _movementObject.SlideDuration;

        while (slideTimer < duration)
        {
            slideTimer += Time.deltaTime;

            //calculate direction
            Transform cam = Camera.main.transform;

            Vector3 forward = cam.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 slideVelocity = forward * _movementObject.SlideSpeed;

            Vector3 horizontalSlide = Vector3.Lerp(
                new Vector3(_playerVelocity.x, 0, _playerVelocity.z),
                slideVelocity,
                slideTimer / duration
            );

            //apply slide force
            _playerVelocity.x = horizontalSlide.x;
            _playerVelocity.z = horizontalSlide.z; _controller.Move(_playerVelocity * Time.deltaTime);

            yield return null;
        }

        //reset player height
        height = new Vector3(headTransform.position.x, _movementObject.DefaultHeight, headTransform.position.z);
        headTransform.position = height;

        //transfer velocity to movement
        _playerVelocity = Vector3.zero;

        _isSliding = false;
        CurrentState = PlayerStates.Locomotion;
    }

    private bool IsAlignedWithDirection(Vector2 moveInput)
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveDir.sqrMagnitude < 0.01f)
            return false;

        moveDir = transform.TransformDirection(moveDir).normalized;

        float alignment = Vector3.Dot(transform.forward, moveDir);

        return alignment > 0.2f;
    }

    private void UpdateFOV()
    {
        Vector3 fovVelocity = _controller.velocity;
        fovVelocity.y = 0f;

        float speed = fovVelocity.magnitude;
        float maxSpeed = _movementObject.SprintSpeed;

        float t = Mathf.Clamp01(speed / maxSpeed);
        
        float targetFov = Mathf.Lerp(minFov, maxFov, t);

        //apply fov
        _camera.Lens.FieldOfView = Mathf.Lerp(
            _camera.Lens.FieldOfView,
            targetFov,
            fovSmoothSpeed * Time.deltaTime);
    }

    private void Die()
    {
        CurrentState = PlayerStates.Dead;
        _controller.height = _movementObject.SlideHeight-1;

    }
}
