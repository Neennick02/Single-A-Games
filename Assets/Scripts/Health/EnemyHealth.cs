using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EnemyObject EnemyObject;

    private MeshRenderer _meshRenderer;
    private Material _startMat;
    [SerializeField] private Material _damageMat;
    [SerializeField] private EnemyHealthBar _healthBar;
    public static event Action<float> OnRestoreSanity;

    private void Start()
    {
        maxHealth = EnemyObject.MaxHealth;
        currentHealth = maxHealth;

        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        if(_meshRenderer == null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        _startMat = _meshRenderer.material;
    }

    public override void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashRed());
        }

        _healthBar.UpdateHealth(currentHealth);
    }

    public override void Heal(float amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;
        _healthBar.UpdateHealth(currentHealth);
    }

    protected override void Die()
    {
        StartCoroutine(FadeOutAndDie());
    }

    private IEnumerator FlashRed()
    {
        _meshRenderer.material = _damageMat;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material = _startMat;
    }
    IEnumerator FadeOutAndDie()
    {
        Dissolver dissolve = GetComponent<Dissolver>();
        dissolve.StartDissolve();

        yield return new WaitForSeconds(0.5f);
        OnRestoreSanity?.Invoke(EnemyObject.SanityRestoreAmount);
        Destroy(gameObject);
    }
}
