using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    private bool _attacking;
    [SerializeField] private float _attackTime; 

    [SerializeField] private Collider _hitBox;
    protected override void MyStart()
    {

    }

    protected override void MyUpdate()
    {
        RotateToTarget();

        if (!_attacking)
        {
            if (CheckIfInRange())
            {
                _agent.ResetPath();
                StartCoroutine(Attack());
                _attacking = true;

            }

            else
            {
                _agent.SetDestination(_target.transform.position);
            }
        }
    }

    IEnumerator Attack()
    {
        CurrentState = EnemyState.Attacking;

        _hitBox.enabled = true;
        float timer = 0;
        while (timer < _attackTime)
        {
            timer += Time.deltaTime;
            

            yield return null;
        }

        _hitBox.enabled = false;
        _attacking = false;
        CurrentState = EnemyState.Moving;
    }

    private void RotateToTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
