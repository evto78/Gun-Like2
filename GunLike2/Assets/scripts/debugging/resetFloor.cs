using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetFloor : MonoBehaviour
{
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.transform.position = Vector3.up * 10f;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
