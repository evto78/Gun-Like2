using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public TextAsset saveFile;
    public TextAsset unlocksFile;
    List<string> save;
    List<string> unlocks;
    void Start()
    {
        save = Unpack(saveFile);
        unlocks = Unpack(unlocksFile);
        if (save[0] != "BUILT")
        {
            Debug.Log("Save file rebuilt");
            save = BuildSaveFile();
        }
        if (unlocks[0] != "BUILT")
        {
            Debug.Log("Unlocks file rebuilt");
            unlocks = BuildUnlockFile();
        }

        if (unlocks[1].EndsWith("Y")) { Debug.Log("Pistol is unlocked"); } else { Debug.Log("Pistol is locked"); }
        if (unlocks[2].EndsWith("Y")) { Debug.Log("Revolver is unlocked"); } else { Debug.Log("Revolver is locked"); }
    }
    private void OnDisable()
    {
        Pack(save, saveFile);
        Pack(unlocks, unlocksFile);
    }
    private void OnDestroy()
    {
        Pack(save, saveFile);
        Pack(unlocks, unlocksFile);
    }
    private void OnApplicationQuit()
    {
        Pack(save, saveFile);
        Pack(unlocks, unlocksFile);
    }
    public List<string> Unpack(TextAsset txtFile)
    {
        List<string> returnedList;
        returnedList = new List<string>();
        returnedList.AddRange(txtFile.ToString().Split("/"));
        return returnedList;
    }
    public void Pack(List<string> list, TextAsset txtFile)
    {
        string packedStr = "";
        foreach (string element in list)
        {
            packedStr = packedStr + element + "/";
        }
        File.WriteAllText("Assets/Resources/" + txtFile.name + ".txt", packedStr);
    }
    List<string> BuildSaveFile()
    {
        List<string> builtList = new List<string>();
        builtList.Add("BUILT");
        builtList.Add("SAVEPRESENT:N");

        return builtList;
    }
    List<string> BuildUnlockFile()
    {
        List<string> builtList = new List<string>();
        builtList.Add("BUILT");
        builtList.Add("PISTOL:Y");
        builtList.Add("REV:Y");
        builtList.Add("DBLBRL:N");
        builtList.Add("VEC:N");
        builtList.Add("AERO:N");
        builtList.Add("LITTLE:N");
        builtList.Add("SEMI:N");
        builtList.Add("EAGLE:N");
        builtList.Add("BOW:N");
        builtList.Add("KNIFE:N");
        builtList.Add("GOO:N");
    
        return builtList;
    }
}
