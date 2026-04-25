using System.Collections;
using UnityEngine;

public class CoinEnemy : Enemy
{
    [Header("Child")]

    private bool _attacking;

    protected override void MyUpdate()
    {
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

    private bool CheckIfInRange()
    {
        byte maxDistance = 3;


        //Check if the object is within a certain distance of the player on the x and z axis.
        return (transform.position.x <= _target.transform.position.x + maxDistance && transform.position.x >= _target.transform.position.x - maxDistance && transform.position.z <= _target.transform.position.z + maxDistance && transform.position.z >= _target.transform.position.z - maxDistance);
    }

    private IEnumerator Attack()
    {

        //Look at the player and set every bit of velocity to 0 before applying the force to the object and disable their navmesh agent.
        transform.LookAt(_target.transform);

        _agent.enabled = false;

        _rb.linearVelocity = Vector3.zero;

        _rb.angularVelocity = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        //Apply force.
        _agent.enabled = false;

        _rb.AddRelativeForce(new Vector3(0, 3, 2) * 25, ForceMode.Impulse);

        yield return new WaitForSeconds(3f);

        //Re-enable and reset velocity again.
        _agent.enabled = true;

        _rb.linearVelocity = Vector3.zero;

        _rb.angularVelocity = Vector3.zero;

        transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);

        _attacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(_healthScript.EnemyObject.Damage);
        }
    }

}
