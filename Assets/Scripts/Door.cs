using UnityEngine;

public class Door : MonoBehaviour
{

    private BoxCollider doorCheck;

    [SerializeField] private BoxCollider door;

    private bool triggered = false;

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

            triggered = true;

        }

    }

    private void Update()
    {
        if (triggered && gameObject.transform.localPosition != Vector3.zero)
        {
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, Vector3.zero, Time.deltaTime * 2);
        }
    }
}
