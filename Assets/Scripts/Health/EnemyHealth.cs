using System.Collections;
using UnityEngine;

public class EnemyHealth : BaseHealth
{
    public EnemyObject EnemyObject;

    private MeshRenderer _meshRenderer;

    private void Start()
    {
        maxHealth = EnemyObject.MaxHealth;
        currentHealth = maxHealth;
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
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
        _meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = Color.white;
    }
}
