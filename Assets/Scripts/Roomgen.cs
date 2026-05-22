using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomgen : MonoBehaviour
{

    [SerializeField] private List<RoomObject> _rooms = new List<RoomObject>();
    [SerializeField] private List<RoomObject> _combatRooms = new List<RoomObject>();
    [SerializeField] private RoomObject _startRoom;
    [SerializeField] private RoomObject _endRoom;

    [SerializeField] private byte _levelSize;

    public bool _Obstructed;

    public bool _Continue;

    void Awake()
    {

        transform.position = Vector3.zero;

    }

    private void Start()
    {

        StartCoroutine(GenerateMap());

    }

    private IEnumerator GenerateMap()
    {
        byte _UntilCombat = 0;

        int _Rotation = 0;

        int _Random = 0;

        GameObject _Attachment = null;

        GameObject _PrevRoom = null;

        GameObject _Room = null;

        List<GameObject> _AllRooms = new List<GameObject>();


        for (int i = 0; i < _levelSize; i++)
        {

            if (i == 0)
            {
                _Room = Instantiate(_startRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z));

                _Continue = true;
            }

            else if (i == _levelSize - 1)
            {

                _Room = Instantiate(_endRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z));

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Room.transform.position = _Attachment.transform.position;


            }

            else
            {

                _Random = Random.Range(0, _rooms.Count);

                _Room = Instantiate(_rooms[_Random]._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z));

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Rotation += _rooms[_Random]._RotationModifier;

                _Room.transform.position = _Attachment.transform.position;

            }

            _PrevRoom = _Room;

            _AllRooms.Add(_Room);


            if (_Obstructed)
            {

                _Obstructed = false;

                foreach (GameObject room in _AllRooms)
                {
                    Destroy(room);
                }

                _AllRooms.Clear();


                StartCoroutine(GenerateMap());

                break;
            }

            yield return new WaitForSeconds(0.02f);




        }
    }

}

