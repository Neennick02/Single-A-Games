using UnityEngine;
using UnityEngine.AI;
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

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out hit, 1f, NavMesh.AllAreas))
        {
            pos = hit.position;
        }
        else
        {
            if (NavMesh.SamplePosition(pos, out hit, 5f, NavMesh.AllAreas))
            {
                pos = hit.position;
            }
            else
            {
                pos = Camera.main.transform.position;
                Debug.Log("To far");
            }
        }

        GameObject buddy = Instantiate(BuddyPrefab, pos, Quaternion.identity);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
            SpawnBuddy();
    }
}
