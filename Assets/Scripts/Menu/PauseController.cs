using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    //Menus
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject currentMenu;

    private bool isMenuActive = false;

    public static bool blockMenu;

    //Set the current menu
    private void Awake()
    {
        currentMenu = pauseMenu;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !blockMenu)
        {
            ToggleMenu();
        }
    }

    public void ResumeGame()
    {
        ToggleMenu();
    }

    public void SettingsGame()
    {
        ToggleMenu();
        currentMenu = settingsMenu;
        ToggleMenu();
    }

    public void ReturnMenu()
    {
        Time.timeScale = 1f;
        ToggleMenu();
        LevelLoader.Instance.ReturnToMenu();

    }


    private void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        if (isMenuActive)
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
}
