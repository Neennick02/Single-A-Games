using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [SerializeField] private List<GameObject> _enemies;

    [SerializeField] private int _amount;

    private void Start()
    {

        for (int i = 0; i < _amount; i++)
        {

            int _Random = Random.Range(0, 2);

            if (_Random > 0)
            {

                int _RandomEnemy = Random.Range(0, _enemies.Count);

                Instantiate(_enemies[_RandomEnemy], transform.position, Quaternion.identity, gameObject.transform);
            }
        }

    }
}
