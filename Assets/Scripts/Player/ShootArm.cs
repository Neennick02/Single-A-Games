using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootArm : MonoBehaviour
{

    private PlayerInput _inputActions;

    [SerializeField] private bool _isAttacking;

    private PlayerAttack _playerAttackScript;

    [SerializeField] private GameObject _attachedArm;
    [SerializeField] private GameObject _shootingArm;

    [SerializeField] private GameObject _camera;

    [SerializeField] private bool _arm = true;

    private CharacterController _cc;

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

    private void Start()
    {

        _playerAttackScript = GetComponent<PlayerAttack>();

        _cc = GetComponent<CharacterController>();

        _cc.detectCollisions = true;
    }
    private void TryAndCollectArm()
    {

        RaycastHit hit;

        byte range = 5;


        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {

            if (hit.collider.CompareTag("Arm"))
            {

                Debug.Log("Arm Detected");
                PickUpArm(hit.collider.gameObject);
            }
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (_arm)
        {
            _isAttacking = context.performed;

            if (_isAttacking)
            {
                StartCoroutine(Attack());
            }

            else
            {
                StopAllCoroutines();
            }

        }

        else
        {
            _isAttacking = context.performed;

            if (_isAttacking)
            {
                TryAndCollectArm();
            }

        }

    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        if (_isAttacking)
        {
            Shoot();
        }

    }

    private void Shoot()
    {

        _isAttacking = false;

        _attachedArm.SetActive(false);

        _playerAttackScript.enabled = false;

        _arm = false;

        Instantiate(_shootingArm, _camera.transform.position, _camera.transform.rotation);
    }

    private void PickUpArm(GameObject arm)
    {

        Destroy(arm);

        _attachedArm.SetActive(true);

        _playerAttackScript.enabled = true;

        _arm = true;

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Arm"))
        {

            PickUpArm(collision.gameObject);

        }
    }
}
