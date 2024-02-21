using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject settingsMenu;
    public PauseMenu pauseMenu;
    private bool ispausePanel = false;

    public void ResumeGame()
    {
        pauseMenu.TogglepauseMenu();
    }

    public void SettingsGame()
    {
        //settingsMenu.SetActive(true);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

}
