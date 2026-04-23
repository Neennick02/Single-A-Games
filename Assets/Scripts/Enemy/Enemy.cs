using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{

    [Header("Parent")]

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

        MyStart();
    }

    private void Update()
    {

        MyUpdate();

    }

    protected virtual void MyStart() { }


    protected virtual void MyUpdate() { }
}
