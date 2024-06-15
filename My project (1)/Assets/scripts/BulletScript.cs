using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public ParticleSystem hitParticle;
    public GameObject mesh;

    bool collided = false;

    public Camera mainCamera;
    Ray ray;
    RaycastHit hit;

    public float damage;
    public bool isCrit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);

    }

    public void setStats(float givenDmg, bool isCritHit)
    {
        damage = givenDmg;
        isCrit = isCritHit;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isCrit)
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform);
            }
            else
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critHit", transform);
            }
        }
        if (collision.gameObject.tag == "EnemyWeakPoint")
        {
            if (!isCrit)
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform);
            }
            else
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform);
            }
        }
        if (!collided)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            Destroy(mesh);
            collided = true;
        }
        
    }

    private void FixedUpdate()
    {
        if (rb.freezeRotation)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
