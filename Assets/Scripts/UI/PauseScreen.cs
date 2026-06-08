using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject Panel;
    private bool _isPaused;
    public static event Action OnReset;

    private void OnEnable()
    {
        InputManager.OnPause += TogglePause;
    }

    private void OnDisable()
    {
        InputManager.OnPause -= TogglePause;
    }

    private void TogglePause()
    {
        if( _isPaused)
        {
            Continue();
        }
        else
        {
            Pause();
        }
        _isPaused = !_isPaused;

    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        Panel.SetActive(true);
    }
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Panel.SetActive(false);
        Time.timeScale = 1;
    }
    public void ReturnToTitle()
    {
        OnReset?.Invoke();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("StartScene");
    }
}
