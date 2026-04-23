using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInput _inputActions;

    private GameObject _camera;

    private void Start()
    {
        _camera = FindAnyObjectByType<Camera>().gameObject;

    }
    private void OnEnable()
    {

        if (_inputActions == null)
        {
            _inputActions = new PlayerInput();
        }

        _inputActions.Enable();

        _inputActions.OnFoot.Attack.performed += Attack;
        _inputActions.OnFoot.Attack.canceled += Attack;

    }

    private void OnDisable()
    {
        _inputActions.OnFoot.Attack.performed -= Attack;
        _inputActions.OnFoot.Attack.canceled -= Attack;
        _inputActions.Disable();
    }

    private void Attack(InputAction.CallbackContext context)
    {

        RaycastHit hit;


        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 3f))
        {

            Debug.Log("Hit: " + hit.collider.name);
        }

    }

    private void Update()
    {
        Debug.DrawRay(new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z), _camera.transform.forward, Color.red);
    }
}
