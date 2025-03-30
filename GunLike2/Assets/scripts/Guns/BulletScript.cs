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
    public int jam;
    bool isFireSpon;
    public GameObject fireSponEffect;
    bool isSharperSpon;
    public GameObject sharperSponEffect;
    bool isSilverSpon;
    public GameObject silverSponEffect;
    bool isHelpingSpon;
    public GameObject helpingSponEffect;
    bool isCoolSpon;
    public GameObject coolSponEffect;
    bool isFastFireSpon;
    public GameObject fastSponEffect;
    bool isLargeSpon;
    public GameObject largeSponEffect;

    public Collider myCollider;

    public Vector3 myPos;

    public string whatHandThisComesFrom;

    List<Collider> collisions = new List<Collider>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);
        collided = false;
    }

    private void Update()
    {
        if(rb.velocity != Vector3.zero) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
    }
    public void setStats(float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd, 
        float givenBulSize, bool givenRico, string whatHand, float isHeavy, int givenHeavySpirits, int givenNuclearBul, int givenIntroTrig, 
        int givenJam, float chanceForFire, float chanceForSharper, float chanceForSilver, float chanceForHelping, float chanceForCool, float chanceForFastFire, float chanceForLarge)
    {
        if(Random.Range(1, 100) < chanceForFire) { isFireSpon = true; fireSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSharper) { isSharperSpon = true; sharperSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSilver) { isSilverSpon = true; silverSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForHelping) { isHelpingSpon = true; helpingSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForCool) { isCoolSpon = true; coolSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForFastFire) { isFastFireSpon = true; fastSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForLarge) { isLargeSpon = true; largeSponEffect.SetActive(true); }

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
        jam = givenJam;

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

        Vector3 forceDir = transform.forward * bulSpd;

        rb.AddForce(forceDir, ForceMode.Impulse);
        if (name == "TRIGBULLET")
        {
            rb.AddForce(forceDir, ForceMode.VelocityChange);
        }
        myPos = transform.position;
        if (Physics.Raycast(myPos, forceDir, out RaycastHit hit, Vector3.Distance(myPos, (myPos + forceDir * Time.fixedDeltaTime))))
        {
            transform.position = hit.point;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged") { RunOnCollide(hit.collider.gameObject); }
        }
    }

    public void IntroTrigSetUp(GameObject givenPairedBullet, bool isLead)
    {
        pairedBullet = givenPairedBullet;
        isTrigLead = isLead;
    }

    void RunOnHit(GameObject hit)
    {
        hit.GetComponentInParent<EnemyHealthManager>().OnHitEffect(jam);
        if (isFireSpon) { hit.GetComponentInParent<EnemyHealthManager>().GiveEffect("burn", 1f); }
        if (isSharperSpon) { hit.GetComponentInParent<EnemyHealthManager>().GiveEffect("bleed", 1f); }
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
                        RunOnHit(gameObject);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(gameObject);
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
                        RunOnHit(gameObject);
                    }

                    gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(gameObject);
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
                    RunOnHit(gameObject);
                }

                gameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (gameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    gameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                    RunOnHit(gameObject);
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
            //transform.SetParent(gameObject.transform);
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
                        GameObject spawnedBullet = Instantiate(bulletPrefab, pairedBullet.transform.position, pairedBullet.transform.rotation);
                        spawnedBullet.name = "TRIGBULLET";
                        spawnedBullet.transform.LookAt(transform);
                        spawnedBullet.GetComponent<BulletScript>().setStats(damage, isCrit, pierce+1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam, 0, 0, 0, 0, 0, 0, 0);
                        spawnedBullet.GetComponent<BulletScript>().mainCamera = Camera.main;

                        spawnedBullet.GetComponent<BulletScript>().collided = false;
                    }
                }
                else
                {
                    if (pairedBullet.GetComponent<BulletScript>().collided)
                    {
                        //from here to there
                        rb.freezeRotation = false;
                        transform.LookAt(pairedBullet.transform);
                        rb.freezeRotation = true;
                        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
                        spawnedBullet.name = "TRIGBULLET";
                        spawnedBullet.GetComponent<BulletScript>().setStats(damage, isCrit, pierce+1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam, 0, 0, 0, 0, 0, 0, 0);
                        spawnedBullet.GetComponent<BulletScript>().mainCamera = Camera.main;

                        spawnedBullet.GetComponent<BulletScript>().collided = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (!collided) { RunOnCollide(collision.gameObject); }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //if (!collided) { RunOnCollide(collision.gameObject); }
    }

    private void FixedUpdate()
    {
        myPos = transform.position;
        if (Physics.Raycast(myPos, rb.velocity, out RaycastHit hit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime))))
        {
            transform.position = hit.point;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged") { RunOnCollide(hit.collider.gameObject); }
        }

        if (rb.freezeRotation && rb.velocity.magnitude > Vector3.zero.magnitude)
        {
            //rb.velocity = Vector3.zero;
            //Debug.Log("Set to 0 " + name);
        }
    }
}