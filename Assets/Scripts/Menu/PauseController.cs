using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    [Header("Pause Settings")]
    public KeyCode pauseKey = KeyCode.Escape;

    private GameObject currentMenu;
    private bool isPaused = false;
    public static bool blockMenu = false;

    //Set the current menu
    private void Awake()
    {
        currentMenu = pauseMenu;
    }

    private void Update()
    {
        if (Input.GetKeyUp(pauseKey) && !blockMenu)
        {
            ToggleMenu();
        }
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            currentMenu.SetActive(true);
            AudioManager.Instance.Play("Pause");
            Time.timeScale = 0f;
        }
        else
        {
            currentMenu.SetActive(false);
            AudioManager.Instance.Play("Unpause");
            Time.timeScale = 1f;
            currentMenu = pauseMenu;
        }
    }

    // Pause Menu exclusive may can be moved
    public void ReturnMenu()
    {
        Time.timeScale = 1f;
        ToggleMenu();
        LevelLoader.Instance.ReturnToMenu();

    }

    public void SettingsGame()
    {
        ToggleMenu();
        currentMenu = settingsMenu;
        ToggleMenu();
    }
}
