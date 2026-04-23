using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EnemyObject EnemyObject;

    private void Start()
    {
        maxHealth = EnemyObject.MaxHealth;
        currentHealth = maxHealth;
    }
    public override void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        currentHealth -= amount;
    }

    public override void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;
    }

    protected override void Die()
    {
        Debug.Log(transform.name + " died");
    }
}
