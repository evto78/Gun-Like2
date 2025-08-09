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
        public BossInformation ChimeraInfo;
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
        public int bulletsFired;
        public int magSizeRecord;
        public int itemsCollected;
        public float accuracy;
        public float elapsedTimeHeld;
        public int difficulyReachedRecord;
    }
    [System.Serializable]
    public class BossInformation
    {
        public int timesFought = 0;
        public int timesDefeated = 0;
        public float timeToKillRecord = 99999f;
    }

    string file; bool createdNew;
    string filePath; string fileName = "SaveData.json";
    public SaveDataFile data;
    public GameDataManager gdm;
    public MainMenuManager menuManager;

    //TelemnetryDataCollection
    [Header("DATA COLLECTION")]
    public bool sendData;
    public List<string> emailQueContent = new List<string>();
    public List<string> emailQueEvent = new List<string>();

    private void Awake()
    {
        emailQueContent = new List<string>();
        emailQueEvent = new List<string>();
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
        if(gdm != null) { gdm.instance = this; }
        if(menuManager != null) { menuManager.instance = this; }

        createdNew = false;
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        data = new SaveDataFile();

        CheckEmpty();
        if (!createdNew) { Deserialize(); }

        data.usrSessions++;
        PlayerPrefs.SetInt("USRSES", data.usrSessions);
    }
    private void Update()
    {
        if(gdm == null && menuManager == null)
        {
            //Try to find GDM
            foreach (GameObject gm in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (gm.name == "CoreProcesses")
                {
                    gdm = gm.GetComponentInChildren<GameDataManager>();
                }
                if (gm.tag == "gdm")
                {
                    gdm = gm.GetComponent<GameDataManager>();
                }
            }
            if (gdm != null) { gdm.instance = this; }
            //Try to find Main Menu Manager
            foreach (GameObject gm in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (gm.name == "Main Menu Manager")
                {
                    menuManager = gm.GetComponent<MainMenuManager>();
                }
            }
            if (menuManager != null) { menuManager.instance = this; }
        }
    }
    public bool RequestDataUpdate()
    {
        if (data == null) { return false; }

        if(gdm != null) { gdm.UpdateRecords(); }

        UpdateSaveFile();
        return true;
    }
    SaveDataFile InitalizeData()
    {
        SaveDataFile newData;
        newData = new SaveDataFile();
        newData.usrID = "NEW";
        if (PlayerPrefs.HasKey("USRID"))
        {
            newData.usrID = PlayerPrefs.GetString("USRID");
        }
        else
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string appenedLetters = "";
            for (int i = 0; i < 40; i++)
            {
                appenedLetters = appenedLetters + letters[Random.Range(0, letters.Length)];
            }
            newData.usrID = appenedLetters + Random.Range(0, int.MaxValue);
            PlayerPrefs.SetString("USRID", newData.usrID);
            Debug.Log("Usr Id created for this device : " + newData.usrID);
        }
        if (PlayerPrefs.HasKey("USRSES"))
        {
            newData.usrSessions = PlayerPrefs.GetInt("USRSES");
        }
        else
        {
            newData.usrSessions = 0;
            PlayerPrefs.SetInt("USRSES", newData.usrSessions);
        }

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
                    newData.gunInfo[i].bulletsFired = 0;
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
                    newData.gunInfo[i].bulletsFired = 0;
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
        newData.ChimeraInfo = new BossInformation();

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
        gunInfo.bulletsFired = 0;
        gunInfo.magSizeRecord = 0;
        gunInfo.itemsCollected = 0;
        gunInfo.accuracy = 0;
        gunInfo.elapsedTimeHeld = 0;
        gunInfo.difficulyReachedRecord = 0;
    }
    private void OnApplicationQuit()
    {
        Serialize();
        AddEmailToQue("GameClose");
        SendAllEmails();
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

        if (PlayerPrefs.HasKey("USRID"))
        {
            data.usrID = PlayerPrefs.GetString("USRID");
        }
        else
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string appenedLetters = "";
            for (int i = 0; i < 40; i++)
            {
                appenedLetters = appenedLetters + letters[Random.Range(0, letters.Length)];
            }
            data.usrID = appenedLetters + Random.Range(0, int.MaxValue);
            PlayerPrefs.SetString("USRID", data.usrID);
            Debug.Log("Usr Id created for this device : " + data.usrID);
        }
        if (PlayerPrefs.HasKey("USRSES"))
        {
            data.usrSessions = PlayerPrefs.GetInt("USRSES");
        }
        else
        {
            data.usrSessions = 0;
            PlayerPrefs.SetInt("USRSES", data.usrSessions);
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
    public void AddEmailToQue(string eventT)
    {
        if (PlayerPrefs.GetInt("SENDDATA") != 1) { return; }
        Debug.Log("Adding " + eventT + " to que");

        TelemData tdata = PrepareData();

        tdata.eventData = eventT;

        string msg = "START";
        msg += "|(UsrID)" + data.usrID;
        msg += "|(TriggeredEvent)" + eventT;
        msg += "|(SessionNum)" + data.usrSessions;
        msg += "|(CurTime)" + tdata.eventTime;
        msg += "|(LeftGun)" + tdata.leftGun;
        msg += "|(RightGun)" + tdata.rightGun;
        msg += "|(TimeE)" + tdata.timeElapsed;
        msg += "|(Room)" + tdata.roomNum;
        msg += "|(Diff)" + tdata.difficulty;
        msg += "|(MRSourceOfDmg)" + tdata.mostRecentSourceOfDmg;
        msg += "|(Cash)" + tdata.currentCash;
        string leftInvTxt = "LEFT(" + FormatInvToString(tdata.leftInv) + ")";
        string rightInvTxt = "RIGHT(" + FormatInvToString(tdata.rightInv) + ")";
        msg += "|(LInv)" + leftInvTxt;
        msg += "|(RInv)" + rightInvTxt;
        msg += "|END";

        emailQueEvent.Add(eventT);
        emailQueContent.Add(msg);
    }
    void SendAllEmails()
    {
        Debug.Log("Emails to send: " + emailQueContent.Count);
        for(int i = 0; i < emailQueContent.Count; i++)
        {
            Debug.Log("Sending: " + emailQueEvent[i]);
            SendDataToEmail(emailQueEvent[i], emailQueContent[i]);
        }
    }
    public void SendDataToEmail(string eventT, string content)
    {
        Emailer.SendAnEmail(content, eventT);
    }
    public string FormatALLInvToString(List<int> inv)
    {
        string result = "";

        foreach (int item in inv)
        {
            result += item + ",";
        }
        result.Remove(result.Length - 1);

        return result;
    }
    public string FormatInvToString(List<int> inv)
    {
        string result = "";
        int i = 0;
        foreach (int item in inv)
        {
            if (item != 0) { result += "ID:" + i + ":" + item + ","; }
            i++;
        }
        if (result != "") { result.Remove(result.Length - 1); }

        return result;
    }
    public TelemData PrepareData()
    {
        TelemData tdata = new TelemData();
        tdata.usr = data.usrID;
        tdata.sessionNum = data.usrSessions.ToString();

        tdata.difficulty = gdm.difficulty;
        tdata.timeElapsed = gdm.timeSpentNoPause;
        tdata.currentCash = gdm.phm.money;
        tdata.roomNum = gdm.roomNumber;
        tdata.eventTime = System.DateTime.Now.ToString("U");
        tdata.leftInv = gdm.pi.leftItems;
        tdata.rightInv = gdm.pi.rightItems;
        tdata.leftGun = gdm.pi.gunManager.leftGunScript.gunName;
        tdata.rightGun = gdm.pi.gunManager.rightGunScript.gunName;
        if (gdm.phm.lastHitMe != null && gdm.phm.lastHitMe.data != null) { tdata.mostRecentSourceOfDmg = gdm.phm.lastHitMe.data.enemyName; } else { tdata.mostRecentSourceOfDmg = "NULL"; }
        return (tdata);
    }
}
