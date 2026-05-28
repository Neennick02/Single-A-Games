using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerObject PlayerData;
    private Image _healthBar;
    private float _prevHealth;
    private void OnEnable()
    {
        PlayerHealth.OnHealthChange += UpdateHealth;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= UpdateHealth;
    }
    private void Start()
    {
        _healthBar = GetComponent<Image>();
    }

    private void UpdateHealth(float current)
    {
        float fillAmount = current / PlayerData.MaxHealth;
        _healthBar.fillAmount = fillAmount;
    }
}
