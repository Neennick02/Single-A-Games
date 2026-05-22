using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public GameObject Panel;

    private void OnEnable()
    {
        InputManager.OnPause += Pause;
    }

    private void OnDisable()
    {
        InputManager.OnPause -= Pause;
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Panel.SetActive(true);
    }
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Panel.SetActive(false);
    }
    public void ReturnToTitle()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("StartScene");
    }
}
