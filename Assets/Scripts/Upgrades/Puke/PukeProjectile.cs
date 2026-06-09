using UnityEngine;

public class PukeProjectile : MonoBehaviour
{

    private SphereCollider _collider;

    private bool _isOut = false;

    private Rigidbody _rb;

    private float speed;
    void Start()
    {

        _collider = GetComponent<SphereCollider>();

        _collider.isTrigger = true;

        _rb = GetComponent<Rigidbody>();

        float _Force = Random.Range(500f, 1000f);

        transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);


        //Apply a starting force to "shoot" the arm forward
        _rb.AddRelativeForce(Vector3.forward * _Force);
    }

    private void Update()
    {
        transform.localScale -= Vector3.one * Time.deltaTime * 0.2f;

        if (transform.localScale.x <= 0.1f)
        {
            Destroy(gameObject);
        }
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

            _EnemyHealth.TakeDamage(0.3f);

            Destroy(gameObject);


            _EnemyHealth = null;

        }
    }

}
