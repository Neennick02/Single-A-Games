using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : Enemy
{
    private bool _attacking;
    [SerializeField] private float _attackTime; 

    [SerializeField] private Collider _hitBox;
    private bool _dead;
    protected override void MyStart()
    {

    }

    protected override void MyUpdate()
    {
        if (_dead) return;

        RotateToTarget();

        if (!_attacking && CurrentState == EnemyState.Moving)
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


        //check if health is low
        if(_healthScript.currentHealth < enemyObject.MaxHealth /3)
        {
            RunAway();
        }

        //check if dead
        if(_healthScript.currentHealth <= 0)
        {
            Die();
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

    public override void Die()
    {
        _dead = true;
       /* Destroy(_agent);
        
        //fall over [not working properly]
        Rigidbody rb = GetComponentInChildren<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
*/
        StartCoroutine(FadeOutAndDie());
    }

    private void RunAway()
    {
        //add later if we agree on flee behaviour
    }

    private void RotateToTarget()
    {
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    IEnumerator FadeOutAndDie()
    {
        Dissolver dissolve = GetComponentInChildren<Dissolver>();
        dissolve.StartDissolve();

        yield return new WaitForSeconds(3f);
       // Destroy(gameObject);
    }
}
