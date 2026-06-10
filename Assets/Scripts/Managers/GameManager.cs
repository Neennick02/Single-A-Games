using System;
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

    private bool _subscribed;
    private bool _firstFloor = true;
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
            _subscribed = true;
        }
    }

    private void ResetPlayer()
    {
        gameStarted = false;
        LevelSize = _startLevelSize;
        Roomgen.LevelSize = LevelSize;
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
        }
        else if (gameStarted)
        {
            //reset player pos
            _currentPlayer.transform.position = startPos;
        }
    }
    private void OnDestroy()
    {
        GameOverScreen.OnReset -= ResetPlayer;
        PauseScreen.OnReset -= ResetPlayer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        }
    }
}
