using UnityEngine;

public class Room : MonoBehaviour
{

    private Roomgen _roomGenScript;


    private void Awake()
    {
        _roomGenScript = FindFirstObjectByType<Roomgen>();

    }


    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Collided with " + collision.gameObject.name);

        if (collision.gameObject.GetComponent<Room>() != null)
        {

            _roomGenScript._Continue = true;

            GameObject _Collided = collision.gameObject;

            Room _Script = _Collided.GetComponent<Room>();

            Destroy(_Script);

            return;
        }

        else if (collision.gameObject.GetComponentInChildren<Attach>() != null)
        {
            Debug.LogWarning("Obstructed!" + gameObject);

            _roomGenScript._Obstructed = true;
        }
    }

}
