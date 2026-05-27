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
    public static event Action OnPause;

    void Awake()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;
        uiActions = playerInput.UI;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        playerInput.OnFoot.ThrowGrenade.performed += ThrowGrenade;
        playerInput.OnFoot.Pause.performed += Pause;
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
        if (blockInputs || motor == null) return;
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
    public void Pause(InputAction.CallbackContext context)
    {
        OnPause?.Invoke();
    }
}
