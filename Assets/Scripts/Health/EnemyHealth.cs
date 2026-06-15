using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EnemyObject EnemyObject;

    public List<Renderer> MeshRenderers;
    private Material _startMat;
    [SerializeField] private Material _damageMat;
    [SerializeField] private EnemyHealthBar _healthBar;
    public static event Action<float> OnRestoreSanity;

    private void Start()
    {
        maxHealth = EnemyObject.MaxHealth;
        currentHealth = maxHealth;


        for (int i = 0; i < MeshRenderers.Count; i++)
        {
            _startMat = MeshRenderers[i].material;
        }
    }

    public override void TakeDamage(float amount)
    {
        Debug.Log(amount);
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
        MeshRenderers[0].material = _damageMat;
        yield return new WaitForSeconds(0.1f);
        MeshRenderers[0].material = _startMat;
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
