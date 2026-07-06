using UnityEngine;

public class MeleeArm : MonoBehaviour
{
    [SerializeField] private EnemyObject enemyObject;
    private float attackInterval = 1f;
    private float timer;
    private void Update()
    {

            timer += Time.deltaTime;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && timer >= attackInterval)
        {
            timer = 0f;

            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if(health == null)
            {
                health = other.GetComponentInParent<PlayerHealth>();
            }
            health.TakeDamage(enemyObject.Damage);
        }
    }
}
