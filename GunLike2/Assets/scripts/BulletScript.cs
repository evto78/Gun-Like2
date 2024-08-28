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
    public float bulSpd;

    public int pierce = 0;

    public int heavySpirits;

    public Collider myCollider;

    List<Collider> collisions = new List<Collider>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);

    }

    public void setStats(float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd, float isHeavy, float givenBulSize, int givenHeavySpirits)
    {
        damage = givenDmg;
        isCrit = isCritHit;
        pierce = givenPierce;
        isAutoWeak = isAutoWeakHit;
        weakDamage = givenWeakDmg;
        bulSpd = givenBulSpd;

        heavySpirits = givenHeavySpirits;

        if (isHeavy != 0f)
        {
            rb.useGravity = true;
            rb.mass = isHeavy;
        }
        else
        {
            rb.useGravity = false;
        }

        transform.localScale = new Vector3(transform.localScale.x * givenBulSize, transform.localScale.y * givenBulSize, transform.localScale.z * givenBulSize);

        GetComponent<Rigidbody>().AddForce(transform.forward * bulSpd, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collision collision)
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

            if (Random.Range(1, 100) <= (50f*(1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().Die();
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

            if (Random.Range(1, 100) <= (50f*(1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                collision.gameObject.GetComponentInParent<EnemyHealthManager>().Die();
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
            collisions.Add(collision.collider);
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collisions.Contains(collision.collider))
        //{
        //Physics.IgnoreCollision(collision.collider, myCollider, false);
        //}
    }

    private void FixedUpdate()
    {
        if (rb.freezeRotation)
        {
            rb.velocity = Vector3.zero;
        }
    }
}