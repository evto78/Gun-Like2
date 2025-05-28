using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunkyAxe : MonoBehaviour
{
    public float damage;
    private void Update()
    {
        transform.Rotate(Vector3.right * 120f * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, "self");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "EnemyWeakPoint")
        {
            collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage*2f, false, "weakHit", transform.position, "self");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
