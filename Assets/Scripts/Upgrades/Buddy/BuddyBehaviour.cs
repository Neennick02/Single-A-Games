using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuddyBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;
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
    public Animator animator;
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
        _agent = GetComponent<NavMeshAgent>();
        _agent.baseOffset = DefaultHeight;
        health = GetComponent<BuddyHealth>();
        player = PlayerMotor.Instance.transform;
        closestEnemy = null;
    }

    private void Update()
    {
        animator.SetFloat("Speed", _agent.velocity.magnitude);


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
            _agent.baseOffset = DefaultHeight + Mathf.PingPong(Time.time * 0.5f, 0.2f);
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
        _agent.speed = MoveSpeed;
        _agent.stoppingDistance = (State == BuddyStates.Following)    ? PlayerStoppingDistance : EnemyStoppingDistance;

        _agent.SetDestination(target);
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
            animator.SetTrigger("Attack");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();
            if (health == null)
            {
                health = collision.gameObject.GetComponentInParent<PlayerHealth>();
            }
            health.TakeDamage(DamageAmount);
            animator.SetTrigger("Attack");
        }
    }

    public void SetState(BuddyStates state)
    {
        State = state;
    }

    public IEnumerator RestRoutine(float targetHeight)
    {
        animator.SetTrigger("Die");
        changeHeight = true;
        float timer = 0;
        float duration = 1f;

        float startHeight = _agent.baseOffset;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            _agent.baseOffset = Mathf.Lerp(startHeight, targetHeight, timer / duration);
            yield return null;
        }

        changeHeight = false;
    }
}
