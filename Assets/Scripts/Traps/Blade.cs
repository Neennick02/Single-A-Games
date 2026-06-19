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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = collision.gameObject.GetComponentInParent<PlayerHealth>();
            
            if(health != null)
            {
                health.TakeDamage(Damage);
            }
        }
    }
}
