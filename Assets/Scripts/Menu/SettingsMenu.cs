using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level01");
    }

    public void GameSettings()
    {

    }

    public void Return()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
