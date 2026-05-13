using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private Image _healthBar;
    [SerializeField] private EnemyObject _enemyObject;
    private void Start()
    {
        _healthBar = GetComponent<Image>();
    }

    public void UpdateHealth(float amount)
    {
        _healthBar.fillAmount = amount / _enemyObject.MaxHealth;

    }
}
