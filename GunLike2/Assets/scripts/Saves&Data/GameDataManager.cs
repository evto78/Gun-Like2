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
    private void Awake()
    {
        phm = GameObject.Find("Player").GetComponent<HealthManager>();
    }
    private void Start()
    {
        roomNumber = 0;
        timeSpent = 0;
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
        gameTimerActive = false;
    }
    private void Update()
    {
        if (gameTimerActive)
        {
            timeSpent += Time.deltaTime;
        }
        difficulty = Mathf.RoundToInt((difficultySelected * timeSpent / 300f) + 1f);
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
        float delayTime = 1;
        foreach (EnemySpawner spawner in activeSpawners)
        {
            spawner.myDelay = delayTime;
            spawner.StartSpawning();
            delayTime += Random.Range(3f, 5f);
        }
    }
}
