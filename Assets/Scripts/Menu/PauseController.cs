using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    public GameObject currentMenu;

    public bool isEnableCurrent = false;

    //Set the current menu
    private void Start()
    {
        currentMenu = pauseMenu;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
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
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }


    public void ToggleMenu()
    {
        isEnableCurrent = !isEnableCurrent;
        if (isEnableCurrent)
        {
            currentMenu.SetActive(true);
            Time.timeScale = 1f;
        }
        else
        {
            currentMenu.SetActive(false);
            currentMenu = pauseMenu;
            Time.timeScale = 1f;
        }
    }
}
