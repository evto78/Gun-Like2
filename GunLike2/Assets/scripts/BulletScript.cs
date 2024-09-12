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

    private void RunOnCollide(GameObject gameObject)
    {
        if (gameObject.tag == "Enemy")
        {
            if (!isCrit)
            {
                if (!isAutoWeak)
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (!isAutoWeak)
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critHit", transform);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }

            }

            if (Random.Range(1, 100) <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                gameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }


        }
        if (gameObject.tag == "EnemyWeakPoint")
        {
            if (!isCrit)
            {
                if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "weakHit", transform);
                }

                gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "critWeakHit", transform);
                }

                gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }

            if (Random.Range(1, 100) <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                gameObject.GetComponentInParent<EnemyHealthManager>().Die();
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

    private void OnTriggerEnter(Collider collision)
    {
        RunOnCollide(collision.gameObject);
    }

    private void FixedUpdate()
    {
        //Debug.Log("Transform : " + transform.position.x + " " + transform.position.y + " " + transform.position.z + " ");
        //Debug.Log("Velocity : " + rb.velocity.x + " " + rb.velocity.y + " " + rb.velocity.z + " ");
        //Debug.Log("Distance : " + Vector3.Distance(transform.position, (transform.position + rb.velocity)));
        if (Physics.Raycast(transform.position, rb.velocity, out RaycastHit hit, Vector3.Distance(transform.position, (transform.position + rb.velocity))))
        {
            //RunOnCollide(hit.collider.gameObject);
        }

        if (rb.freezeRotation)
        {
            rb.velocity = Vector3.zero;
        }
    }
}