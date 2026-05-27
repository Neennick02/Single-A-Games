using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject PlayerPrefab;


    public bool gameStarted;
    private GameObject _currentPlayer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        Vector3 startPos = new Vector3(0, -2.1f, 0);

        if (SceneManager.GetActiveScene().name == "MainScene" && !gameStarted)
        {
            _currentPlayer = Instantiate(PlayerPrefab, startPos, Quaternion.identity);
            Debug.Log("start");
            gameStarted = true;
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
    }
    private void OnDisable()
    {
        GameOverScreen.OnReset -= ResetPlayer;
        PauseScreen.OnReset -= ResetPlayer;
    }

    private void ResetPlayer()
    {
        gameStarted = false;
        Destroy(_currentPlayer);
    }



}
