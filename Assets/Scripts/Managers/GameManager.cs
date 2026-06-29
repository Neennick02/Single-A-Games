using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PlayerPrefab;

    public static event Action OnGameStart;
    public bool gameStarted;
    public GameObject _currentPlayer;


    //calculate level size
    public byte _startLevelSize = 5;
    public byte LevelSize;


    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] private List<GameObject> Traps;
    public int TrapFloorSpawn = 3;
    private bool _subscribed;
    private bool _firstFloor = true;

    private bool _vomit;
    public static event Action OnEnableVomitUI;
    public static event Action<string> OnShowObjectiveText;
    public string ObjectiveText = "hello";
    //scores
    public float RunTime { get; private set; }
    public int FloorCount {get; private set; }

    public int KillCount { get; private set; }
    //private int _roomCount;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LevelSize = _startLevelSize;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (!_subscribed)
        {
            GameOverScreen.OnReset += ResetPlayer;
            PauseScreen.OnReset += ResetPlayer;
            SceneManager.sceneLoaded += OnSceneLoaded;
            PukeUpgrade.OnGrabVomit += EnableVomit;
            _subscribed = true;
        }

        SpawnEnemy.Enemies.Clear();
        for(int i = 0; i < Enemies.Count; i++)
        {
            SpawnEnemy.Enemies.Add(Enemies[i]);
        }
    }

    private void Update()
    {
        RunTime += Time.deltaTime;
    }

    private void ResetPlayer()
    {
        gameStarted = false;
        LevelSize = _startLevelSize;
        Roomgen.LevelSize = LevelSize;

        //deactivate effects
        _vomit = false;
        BlindEye.Active = false;

        //reset scores
        RunTime = 0f;
        FloorCount = 0;
        KillCount = 0;

        for (int i = 0; i < Traps.Count; i++)
        {
            SpawnEnemy.Enemies.Remove(Traps[i]);
        }
        Destroy(_currentPlayer);
    }

    private void LoadMainScene()
    {
        Vector3 startPos = new Vector3(0, -2.1f, 0);

        if (SceneManager.GetActiveScene().name == "MainScene" && !gameStarted)
        {
            //create player
            _currentPlayer = Instantiate(PlayerPrefab, startPos, Quaternion.identity);
            gameStarted = true;

            OnGameStart?.Invoke();
            OnShowObjectiveText?.Invoke(ObjectiveText);
        }
        else if (gameStarted)
        {
            //reset player pos
            _currentPlayer.transform.position = startPos;
            FloorCount++;
        }

        //add traps to 
        if(FloorCount >= TrapFloorSpawn)
        {
            for (int i = 0; i < Traps.Count; i++)
            {
                SpawnEnemy.Enemies.Add(Traps[i]);
            }
        }
    }
    private void OnDestroy()
    {
        GameOverScreen.OnReset -= ResetPlayer;
        PauseScreen.OnReset -= ResetPlayer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PukeUpgrade.OnGrabVomit -= EnableVomit;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            LoadMainScene();

            if (!_firstFloor)
                LevelSize += 2;

            Roomgen.LevelSize = LevelSize;

            _firstFloor = false;

            FindFirstObjectByType<Roomgen>().Generate();

            if (_vomit)
            {
                OnEnableVomitUI?.Invoke();
            }
        }
    }

    private void EnableVomit()
    {
        _vomit = true;
        OnEnableVomitUI?.Invoke();
    }

    public void AddKill()
    {
        KillCount ++;
    } 
}
