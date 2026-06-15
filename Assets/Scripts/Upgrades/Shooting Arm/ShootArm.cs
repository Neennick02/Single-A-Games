using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootArm : MonoBehaviour
{

    private PlayerInput _inputActions;

    [SerializeField] private bool _isAttacking;

    [SerializeField] private bool _isShaking;

    private float _isShakingImpact = 0f;

    private PlayerAttack _playerAttackScript;

    [SerializeField] private GameObject _attachedArm;
    [SerializeField] private GameObject _shootingArm;

    [SerializeField] private GameObject _camera;

    [SerializeField] private bool _arm = true;

    private CharacterController _cc;

    public static event Action<string, bool> InstantMessage;

    public AudioClip ChargeAudio;

    public AudioClip ShootAudio;
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

    private void OnSceneChange()
    {

        Debug.Log("log test");

        _attachedArm.SetActive(true);

        _playerAttackScript.enabled = true;

        _arm = true;

        _isAttacking = false;
    }


    private void Start()
    {
        _attachedArm = GameObject.FindGameObjectWithTag("DefaultArm");

        _playerAttackScript = GetComponentInParent<PlayerAttack>();

        _cc = GetComponentInParent<CharacterController>();

        _cc.detectCollisions = true;

        _camera = Camera.main.gameObject;
    }

    private void Update()
    {

        if (!_arm)
        {
            TryAndCollectArm();
        }

        if (_isAttacking && _isShaking)
        {
            float _Impact = _isShakingImpact += Time.deltaTime * 3;

            _attachedArm.transform.localPosition = AddNoiseOnAngle(-_Impact, _Impact);
        }


        else
        {

            _isShakingImpact = 0f;

            _attachedArm.transform.localPosition = Vector3.zero;
        }

    }
    private void TryAndCollectArm()
    {

        RaycastHit _Hit;

        byte _Range = 5;

        Debug.DrawRay(_camera.transform.position, _camera.transform.forward, Color.red, 0.1f);

        //Send out to detect arm
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out _Hit, _Range))
        {

            if (_Hit.collider.CompareTag("Arm"))
            {

                InstantMessage?.Invoke("Grab Arm", false);

                if (_isAttacking)
                {

                    PickUpArm(_Hit.collider.gameObject);

                    InstantMessage?.Invoke("Grab Arm", true);
                }
            }

            else
            {
                InstantMessage?.Invoke("Grab Arm", true);
            }
        }

        else
        {
            InstantMessage?.Invoke("Grab Arm", true);
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

        }

    }

    private IEnumerator Attack()
    {

        //Wait 1.5 seconds to see if there holding click.

        _isShaking = false;

        yield return new WaitForSeconds(0.5f);

        _isShaking = true;

        //play sound
        AudioManager.Instance.PlayClip(ChargeAudio);

        yield return new WaitForSeconds(1f);

        if (_isAttacking)
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        //Set arm to false, disable the attack script and attached arm, then instantiate the shooting arm prefab

        Instantiate(_shootingArm, _camera.transform.position, _camera.transform.rotation);

        _isAttacking = false;

        _attachedArm.SetActive(false);

        _playerAttackScript.enabled = false;

        _arm = false;

        AudioManager.Instance.PlayClip(ShootAudio);
    }

    private void PickUpArm(GameObject arm)
    {

        //Reset the arm, enable the attached arm and attack script, then destroy the arm on the ground
        Destroy(arm);

        _attachedArm.SetActive(true);

        _playerAttackScript.enabled = true;

        _arm = true;

        _isAttacking = false;

    }


    private Vector3 AddNoiseOnAngle(float min, float max)
    {

        // Find random angle between min & max inclusive
        float xNoise = UnityEngine.Random.Range(min, max);
        float yNoise = UnityEngine.Random.Range(min, max);
        float zNoise = UnityEngine.Random.Range(min, max);

        // Convert Angle to Vector3
        Vector3 noise = new Vector3(Mathf.Sin(2 * Mathf.PI * xNoise / 360), Mathf.Sin(2 * Mathf.PI * yNoise / 360), Mathf.Sin(2 * Mathf.PI * zNoise / 360));

        return noise;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Arm"))
        {

            PickUpArm(collision.gameObject);

        }
    }
}
