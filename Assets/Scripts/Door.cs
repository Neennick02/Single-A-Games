using UnityEngine;

public class Door : MonoBehaviour
{

    private BoxCollider doorCheck;

    [SerializeField] private BoxCollider door;

    private bool triggered = false;

    public AudioClip AudioClip;

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

            AudioManager.Instance.PlayClip(AudioClip, 1, Random.Range(0.8f, 1.2f));

        }

    }

    private void Update()
    {
        if (triggered && gameObject.transform.localPosition != Vector3.zero)
        {
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, Vector3.zero, Time.deltaTime * 3);
        }
    }
}
