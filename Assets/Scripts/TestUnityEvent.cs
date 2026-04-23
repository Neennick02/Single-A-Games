using UnityEngine;
using UnityEngine.Events;

public class TestUnityEvent : MonoBehaviour
{
    public UnityEvent Event;

    private void OnTriggerEnter(Collider other)
    {
        Event?.Invoke();
    }
}
