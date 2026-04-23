using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : BaseHealth
{
    public PlayerObject PlayerObject;

    public static event Action<float> OnHealthChange;

    [SerializeField] private Volume _damageEffect;

    private void Start()
    {
        maxHealth = PlayerObject.MaxHealth;
        currentHealth = maxHealth;

    }
    public override void TakeDamage(float amount)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        Debug.Log(amount * 0.1f);

        _damageEffect.weight += amount * 0.05f;

        Debug.Log(_damageEffect.weight);

        currentHealth -= amount;
        OnHealthChange?.Invoke(currentHealth);
    }

    public override void Heal(float amount)
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
