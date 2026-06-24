using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float DamageAmount = 1f;
    public float ResetDur = 1f;
    private float timer;
    public AudioClip DamageClip;

    private Collider hitxBox;

    [SerializeField] private List<GameObject> ObjectsInRange;
    private void Start()
    {
        timer = ResetDur;
        hitxBox = GetComponent<Collider>();
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

        if (!hitxBox.enabled)
        {
            ObjectsInRange.Clear();
        }
    }

    private void DealDamage()
    {
        for(int i = 0; i < ObjectsInRange.Count; i++)
        {
            GameObject obj = ObjectsInRange[i];

            if (obj != null)
            {
                PlayerHealth Phealth = obj.GetComponent<PlayerHealth>();

                if (Phealth != null)
                {
                    Phealth.TakeDamage(DamageAmount);
                    
                    AudioManager.Instance.PlayClip(DamageClip, .5f, Random.Range(0.8f, 1.2f));
                    
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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
