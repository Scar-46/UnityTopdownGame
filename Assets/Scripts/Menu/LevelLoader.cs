using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;
using static System.TimeZoneInfo;

public class LevelLoader : MonoBehaviour
{
    [Header("UI Elements")]
    public Animator transition;
    public GameObject overlay;
    public GameObject gameOverMenu;

    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Transition Settings")]
    public float transitionTime = 1f;


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

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex, true);
        PauseController.blockMenu = false;
        PlayerStats.Instance?.InitializePlayer();
        PlayerStats.Instance?.ResetGameState();
        gameOverMenu?.SetActive(false);
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

    private IEnumerator EnableOverlay()
    {
        yield return new WaitForSeconds(0.5f);
        overlay.SetActive(true);
    }

    private IEnumerator LoadLevelCoroutine(object level, bool instantiatePlayer)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        if (level is string levelName)
        {
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
