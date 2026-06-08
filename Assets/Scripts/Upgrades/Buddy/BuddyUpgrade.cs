using UnityEngine;

public class BuddyUpgrade : MonoBehaviour
{
    public GameObject BuddyPrefab;

    private void OnEnable()
    {
        SceneLoader.OnMainSceneLoad += SpawnBuddy;
    }

    private void OnDisable()
    {
        SceneLoader.OnMainSceneLoad -= SpawnBuddy;
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
}
