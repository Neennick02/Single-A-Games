using System;
using System.Security.Cryptography;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PlayerPrefab;

    public static event Action OnGameStart;
    public bool gameStarted;
    public GameObject _currentPlayer;

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
        Vector3 startPos = new Vector3(0, -2.1f, 0);

        if (SceneManager.GetActiveScene().name == "MainScene" && !gameStarted)
        {
            _currentPlayer = Instantiate(PlayerPrefab, startPos, Quaternion.identity);
            gameStarted = true;
            OnGameStart?.Invoke();
        }
        else if (gameStarted)
        {
            _currentPlayer.transform.position = startPos;
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
