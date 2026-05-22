using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.UIActions uiActions;
    PlayerMotor motor;
    PlayerLook look;

    bool blockInputs = false;

    public static event Action OnThrowGrenade;
    void Awake()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;
        uiActions = playerInput.UI;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
      //  onFoot.Jump.performed += ctx => motor.Jump();

        playerInput.OnFoot.ThrowGrenade.performed += ThrowGrenade;

    }
    void OnEnable()
    {
        onFoot.Enable();
        uiActions.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
        uiActions.Disable();
    }

    private void FixedUpdate()
    {
        if (blockInputs) return;
        //tell playermotor to move using values from movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        if (blockInputs) return;
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }


    public void BlockInput(bool value)
    {
        blockInputs = value;
    }

    public void ThrowGrenade(InputAction.CallbackContext context)
    {
        OnThrowGrenade?.Invoke();
    }
}
