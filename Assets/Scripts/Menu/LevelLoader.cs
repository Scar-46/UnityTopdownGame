using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
using static System.TimeZoneInfo;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance;

    [Header("UI Elements")]
    public Animator transition;
    public GameObject overlay;
    public GameObject gameOverMenu;
    public GameObject bossHealthBar;

    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Transition Settings")]
    public float transitionTime = 1f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void StartGame()
    {
        LoadLevel("Level02", true);
        AudioManager.Instance.Play("Background");
        AudioManager.Instance.Stop("Menu");
        StartCoroutine(EnableOverlay());
    }

    public void GameSettings()
    {
        LoadLevel("SettingsMenu", false);
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
    
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex, true);
        PauseController.blockMenu = false;
        PlayerStats.Instance?.InitializePlayer();
        PlayerStats.Instance?.ResetGameState();
        gameOverMenu?.SetActive(false);
        bossHealthBar?.SetActive(false);
    }

    public void ReturnToMenu()
    {
        LoadLevel("MainMenu", false);
        PauseController.blockMenu = false;
        PlayerStats.Instance?.ResetGameState();
        gameOverMenu?.SetActive(false);
        overlay?.SetActive(false);
        bossHealthBar?.SetActive(false);

    }

    private void EnableGameOverMenu()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
            PauseController.blockMenu = true;
        }
        else
        {
            Debug.LogError("Game Over Menu is not assigned!");
        }
    }

    private IEnumerator EnableOverlay()
    {
        yield return new WaitForSeconds(0.5f);
        overlay.SetActive(true);
    }

    private void LoadLevel(object level, bool instantiatePlayer)
    {
        if (transition == null)
        {
            Debug.LogError("Transition Animator is not assigned!");
            return;
        }

        StartCoroutine(LoadLevelCoroutine(level, instantiatePlayer));
    }


    private IEnumerator LoadLevelCoroutine(object level, bool instantiatePlayer)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        if (level is string levelName)
        {
            Debug.Log(levelName);
            SceneManager.LoadScene(levelName);
        }
        else if (level is int levelIndex)
        {
            SceneManager.LoadScene(levelIndex);
        }

        yield return new WaitForSeconds(0.1f);

        if (instantiatePlayer)
        {
            GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
            if (spawnPoint != null)
            {
                Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
                PlayerStats.Instance?.InitializePlayer();
            }
            else
            {
                Debug.LogError("Spawn point not found!");
            }
        }
        yield return new WaitForSeconds(1f);
        transition.SetTrigger("End");
    }
}
