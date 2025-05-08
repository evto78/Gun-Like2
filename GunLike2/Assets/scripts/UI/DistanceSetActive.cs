using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSetActive : MonoBehaviour
{
    Transform playerPos;
    public GameObject affectedObject;
    public float renDistance;
    void Start()
    {
        playerPos = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(affectedObject.transform.position, playerPos.position) < renDistance)
        {
            affectedObject.SetActive(true);
        }
        else
        {
            affectedObject.SetActive(false);
        }
    }
}
