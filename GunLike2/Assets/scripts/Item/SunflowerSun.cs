using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerSun : MonoBehaviour
{
    Rigidbody rb;
    Transform player;
    void Start()
    {
        Destroy(gameObject, 50f);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        player = GameObject.Find("Player").transform;
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * 200f * Time.fixedDeltaTime);
        rb.AddForce((player.position - transform.position) * 5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
