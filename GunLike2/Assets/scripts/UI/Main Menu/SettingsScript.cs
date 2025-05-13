using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    List<string> keys;
    public List<Slider> sliders;
    public Toggle fullscreen;
    private void Start()
    {
        BuildKeys();

        int index = 0;
        foreach(string key in keys)
        {
            if (PlayerPrefs.HasKey(keys[index]))
            {
                sliders[index].value = PlayerPrefs.GetFloat(keys[index]);
            }
            index++;
        }

        if (!PlayerPrefs.HasKey("FULLSCREEN")) { PlayerPrefs.SetString("FULLSCREEN", "TRUE"); }
        Screen.fullScreen = PlayerPrefs.GetString("FULLSCREEN") == "TRUE";
        fullscreen.isOn = Screen.fullScreen;
    }
    private void Update()
    {
        int index = 0;
        foreach(Slider slider in sliders)
        {
            PlayerPrefs.SetFloat(keys[index], slider.value);
            index++;
        }
    }
    public void Fullscreen(bool input)
    {
        Screen.fullScreen = input;
        if (input)
        {
            PlayerPrefs.SetString("FULLSCREEN", "TRUE");
        }
        else
        {
            PlayerPrefs.SetString("FULLSCREEN", "FALSE");
        }
    }
    void BuildKeys()
    {
        keys = new List<string>();
        keys.Add("MASTERVOL");
        keys.Add("MUSICVOL");
        keys.Add("EFFECTVOL");
        keys.Add("FOV");
        keys.Add("SENS");
    }
}
