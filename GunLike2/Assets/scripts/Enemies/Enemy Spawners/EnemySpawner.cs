using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameDataManager gdm;
    PlayerItem pi;
    public Transform spawnPoint;
    public List<Spawnable> spawnableEnemies;
    float spawnRate; float timer;
    bool spawning;
    public Vector2 pointAmount; float pointsLeft;
    float minCost; int attempts;
    public bool canSpawnWalker;
    private void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        pi = gdm.phm.playerItem;
        gdm.activeSpawners.Add(this);
        spawnRate = 1f * (1 + gdm.difficulty/4f); timer = 4f;
        spawnableEnemies = new List<Spawnable>();
        spawnableEnemies.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
        if (!canSpawnWalker)
        {
            List<Spawnable> temp = new List<Spawnable>();
            foreach(Spawnable s in spawnableEnemies)
            {
                if(s.type == Spawnable.Type.walker) { temp.Add(s); }
            }
            foreach(Spawnable s in temp)
            {
                spawnableEnemies.Remove(s);
            }
        }
    }
    private void OnDestroy()
    {
        if (gdm.activeSpawners.Contains(this)) { gdm.activeSpawners.Remove(this); }
    }
    public void StartSpawning()
    {
        spawning = true;
        pointsLeft = Random.Range(pointAmount.x , pointAmount.y * gdm.difficulty);
        pointsLeft = pointsLeft * (1 + (0.5f * (pi.leftItems[185] + pi.rightItems[185])));
        minCost = 999999f;
        foreach(Spawnable thing in spawnableEnemies)
        {
            if(thing.pointCost < minCost) { minCost = thing.pointCost; }
        }
    }
    private void Update()
    {
        timer -= spawnRate * Time.deltaTime;
        if (!spawning || timer > 0) { return; } if(pointsLeft < minCost) { spawning = false; return; }
        attempts = 0;
        Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]);
        timer = 4f;
    }
    void Spawn(Spawnable thing)
    {
        if(attempts > 25) { return; }
        if(thing.pointCost > pointsLeft) { attempts++; Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]); }
        pointsLeft -= thing.pointCost;

        Instantiate(thing.thingToSpawn, spawnPoint.position, spawnPoint.rotation);
    }
}
