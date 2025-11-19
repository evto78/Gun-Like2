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
        if(Physics.Raycast(new Ray(transform.position, -Vector3.up), out RaycastHit hitDwn, 300f))
        {
            transform.position = hitDwn.point;
        }
        else if(Physics.Raycast(new Ray(transform.position, -Vector3.up), out RaycastHit hitUp, 300f))
        {
            transform.position = hitUp.point;
        }
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        player = GameObject.Find("Player").transform;
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.up * 200f * Time.fixedDeltaTime);
        rb.AddForce((player.position - transform.position).normalized * 100f * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            HealthManager hm = other.gameObject.GetComponent<HealthManager>();
            if (hm.activeEffects[27].x < 30 + (30 * hm.sunflower)) { hm.GiveEffect(27, 30f + (30f * hm.sunflower)); }
            Destroy(gameObject);
        }
    }
}
