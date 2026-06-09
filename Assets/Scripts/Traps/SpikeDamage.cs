using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float DamageAmount = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();

            health.TakeDamage(DamageAmount);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {

            EnemyHealth health = other.gameObject.GetComponent<EnemyHealth>();

            health.TakeDamage(DamageAmount);
        }
    }
}
