using System;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public static event Action OnOpenShop;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnOpenShop?.Invoke();
        }
    }
}
