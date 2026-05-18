using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInput _inputActions;

    private GameObject _camera;

    private PlayerHealth _healthScript;

    private bool _isAttacking;

    private void Start()
    {
        _camera = FindAnyObjectByType<Camera>().gameObject;

        _healthScript = GetComponent<PlayerHealth>();

        Cursor.lockState = CursorLockMode.Locked;

    }
    private void OnEnable()
    {

        if (_inputActions == null)
        {
            _inputActions = new PlayerInput();
        }

        _inputActions.Enable();

        _inputActions.OnFoot.Attack.performed += Attack;

    }

    private void OnDisable()
    {
        _inputActions.OnFoot.Attack.performed -= Attack;
        _inputActions.Disable();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (!_isAttacking)
        {

            StartCoroutine(Attack());
            _isAttacking = true;

        }

    }

    private void Update()
    {
        Debug.DrawRay(new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z), _camera.transform.forward, Color.red);
    }

    private IEnumerator Attack()
    {
        RaycastHit hit;

        byte range = 5;


        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyHealth>().TakeDamage(_healthScript.PlayerObject.Damage);
            }

        }

        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
    }
}
