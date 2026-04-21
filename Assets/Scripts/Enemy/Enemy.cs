using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent _agent;

    protected Rigidbody _rb;

    protected GameObject _target;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _rb = GetComponent<Rigidbody>();

        _target = FindFirstObjectByType<PlayerMotor>().gameObject;

        MyStart();
    }

    private void Update()
    {

        MyUpdate();

    }

    protected virtual void MyStart() { }


    protected virtual void MyUpdate() { }
}
