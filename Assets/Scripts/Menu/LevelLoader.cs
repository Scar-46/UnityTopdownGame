using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public GameObject gameOverMenu;

    public float transitionTime = 1f;
    public void StartGame()
    {
        StartCoroutine(LoadLevel("Level02"));
    }

    public void GameSettings()
    {
        StartCoroutine(LoadLevel("SettingsMenu"));
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    private void OnEnable()
    {
        PlayerStats.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerStats.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        PauseController.blockMenu = true;
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
        PauseController.blockMenu = false;
    }

    IEnumerator LoadLevel(int level)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(level);
    }

    IEnumerator LoadLevel(string level)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(level);
    }
}
