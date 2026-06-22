using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private PlayerObject PlayerData;
    private Image _healthBar;
    private float currentH;
    private void OnEnable()
    {
        _healthBar = GetComponent<Image>();

        PlayerHealth.OnHealthChange += UpdateHealth;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        PlayerHealth.OnHealthChange -= UpdateHealth;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateHealth(float current)
    {
        float fillAmount = current / PlayerData.MaxHealth;
        currentH = current;
        _healthBar.fillAmount = fillAmount;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && PlayerMotor.Instance != null)
        {
            UpdateHealth(PlayerMotor.Instance.GetComponent<PlayerHealth>().GetHealth());
        }
    }
}
