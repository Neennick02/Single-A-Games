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

        int _Force = 1000;


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

            //If the speed is fast enough make it do damage.
            if (speed > 5)
            {
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(4);
            }
        }
    }

}
