using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    List<string> save;
    List<string> unlocks;

    void Start()
    {
        if (PlayerPrefs.HasKey("UNLOCKSBUILT")) { BuildUnlocks(); }
        if (PlayerPrefs.HasKey("SAVESBUILT")) { BuildSave(); }
    }
    void BuildUnlocks()
    {
        PlayerPrefs.SetString("UNLOCKSBUILT", "YES");
        unlocks = new List<string>();
        unlocks.Add("PISTOL:Y");
        unlocks.Add("REV:Y");
        unlocks.Add("DBLBRL:N");
        unlocks.Add("VEC:N");
        unlocks.Add("AERO:N");
        unlocks.Add("LITTLE:N");
        unlocks.Add("SEMI:N");
        unlocks.Add("EAGLE:N");
        unlocks.Add("BOW:N");
        unlocks.Add("KNIFE:N");
        unlocks.Add("GOO:N");
    }
    void BuildSave()
    {
        PlayerPrefs.SetString("SAVESBUILT", "YES");
        save = new List<string>();
        unlocks.Add("RUNSAVED:N");
    }
}
