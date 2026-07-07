using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private bool _attacking;
    [SerializeField] private float _attackTime;

    [SerializeField] private Collider _hitBox;
    private bool _dead;

    private Animator _animator;
    protected override void MyStart()
    {
        _rb = GetComponentInChildren<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();
    }

    protected override void MyUpdate()
    {
        if (_dead) return;

        AssignTarget();
        RotateToTarget();
        _animator.SetInteger("State", (int)CurrentState);

        if (_attacking || CurrentState == EnemyState.Attacking) return;

        //check for attack range
        if (CheckIfInRange())
        {
            _agent.ResetPath();
            StartCoroutine(Attack());
            _attacking = true;
            return;
        }

        //if player is spotted
        if (CanSeeTarget(_target))
        {
            _isWandering = false;
            _agent.SetDestination(_target.transform.position);
        }
        else
        {
            if (!_isWandering)
            {
                FindTargetLocation();
            }

            HandleWandering(); 
        }

        if (_healthScript.currentHealth < enemyObject.MaxHealth / 3)
        {
            RunAway();
        }

        //check health
        if (_healthScript.currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator Attack()
    {
        CurrentState = EnemyState.Attacking;

        _agent.enabled = false;

        _hitBox.enabled = true;

        yield return new WaitForSeconds(_attackTime);

        _hitBox.enabled = false;
        _attacking = false;
        _agent.enabled = true;
        CurrentState = EnemyState.Moving;
        _isWandering = false;
    }

    public override void Die()
    {
        _dead = true;
        Destroy(_agent);

        //fall over [not working properly]
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    private void RunAway()
    {
        //add later if we agree on flee behaviour
    }

    private void RotateToTarget()
    {
        Vector3 direction;
        if (_isWandering)
        {
            direction = (currentTarget - transform.position).normalized;
        }
        else
        {
            direction = (_target.transform.position - transform.position).normalized;
        }


        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
