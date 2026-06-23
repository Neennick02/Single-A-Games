using System.Collections;
using UnityEngine;

public class CoinEnemy : Enemy
{
    [Header("Child")]

    private bool _attacking;

    private bool _cd;
    [SerializeField] private Animator _animator;
    protected override void MyUpdate()
    {
        AssignTarget();

        if (CurrentState == EnemyState.Moving)
        {
            HandleWandering();
        }

        if (Vector3.Distance(currentTarget, transform.position) < 0.1f)
        {
            FindTargetLocation();
        }

        if (!_attacking)
        {
            if (CheckIfInRange() && !_cd)
            {

                _agent.ResetPath();
                StartCoroutine(Attack());
                _animator.SetTrigger("Attack");
                _attacking = true;
                _cd = true;

            }

            else
            {
                if(CanSeePlayer())
                {
                    _agent.SetDestination(_target.transform.position);
                    _isWandering = false;
                }
                else
                {
                    if (Vector3.Distance(currentTarget, transform.position) < 0.1f)
                    {
                        FindTargetLocation();
                    }

                }
            }
        }
        Vector3 vel = _agent.velocity;
        vel.y = 0;
        _animator.SetFloat("Speed", vel.magnitude);
    }

    private IEnumerator Attack()
    {
        CurrentState = EnemyState.Attacking;

        //Look at the player and set every bit of velocity to 0 before applying the force to the object and disable their navmesh agent.
        transform.LookAt(_target.transform);

        _agent.enabled = false;

        _rb.linearVelocity = Vector3.zero;

        _rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        //Apply force.
        _agent.enabled = false;

        _rb.AddRelativeForce(new Vector3(0, 3, 2) * 25, ForceMode.Impulse);

        yield return new WaitForSeconds(0.5f);

        _rb.AddRelativeForce(new Vector3(0, -5, 2) * 25, ForceMode.Impulse);


        yield return new WaitForSeconds(3f);

        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        _agent.enabled = true;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        FindTargetLocation(); 

        CurrentState = EnemyState.Moving;
        _attacking = false;

        yield return new WaitForSeconds(3f);

        _cd = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();

            if(health == null)
            {
                health = collision.gameObject.GetComponentInParent<PlayerHealth>();
            }
            health.TakeDamage(enemyObject.Damage);
        }
        else if (collision.gameObject.CompareTag("Buddy"))
        {
            collision.gameObject.GetComponent<BuddyHealth>().TakeDamage(enemyObject.Damage);
        }
    }

}
