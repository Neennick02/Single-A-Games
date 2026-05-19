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

    public List<EnemyHealth> EnemyList = new List<EnemyHealth>();

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
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if( EnemyList[i] == null ) EnemyList.RemoveAt(i);

            Debug.Log("Boom" + EnemyList[i].name);

            EnemyList[i].TakeDamage(_damage);
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

            EnemyList.Add(current);
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

            EnemyList.Remove(current);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth current = collision.gameObject.GetComponentInChildren<EnemyHealth>();
            if (current == null)
            {
                current = collision.gameObject.GetComponentInParent<EnemyHealth>();
            }

            if(current != null) 
            current.TakeDamage(1f);
        }
    }
}
