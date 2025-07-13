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
        roomNumber += 1;
        phm.attackedThisRoom = false;
        phm.brokenSpeakerItemDropped = false;
        timeSpent += 120f;
    }
    public void BeginSpawning()
    {
        List<int> index = new List<int>(); int offset = Random.Range(0,activeSpawners.Count);
        for(int i = 0; i < activeSpawners.Count; i++)
        {
            int temp1 = i + offset; if(temp1 > index.Count - 1) { temp1 = temp1-index.Count-1; }
            index.Add(temp1);
        }
        List<EnemySpawner> newOrder = new List<EnemySpawner>();
        for(int i = 0; i < activeSpawners.Count; i++)
        {
            newOrder.Add(activeSpawners[index[i]]);
        }
        activeSpawners = newOrder;
        pointsLeft = Random.Range(basePoints.x, basePoints.y); pointsLeft += flatPointsPerDifficulty * difficulty;
        pointsLeft *= difficulty / 2f; pointsLeft = pointsLeft * (1 + (0.5f * (phm.playerItem.leftItems[185] + phm.playerItem.rightItems[185])));
        float delayTime = 1;
        foreach (EnemySpawner spawner in activeSpawners)
        {
            delayTime += Random.Range(3f, 5f);
            spawner.myDelay = delayTime;
            spawner.StartSpawning();
        }
    }
}
