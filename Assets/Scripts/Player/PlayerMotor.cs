using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private float _slideIntervalTimer;
    private bool _slideEnded;
    private Coroutine _slideRoutine;

    void Start()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<CharacterController>();
        _mainCam = GetComponentInChildren<Camera>();
        _controller.height = _movementObject.DefaultHeight;
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

        HandleSlideInput(); //check for crouch input
    }

    public void Jump()
    {
        if (_isGrounded)
        {
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
        //calculate slide interval
        if (_slideEnded)
        {
            _slideIntervalTimer += Time.fixedDeltaTime;
            if( _slideIntervalTimer >= _movementObject.SlideInterval)
            {
                _slideEnded = false;
            } 
        }

        if (_isSliding) return; // ignore crouch input during slide

        Vector2 moveInput = _input.onFoot.Movement.ReadValue<Vector2>();
        bool dashPressed = _input.onFoot.Dash.triggered;

        //check if player is moving forward

/*        Vector3 moveDir = new Vector3(_playerVelocity.x, 0f, _playerVelocity.z).normalized;
        Vector3 camForward = _mainCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        float alignment = Vector3.Dot(moveDir, camForward);*/

        //check if player is sliding
        if (!_slideEnded && dashPressed && !_isSliding && _playerVelocity.magnitude > 0.1f)
        {
            //empty coroutine reference
            if(_slideRoutine != null) StopCoroutine(_slideRoutine);

            //start new slide
            _isSliding = true;
            _slideRoutine = StartCoroutine(Slide(moveInput));
        }
    }

    IEnumerator Slide(Vector2 inputDir)
    {
        //make player smaller
        _controller.height = _movementObject.SlideHeight;

        //add gravity
        _playerVelocity.y = -2f;

        //calculate direction
        Vector3 moveDir = transform.forward;
        Vector3 slideVelocity = moveDir * _movementObject.SlideSpeed;

        float slideTimer = 0f;
        float duration = _movementObject.SlideDuration;

        while (slideTimer < duration)
        {
            slideTimer += Time.fixedDeltaTime;

            _playerVelocity = Vector3.Lerp(_playerVelocity, slideVelocity, slideTimer / duration);
            _controller.Move(_playerVelocity * Time.fixedDeltaTime);
            
            yield return null;
        }

        //reset player height
        _controller.height = _movementObject.DefaultHeight;

        //reset variables
        _isSliding = false;
        _slideEnded = true;
        _slideIntervalTimer = 0;

        //transfer velocity to movement
        _playerVelocity = Vector3.zero;
    }
}
