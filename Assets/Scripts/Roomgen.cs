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

    private float _Timer;

    public GameObject _sceneLoader;

    void Awake()
    {

        transform.position = Vector3.zero;

    }

    private void Start()
    {

        StartCoroutine(GenerateMap());

    }

    private void Update()
    {
        if (_Obstructed)
        {
            _Timer += Time.deltaTime;
        }

        else
        {
            _Timer = 0;
        }


        if (_Timer >= 0.3f)
        {
            foreach (Transform child in transform) Destroy(child.gameObject);

            Debug.Log("Reset came from the update");

            StartCoroutine(GenerateMap());
        }

    }

    private IEnumerator GenerateMap()
    {
        byte _UntilCombat = 0;

        int _Rotation = 0;

        int _Random = 0;

        GameObject _Attachment = null;

        GameObject _PrevRoom = null;

        GameObject _Room = null;

        _Obstructed = false;


        for (int i = 0; i < _levelSize; i++)
        {

            if (i == 0)
            {
                _Room = Instantiate(_startRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);
                _Continue = true;

                GameObject player = GameManager.Instance._currentPlayer;
                while(player == null)
                {
                    player = GameManager.Instance._currentPlayer;
                    yield return null;
                }
                player.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            }

            else if (i == _levelSize - 1)
            {

                _Room = Instantiate(_endRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Room.transform.position = _Attachment.transform.position;


            }

            else
            {

                _Random = Random.Range(0, _rooms.Count);

                _Room = Instantiate(_rooms[_Random]._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Rotation += _rooms[_Random]._RotationModifier;

                _Room.transform.position = _Attachment.transform.position;

            }

            _PrevRoom = _Room;

            yield return new WaitForSeconds(0.02f);

            if (_Obstructed)
            {

                foreach (Transform child in transform) Destroy(child.gameObject);

                Debug.Log("Reset");

                StartCoroutine(GenerateMap());

                break;
            }


        }
    }

    public void SetLevelSize(byte size)
    {
        _levelSize = size;
    }
}

