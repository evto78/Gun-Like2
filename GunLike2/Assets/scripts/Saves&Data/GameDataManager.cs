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
    public bool gameTimerActive;
    public int roomNumber;
    public HealthManager phm;
    PlayerItem pi;
    float pointregenTimer;
    //Checking Change
    bool changedLastFrame;
    List<int> leftSnapshot;
    List<int> rightSnapshot;

    private void Awake()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
        pi = phm.playerItem;
    }
    private void Start()
    {
        roomNumber = 0;
        timeSpent = 0;
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
        gameTimerActive = false;
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
        timeSpent += 120f;
        foreach(EnemyHealthManager ehm in activeEhms)
        {
            Destroy(ehm.gameObject);
        }
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
}
