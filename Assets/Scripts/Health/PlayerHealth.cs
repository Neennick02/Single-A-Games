using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : BaseHealth
{
    public PlayerObject PlayerObject;

    public static event Action<float> OnHealthChange;
    public static event Action OnDeath;
    [SerializeField] private Volume _damageEffect;
    private float _effectDissolveSpeed = 1;

    [SerializeField] private CameraShakeManager _cameraShakeManager;

    public static event Action<float> OnSetRunTime;
    public static event Action<int> OnSetFloorCount;

    private void Start()
    {
        _cameraShakeManager = FindFirstObjectByType<CameraShakeManager>();
    }
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
        //add vignette effect
        _damageEffect.weight = 1;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        OnHealthChange?.Invoke(currentHealth);

        _cameraShakeManager.CameraShake(gameObject, 0.7f);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }

    private void Update()
    {
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
