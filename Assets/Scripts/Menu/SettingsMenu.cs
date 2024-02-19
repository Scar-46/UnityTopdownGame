using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    public Dropdown Resolutionlist;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;


        Resolutionlist.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        
        Resolutionlist.AddOptions(options);
        Resolutionlist.value = currentResolutionIndex;
        Resolutionlist.RefreshShownValue();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ReturnBotton()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void SetFullscreen(bool isFullscreen)
    {

        Screen.fullScreen = isFullscreen;

    }

}
