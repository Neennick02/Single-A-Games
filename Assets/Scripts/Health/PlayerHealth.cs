using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public PlayerObject PlayerObject;

    public static event Action<float> OnHealthChange;

    private void Start()
    {
        maxHealth = PlayerObject.MaxHealth;
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
        OnHealthChange?.Invoke(currentHealth);
    }

    public override void Heal(int amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;
        OnHealthChange?.Invoke(currentHealth);
    }

    protected override void Die()
    {
        Debug.Log("Player is dead");
    }
}
