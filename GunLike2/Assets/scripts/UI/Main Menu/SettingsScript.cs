using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsScript : MonoBehaviour
{
    GameDataManager gdm;
    public GameObject tab;
    List<string> keys;
    public List<Slider> sliders;
    public Toggle fullscreen;
    public Toggle vsync;
    public TMP_Dropdown resolution;
    public TMP_Dropdown bulletDisplayType;
    public Toggle sendGameData;
    public Toggle dfov;
    bool built = false;
    private void Start()
    {
        if (!built) { BuildAll(); }
        
        built = true;

        Apply();
        if (SceneManager.GetActiveScene().name == "Main Menu") { tab.SetActive(false); }
    }
    void BuildAll()
    {
        BuildKeys();

        if (!PlayerPrefs.HasKey("FIRSTLOAD")) { ResetSettings(); }

        int index = 0;
        foreach (string key in keys)
        {
            if (PlayerPrefs.HasKey(keys[index]))
            {
                sliders[index].value = PlayerPrefs.GetFloat(keys[index]) / 100f;
            }
            index++;
        }

        if (!PlayerPrefs.HasKey("FULLSCREEN")) { PlayerPrefs.SetInt("FULLSCREEN", 1); }
        if (!PlayerPrefs.HasKey("VSYNC")) { PlayerPrefs.SetInt("VSYNC", 0); }
        if (!PlayerPrefs.HasKey("SENDDATA")) { PlayerPrefs.SetInt("SENDDATA", 1); }
        if (!PlayerPrefs.HasKey("DFOV")) { PlayerPrefs.SetInt("DFOV", 1); }
        sendGameData.isOn = PlayerPrefs.GetInt("SENDDATA") == 1;
        dfov.isOn = PlayerPrefs.GetInt("DFOV") == 1;
        Screen.fullScreen = PlayerPrefs.GetInt("FULLSCREEN") == 1;
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC");
        fullscreen.isOn = Screen.fullScreen;
        vsync.isOn = PlayerPrefs.GetInt("VSYNC") == 1;
        if (!PlayerPrefs.HasKey("BULLETDISPLAYTYPE")) { PlayerPrefs.SetInt("BULLETDISPLAYTYPE", 2); bulletDisplayType.value = 2; } 
        else { bulletDisplayType.value = PlayerPrefs.GetInt("BULLETDISPLAYTYPE"); }
        if (!PlayerPrefs.HasKey("RES")) { PlayerPrefs.SetInt("RES", 0); }
        resolution.value = PlayerPrefs.GetInt("RES");
        switch (resolution.value)
        {
            case 0: Screen.SetResolution(1920, 1080, Screen.fullScreen); resolution.value = 0; break;
            case 1: Screen.SetResolution(2560, 1080, Screen.fullScreen); resolution.value = 1; break;
            case 2: Screen.SetResolution(2560, 1440, Screen.fullScreen); resolution.value = 2; break;
        }
    }
    public void Apply()
    {
        if (!built) { BuildAll(); }
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSYNC");
        Screen.fullScreen = PlayerPrefs.GetInt("FULLSCREEN") == 1;
        int index = 0;
        foreach (Slider slider in sliders)
        {
            PlayerPrefs.SetFloat(keys[index], slider.value * 100f);
            index++;
        }
        Application.targetFrameRate= Mathf.RoundToInt(PlayerPrefs.GetFloat("FPS"));
        if(Mathf.RoundToInt(PlayerPrefs.GetFloat("FPS")) == 120) { Application.targetFrameRate = -1; }

        if (gdm == null && GameObject.FindGameObjectWithTag("gdm") != null) { gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>(); }
        if (gdm != null) { gdm.phm.playerMvt.UpdateSettings(); gdm.phm.lsm.UpdateSettings(); }
        try
        {
            if (gdm == null) { Camera.main.gameObject.GetComponent<LocalSoundManager>().UpdateSettings(); }
        }
        catch {  }
    }
    private void Update()
    {
        Apply();
    }
    public void Fullscreen(bool input)
    {
        input = fullscreen.isOn;
        Screen.fullScreen = input;
        if (input){PlayerPrefs.SetInt("FULLSCREEN", 1);}else{PlayerPrefs.SetInt("FULLSCREEN", 0);}
    }
    public void Vsync(bool input)
    {
        input = vsync.isOn;
        if (input) { PlayerPrefs.SetInt("VSYNC", 1); } else { PlayerPrefs.SetInt("VSYNC", 0); }
        QualitySettings.vSyncCount = Mathf.RoundToInt(PlayerPrefs.GetFloat("VSYNC"));
    }
    public void DynamicFov(bool input)
    {
        input = dfov.isOn;
        if (input) { PlayerPrefs.SetInt("DFOV", 1); } else { PlayerPrefs.SetInt("DFOV", 0); }
    }
    public void SendData(bool input)
    {
        input = sendGameData.isOn;
        if (input) { PlayerPrefs.SetInt("SENDDATA", 1); } else { PlayerPrefs.SetInt("SENDDATA", 0); }
    }
    public void ChangeResolution(int input)
    {
        PlayerPrefs.SetInt("RES", resolution.value);
        switch (resolution.value)
        {
            case 0: Screen.SetResolution(1920, 1080, Screen.fullScreen); break;
            case 1: Screen.SetResolution(2560, 1080, Screen.fullScreen); break;
            case 2: Screen.SetResolution(2560, 1440, Screen.fullScreen); break;
        }
    }
    public void ChangeBulletDisplayType(int input)
    {
        PlayerPrefs.SetInt("BULLETDISPLAYTYPE", bulletDisplayType.value);
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
        keys.Add("ENEMYVOL");
        keys.Add("GUNVOL");
    }
    public void ResetSettings()
    {
        PlayerPrefs.SetInt("FIRSTLOAD", 1);

        PlayerPrefs.SetFloat("MASTERVOL", 75f);
        PlayerPrefs.SetFloat("MUSICVOL", 80f);
        PlayerPrefs.SetFloat("EFFECTVOL", 80f);
        PlayerPrefs.SetFloat("FOV", 80f);
        PlayerPrefs.SetFloat("SENS", 50f);
        PlayerPrefs.SetFloat("FPS", 120f);
        PlayerPrefs.SetFloat("UIVOL", 60f);
        PlayerPrefs.SetFloat("GUNVOL", 60f);
        PlayerPrefs.SetFloat("ENEMYVOL", 80f);
        PlayerPrefs.SetInt("SENDDATA", 1);
        PlayerPrefs.SetInt("DFOV", 1);
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
        PlayerPrefs.SetInt("RES", 0);
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }
}
