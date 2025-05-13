using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public TextAsset settingsFile;
    List<RichSetting> settings;
    public List<SettingSlider> sliders;

    [System.Serializable]
    public struct RichSetting
    {        
        public string name;
        public float val;
    }
    [System.Serializable]
    public struct SettingSlider
    {
        public string name;
        public Slider slider;
    }
    void Start()
    {
        settings = Unpack(settingsFile);
        if(settings[0].name != "BUILT")
        {
            Debug.Log("Settings file rebuilt");
            settings = BuildSettingsFile();
        }

        int index = 0;
        foreach (SettingSlider element in sliders)
        {
            element.slider.value = settings[index+1].val / 100f;

            index++;
        }

        UpdatePrefs();
    }
    private void Update()
    {
        int index = 0;
        foreach(SettingSlider element in sliders)
        {
            RichSetting temp;
            temp.name = settings[index + 1].name;
            temp.val = element.slider.value * 100f;
            settings[index + 1] = temp;

            index++;
        }
    }
    private void OnDisable()
    {
        Pack(settings, settingsFile);
    }
    private void OnDestroy()
    {
        Pack(settings, settingsFile);
    }
    private void OnApplicationQuit()
    {
        Pack(settings, settingsFile);
    }
    public List<RichSetting> Unpack(TextAsset txtFile)
    {
        List<RichSetting> returnedList;
        returnedList = new List<RichSetting>();
        foreach( string element in txtFile.ToString().Split("/"))
        {
            if (element.Contains(":"))
            {
                RichSetting constructing = new RichSetting();
                constructing.name = element.Split(":")[0];
                constructing.val = float.Parse(element.Split(":")[1]);
                returnedList.Add(constructing);
            }
        }

        return returnedList;
    }
    public void Pack(List<RichSetting> settingList, TextAsset txtFile)
    {
        string packedStr = "";
        foreach (RichSetting element in settingList)
        {
            packedStr = packedStr + (element.name + ":" + element.val) + "/";
        }
        File.WriteAllText("Assets/Resources/" + txtFile.name + ".txt", packedStr);
    }
    List<RichSetting> BuildSettingsFile()
    {
        RichSetting tempSetting;
        List<RichSetting> builtList = new List<RichSetting>();
        tempSetting.name = "BUILT"; tempSetting.val = 0f; builtList.Add(tempSetting);
        tempSetting.name = "MASTERVOL"; tempSetting.val = 100f; builtList.Add(tempSetting);
        tempSetting.name = "MUSICVOL"; tempSetting.val = 100f; builtList.Add(tempSetting);
        tempSetting.name = "EFFECTVOL"; tempSetting.val = 100f; builtList.Add(tempSetting);

        return builtList;
    }
    public void UpdatePrefs()
    {
        foreach(RichSetting element in settings)
        {
            PlayerPrefs.SetFloat(element.name, element.val);
        }
    }
}
