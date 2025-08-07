using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public List<EnemyHealthManager> activeEhms = new List<EnemyHealthManager>();
    public List<EnemySpawner> activeSpawners = new List<EnemySpawner>();
    public Vector2 basePoints; public float flatPointsPerDifficulty;
    public float pointsLeft;
    public float difficulty;
    public float difficultySelected;
    public float timeSpent;
    public float timeSpentNoPause;
    public bool gameTimerActive;
    public int roomNumber;
    public HealthManager phm;
    PlayerItem pi;
    float pointregenTimer;
    //Checking Change
    bool changedLastFrame;
    List<int> leftSnapshot;
    List<int> rightSnapshot;

    [Header("Bosses")]
    public GameObject chimera;

    //TelemnetryDataCollection
    [Header("DATA COLLECTION")]
    string usrID; int usrSessionNum;
    public bool sendData;
    private void Awake()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
        pi = phm.playerItem;

        if (PlayerPrefs.HasKey("USRID"))
        {
            usrID = PlayerPrefs.GetString("USRID");
        }
        else
        {
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string appenedLetters = "";
            for(int i = 0; i < 40; i++)
            {
                appenedLetters = appenedLetters+letters[Random.Range(0,letters.Length)];
            }
            usrID = appenedLetters+Random.Range(0, int.MaxValue);
            PlayerPrefs.SetString("USRID", usrID);
            Debug.Log("Usr Id created for this device : " + usrID);
        }
        if (PlayerPrefs.HasKey("USRSES"))
        {
            usrSessionNum = PlayerPrefs.GetInt("USRSES");
        }
        else
        {
            usrSessionNum = 0;
            PlayerPrefs.SetInt("USRSES", usrSessionNum);
        }
        usrSessionNum++;
        PlayerPrefs.SetInt("USRSES", usrSessionNum);
        Debug.Log("Session #" + usrSessionNum + " For this user.");
    }
    private void Start()
    {
        roomNumber = 0;
        timeSpent = 0; timeSpentNoPause = 0;
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
        gameTimerActive = false;

        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(pi.leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(pi.rightItems);

        SendDataToEmail("RunStart");
    }
    private void Update()
    {
        if (gameTimerActive)
        {
            timeSpent += Time.deltaTime;
            pointregenTimer += Time.deltaTime;
            if(pointregenTimer >= 60) { pointsLeft += ((flatPointsPerDifficulty * difficulty) / 2f) * Random.Range(0, 1); }
            
        }
        timeSpentNoPause += Time.deltaTime;
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
        CheckForItemGainAndDestroy();
    }
    void CheckForItemGainAndDestroy()
    {
        bool change = false;
        for (int i = 0; i < pi.leftItems.Count; i++)
        {
            leftSnapshot[i] = pi.leftItems[i] - leftSnapshot[i];
            if (leftSnapshot[i] < 0) { pi.OnItemDestroy(i, leftSnapshot[i], "left"); change = true; }
            if (leftSnapshot[i] > 0) { pi.OnItemGain(i, leftSnapshot[i], "left"); change = true; }
        }
        for (int i = 0; i < pi.rightItems.Count; i++)
        {
            rightSnapshot[i] = pi.rightItems[i] - rightSnapshot[i];
            if (rightSnapshot[i] < 0) { pi.OnItemDestroy(i, rightSnapshot[i], "right"); change = true; }
            if (rightSnapshot[i] > 0) { pi.OnItemGain(i, rightSnapshot[i], "right"); change = true; }
        }
        if (change) { changedLastFrame = true; }

        if (change || (!change && changedLastFrame)) { pi.uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory(); if (!change) { changedLastFrame = false; } }

        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(pi.leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(pi.rightItems);
    }
    //NEEDS to be called when the player goes into the next room.
    public void AdvanceToNextRoom()
    {
        SendDataToEmail("RoomExit");

        gameTimerActive = false;
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
        timeSpent += 120f;
        foreach(EnemyHealthManager ehm in activeEhms)
        {
            Destroy(ehm.gameObject);
        }

        SendDataToEmail("RoomEnter");
    }
    public void BeginSpawning()
    {
        gameTimerActive = true;

        List<EnemySpawner> newOrder = new List<EnemySpawner>();
        int initialCount = activeSpawners.Count;
        for(int i = 0; i < initialCount; i++)
        {
            int rand = Random.Range(0, activeSpawners.Count);
            newOrder.Add(activeSpawners[rand]);
            activeSpawners.RemoveAt(rand);
        }
        activeSpawners = newOrder;

        pointsLeft = Random.Range(basePoints.x, basePoints.y); pointsLeft += flatPointsPerDifficulty * difficulty;
        pointsLeft *= difficulty / 2f; pointsLeft = pointsLeft * (1 + (0.5f * (phm.playerItem.leftItems[185] + phm.playerItem.rightItems[185])));
        float delayTime = 3;
        foreach (EnemySpawner spawner in activeSpawners)
        {
            spawner.myDelay = delayTime;
            spawner.StartSpawning();
            delayTime += Random.Range(7f, 13f);
        }
    }
    public void SpawnBoss(string boss)
    {
        gameTimerActive = true;
        switch (boss)
        {
            case "Chimera": Instantiate(chimera);
                break;
        }
    }
    private void OnApplicationQuit()
    {
        SendDataToEmail("GameClose");
    }
    public void SendDataToEmail(string eventT)
    {
        if (!sendData) { return; }
        string eventType = eventT;
        TelemData tdata = PrepareData();

        tdata.eventData = eventType;

        string msg = "START";
        msg += "|(UsrID)"+tdata.usr;
        msg += "|(SessionNum)"+tdata.sessionNum;
        msg += "|(CurTime)"+tdata.eventTime;
        msg += "|(LeftGun)"+tdata.leftGun;
        msg += "|(RightGun)"+tdata.rightGun;
        msg += "|(TimeE)"+tdata.timeElapsed;
        msg += "|(Room)"+tdata.roomNum;
        msg += "|(Diff)"+tdata.difficulty;
        msg += "|(MRSourceOfDmg)"+tdata.mostRecentSourceOfDmg;
        msg += "|(Cash)"+tdata.currentCash;
        string leftInvTxt = "LEFT("+FormatInvToString(tdata.leftInv)+")";
        string rightInvTxt = "RIGHT("+FormatInvToString(tdata.rightInv)+")";
        msg += "|(LInv)"+leftInvTxt;
        msg += "|(RInv)"+rightInvTxt;
        msg += "|END";

        Emailer.SendAnEmail(msg, eventType);
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
            if(item != 0) { result += "ID:"+i+":" + item + ","; }
            i++;
        }
        if(result != "") { result.Remove(result.Length - 1); }

        return result;
    }
    public TelemData PrepareData()
    {
        TelemData tdata = new TelemData();
        tdata.usr = usrID;
        tdata.sessionNum = usrSessionNum.ToString();

        tdata.difficulty = difficulty;
        tdata.timeElapsed = timeSpentNoPause;
        tdata.currentCash = phm.money;
        tdata.roomNum = roomNumber;
        tdata.eventTime = System.DateTime.Now.ToString("U");
        tdata.leftInv = pi.leftItems;
        tdata.rightInv = pi.rightItems;
        tdata.leftGun = pi.gunManager.leftGunScript.gunName;
        tdata.rightGun = pi.gunManager.rightGunScript.gunName;
        if(phm.lastHitMe != null && phm.lastHitMe.data != null) { tdata.mostRecentSourceOfDmg = phm.lastHitMe.data.enemyName; } else { tdata.mostRecentSourceOfDmg = "NULL"; }
        return (tdata);
    }
}
