using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject GameOverScreenObject;
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
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("StartScene");
    }
}
