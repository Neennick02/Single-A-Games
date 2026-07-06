using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : BaseHealth
{
    public PlayerObject PlayerObject;

    public static event Action<float> OnHealthChange;
    public static event Action OnDeath;
    [SerializeField] private Volume _damageEffect;
    private float _effectDissolveSpeed = 1;

    public static event Action<float> OnSetRunTime;
    public static event Action<int> OnSetFloorCount;
    [SerializeField] private List<AudioClip> damageClips;
    public AudioClip DeathClip;
    private bool isdead;
    #region OnEnable
    private void OnEnable()
    {
        maxHealth = PlayerObject.MaxHealth;
        currentHealth = maxHealth;

        SanityManager.OnTakeSanityDamage += TakeDamage;
        GameManager.OnGameStart += StartGame;
    }

    private void OnDisable()
    {
        SanityManager.OnTakeSanityDamage -= TakeDamage;
        GameManager.OnGameStart -= StartGame;
    }
    #endregion
    public override void TakeDamage(float amount)
    {
        if (isdead) return;
        //add vignette effect
        _damageEffect.weight = 1;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChange?.Invoke(currentHealth);

        CameraShakeManager.Instance.CameraShake(gameObject, 0.7f);

        if (currentHealth <= 0)
        {
            Die();
            isdead = true;
            return;
        }

            AudioManager.Instance.PlayClips(damageClips, 1, UnityEngine.Random.Range(0.8f, 1.1f));
    }

    private void Update()
    {
        if(isdead ) return;
        if (_damageEffect.weight > 0)
        {
            _damageEffect.weight -= Time.deltaTime * _effectDissolveSpeed;
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
        Cursor.lockState = CursorLockMode.None;
        OnDeath?.Invoke();
        AudioManager.Instance.PlayClip(DeathClip, 1, UnityEngine.Random.Range(0.8f, 1.1f));
        OnSetRunTime?.Invoke(GameManager.Instance.RunTime);
        OnSetFloorCount?.Invoke(GameManager.Instance.FloorCount);
    }

    private void StartGame()
    {
        currentHealth = maxHealth;
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
