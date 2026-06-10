using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public GameObject SettingsObject;
    public GameObject HomeObject;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void OpenScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OpenSettings()
    {
        SettingsObject.SetActive(true);
        HomeObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void CloseSettings()
    {
        SettingsObject.SetActive(false);
        HomeObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}
