using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EnemyObject EnemyObject;

    public List<Renderer> MeshRenderers;
    private Material _startMat;
    [SerializeField] private Material _damageMat;
    [SerializeField] private EnemyHealthBar _healthBar;
    public static event Action<float> OnRestoreSanity;

    public GameObject _deathParticle;

    public GameObject BlobPrerfab;

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
        currentHealth -= amount;

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(RevealFlesh(amount));
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

        Instantiate(_deathParticle, transform.position, Quaternion.identity);

        GameManager.Instance.AddKill();

    }

    private IEnumerator FlashRed()
    {
        Debug.Log(_damageMat);
        MeshRenderers[0].material = _damageMat;
        yield return new WaitForSeconds(0.1f);
        MeshRenderers[0].material = _startMat;
    }

    private IEnumerator RevealFlesh(float strength)
    {

        float normalizedStrength = strength / EnemyObject.MaxHealth;

        Dissolver dissolve = GetComponent<Dissolver>();
        dissolve.DissolveBasedOnDamage(normalizedStrength);
        yield return new WaitForSeconds(0.1f);
    }
    IEnumerator FadeOutAndDie()
    {
        Dissolver dissolve = GetComponent<Dissolver>();
        dissolve.StartDissolve();

        yield return new WaitForSeconds(0.5f);
        OnRestoreSanity?.Invoke(EnemyObject.SanityRestoreAmount);
        for (int i = 0; i < UnityEngine.Random.Range(1, 3); i++)
        {
            Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 0.1f;
            Instantiate(BlobPrerfab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
