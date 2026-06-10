using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour
{

    private Roomgen _roomGenScript;

    [SerializeField] private GameObject _neighbour;

    [SerializeField] private GameObject fuck;


    private void Awake()
    {
        _roomGenScript = FindFirstObjectByType<Roomgen>();
    }

    public void SetNeighbour(GameObject neighbour)
    {
        _neighbour = neighbour;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Room>() != null)
        {

            _roomGenScript._Continue = true;

            GameObject _Collided = collision.gameObject;

            Debug.Log(_Collided);

            Room _Script = _Collided.GetComponent<Room>();

            StartCoroutine(DeleteLater(_Script));

            return;
        }

        else if (collision.gameObject.GetComponentInChildren<Attach>() != null && collision.gameObject != _neighbour)
        {

            fuck = collision.gameObject;

            Debug.Log("Obstructed ");

            _roomGenScript._Obstructed = true;
        }
    }


    private IEnumerator DeleteLater(Room room)
    {

        yield return new WaitForSeconds(0.01f);

        Destroy(room);

    }

}
