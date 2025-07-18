using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateCrabGlob : MonoBehaviour
{
    Rigidbody rb; public float speed; public float damage; Transform player; public EnemyHealthManager ehm; public float gravity; public float turnSpeed;
    float spinamount; public GameObject hitEffect;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = ehm.playerHM.transform;
        spinamount = 0f;
    }
    void Update()
    {
        rb.AddForce((player.position-transform.position).normalized * speed * Time.deltaTime);
        rb.AddForce(Vector3.up * -gravity * Time.deltaTime);
        if (rb.velocity.magnitude != 0) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
        spinamount += turnSpeed * Time.deltaTime;
        transform.Rotate(transform.forward * spinamount);
        if(spinamount > 360) { spinamount -= 360; }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ehm.playerHM.TakeDamage(damage, false, ehm);
            hitEffect.SetActive(true);
            hitEffect.transform.SetParent(null);
            Destroy(hitEffect, 3f);
            Destroy(gameObject);
        }
    }
}
