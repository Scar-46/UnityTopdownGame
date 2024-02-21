using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullScreenSettings : MonoBehaviour
{
    public Toggle toggle;


    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {

        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        checkResolution();

    }


    public void activeFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void checkResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int actualresolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen && resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                actualresolution = i;
            }

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = actualresolution;
        resolutionDropdown.RefreshShownValue();


        resolutionDropdown.value = PlayerPrefs.GetInt("resolutionsNumber", 0);

    }

    public void changeResolution(int resolutionIndex)
    {
        PlayerPrefs.SetInt("resolutionsNumber", resolutionDropdown.value);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
