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

    protected Rigidbody _rb;

    protected GameObject _target;
    private GameObject _player;
    private GameObject _buddy;

    private float _buddyCheckInterval = 3f;
    private float timer = 10;


    protected EnemyHealth _healthScript;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _rb = GetComponent<Rigidbody>();

        _player = PlayerMotor.Instance.gameObject;
        _target = _player;

        _healthScript = GetComponent<EnemyHealth>();

        CurrentState = EnemyState.Moving;

        MyStart();
    }

    private void Update()
    {

        MyUpdate();


        timer += Time.deltaTime;

        if(timer >= _buddyCheckInterval && _buddy == null)
        {
            BuddyBehaviour script = FindFirstObjectByType<BuddyBehaviour>();
            if(script != null)
            {
                _buddy = script.gameObject;
            }
            timer = 0f;
        }

    }

    protected virtual void MyStart() { }


    protected virtual void MyUpdate() { }

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
        if (_buddy == null) _target = _player;

        float playerDistance = Vector3.Distance(transform.position, _player.transform.position);
        float buddyDistance = Vector3.Distance(transform.position, _buddy.transform.position);

        if (playerDistance < buddyDistance) _target = _player;
        else if (buddyDistance < buddyDistance) _target = _buddy;        
    }
}
