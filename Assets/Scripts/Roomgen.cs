using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomgen : MonoBehaviour
{

    [SerializeField] private List<RoomObject> _rooms = new List<RoomObject>();
    [SerializeField] private List<RoomObject> _combatRooms = new List<RoomObject>();

    [SerializeField] private RoomObject _startRoom;
    [SerializeField] private RoomObject _endRoom;

    private Coroutine _mapRoutine;
    public List<GameObject> Rooms = new List<GameObject>();
    public static byte LevelSize;

    public bool _Obstructed;
    private bool _isGenerating;
    public bool _Continue;

    private float _Timer;

    void Awake()
    {

        transform.position = Vector3.zero;

    }

    public void Start()
    {
        _mapRoutine = StartCoroutine(GenerateMap());
    }

    private void Update()
    {
        if (_isGenerating) return;

        if (_Obstructed)
            _Timer += Time.deltaTime;
        else
            _Timer = 0;

        if (_Timer >= 0.3f)
        {
            foreach (Transform child in transform) Destroy(child.gameObject);

            Debug.Log("Reset came from the update");

            StopCoroutine(_mapRoutine);
            _mapRoutine = StartCoroutine(GenerateMap());
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

        _isGenerating = true;
        _Obstructed = false;

        for (int i = 0; i < LevelSize; i++)
        {
            //start room
            if (i == 0)
            {
                _Room = Instantiate(_startRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);
                Rooms.Add(_Room);
                _Continue = true;

                GameObject player = GameManager.Instance._currentPlayer;
                while (player == null)
                {
                    player = GameManager.Instance._currentPlayer;
                    yield return null;
                }
                player.transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            }

            //end room
            else if (i == LevelSize - 1)
            {

                _Room = Instantiate(_endRoom._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);
                Rooms.Add(_Room);

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Room.transform.position = _Attachment.transform.position;


            }

            //other rooms
            else
            {

                _Random = Random.Range(0, _rooms.Count);

                _Room = Instantiate(_rooms[_Random]._RoomObject, transform.position, Quaternion.Euler(transform.eulerAngles.x, _Rotation, transform.eulerAngles.z), gameObject.transform);
                Rooms.Add(_Room);

                _Attachment = _PrevRoom.GetComponentInChildren<Attach>().gameObject;

                _Room.GetComponentInChildren<Room>().SetNeighbour(_PrevRoom.GetComponentInChildren<Rigidbody>().gameObject);

                _Rotation += _rooms[_Random]._RotationModifier;

                _Room.transform.position = _Attachment.transform.position;

            }

            _PrevRoom = _Room;

            yield return new WaitForSeconds(0.02f);

            if (_Obstructed)
            {

                Rooms.Clear();
                foreach (Transform child in transform) Destroy(child.gameObject);
                StopCoroutine(_mapRoutine);
                _mapRoutine = StartCoroutine(GenerateMap());

                yield break;
            }
        }
    }
}

