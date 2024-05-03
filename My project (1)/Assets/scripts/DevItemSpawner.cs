using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevItemSpawner : MonoBehaviour
{
    public GameObject item;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.End))
        {
            SpawnItem(Random.Range(0,10));
        }
        
    }

    private void SpawnItem(int iD)
    {
        GameObject spawnedItem;
        spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z + 5f), transform.rotation);
        spawnedItem.GetComponent<Item>().SetItemID(iD);
    }
}
