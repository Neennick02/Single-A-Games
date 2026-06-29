using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BuddyUpgrade : MonoBehaviour
{
    public GameObject BuddyPrefab;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.Instance.SetBuddy(true);
    }

    private void OnDisable()
    {
        SceneLoader.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SpawnBuddy(Camera.main.transform.position + new Vector3(0, 0, 2));
    }

    private void SpawnBuddy(Vector3 pos)
    {

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
        }

        GameObject buddy = Instantiate(BuddyPrefab, pos, Quaternion.identity);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && GameManager.Instance.CheckBuddy())
            SpawnBuddy(new Vector3(0, -15, 0));
    }
}
