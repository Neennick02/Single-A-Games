using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    protected float currentHealth;
    protected float maxHealth;
    public virtual void TakeDamage(int amount)
    {

    }

    public virtual void Heal(int amount)
    {

    }

    protected virtual void Die()
    {

    }
}
