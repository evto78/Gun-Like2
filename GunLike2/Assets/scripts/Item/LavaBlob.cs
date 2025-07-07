using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBlob : MonoBehaviour
{
    public float damage;
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > 0) { transform.LookAt(transform.position + rb.velocity); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, HitType.ht.normal, transform.position, "self");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "EnemyWeakPoint")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * 2f, false, HitType.ht.weak, transform.position, "self");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
