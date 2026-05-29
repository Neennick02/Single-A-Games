using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class BuddyBehaviour : MonoBehaviour
{
    NavMeshAgent agent;
    public float DamageAmount = 1;

    private bool _targeting;
    public float PlayerStoppingDistance = 1f;
    public float EnemyStoppingDistance = 0f;
    private Transform player;
    private GameObject closestEnemy;

    public float DefaultHeight;
    public float RestHeight;

    private bool recharging = false;
    private float regenInterval = 1f;
    private float regenTimer = 0f;
    private BuddyHealth health;

    private bool changeHeight;
    public enum BuddyStates
    {
        Following,
        ChasingEnemy,
        Recharging
    }

    public BuddyStates State = BuddyStates.Following;
    public List<GameObject> EnemiesSpotted;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<BuddyHealth>();
        player = PlayerMotor.Instance.transform;
        closestEnemy = null;
    }

    private void Update()
    {
        switch (State)
        {
            case BuddyStates.Following:

                if (recharging)
                {
                    StartCoroutine(RestRoutine(DefaultHeight));
                    recharging = false;
                }

                agent.SetDestination(player.position);
                break;
                
            case BuddyStates.ChasingEnemy:
                if (closestEnemy == null)
                {
                    EnemiesSpotted.RemoveAll(GameObject => GameObject == null);
                    closestEnemy = FindClosestTarget();
                }
                else
                {
                    agent.SetDestination(closestEnemy.transform.position);
                }
                break;

            case BuddyStates.Recharging:

                if (!recharging)
                {
                    StartCoroutine(RestRoutine(RestHeight));
                    recharging = true;
                }
                regenTimer += Time.deltaTime;

                if(regenTimer > regenInterval)
                {
                    health.Heal(1f);
                    regenTimer = 0f;
                }

                break;
        }


        if (!changeHeight &&
            (State == BuddyStates.Following ||
            State == BuddyStates.ChasingEnemy))
        {
            agent.baseOffset = DefaultHeight + Mathf.PingPong(Time.time, 0.2f);
        }
    }

    private GameObject FindClosestTarget()
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

        return target;   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(State != BuddyStates.ChasingEnemy)
            {
                State = BuddyStates.ChasingEnemy;
            }

            EnemiesSpotted.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && EnemiesSpotted.Contains(other.gameObject))
        {
            EnemiesSpotted.Remove(other.gameObject);
            if (EnemiesSpotted.Count < 1)
            {
                State = BuddyStates.Following;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
            health.TakeDamage(DamageAmount);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            health.TakeDamage(DamageAmount);
        }
    }

    public void SetState(BuddyStates state)
    {
        State = state;
    }

    public IEnumerator RestRoutine(float targetOffset)
    {
        changeHeight = true;
        float timer = 0;
        float duration = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            agent.baseOffset = Mathf.Lerp(agent.baseOffset, targetOffset, timer / duration);

            yield return null;
        }
        agent.baseOffset = targetOffset;
        changeHeight = false;
    }
}
