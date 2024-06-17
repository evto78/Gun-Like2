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
    public bool isAutoWeak;
    public float weakDamage;

    public int pierce = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);

    }

    public void setStats(float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg)
    {
        damage = givenDmg;
        isCrit = isCritHit;
        pierce = givenPierce;
        isAutoWeak = isAutoWeakHit;
        weakDamage = givenWeakDmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isCrit)
            {
                if (!isAutoWeak)
                {
                    collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform);
                }
                else
                {
                    collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform);
                }
            }
            else
            {
                if (!isAutoWeak)
                {
                    collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critHit", transform);
                }
                else
                {
                    collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform);
                }
            }
        }
        if (collision.gameObject.tag == "EnemyWeakPoint")
        {
            if (!isCrit)
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "weakHit", transform);
            }
            else
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "critWeakHit", transform);
            }
        }
        if (!collided && pierce < 1)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            Destroy(mesh);
            collided = true;
        }
        else
        {
            pierce -= 1;
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
