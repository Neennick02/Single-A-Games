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

    public static event Action<float> OnHealthAmountChange;


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

    private void Update()
    {
        OnHealthAmountChange?.Invoke(currentHealth);
    }
    public override void TakeDamage(float amount)
    {
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        currentHealth -= amount;
    }

    public override void Heal(float amount)
    {
        if (currentHealth >= maxHealth) return;

        currentHealth += amount;
    }

    protected override void Die()
    {
        Destroy(gameObject);
        Debug.Log(transform.name + " died");
    }

    private IEnumerator FlashRed()
    {
        _meshRenderer.material = _damageMat;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material = _startMat;
    }
}
