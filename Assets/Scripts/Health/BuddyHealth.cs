using UnityEngine;
using UnityEngine.UI;

public class BuddyHealth : BaseHealth
{
    private BuddyBehaviour _behaviour;
    [SerializeField] private Image healthBarImage;

    private void Start()
    {
        _behaviour = GetComponent<BuddyBehaviour>();
        maxHealth = 10f;
        currentHealth = maxHealth;
    }
    private void Update()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }

    public override void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0 )
        {
            currentHealth = 0;
            Die();
        }
    }
    public override void Heal(float amount)
    {
        currentHealth += amount;
        if( currentHealth == maxHealth )
        {
            currentHealth = maxHealth;
            _behaviour.SetState(BuddyBehaviour.BuddyStates.Following);
        }
    }

    protected override void Die()
    {
        _behaviour.SetState(BuddyBehaviour.BuddyStates.Recharging);
    }
}
