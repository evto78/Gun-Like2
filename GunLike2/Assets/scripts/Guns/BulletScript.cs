using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Rigidbody rb;
    public ParticleSystem hitParticle;
    public GameObject mesh;

    public GameObject bulletPrefab;

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
    public bool ricochet = false;

    public int heavySpirits;
    public int nuclearBullets;
    public int introTrig;
    public GameObject pairedBullet;
    public bool isTrigLead;
    public float myIsHeavy;

    public Collider myCollider;

    public Vector3 myPos;

    public string whatHandThisComesFrom;

    List<Collider> collisions = new List<Collider>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);

    }

    private void Update()
    {
        if(rb.velocity != Vector3.zero) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
    }

    public void setStats(float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd, float isHeavy, float givenBulSize, int givenHeavySpirits, int givenNuclearBul, bool givenRico, string whatHand, int givenIntroTrig)
    {
        whatHandThisComesFrom = whatHand;

        damage = givenDmg;
        isCrit = isCritHit;
        pierce = givenPierce;
        isAutoWeak = isAutoWeakHit;
        weakDamage = givenWeakDmg;
        bulSpd = givenBulSpd;

        heavySpirits = givenHeavySpirits;
        nuclearBullets = givenNuclearBul;
        introTrig = givenIntroTrig;

        ricochet = givenRico;

        myIsHeavy = isHeavy;
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

    public void IntroTrigSetUp(GameObject givenPairedBullet, bool isLead)
    {
        pairedBullet = givenPairedBullet;
        isTrigLead = isLead;
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
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, whatHandThisComesFrom);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform.position, whatHandThisComesFrom);
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
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critHit", transform.position, whatHandThisComesFrom);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }

            }

            if ((gameObject.GetComponentInParent<EnemyHealthManager>().curHp / gameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                gameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBullets > 0)
            {
                if (Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
                }
            }

        }
        if (gameObject.tag == "EnemyWeakPoint")
        {
            if (!isCrit)
            {
                if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "weakHit", transform.position, whatHandThisComesFrom);
                }

                gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                }

                gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }

            if ((gameObject.GetComponentInParent<EnemyHealthManager>().curHp / gameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                gameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBullets > 0)
            {
                if(Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
                }
            }
        }
        if (!collided && pierce < 1)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            Destroy(mesh);
            collided = true;

            if(introTrig > 0)
            {
                if (isTrigLead)
                {
                    if (pairedBullet.GetComponent<BulletScript>().collided)
                    {
                        //from there to here
                    }
                }
                else
                {
                    if (pairedBullet.GetComponent<BulletScript>().collided)
                    {
                        rb.freezeRotation = false;
                        transform.LookAt(pairedBullet.transform);
                        rb.freezeRotation = true;
                        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                        spawnedBullet.GetComponent<BulletScript>().setStats(damage, isCrit, pierce, isAutoWeak, weakDamage, bulSpd, myIsHeavy, transform.localScale.x, heavySpirits, nuclearBullets, ricochet, whatHandThisComesFrom, 0);
                    }
                }
            }
        }
        else
        {
            pierce -= 1;

            if (ricochet)
            {
                Ray ricoRay = new Ray(transform.position, transform.forward);
                RaycastHit ricoHit;

                myPos = transform.position;
                if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime))))
                {
                    Vector3 reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);

                    //float ricoRotX = 90f - Mathf.Atan2(reflectDir.z, reflectDir.y) * Mathf.Rad2Deg;
                    //float ricoRotY = 90f - Mathf.Atan2(reflectDir.x, reflectDir.z) * Mathf.Rad2Deg;
                    //float ricoRotZ = 90f - Mathf.Atan2(reflectDir.x, reflectDir.y) * Mathf.Rad2Deg;
                    
                    //transform.eulerAngles = new Vector3(ricoRotX, ricoRotY, ricoRotZ);

                    Vector3 storedVelocity = rb.velocity;

                    rb.velocity = Vector3.zero;
                    if (rb.useGravity == true) { rb.velocity = ((reflectDir * storedVelocity.magnitude) / 2f) + Vector3.up * 2 + transform.forward * 2; }
                    if (rb.useGravity == false) { rb.velocity = ((reflectDir * storedVelocity.magnitude) / 2f) + transform.forward * 2; }

                    transform.rotation = Quaternion.LookRotation(rb.velocity);
                }
            }
            else
            {

            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //RunOnCollide(collision.gameObject);
    }

    private void FixedUpdate()
    {
        myPos = transform.position;
        if (Physics.Raycast(myPos, rb.velocity, out RaycastHit hit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime))))
        {
            transform.position = hit.point;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint") { RunOnCollide(hit.collider.gameObject); }
        }

        if (rb.freezeRotation)
        {
            rb.velocity = Vector3.zero;
        }
    }
}