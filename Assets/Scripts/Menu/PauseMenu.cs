using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject pausePanel;
    public GameObject CanvaPause;

    private bool ispausePanel = false;
    private bool issettingspause = false;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TogglepauseMenu();
            
        }
    }

    public void TogglepauseMenu()
    {
        
        ispausePanel = !ispausePanel;
        if (ispausePanel)
        {
            
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    
}
