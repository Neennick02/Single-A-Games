using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    [Header("Parent")]

    [SerializeField] protected EnemyObject enemyObject;

    public enum EnemyState
    {
        Moving,
        Attacking,
        Fleeing,
        Dead,
    }

    public EnemyState CurrentState;

    protected NavMeshAgent _agent;
    protected NavMeshPath _path;
    protected Rigidbody _rb;

    protected GameObject _target;
    protected float _radius = 10f;


    private GameObject _player;
    private GameObject _buddy;

    private float _buddyCheckInterval = 3f;
    private float timer = 10;
    protected Vector3 currentTarget;
    protected float _spottingRadius = 10f;
    protected float startY;

    protected EnemyHealth _healthScript;
    protected bool _isWandering;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.avoidancePriority = UnityEngine.Random.Range(20, 80);

        _path = new NavMeshPath();
        startY = transform.position.y;

        _rb = GetComponent<Rigidbody>();

        _player = PlayerMotor.Instance.gameObject;
        _target = _player;

        _healthScript = GetComponent<EnemyHealth>();

        CurrentState = EnemyState.Moving;
        FindTargetLocation();
        MyStart();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= _buddyCheckInterval)
        {
            BuddyBehaviour script = FindFirstObjectByType<BuddyBehaviour>();
            if (script != null)
                _buddy = script.gameObject;

            timer = 0f;
        }
        MyUpdate();
    }

    protected virtual void MyStart() { }


    protected virtual void MyUpdate() 
    {
        AssignTarget();
    }

    public virtual void Die()
    {
        CurrentState = EnemyState.Dead;
    }

    protected virtual bool CheckIfInRange()
    {
        //Check if the object is within a certain distance of the player on the x and z axis.
        return (transform.position.x <= _target.transform.position.x + enemyObject.StoppingDistance &&
            transform.position.x >= _target.transform.position.x - enemyObject.StoppingDistance &&
            transform.position.z <= _target.transform.position.z + enemyObject.StoppingDistance &&
            transform.position.z >= _target.transform.position.z - enemyObject.StoppingDistance);
    }

    protected virtual void AssignTarget()
    {
        GameObject best = _player;

        if (_buddy != null)
        {
            float playerDist = Vector3.Distance(transform.position, _player.transform.position);
            float buddyDist = Vector3.Distance(transform.position, _buddy.transform.position);

            if (buddyDist < playerDist || !CanSeeTarget(_target))
                best = _buddy;
        }

        _target = best;
    }

    protected void FindTargetLocation()
    {
        Vector3 randomPos = transform.position + UnityEngine.Random.insideUnitSphere * _radius;
        randomPos.y = startY;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, _radius, NavMesh.AllAreas))
        {
            NavMeshPath path = new NavMeshPath();
            if (_agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                currentTarget = hit.position;
                _agent.SetDestination(currentTarget);
                _isWandering = true;
                return;
            }
        }

        currentTarget = transform.position;
    }
    protected void HandleWandering()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= 0.2f)
        {
            FindTargetLocation();
        }
    }

    protected bool CanSeeTarget(GameObject target)
    {
        if (target == null) return false;

        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > _spottingRadius) return false;

        Vector3 dir = target.transform.position - transform.position;

        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, _spottingRadius))
            return hit.collider.gameObject == target;

        return false;
    }
}
