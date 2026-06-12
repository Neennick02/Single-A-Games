using System.Collections.Generic;
using UnityEngine;

public class ShootingArm : MonoBehaviour
{

    private CapsuleCollider _collider;

    private bool _isOut = false;

    private Rigidbody _rb;

    private float speed;

    [SerializeField] private PlayerAnimator playerAnimator;

    [SerializeField] private List<GameObject> _fases = new List<GameObject>();


    private void Awake()
    {
        playerAnimator = FindAnyObjectByType<PlayerAnimator>();
    }

    void Start()
    {

        _collider = GetComponent<CapsuleCollider>();

        _collider.isTrigger = true;

        _rb = GetComponent<Rigidbody>();

        int _Force = 1000;

        Debug.Log(playerAnimator);

        GetRightFase();

        //Apply a starting force to "shoot" the arm forward
        _rb.AddRelativeForce(Vector3.forward * _Force);
    }

    private void FixedUpdate()
    {

        //Keep setting the current magnitude of the arm's velocity to a variable to check for damage on collision
        speed = _rb.linearVelocity.magnitude;

    }


    private void OnTriggerEnter(Collider other)
    {

        //If the arm collides with anything that isn't the player, make it solid so it can interact with the environment and enemies
        if (!other.gameObject.CompareTag("Player"))
        {
            if (!_isOut)
            {
                _isOut = true;
                _collider.isTrigger = false;
            }

        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            EnemyHealth _EnemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (_EnemyHealth == null)
            {

                _EnemyHealth = collision.gameObject.GetComponentInParent<EnemyHealth>();

            }


            //If the speed is fast enough make it do damage.
            if (speed > 5)
            {
                _EnemyHealth.TakeDamage(4);
            }


            _EnemyHealth = null;

        }
    }

    private void GetRightFase()
    {

        Debug.Log(playerAnimator._state);

        switch (playerAnimator._state)
        {
            case 0:
                _fases[0].SetActive(true);
                break;
            case 1:
                _fases[1].SetActive(true);
                break;
            case 2:
                _fases[2].SetActive(true);
                break;
            case 3:
                _fases[3].SetActive(true);
                break;
        }
    }

}
