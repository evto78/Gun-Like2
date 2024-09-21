using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{

    public GameObject enemy;
    int counter = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            GameObject spawnedEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z), transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameObject spawnedEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z), transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            counter = 300;
        }
        if (counter > 0)
        {
            GameObject spawnedEnemy = Instantiate(enemy, new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z), transform.rotation);
            counter--;
        }
    }
}
