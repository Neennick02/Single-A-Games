using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public static event Action OnSwitchScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnSwitchScene?.Invoke();
            SceneManager.LoadScene("MainScene");
        }
    }
}
