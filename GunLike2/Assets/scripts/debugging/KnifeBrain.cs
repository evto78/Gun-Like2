using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBrain : MonoBehaviour
{
    public flyingEnemyNavController navController;
    public GameObject player;
    MeshRenderer mr;

    public Material leadMat;
    public Material followMat;

    public bool isLead;
    public int spawnAmount;
    public int spawnVariance;
    public GameObject knifePrefab;

    void Start()
    {
        player = GameObject.Find("Player");
        mr = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        navController = GetComponent<flyingEnemyNavController>();

        if (isLead)
        {
            navController.player = player;

            SpawnFollowers(Random.Range(spawnAmount-spawnVariance, spawnAmount+spawnVariance));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLead) 
        { 
            mr.material = leadMat; 
        }
        else 
        { 
            mr.material = followMat; 

        }

        if(navController.player == null)
        {
            navController.player = player;
        }
    }

    void SpawnFollowers(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject spawned = Instantiate(knifePrefab);
            spawned.transform.position = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z + Random.Range(-2f, 2f));
            spawned.GetComponent<KnifeBrain>().isLead = false;
            spawned.GetComponent<flyingEnemyNavController>().player = transform.gameObject;
        }
    }
}
