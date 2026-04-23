using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{

    [SerializeField] protected float currentHealth;
    protected float maxHealth;
    public virtual void TakeDamage(float amount)
    {

    }

    public virtual void Heal(float amount)
    {

    }

    protected virtual void Die()
    {

    }
}
