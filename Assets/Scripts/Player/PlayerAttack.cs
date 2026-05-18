using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInput _inputActions;

    private GameObject _camera;

    private PlayerHealth _healthScript;

    private bool _isAttacking;

    private float _damageMultiplier = 1;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>().gameObject;

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

        SanityManager.OnDamageOutputChange += IncreaseDamageOutput;
    }

    private void OnDisable()
    {
        _inputActions.OnFoot.Attack.performed -= Attack;
        SanityManager.OnDamageOutputChange -= IncreaseDamageOutput;
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


        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, 3f))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                //find health script
                EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();

                if(health == null)
                {
                    health = hit.collider.GetComponentInParent<EnemyHealth>();
                }

                health.TakeDamage(_healthScript.PlayerObject.Damage * _damageMultiplier);
                Debug.Log("hit " + hit.transform.name);
            }

        }

        yield return new WaitForSeconds(0.2f);
        _isAttacking = false;
    }

    public void IncreaseDamageOutput(float multiplier)
    {
        _damageMultiplier += multiplier;
    }
}
