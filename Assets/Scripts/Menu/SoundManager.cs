using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public Slider slider;
    public float sliderValue;
    public Image imageMute;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("volumenAudio", 0.5f);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    public void ChangeSlider(float value)
    {
        sliderValue = value;
        PlayerPrefs.SetFloat("volumenAudio", sliderValue);
        AudioListener.volume = slider.value;
        CheckMute();
    }

    public void CheckMute()
    {
        if (sliderValue == 0 && imageMute)
        {
            imageMute.enabled = true;
        }
        else if (imageMute)
        {
            imageMute.enabled= false;
        }
    }

}
