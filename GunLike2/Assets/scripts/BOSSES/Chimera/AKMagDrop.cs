using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKMagDrop : MonoBehaviour
{
    public GameObject shockwave; public float damage;
    Rigidbody rb; public float turnSpeed; public Vector3 turnAngle;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        rb.AddTorque(turnAngle * turnSpeed * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 5f); Destroy(gameObject.GetComponent<Collider>());
            GameObject spawnedShockeave = Instantiate(shockwave, transform.position, transform.rotation);
            spawnedShockeave.GetComponent<Shockwave>().damage = damage;
            spawnedShockeave.GetComponent<Shockwave>().lifetime = 5f;
        }
    }

}
