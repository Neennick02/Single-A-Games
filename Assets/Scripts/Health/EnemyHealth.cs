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
        if (currentHealth <= 0)
        {
            Die();
            //return;
        }


        currentHealth -= amount;
        if (currentHealth > 0)
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
        //Destroy(gameObject);
        Debug.Log(transform.name + " died");
    }

    private IEnumerator FlashRed()
    {
        _meshRenderer.material = _damageMat;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material = _startMat;
    }
}
