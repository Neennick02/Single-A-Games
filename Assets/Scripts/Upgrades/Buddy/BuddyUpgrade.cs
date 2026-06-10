using UnityEngine;
using UnityEngine.SceneManagement;

public class BuddyUpgrade : MonoBehaviour
{
    public GameObject BuddyPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneLoader.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SpawnBuddy();
    }

    private void SpawnBuddy()
    {
        Vector3 pos = Camera.main.transform.position + new Vector3(0, 0, 2);

        GameObject buddy = Instantiate(BuddyPrefab, pos, Quaternion.identity);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
            SpawnBuddy();
    }
}
