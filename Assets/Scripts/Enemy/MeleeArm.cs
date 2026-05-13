using UnityEngine;

public class MeleeArm : MonoBehaviour
{
    [SerializeField] private EnemyObject enemyObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            health.TakeDamage(enemyObject.Damage);
        }
    }
}
