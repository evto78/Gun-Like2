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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameObject spawnedItem;
            spawnedItem = Instantiate(item, new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f), transform.rotation);
            spawnedItem.GetComponent<Item>().SetItemID(5);
        }
    }
}
