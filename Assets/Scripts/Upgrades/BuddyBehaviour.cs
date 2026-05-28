using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public float DamageAmount = 1;


    private bool _targeting;

    public List<GameObject> EnemiesSpotted;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
       

        if(EnemiesSpotted.Count > 0)
        {
            _targeting = true;
        }
        else
        {
            _targeting = false;
        }

        if (_targeting)
        {
            agent.SetDestination(FindClosestTarget());
        }
        else
        {
            agent.SetDestination(PlayerMotor.Instance.transform.position);
        }
    }

    private Vector3 FindClosestTarget()
    {
        float distance = Mathf.Infinity;
        GameObject target = null;

        for (int i = 0; i < EnemiesSpotted.Count; i++)
        {
            float temp = Vector3.Distance(transform.position, EnemiesSpotted[i].gameObject.transform.position);

            if(temp < distance)
            {
                distance = temp;
                target = EnemiesSpotted[i].gameObject;
            }
        }
        return target.transform.position;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemiesSpotted.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && EnemiesSpotted.Contains(other.gameObject))
        {
            EnemiesSpotted.Remove(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
            health.TakeDamage(DamageAmount);
        }
    }
}
