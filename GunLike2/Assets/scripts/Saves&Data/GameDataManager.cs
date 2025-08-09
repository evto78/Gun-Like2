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
    public PlayerItem pi;
    float pointregenTimer;
    //Checking Change
    bool changedLastFrame;
    List<int> leftSnapshot;
    List<int> rightSnapshot;
    [Header("SaveData")]
    public GameObject saveDataReaderPrefab;
    public SaveFileReadWrite instance;
    bool requesting = false;

    [Header("Bosses")]
    public GameObject chimera; float timeTakenToDefeatChimera;

    private void Awake()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
        pi = phm.playerItem;
    }
    private void Start()
    {
        if (instance == null)
        {
            GameObject spawnedSaveData = Instantiate(saveDataReaderPrefab);
            instance = spawnedSaveData.GetComponent<SaveFileReadWrite>();
            instance.gdm = this;
        }

        roomNumber = 0;
        timeSpent = 0; timeSpentNoPause = 0;
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
        gameTimerActive = false;

        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(pi.leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(pi.rightItems);
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
    private void LateUpdate()
    {
        requesting = false;
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
        gameTimerActive = false;
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
        timeSpent += 120f;
        foreach(EnemyHealthManager ehm in activeEhms)
        {
            Destroy(ehm.gameObject);
        }

        instance.AddEmailToQue("RoomEnter");
    }
    public void BeginSpawning()
    {
        if(roomNumber == 0) { instance.AddEmailToQue("RunStart"); }
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
            case "Chimera": 
                Instantiate(chimera); instance.AddEmailToQue("BossSpawned"); instance.data.ChimeraInfo.timesFought++; timeTakenToDefeatChimera = 0f; StartCoroutine(ChimeraBossTimer());
                break;
        }
    }
    public void BossKilled(string boss)
    {
        gameTimerActive = false;
        switch (boss)
        {
            case "Chimera":
                instance.AddEmailToQue("BossKilled");
                GunManager gm = pi.gunManager;
                int leftGun = gm.leftHandVal; SaveFileReadWrite.GunInformation infoL = instance.data.gunInfo[leftGun];
                int rightGun = gm.rightHandVal; SaveFileReadWrite.GunInformation infoR = instance.data.gunInfo[rightGun];
                infoL.winningRuns++; infoR.winningRuns++;
                instance.data.ChimeraInfo.timesDefeated++;
                if(timeTakenToDefeatChimera < instance.data.ChimeraInfo.timeToKillRecord) { instance.data.ChimeraInfo.timeToKillRecord = timeTakenToDefeatChimera; }
                break;
        }
    }
    IEnumerator ChimeraBossTimer()
    {
        Debug.Log("Started");
        while (gameTimerActive)
        {
            timeTakenToDefeatChimera += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Ended: "+timeTakenToDefeatChimera);
    }
    private void OnDisable()
    {
        if (!requesting) { instance.RequestDataUpdate(); requesting = true; }
    }
    private void OnDestroy()
    {
        if (!requesting) { instance.RequestDataUpdate(); requesting = true; }
    }
    private void OnApplicationQuit()
    {
        if (!requesting) { instance.RequestDataUpdate(); requesting = true; }
    }
    public void UpdateRecords()
    {
        GunManager gm = pi.gunManager;
        int leftGun = gm.leftHandVal; SaveFileReadWrite.GunInformation infoL = instance.data.gunInfo[leftGun];
        int rightGun = gm.rightHandVal; SaveFileReadWrite.GunInformation infoR = instance.data.gunInfo[rightGun];
        infoL.kills += gm.leftKillsDATA; infoR.kills += gm.rightKillsDATA;
        infoL.totalDamage += gm.leftDamageDATA; infoR.totalDamage += gm.rightDamageDATA;
        if (gm.leftMaxDmgDATA > infoL.damageRecord) { infoL.damageRecord = gm.leftMaxDmgDATA; }
        if (gm.rightMaxDmgDATA > infoR.damageRecord) { infoR.damageRecord = gm.rightMaxDmgDATA; }
        infoL.bulletsFired += gm.leftBulletsFiredDATA; infoR.bulletsFired += gm.rightBulletsFiredDATA;
        if (gm.leftGunScript.magSize > infoL.magSizeRecord) { infoL.magSizeRecord = (int)gm.leftGunScript.magSize; }
        if (gm.rightGunScript.magSize > infoR.magSizeRecord) { infoR.magSizeRecord = (int)gm.rightGunScript.magSize; }
        infoL.itemsCollected += gm.leftItemsCollectedDATA; infoR.itemsCollected += gm.rightItemsCollectedDATA;
        infoL.elapsedTimeHeld += timeSpentNoPause; infoR.elapsedTimeHeld += timeSpentNoPause;
        infoL.accuracy = (((infoL.accuracy/100)+(gm.leftBulletsFiredDATA/(gm.leftHitsDATA+1)))/2)*100f;
        infoR.accuracy = (((infoR.accuracy/100)+(gm.rightBulletsFiredDATA/(gm.rightHitsDATA+1)))/2)*100f;
        if (difficulty > infoL.difficulyReachedRecord) { infoL.difficulyReachedRecord = (int)difficulty; }
        if (difficulty > infoR.difficulyReachedRecord) { infoR.difficulyReachedRecord = (int)difficulty; }
    }
}
