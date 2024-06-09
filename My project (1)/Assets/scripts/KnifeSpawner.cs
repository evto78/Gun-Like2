using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{

    public GameObject knife;

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            GameObject spawnedKnife = Instantiate(knife, new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), transform.rotation);
        }
    }
}
