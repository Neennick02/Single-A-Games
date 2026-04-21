using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private MovementObject _movementObject;
    private InputManager _input;
    private CharacterController _controller;
    private Camera _mainCam;
    private enum PlayerStates
    {
        Locomotion,
        Jumping,
        Sliding,
        Dead
    }


    private Vector3 _playerVelocity;
    public int CurrentSpeed;


    private bool _isGrounded;
    private bool _isSprinting = false;

    //slide
    private bool _isSliding = false;
    private Coroutine _slideRoutine;

    //fov
    private float startFov;
    private float currentFov;

    void Start()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<CharacterController>();
        _mainCam = GetComponentInChildren<Camera>();
        _controller.height = _movementObject.DefaultHeight;
        startFov = _mainCam.fieldOfView;
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

        HandleSlideInput(); //check for slide input
    }

    private void Update()
    {
        //scale fov with current speed
        Vector3 fovVelocity = _controller.velocity;
        fovVelocity.y = 0f;

        if(fovVelocity.magnitude > 0.1f)
        {
            //scale with speed?
        }
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            //cancel current slide
            if(_slideRoutine != null)
            {
                StopCoroutine(_slideRoutine);
                //reset player height
                _controller.height = _movementObject.DefaultHeight;

                //transfer velocity to movement
                _playerVelocity = Vector3.zero;

                _isSliding = false;
            }

            _playerVelocity.y = Mathf.Sqrt((_movementObject.JumpHeight * -3) * _movementObject.Gravity);
        }
    }

    //receive input from InputManager and apply to CharacterController
    public void ProcessMove(Vector2 input)
    {
        if (_isSliding) return;

        NormalMovement(input);
    }

    void NormalMovement(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero; //reset direction and set x & y
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        if (_isSprinting) //when sprinting change the movement speed and set currentSpeed to 2
        {
            _controller.Move(transform.TransformDirection(moveDirection) * _movementObject.SprintSpeed * Time.deltaTime);
            CurrentSpeed = 3;
        }
        else if (!_isSprinting) //when not sprinting currentSpeed = 1  
        {
           _controller.Move(transform.TransformDirection(moveDirection) * _movementObject.Speed * Time.deltaTime);
            CurrentSpeed = 2;
        }

        if (input.magnitude < 0.1f) //when no input currentspeed == 0
        {
            CurrentSpeed = 0;
        }

        if (_controller.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2;
        }

        //handles gravity
        _playerVelocity.y += _movementObject.Gravity * Time.deltaTime;

        //apply vertical velocity
        _controller.Move(_playerVelocity * Time.deltaTime);
    }


    void HandleSlideInput()
    {
        if (_isSliding) return; // ignore crouch input during slide

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();
        bool dashPressed = _input.onFoot.Dash.triggered;

        //check if player is moving forward
        bool aligned = IsAlignedWithDirection(moveInput);

        //check if player is sliding
        if (dashPressed && !_isSliding && _playerVelocity.magnitude > 0.1f && aligned)
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
        //make player smaller
        _controller.height = _movementObject.SlideHeight;

        //add gravity
        _playerVelocity.y = -2f;

        float slideTimer = 0f;
        float duration = _movementObject.SlideDuration;

        while (slideTimer < duration)
        {
            slideTimer += Time.fixedDeltaTime;

            //calculate direction
            Vector3 slideVelocity = transform.forward * _movementObject.SlideSpeed;

            _playerVelocity = Vector3.Lerp(_playerVelocity, slideVelocity, slideTimer / duration);
            _controller.Move(_playerVelocity * Time.fixedDeltaTime);
            yield return null;
        }

        //reset player height
        _controller.height = _movementObject.DefaultHeight;

        //transfer velocity to movement
        _playerVelocity = Vector3.zero;

        _isSliding = false;
    }

    private bool IsAlignedWithDirection(Vector2 moveInput)
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveDir.magnitude > 0.1f)
        {
            moveDir = transform.TransformDirection(moveDir).normalized;
        }
        Vector3 camForward = _mainCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        float alignment = Vector3.Dot(moveDir, camForward);

        if(alignment > 0.7f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
