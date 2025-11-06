using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using UnityEngine.Audio;

[DefaultExecutionOrder(-10)]
public class GameManager : MonoBehaviour
{
    public delegate void PlayerSpawnDelegate(PlayerController playerInstance);
    public event PlayerSpawnDelegate OnPlayerControllerCreated;

    #region Player Controller Information
    public PlayerController playerPrefab;
    private PlayerController _playerInstance;
    public PlayerController playerInstance => _playerInstance;
    private Vector3 currentCheckpoint;
    #endregion

    public event Action<int> OnLivesChanged;
    public event Action<int> OnScoreChanged;
    public event Action<float> OnLevelEnd;

    public AudioMixerGroup masterMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    #region Stats
    private int _lives = 3;
    private int _score = 0;
    private int _ammo = 100;
    public float time = 0;
    private bool isCounting = false;
    public int score
    {
        get => _score;
        set
        {
            if (value < 0)
            {
                _score = 0;
            }
            else
            {
                _score = value;
            }
            Debug.Log($"Score: {_score}");
            OnScoreChanged?.Invoke(_score);
        }
    }
    public int lives
    {
        get => _lives;
        set
        {
            if (value < 1)
            {
                //gameover goes here
                Debug.Log("Game Over! You have no lives left.");
                GameOver();
                _lives = 0;


            }
            else if (value < _lives)
            {
                //play hurt sound
                Debug.Log("Ouch! You lost a life.");
                Respawn();

                _lives = value;
            }
            else if (value > maxLives)
            {
                _lives = maxLives;
            }
            else
            {
                _lives = value;
            }
            Debug.Log($"Lives: {_lives}");
            OnLivesChanged?.Invoke(_lives);
        }
    }
    

    public int maxLives = 9;
    #endregion

    #region Singleton Pattern
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }
    #endregion

    void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneChanged;
    }

    void SceneChanged(Scene current, Scene next)
    {
        if (next.name != "Game")
        {
            isCounting = false;
        }

        if (next.name == "Victory")
        {
            OnLevelEnd?.Invoke(time);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void GameOver()
    {
        SceneManager.LoadScene(2);
     
    }
    void Respawn()
    {
        _playerInstance.transform.position = currentCheckpoint;

    }
    public void Victory()
    {
        isCounting = false;
        OnLevelEnd?.Invoke(time);
        Debug.Log("Victory! Level completed in " + time + " seconds.");
        SceneManager.LoadScene(3);
    }

    public void StartLevel(Vector3 startPositon)
    {
        currentCheckpoint = startPositon;
        _playerInstance = Instantiate(playerPrefab, currentCheckpoint, Quaternion.identity);
        OnPlayerControllerCreated.Invoke(_playerInstance);
        _lives = 3;
        _score = 0;
        time = 0;
        isCounting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GameOver();
                SceneManager.LoadScene(1);
               
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            lives++;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            lives--;
        }

        if (isCounting)
        {
            time += Time.deltaTime;
            
        }
    }
}