using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBehaviour : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public float DamageAmount = 1;

    public float PlayerStoppingDistance = 3f;
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
    public List<GameObject> EnemiesSpotted = new List<GameObject>();
    private void Start()
    {
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

                FollowPlayer();
                break;
                
            case BuddyStates.ChasingEnemy:
                ChaseEnemy();
                break;

            case BuddyStates.Recharging:
                Recharge();

                break;
        }


        if (!changeHeight && (State == BuddyStates.Following || State == BuddyStates.ChasingEnemy))
        {
            Vector3 pos = transform.position;
            pos.y = DefaultHeight + Mathf.PingPong(Time.time * 0.5f, 0.2f);
            transform.position = pos;
        }
    }
    private void FollowPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > PlayerStoppingDistance)
        {
            MoveTowards(player.position);
        }
    }

    private void ChaseEnemy()
    {
        if (closestEnemy == null)
        {
            EnemiesSpotted.RemoveAll(e => e == null);
            closestEnemy = FindClosestTarget();
        }

        if (closestEnemy == null)
        {
            State = BuddyStates.Following;
            return;
        }

        float dist = Vector3.Distance(transform.position, closestEnemy.transform.position);

        if (dist > EnemyStoppingDistance)
        {
            MoveTowards(closestEnemy.transform.position);
        }
    }

    private void Recharge()
    {
        if (!recharging)
        {
            StartCoroutine(RestRoutine(RestHeight));
            recharging = true;
        }

        regenTimer += Time.deltaTime;

        if (regenTimer > regenInterval)
        {
            health.Heal(1f);
            regenTimer = 0f;
        }
    }
    private void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
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
            if(State != BuddyStates.ChasingEnemy &&
                State != BuddyStates.Recharging)
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

            if (EnemiesSpotted.Count < 1 && State != BuddyStates.Recharging)
            {
                State = BuddyStates.Following;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //dont deal damage when recharging
        if (State == BuddyStates.Recharging) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();

            if(health == null)
            {
                health = collision.gameObject.GetComponentInParent<EnemyHealth>();
            }
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

    public IEnumerator RestRoutine(float targetHeight)
    {
        changeHeight = true;
        float timer = 0;
        float duration = 1f;

        float startHeight = transform.position.y;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            Vector3 pos = transform.position;
            pos.y = Mathf.Lerp(startHeight, targetHeight, timer / duration);
            transform.position = pos;

            yield return null;
        }
        changeHeight = false;
    }
}
