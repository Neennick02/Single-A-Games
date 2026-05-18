using UnityEngine;

public class ShootingArm : MonoBehaviour
{

    private CapsuleCollider _collider;

    private bool _isOut = false;

    private Rigidbody _rb;

    private float speed;
    void Start()
    {

        _collider = GetComponent<CapsuleCollider>();

        _collider.isTrigger = true;

        _rb = GetComponent<Rigidbody>();

        int force = 1000;

        _rb.AddRelativeForce(Vector3.forward * force);
    }

    private void FixedUpdate()
    {

        speed = _rb.linearVelocity.magnitude;

    }


    private void OnTriggerEnter(Collider other)
    {
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
            if (speed > 5)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(4);
            }
        }
    }

}
