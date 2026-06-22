using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInput _inputActions;

    private GameObject _camera;

    private PlayerHealth _healthScript;

    private bool _isAttacking;

    public byte range = 5;

    private float _damageMultiplier = 1;

    [SerializeField] private PlayerAnimator _animator;

    [SerializeField] private CameraShakeManager _cameraShakeManager;

    private CinemachineImpulseSource _impulseSource;

    private bool _isDead;

    public AudioClip SwingAudio;
    public AudioClip HitAudio;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>().gameObject;

        _healthScript = GetComponent<PlayerHealth>();

        _impulseSource = GetComponent<CinemachineImpulseSource>();

        Cursor.lockState = CursorLockMode.Locked;

        _cameraShakeManager = FindFirstObjectByType<CameraShakeManager>();

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
        PlayerHealth.OnDeath += Die;
    }

    private void OnDisable()
    {
        _inputActions.OnFoot.Attack.performed -= Attack;
        SanityManager.OnDamageOutputChange -= IncreaseDamageOutput;
        PlayerHealth.OnDeath -= Die;
        _inputActions.Disable();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (_isDead) return;

        if (!_isAttacking)
        {
            _animator.AttackAnimation();
            StartCoroutine(Attack());
            _isAttacking = true;


            //player swing sound
            AudioManager.Instance.PlayClip(SwingAudio, 1, Random.Range(0.8f, 1.2f));
        }

    }

    private IEnumerator Attack()
    {
        RaycastHit hit;

        yield return new WaitForSeconds(0.2f);

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
        {

            if (hit.collider.CompareTag("Enemy"))
            {
                //find health script
                EnemyHealth health = hit.collider.GetComponent<EnemyHealth>();
                if (health == null)
                {
                    health = hit.collider.GetComponentInParent<EnemyHealth>();
                }

                health.TakeDamage(_healthScript.PlayerObject.Damage * _damageMultiplier);

                float X = Random.Range(-0.1f, 0.1f);
                float Y = Random.Range(-0.1f, 0.1f);

                _impulseSource.DefaultVelocity = new Vector3(X, Y, 0f);

                _cameraShakeManager.CameraShake(_impulseSource, 1);
                AudioManager.Instance.PlayClip(HitAudio, 1, Random.Range(0.8f, 1.2f));
            }

        }

        yield return new WaitForSeconds(0.2f);
        _isAttacking = false;
    }

    public void IncreaseDamageOutput(float multiplier)
    {
        _damageMultiplier = multiplier;
    }

    private void Die()
    {
        _isDead = true;
    }
}
