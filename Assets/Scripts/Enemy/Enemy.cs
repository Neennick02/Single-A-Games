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

    protected EnemyHealth _healthScript;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _rb = GetComponent<Rigidbody>();

        _target = FindFirstObjectByType<PlayerMotor>().gameObject;

        _healthScript = GetComponent<EnemyHealth>();

        CurrentState = EnemyState.Moving;

        MyStart();
    }

    private void Update()
    {

        MyUpdate();

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
}
