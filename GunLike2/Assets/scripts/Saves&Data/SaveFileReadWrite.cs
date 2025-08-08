using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveFileReadWrite : MonoBehaviour
{
    [System.Serializable]
    public class SaveDataFile
    {
        public string usrID = "NULL";
        public int usrSessions;
        public List<GunInformation> gunInfo;
    }
    [System.Serializable]
    public class GunInformation
    {
        public string gunName;
        public int gunID;
        public bool unlocked;
        public float unlockProgression;
        public int runs;
        public int deaths;
        public int winningRuns;
        public int kills;
        public float totalDamage;
        public float damageRecord;
        public int magSizeRecord;
        public int itemsCollected;
        public float accuracy;
        public float elapsedTimeHeld;
        public int difficulyReachedRecord;
    }

    string file; bool createdNew;
    string filePath; string fileName = "SaveData.json";
    public SaveDataFile data;
    public GameDataManager gdm;
    public MainMenuManager menuManager;
    private void Awake()
    {
        //Try to find GDM
        foreach (GameObject gm in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if(gm.name == "CoreProcesses")
            {
                gdm = gm.GetComponentInChildren<GameDataManager>();
            }
            if(gm.tag == "gdm")
            {
                gdm = gm.GetComponent<GameDataManager>();
            }
        }
        //Try to find Main Menu Manager
        foreach (GameObject gm in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (gm.name == "Main Menu Manager")
            {
                menuManager = gm.GetComponent<MainMenuManager>();
            }
        }
        if(gdm != null) { gdm.instance = this; gdm.RequestSaveData(); }
        if(menuManager != null) { menuManager.instance = this; }

        createdNew = false;
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        data = new SaveDataFile();

        CheckEmpty();
        if (!createdNew) { Deserialize(); }
    }
    public bool RequestDataUpdate()
    {
        if (data == null || gdm == null) { return false; }

        data.usrID = gdm.usrID;
        data.usrSessions = gdm.usrSessionNum;
        data.gunInfo = gdm.gunInfo;
        UpdateSaveFile();
        return true;
    }
    SaveDataFile InitalizeData()
    {
        SaveDataFile newData;
        newData = new SaveDataFile();
        newData.usrID = "NEW";
        newData.gunInfo = new List<GunInformation>();
        for (int i = 0; i < 12; i++)
        {
            newData.gunInfo.Add(new GunInformation()); //Write Basic Gun Info
            switch (i)
            {
                case 0: //Pistol
                    newData.gunInfo[i].gunName = "Pistol";
                    newData.gunInfo[i].gunID = i;
                    newData.gunInfo[i].unlocked = true;
                    newData.gunInfo[i].unlockProgression = 1;
                    newData.gunInfo[i].runs = 0;
                    newData.gunInfo[i].deaths = 0;
                    newData.gunInfo[i].winningRuns = 0;
                    newData.gunInfo[i].kills = 0;
                    newData.gunInfo[i].totalDamage = 0;
                    newData.gunInfo[i].damageRecord = 0;
                    newData.gunInfo[i].magSizeRecord = 0;
                    newData.gunInfo[i].itemsCollected = 0;
                    newData.gunInfo[i].accuracy = 0;
                    newData.gunInfo[i].elapsedTimeHeld = 0;
                    newData.gunInfo[i].difficulyReachedRecord = 0;
                    break;
                case 1: //Revolver
                    newData.gunInfo[i].gunName = "Revolver";
                    newData.gunInfo[i].gunID = i;
                    newData.gunInfo[i].unlocked = true;
                    newData.gunInfo[i].unlockProgression = 1;
                    newData.gunInfo[i].runs = 0;
                    newData.gunInfo[i].deaths = 0;
                    newData.gunInfo[i].winningRuns = 0;
                    newData.gunInfo[i].kills = 0;
                    newData.gunInfo[i].totalDamage = 0;
                    newData.gunInfo[i].damageRecord = 0;
                    newData.gunInfo[i].magSizeRecord = 0;
                    newData.gunInfo[i].itemsCollected = 0;
                    newData.gunInfo[i].accuracy = 0;
                    newData.gunInfo[i].elapsedTimeHeld = 0;
                    newData.gunInfo[i].difficulyReachedRecord = 0;
                    break;
                case 2: //BulkFed
                    newData.gunInfo[i].gunName = "Bulk-Fed Double Barrel";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 3: //Vector3
                    newData.gunInfo[i].gunName = "Vector3";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 4: //Aero-Rifle
                    newData.gunInfo[i].gunName = "Aero-Rifle";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 5: //Little Gun
                    newData.gunInfo[i].gunName = "Little Gun";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 6: //Da Eagle
                    newData.gunInfo[i].gunName = "Da Eagle";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 7: //Crossbow
                    newData.gunInfo[i].gunName = "Crossbow";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 8: //Mutated Knife
                    newData.gunInfo[i].gunName = "Mutated Knife";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 9: //Hand Cannon
                    newData.gunInfo[i].gunName = "Hand Cannon";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 10: //Archer Fish
                    newData.gunInfo[i].gunName = "Archer Fish";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
                case 11: //Shape Changing Goo
                    newData.gunInfo[i].gunName = "Shape Changing Goo";
                    newData.gunInfo[i].gunID = i;
                    BasicNulGunInfoAssembler(newData.gunInfo[i]);
                    break;
            }
        }
        return newData;
    }
    void BasicNulGunInfoAssembler(GunInformation gunInfo)
    {
        gunInfo.unlocked = false;
        gunInfo.unlockProgression = 0;
        gunInfo.runs = 0;
        gunInfo.deaths = 0;
        gunInfo.winningRuns = 0;
        gunInfo.kills = 0;
        gunInfo.totalDamage = 0;
        gunInfo.damageRecord = 0;
        gunInfo.magSizeRecord = 0;
        gunInfo.itemsCollected = 0;
        gunInfo.accuracy = 0;
        gunInfo.elapsedTimeHeld = 0;
        gunInfo.difficulyReachedRecord = 0;
    }
    private void OnApplicationQuit()
    {
        Serialize();
    }
    void CheckEmpty()
    {
        if (!File.Exists(filePath)) {
            CreateSaveData();}
    }
    void CreateSaveData()
    {
        StreamWriter sw = File.CreateText(filePath); sw.Close();
        SaveDataFile newData = InitalizeData();
        File.WriteAllText(filePath, JsonUtility.ToJson(newData));
        file = JsonUtility.ToJson(newData);
        data = JsonUtility.FromJson<SaveDataFile>(file);
        createdNew = true;
    }
    void Deserialize()
    {
        bool makeNew = false;
        file = File.ReadAllText(filePath);
        try
        {
            data = JsonUtility.FromJson<SaveDataFile>(file);
        }
        catch { makeNew = true; }
        if(data == null || makeNew || data.usrID == "NULL")//File is empty or corrupted, write new data to it.
        {
            File.Delete(filePath);
            CreateSaveData();
        }
    }
    public void UpdateSaveFile()
    {
        Serialize();
    }
    void Serialize()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(data));
    }
}
