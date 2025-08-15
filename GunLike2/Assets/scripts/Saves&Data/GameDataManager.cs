using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    [Header("Info")]
    public List<EnemyHealthManager> activeEhms = new List<EnemyHealthManager>();
    public List<EnemySpawner> activeSpawners = new List<EnemySpawner>();
    public HealthManager phm;
    public PlayerItem pi;
    //Checking Change
    bool changedLastFrame;
    List<int> leftSnapshot;
    List<int> rightSnapshot;
    
    [Header("Difficulty")]
    public float difficulty;
    public int difficultyIDSelected;
    float difficultyProgressionModifier;
    public List<int> mutatedRules = new List<int>();
    public List<float> mutatedStatModifiers = new List<float>();
    public Spawnable mutatedEnemySelected;

    [Header("Timer")]
    public float timeSpent;
    public float timeSpentNoPause;
    public bool gameTimerActive;

    [Header("Points System")]
    float pointregenTimer;
    public int roomNumber;
    public Vector2 basePoints; public float flatPointsPerDifficulty;
    public float pointsLeft;

    [Header("SaveData")]
    public GameObject saveDataReaderPrefab;
    public SaveFileReadWrite instance;
    bool requesting = false;

    [Header("Bosses")]
    public GameObject chimera; float timeTakenToDefeatChimera;

    private void Awake()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
        phm.gearscript.Turn(roomNumber);
        pi = phm.playerItem;

        mutatedEnemySelected = null;
        mutatedRules = new List<int>();
        mutatedStatModifiers = new List<float>(); for (int i = 0; i < 29; i++) { mutatedStatModifiers.Add(1f); }

        difficultyIDSelected = 1;
        if (PlayerPrefs.HasKey("SELECTEDDIFFICULTY"))
        {
            difficultyIDSelected = PlayerPrefs.GetInt("SELECTEDDIFFICULTY");
            switch (difficultyIDSelected)
            {
                case 0:
                    difficultyProgressionModifier = 0.5f; phm.freeRelaxedRevive = true;//2x base HP and HP REGEN, 2x more cash dropped from enemies, bosses are less aggresive
                    break;
                case 1:
                    difficultyProgressionModifier = 1f;
                    break;
                case 2:
                    difficultyProgressionModifier = 2f;
                    break;
                case 3:
                    difficultyProgressionModifier = 5f;
                    break;
                case 4:
                    ReadAndApplyMutatedRules();
                    break;
            }
        }
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
        difficulty = Mathf.RoundToInt((difficultyProgressionModifier * timeSpent / 300f) + 1f);
        gameTimerActive = false;

        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(pi.leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(pi.rightItems);

        pi.ItemsFromMutatedModifcataion(mutatedRules);
    }
    void ReadAndApplyMutatedRules()
    {
        mutatedRules = new List<int>();
        mutatedStatModifiers = new List<float>(); for (int i = 0; i < 29; i++) { mutatedStatModifiers.Add(1f); }
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE1"));
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE2"));
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE3"));
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE4"));
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE5"));
        mutatedRules.Add(PlayerPrefs.GetInt("MUTATEDRULE6"));
        difficultyProgressionModifier = 1f;
        for(int i = 0; i < mutatedRules.Count; i++)
        {
            int rule = mutatedRules[i];
            switch (rule)
            {
                case 0: difficultyProgressionModifier += 1f; break; // +1x difficulty progression
                case 1: break; // Enemies have a chance to explode - Handled outside
                case 2: break; // Crate Crabs can be spawned naturaly - Handled outside
                case 3: break; // All enemies are "Mutated" - Handled outside
                case 4: break; // 3x cash drop on enemy killed - Handled outside
                case 5: difficultyProgressionModifier += 2f; break; // +2x difficulty progression
                case 6: break; // Enemies have a chance to drop an item on death - Handled outside
                case 7: MutatedRandomStatMult(2f, i); break; // 2x to a random stat
                case 8: break; // Guns are randomized after every room
                case 9: MutatedRandomStatMult(0.5f, i); break; // 0.5x to a random stat
                case 10: List<Spawnable> options = new List<Spawnable>(); options.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
                    foreach(Spawnable enemy in options) { if(enemy.enemyName == PlayerPrefs.GetString("MUTATEDRULELONEENEMYSLOT"+i.ToString())) { mutatedEnemySelected = enemy; } } 
                    break; // All enemies are now a random enemy - Handled outside
                case 11: break; // No gravity, shooting knocks you back relative to bulspeed - Handled outsde
                case 12: break; // Start with 5 random items on each gun - Handled outside
            }
        }
    }
    void MutatedRandomStatMult(float mult, int slot)
    {
        switch (mult)
        {
            case 0.5f: mutatedStatModifiers[PlayerPrefs.GetInt("MUTATEDRULEHALFSTATSLOT"+slot.ToString())] *= mult; break;
            case 2f: mutatedStatModifiers[PlayerPrefs.GetInt("MUTATEDRULEDOUBLESTATSLOT"+slot.ToString())] *= mult; break;
        }
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
        difficulty = Mathf.RoundToInt((difficultyProgressionModifier * timeSpent / 300f) + 1f);
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
            if (leftSnapshot[i] < 0) { pi.OnItemDestroy(i, leftSnapshot[i], "left"); change = true; changedLastFrame = true; }
            if (leftSnapshot[i] > 0) { pi.OnItemGain(i, leftSnapshot[i], "left"); change = true; changedLastFrame = true; }
            rightSnapshot[i] = pi.rightItems[i] - rightSnapshot[i];
            if (rightSnapshot[i] < 0) { pi.OnItemDestroy(i, rightSnapshot[i], "right"); change = true; changedLastFrame = true; }
            if (rightSnapshot[i] > 0) { pi.OnItemGain(i, rightSnapshot[i], "right"); change = true; changedLastFrame = true; }
        }

        if (changedLastFrame) { pi.uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory(); if (!change) { changedLastFrame = false; } }

        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(pi.leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(pi.rightItems);
    }
    //NEEDS to be called when the player goes into the next room.
    public void AdvanceToNextRoom()
    {
        if (mutatedRules.Contains(8)) { phm.playerItem.gunManager.RandomizeHeldGuns(); }
        gameTimerActive = false;
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
        phm.gearscript.Turn(roomNumber);
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
