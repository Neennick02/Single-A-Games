using System;
using System.Security.Cryptography;
using Unity.Multiplayer.PlayMode;
using Unity.VisualScripting;
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
    [SerializeField] private Roomgen _roomGen;
    private byte _levelSize = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ResetPlayer()
    {
        gameStarted = false;
        Destroy(_currentPlayer);
    }

    private void LoadMainScene()
    {
        Debug.Log("test");
        Vector3 startPos = new Vector3(0, -2.1f, 0);

        if (SceneManager.GetActiveScene().name == "MainScene" && !gameStarted)
        {
            //create player
            _currentPlayer = Instantiate(PlayerPrefab, startPos, Quaternion.identity);

            gameStarted = true;
            OnGameStart?.Invoke();

            //set default room amount
            _roomGen.SetLevelSize(_levelSize);
            _levelSize += 2;

        }
        else if (gameStarted)
        {
            //reset player pos
            _currentPlayer.transform.position = startPos;

            //increase level size
            //_levelSize += 2;

            //set level size
            _roomGen.SetLevelSize(_levelSize);
        }
    }
    private void OnEnable()
    {
        GameOverScreen.OnReset += ResetPlayer;
        PauseScreen.OnReset += ResetPlayer;
        SceneLoader.OnMainSceneLoad += LoadMainScene;
    }
    private void OnDisable()
    {
        GameOverScreen.OnReset -= ResetPlayer;
        PauseScreen.OnReset -= ResetPlayer;
        SceneLoader.OnMainSceneLoad -= LoadMainScene;
    }
}
