using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _detonationTime = 3f;
    [SerializeField] private float _damage = 1;

    private float _timer = 0;
    private Collider _rangeCollider;

    private List<EnemyHealth> _enemyList = new List<EnemyHealth>();

    private void Start()
    {
        _rangeCollider = GetComponent<Collider>();
    }
    private void Update()
    {
        _timer += Time.deltaTime;

        if( _timer > _detonationTime)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Debug.Log("Boom");

        for (int i = 0; i < _enemyList.Count; i++)
        {
            _enemyList[i].TakeDamage(_damage);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth current = other.GetComponent<EnemyHealth>();
            if(current == null)
            {
                current = other.GetComponentInChildren<EnemyHealth>();
            }

            _enemyList.Add(current);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth current = other.GetComponent<EnemyHealth>();
            if (current == null)
            {
                current = other.GetComponentInChildren<EnemyHealth>();
            }

            _enemyList.Remove(current);
        }
    }
}
