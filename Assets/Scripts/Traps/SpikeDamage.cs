using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float DamageAmount = 1f;
    public float ResetDur = 1f;
    private float timer;

    public List<GameObject> ObjectsInRange = new List<GameObject>();

    private void Start()
    {
        timer = ResetDur;
    }
    private void Update()
    {
        if(ObjectsInRange.Count > 0)
        {
            timer += Time.deltaTime;

            if(timer >= ResetDur)
            {
                DealDamage();
                timer = 0;
            }
        }
    }

    private void DealDamage()
    {
        for(int i = 0; i < ObjectsInRange.Count; i++)
        {
            GameObject obj = ObjectsInRange[i];

            PlayerHealth Phealth = obj.GetComponent<PlayerHealth>();
            EnemyHealth  Ehealth = obj.GetComponent<EnemyHealth>();

            if(Phealth != null)
            {
                Phealth.TakeDamage(DamageAmount);
            }
            else if(Ehealth != null)
            {
                Ehealth.TakeDamage(DamageAmount);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") ||
            other.gameObject.CompareTag("Enemy"))
        {
            ObjectsInRange.Add(other.gameObject);
            DealDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ObjectsInRange.Contains(other.gameObject))
        {
            ObjectsInRange.Remove(other.gameObject);
        }
    }
}
