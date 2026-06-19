using UnityEngine;

public class Blade : MonoBehaviour
{
    private Animator animator;
    public float Speed = 1f;
    public float Damage = 1f;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = other.gameObject.GetComponentInParent<PlayerHealth>();
            Debug.Log(health);
            if (health != null)
            {
                health.TakeDamage(Damage);
            }
        }
    }
}
