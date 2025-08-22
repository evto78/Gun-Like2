using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameDataManager gdm;
    PlayerItem pi;
    public Transform spawnPoint;
    public List<Spawnable> spawnableEnemies;
    public List<SpawnableWave> spawnableWaves;
    public List<SpawnableWave> weightedSpawnableWaves = new List<SpawnableWave>();
    float spawnRate; float timer;
    bool spawning;
    float minCost; int attempts;
    public bool canSpawnWalker;
    public float myDelay;
    bool spawningWave; bool pauseTimer; public GameObject myLock;
    private void Start()
    {
        myLock.SetActive(false);
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
            if (gdm.mutatedRules.Count > 0 && gdm.mutatedRules.Contains(2) && s.enemyName == "Crate Crab") { temp.Remove(s); }
        }
        foreach (Spawnable s in temp)
        {
            spawnableEnemies.Remove(s);
        }
        if(gdm.mutatedEnemySelected != null) { spawnableEnemies = new List<Spawnable>(); spawnableEnemies.Add(gdm.mutatedEnemySelected); 
            if(gdm.mutatedEnemySelected.type == Spawnable.Type.walker && !canSpawnWalker) { spawnableEnemies = new List<Spawnable>(); }
        }
    }
    private void OnDestroy()
    {
        if (gdm.activeSpawners.Contains(this)) { gdm.activeSpawners.Remove(this); }
    }
    IEnumerator PrepareSpawnables()
    {
        spawnableEnemies = new List<Spawnable>();
        spawnableEnemies.AddRange(Resources.LoadAll<Spawnable>("Enemies"));
        List<Spawnable> temp = new List<Spawnable>();
        foreach (Spawnable s in spawnableEnemies)
        {
            pauseTimer = true;
            if (s.type == Spawnable.Type.walker && !canSpawnWalker) { temp.Add(s); }pauseTimer = true;
            if (s.difficultyRequirement > gdm.difficulty) { temp.Add(s); }
            if (gdm.mutatedRules.Count > 0 && gdm.mutatedRules.Contains(2) && s.enemyName == "Crate Crab") { temp.Remove(s); }
            yield return new WaitForEndOfFrame();
        }
        foreach (Spawnable s in temp)
        {
            pauseTimer = true;
            spawnableEnemies.Remove(s);
            yield return new WaitForEndOfFrame();
        }
        if (gdm.mutatedEnemySelected != null)
        {
            spawnableEnemies = new List<Spawnable>(); spawnableEnemies.Add(gdm.mutatedEnemySelected);
            if (gdm.mutatedEnemySelected.type == Spawnable.Type.walker && !canSpawnWalker) { spawnableEnemies = new List<Spawnable>(); }
        }
        foreach (Spawnable thing in spawnableEnemies)
        {
            pauseTimer = true;
            if (thing.pointCost < minCost) { minCost = thing.pointCost; }
            yield return new WaitForEndOfFrame();
        }
        pauseTimer = false;
        yield return null;
    }
    IEnumerator PrepareWaves()
    {
        spawnableWaves = new List<SpawnableWave>();
        if(gdm.mutatedEnemySelected != null) { yield return null; }
        spawnableWaves.AddRange(Resources.LoadAll<SpawnableWave>("Enemy Waves"));
        List<SpawnableWave> temp = new List<SpawnableWave>(); List<Spawnable> temp2 = new List<Spawnable>();
        foreach (SpawnableWave sw in spawnableWaves)
        {
            pauseTimer = true;
            float diffReq = 0; float cost = 0;
            temp2 = new List<Spawnable>(); temp2.AddRange(sw.spawnables); foreach(Spawnable s in temp2) {
                if (s.difficultyRequirement > diffReq) { diffReq = s.difficultyRequirement; }
                if (s.type == Spawnable.Type.walker && !canSpawnWalker) { temp.Add(sw); break; }
                cost += s.pointCost / 2;
            }
            if (diffReq + sw.additonalDifficultyRequirement > gdm.difficulty) { temp.Add(sw); }
            sw.cost = cost;
            sw.difficultyReq = diffReq + sw.additonalDifficultyRequirement;
            yield return new WaitForEndOfFrame();
        }
        foreach (SpawnableWave sw in temp)
        {
            pauseTimer = true;
            spawnableWaves.Remove(sw);
        }
        weightedSpawnableWaves = new List<SpawnableWave>();
        foreach (SpawnableWave sw in spawnableWaves)
        {
            pauseTimer = true;
            for (int i = 0; i < sw.weight; i++) { weightedSpawnableWaves.Add(sw); }
            yield return new WaitForEndOfFrame();
        }

        //Shuffle
        List<int> newIndexs = new List<int>();
        List<SpawnableWave> staticList = new List<SpawnableWave>(); foreach(SpawnableWave sw in weightedSpawnableWaves) { pauseTimer = true; staticList.Add(sw); newIndexs.Add(-1); yield return new WaitForEndOfFrame(); }
        int x = 0; List<int> indexesLeft = new List<int>(); foreach(int i in newIndexs) { indexesLeft.Add(x); x++; } x = 0;
        for(int i = 0; i < newIndexs.Count; i++) { int tarIndex = indexesLeft[Random.Range(0, indexesLeft.Count)]; newIndexs[i] = tarIndex; indexesLeft.Remove(tarIndex); }
        foreach(int i in newIndexs) { pauseTimer = true; weightedSpawnableWaves[x] = staticList[i]; x++; yield return new WaitForEndOfFrame(); }

        pauseTimer = false;
        yield return null;
    }
    public void StartSpawning()
    {
        myLock.SetActive(false);
        pauseTimer = true;
        spawningWave = false;
        spawning = true;
        timer = myDelay;
        minCost = 999999f;

        StartCoroutine(PrepareSpawnables());
        StartCoroutine(PrepareWaves());
    }
    private void Update()
    {
        if (gdm.pointsLocked) { myLock.SetActive(true); }
        if (!pauseTimer) { timer -= spawnRate * Time.deltaTime; }
        if (!spawning || timer > 0 || gdm.pointsLeft < minCost || spawningWave || pauseTimer) { return; }
        attempts = 0;
        timer = Random.Range(5f, 10f);
        if(gdm.activeEhms.Count > gdm.unroundedDiff * 15) { return; }
        StartCoroutine(AttemptSpawn());
    }
    IEnumerator AttemptSpawn()
    {
        pauseTimer = true;
        bool selected = false; int attemptsLeft = 25;
        if(weightedSpawnableWaves.Count > 0 && gdm.mutatedEnemySelected == null)
        { while (!selected && attemptsLeft > 0)
        {
            SpawnableWave sw = weightedSpawnableWaves[Random.Range(0, weightedSpawnableWaves.Count)];
            if (sw.difficultyReq <= gdm.difficulty && sw.cost <= gdm.pointsLeft)
            {
                StartCoroutine(SpawnWave(sw));
                selected = true;
            }
            attemptsLeft--;
            yield return new WaitForEndOfFrame();
        } }
        
        if (!selected)
        {
            SpawnRand(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]);
        }
        pauseTimer = false;
        yield return null;
    }
    void SpawnRand(Spawnable thing)
    {
        if(attempts > 25) { return; }
        if (thing.pointCost > gdm.pointsLeft) { attempts++; SpawnRand(spawnableEnemies[Random.Range(0, spawnableEnemies.Count)]); return; }
        gdm.pointsLeft -= thing.pointCost;

        for (int i = 0; i < Random.Range(thing.amountToSpawn.x, thing.amountToSpawn.y); i++){
            SpawnEnemyGiven(thing);
        }
    }
    IEnumerator SpawnWave(SpawnableWave wave)
    {
        spawningWave = true; pauseTimer = true;

        for(int i = 0; i < wave.spawnables.Count; i++)
        {
            SpawnEnemyGiven(wave.spawnables[i]);
            yield return new WaitForSeconds(wave.timeBetween[i]);
        }

        spawningWave = false; pauseTimer = false;
        yield return null;
    }
    void SpawnEnemyGiven(Spawnable enemy)
    {
        if (gdm.pointsLocked) { return; }
        GameObject spawned = Instantiate(enemy.thingToSpawn, spawnPoint.position, spawnPoint.rotation); spawned.GetComponent<EnemyHealthManager>().gdm = gdm;
    }
}
