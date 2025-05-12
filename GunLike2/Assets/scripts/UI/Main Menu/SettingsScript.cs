using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SettingsScript : MonoBehaviour
{
    public TextAsset settingsFile;
    public List<RichSetting> settings;

    public struct RichSetting
    {
        public string name;
        public float val;
    }
    void Start()
    {
        settings = Unpack(settingsFile);
        if(settings[0].name != "BUILT")
        {
            Debug.Log("Settings file rebuilt");
            settings = BuildSettingsFile();
        }

        UpdatePrefs();
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
