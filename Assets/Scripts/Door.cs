using UnityEngine;

public class Door : MonoBehaviour
{

    private BoxCollider doorCheck;

    [SerializeField] private BoxCollider door;

    private void Start()
    {
        doorCheck = GetComponent<BoxCollider>();

        door.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            door.enabled = true;

        }

    }
}
