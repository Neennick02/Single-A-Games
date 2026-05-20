using System;
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

    public static event Action<string> OnGrabArmAvailable;

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
        _attachedArm = GameObject.FindGameObjectWithTag("DefaultArm");

        _playerAttackScript = GetComponentInParent<PlayerAttack>();

        _cc = GetComponentInParent<CharacterController>();

        _cc.detectCollisions = true;

        _camera = Camera.main.gameObject;
    }
    private void TryAndCollectArm()
    {

        RaycastHit _Hit;

        byte _Range = 10;


        //Send out to detect arm
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _Hit, _Range))
        {

            if (_Hit.collider.CompareTag("Arm"))
            {

                Debug.Log("Arm Detected");
                OnGrabArmAvailable?.Invoke("Grab Arm");

                PickUpArm(_Hit.collider.gameObject);
            }
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {

        //If the player has the arm, they can attack, if not they can try and collect it
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

        //Wait 2 seconds to see if there holding click.

        yield return new WaitForSeconds(2f);

        if (_isAttacking)
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        //Set arm to false, disable the attack script and attached arm, then instantiate the shooting arm prefab
        _isAttacking = false;

        _attachedArm.SetActive(false);

        _playerAttackScript.enabled = false;

        _arm = false;

        Instantiate(_shootingArm, _camera.transform.position, _camera.transform.rotation);
    }

    private void PickUpArm(GameObject arm)
    {

        //Reset the arm, enable the attached arm and attack script, then destroy the arm on the ground
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
