using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    public GameObject tab;
    List<string> keys;
    public List<Slider> sliders;
    public Toggle fullscreen;
    public Toggle vsync;
    private void Start()
    {
        BuildKeys();

        if (!PlayerPrefs.HasKey("FIRSTLOAD")) { ResetSettings(); }

        int index = 0;
        foreach(string key in keys)
        {
            if (PlayerPrefs.HasKey(keys[index]))
            {
                sliders[index].value = PlayerPrefs.GetFloat(keys[index]) / 100f;
            }
            index++;
        }

        if (!PlayerPrefs.HasKey("FULLSCREEN")) { PlayerPrefs.SetInt("FULLSCREEN", 1); }
        if (!PlayerPrefs.HasKey("VSYNC")) { PlayerPrefs.SetInt("VSYNC", 0); }
        Screen.fullScreen = PlayerPrefs.GetInt("FULLSCREEN") == 1;
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC");
        fullscreen.isOn = Screen.fullScreen;
        vsync.isOn = PlayerPrefs.GetInt("VSYNC") == 1;
        //Debug.Log("Applying Settings...");
        Apply();
        if (SceneManager.GetActiveScene().name == "Main Menu") { tab.SetActive(false); }
    }
    void Apply()
    {
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC");
        Screen.fullScreen = PlayerPrefs.GetInt("FULLSCREEN") == 1;
        int index = 0;
        foreach (Slider slider in sliders)
        {
            PlayerPrefs.SetFloat(keys[index], slider.value * 100f);
            index++;
        }
    }
    private void Update()
    {
        Apply();
    }
    public void Fullscreen(bool input)
    {
        Screen.fullScreen = input;
        if (input){PlayerPrefs.SetInt("FULLSCREEN", 1);}else{PlayerPrefs.SetInt("FULLSCREEN", 0);}
    }
    public void Vsync(bool input)
    {
        if (input) { PlayerPrefs.SetInt("VSYNC", 1); } else { PlayerPrefs.SetInt("VSYNC", 0); }
        QualitySettings.vSyncCount = Mathf.RoundToInt(PlayerPrefs.GetFloat("VSYNC"));
    }
    public void DynamicFov(bool input)
    {
        if (input) { PlayerPrefs.SetInt("DFOV", 1); } else { PlayerPrefs.SetInt("DFOV", 0); }
    }
    void BuildKeys()
    {
        keys = new List<string>();
        keys.Add("MASTERVOL");
        keys.Add("MUSICVOL");
        keys.Add("EFFECTVOL");
        keys.Add("FOV");
        keys.Add("SENS");
        keys.Add("FPS");
        keys.Add("UIVOL");
    }
    public void ResetSettings()
    {
        PlayerPrefs.SetInt("FIRSTLOAD", 1);

        PlayerPrefs.SetFloat("MASTERVOL", 100f);
        PlayerPrefs.SetFloat("MUSICVOL", 80f);
        PlayerPrefs.SetFloat("EFFECTVOL", 80f);
        PlayerPrefs.SetFloat("FOV", 80f);
        PlayerPrefs.SetFloat("SENS", 50f);
        PlayerPrefs.SetFloat("FPS", 120f);
        PlayerPrefs.SetFloat("UIVOL", 60f);
        int index = 0;
        foreach (string key in keys)
        {
            if (PlayerPrefs.HasKey(keys[index]))
            {
                sliders[index].value = PlayerPrefs.GetFloat(keys[index])/100f;
            }
            index++;
        }
        PlayerPrefs.SetInt("FULLSCREEN", 1);
        PlayerPrefs.SetInt("VSYNC", 0);
        Screen.fullScreen = PlayerPrefs.GetInt("FULLSCREEN") == 1;
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC");
        fullscreen.isOn = Screen.fullScreen;
        vsync.isOn = PlayerPrefs.GetInt("VSYNC") == 1;
    }
}
