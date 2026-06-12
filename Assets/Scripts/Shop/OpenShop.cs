using System;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public static event Action OnOpenShop;
    private Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnOpenShop?.Invoke();
            Destroy(collider);
        }
    }
}
