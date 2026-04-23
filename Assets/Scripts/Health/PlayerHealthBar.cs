using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerObject PlayerData;
    private Image _healthBar;

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

    private void UpdateHealth(float amount)
    {
        float fillAmount = amount / PlayerData.MaxHealth;
        _healthBar.fillAmount = fillAmount;
    }
}
