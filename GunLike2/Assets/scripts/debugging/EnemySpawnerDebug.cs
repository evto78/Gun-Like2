using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerDebug : MonoBehaviour
{
    public GameObject enemy;
    public int spawns;
    public float spawnDistance;
    void Start()
    {
        for(int i = 0; i < spawns; i++)
        {
            GameObject spawned = Instantiate(enemy);
            spawned.transform.position = new Vector3(transform.position.x + Random.Range(-1f,1f) * spawnDistance, transform.position.y + Random.Range(-1f, 1f) * spawnDistance, transform.position.z + Random.Range(-1f, 1f) * spawnDistance);
        }
        //spawned
    }

}
