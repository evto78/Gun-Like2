using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrbitPoint : MonoBehaviour
{

    public GameObject player;
    public float orbitSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.up, (orbitSpeed + Random.Range(-1f, 2f)) * Time.deltaTime);
    }
}
