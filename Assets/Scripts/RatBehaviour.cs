using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using System.Collections;
public class RatBehaviour : MonoBehaviour
{
    private NavMeshAgent _agent;
    private NavMeshPath _navMeshPath;
    private bool _isDead;
    [SerializeField] private Animator _animator;

    [SerializeField] private Vector3 currentTarget;
    [SerializeField] private float radius = 10f;
    private float startY;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _navMeshPath = new NavMeshPath();

        startY = transform.position.y;

        FindTargetLocation();
    }

    private void Update()
    {
        if(Vector3.Distance(currentTarget, transform.position) < 0.1f)
        {
            FindTargetLocation();
        }

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void FindTargetLocation()
    {
            currentTarget = transform.position + Random.insideUnitSphere * radius;
            currentTarget.y = startY;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(currentTarget, out hit, radius, NavMesh.AllAreas))
        {
            currentTarget = hit.position;
            _agent.SetDestination(currentTarget);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isDead) return;
        if (other.gameObject.CompareTag("Player"))
        {
            _isDead = true;
            _animator.SetTrigger("Dead");
            _agent.ResetPath();
            StartCoroutine(WaitForDeath());
        }
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

}
