using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : BaseHealth
{
    public PlayerObject PlayerObject;

    public static event Action<float> OnHealthChange;
    public static event Action OnDeath;
    [SerializeField] private Volume _damageEffect;
    [SerializeField] private float _effectDissolveSpeed = 1;
    #region OnEnable
    private void OnEnable()
    {
        SanityManager.OnTakeSanityDamage += TakeDamage;
    }

    private void OnDisable()
    {
        SanityManager.OnTakeSanityDamage -= TakeDamage;
    }
    #endregion
    private void Start()
    {
        maxHealth = PlayerObject.MaxHealth;
        currentHealth = maxHealth;
    }
    public override void TakeDamage(float amount)
    {
        //add vignette effect
        _damageEffect.weight += amount * 0.02f;

        currentHealth -= amount;
        OnHealthChange?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    private void Update()
    {
        if(currentHealth >  maxHealth / 3 && _damageEffect.weight > 0)
        {
            _damageEffect.weight -= _effectDissolveSpeed * Time.deltaTime;
            if (_damageEffect.weight <= 0) _damageEffect.weight = 0;
        }
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
        Cursor.lockState = CursorLockMode.None;
        OnDeath?.Invoke();
    }
}
