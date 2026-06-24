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

    private bool _isDead;

    public AudioClip SwingAudio;
    public AudioClip HitAudio;

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
            StartCoroutine(Attack());
            _isAttacking = true;


            //player swing sound
            AudioManager.Instance.PlayClip(SwingAudio, 1, Random.Range(0.8f, 1.2f));
        }

    }

    private IEnumerator Attack()
    {
        _animator.AttackAnimation();

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(DetectAttack());

        yield return new WaitForSeconds(0.5f);

        _isAttacking = false;
    }

    private IEnumerator DetectAttack()
    {
        RaycastHit hit;

        bool attacked = false;

        while (_isAttacking && !attacked)
        {

            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range) && !attacked)
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

                    CameraShakeManager.Instance.CameraShake(gameObject, 0.5f);

                    AudioManager.Instance.PlayClip(HitAudio, 1, Random.Range(0.8f, 1.2f));

                    attacked = true;

                }
            }

            Debug.Log("test");

            yield return null;

        }

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
