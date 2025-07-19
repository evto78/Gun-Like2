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
        spawnRate = 1f; timer = 0f; spawning = false;
        spawnableEnemies = new List<Spawnable>();
        spawnableEnemies.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
        List<Spawnable> temp = new List<Spawnable>();
        foreach(Spawnable s in spawnableEnemies)
        {
            if(s.type == Spawnable.Type.walker && !canSpawnWalker) { temp.Add(s); }
            if(s.difficultyRequirement > gdm.difficulty) { temp.Add(s); }
        }
        foreach(Spawnable s in temp)
        {
            spawnableEnemies.Remove(s);
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
        spawnableEnemies = new List<Spawnable>();
        spawnableEnemies.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
        List<Spawnable> temp = new List<Spawnable>();
        foreach (Spawnable s in spawnableEnemies)
        {
            if (s.type == Spawnable.Type.walker && !canSpawnWalker) { temp.Add(s); }
            if (s.difficultyRequirement > gdm.difficulty) { temp.Add(s); }
        }
        foreach (Spawnable s in temp)
        {
            spawnableEnemies.Remove(s);
        }
        foreach (Spawnable thing in spawnableEnemies)
        {
            if (thing.pointCost < minCost) { minCost = thing.pointCost; }
        }
        Debug.Log(timer + " START " + gdm.activeSpawners.IndexOf(this));
    }
    private void Update()
    {
        timer -= spawnRate * Time.deltaTime;
        if (!spawning || timer > 0 || gdm.pointsLeft < minCost) { return; }
        Debug.Log("Attempting to spawn: " + timer + " : " + gdm.activeSpawners.IndexOf(this));
        attempts = 0;
        timer = Random.Range(5f, 10f);
        Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]);
    }
    void Spawn(Spawnable thing)
    {
        if(attempts > 25) { return; }
        if (thing.pointCost > gdm.pointsLeft) { attempts++; Spawn(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]); }
        gdm.pointsLeft -= thing.pointCost;

        for(int i = 0; i < Random.Range(thing.amountToSpawn.x, thing.amountToSpawn.y); i++){
            GameObject spawned = Instantiate(thing.thingToSpawn, spawnPoint.position, spawnPoint.rotation); spawned.GetComponent<EnemyHealthManager>().gdm = gdm;
        }
    }
}
