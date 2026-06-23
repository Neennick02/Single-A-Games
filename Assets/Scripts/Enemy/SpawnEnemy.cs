using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public static List<GameObject> Enemies = new List<GameObject>();
    [SerializeField] private int _amount;

    private List<Transform> SpawnPositions = new List<Transform>();
    private List<Transform> AvailablePositions;

    private void Start()
    {
        //add spawn points
        foreach(Transform child in transform)
        {
            SpawnPositions.Add(child);
        }

        //copy the list
        AvailablePositions = new List<Transform>(SpawnPositions);

        for (int i = 0; i < SpawnPositions.Count; i++)
        {

            int _Random = Random.Range(0, 2);

            if (_Random > 0 && AvailablePositions.Count > 0)
            {

                int _RandomEnemy = Random.Range(0, Enemies.Count);

                int currentPos = Random.Range(0, AvailablePositions.Count);

                if (Enemies[_RandomEnemy] != null)
                {
                    Instantiate(Enemies[_RandomEnemy],
                        SpawnPositions[currentPos].position,
                        Quaternion.identity,
                        gameObject.transform);

                    AvailablePositions.RemoveAt(currentPos);
                }
            }
        }
    }
}
