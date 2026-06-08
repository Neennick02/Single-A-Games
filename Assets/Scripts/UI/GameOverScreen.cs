using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject GameOverScreenObject;
    public static event Action OnReset;
    private void OnEnable()
    {
        PlayerHealth.OnDeath += EnableGameOverScreen;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDeath -= EnableGameOverScreen;
    }

    private void EnableGameOverScreen()
    {
        GameOverScreenObject.SetActive(true);
    }
    public void Restart()
    {
        OnReset?.Invoke();
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToTitle()
    {
        OnReset?.Invoke();
        SceneManager.LoadScene("StartScene");
    }
}
