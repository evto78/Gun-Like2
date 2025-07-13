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
    float minCost; int attempts;
    public bool canSpawnWalker;
    public float myDelay;
    private void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        pi = gdm.phm.playerItem;
        gdm.activeSpawners.Add(this);
        spawnRate = 1f; timer = 4f;
        spawnableEnemies = new List<Spawnable>();
        spawnableEnemies.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
        if (!canSpawnWalker)
        {
            List<Spawnable> temp = new List<Spawnable>();
            foreach(Spawnable s in spawnableEnemies)
            {
                if(s.type == Spawnable.Type.walker) { temp.Add(s); }
                if(s.difficultyRequirement > gdm.difficulty) { temp.Add(s); }
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
        timer = myDelay;
        minCost = 999999f;
        foreach(Spawnable thing in spawnableEnemies)
        {
            if(thing.pointCost < minCost) { minCost = thing.pointCost; }
        }
    }
    private void Update()
    {
        timer -= spawnRate * Time.deltaTime;
        if (!spawning || timer > 0) { return; } if(gdm.pointsLeft < minCost) { spawning = false; return; }
        attempts = 0;
        timer = Random.Range(5f, 10f);
        Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]);
    }
    void Spawn(Spawnable thing)
    {
        Debug.Log(timer + " | " + thing.name + " | " + gdm.pointsLeft);
        if(attempts > 25) { return; }
        if(thing.pointCost > gdm.pointsLeft) { attempts++; Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]); }
        gdm.pointsLeft -= thing.pointCost;

        for(int i = 0; i < Random.Range(thing.amountToSpawn.x, thing.amountToSpawn.y); i++){
            Instantiate(thing.thingToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
