using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Image _healthBar;
    [SerializeField] private EnemyObject _enemyObject;
    private void OnEnable()
    {
        EnemyHealth.OnHealthAmountChange += UpdateHealth;
        _healthBar = GetComponent<Image>();
    }

    private void OnDisable()
    {
        EnemyHealth.OnHealthAmountChange -= UpdateHealth;
    }

    private void UpdateHealth(float amount)
    {
        _healthBar.fillAmount = amount / _enemyObject.MaxHealth;

    }
}
