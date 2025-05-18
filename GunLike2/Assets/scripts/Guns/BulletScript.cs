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
    Vector3 collidedPos;

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
    public int advTrig;
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
        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }

        Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.cyan);
    }
    public void setStats(float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd, 
        float givenBulSize, bool givenRico, string whatHand, float isHeavy, int givenHeavySpirits, int givenNuclearBul, int givenIntroTrig, 
        int givenJam, float chanceForFire, float chanceForSharper, float chanceForSilver, float chanceForHelping, float chanceForCool,
        float chanceForFastFire, float chanceForLarge, int givenAdvTrig)
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
        advTrig = givenAdvTrig;
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
            transform.position = hit.point - transform.forward;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged" || hit.collider.gameObject.layer == 0) { RunOnCollide(hit.collider.gameObject); }
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
        if (isSilverSpon) { hit.GetComponentInParent<EnemyHealthManager>().GiveEffect("lucky", 1f); }
    }

    private void RunOnCollide(GameObject givenGameObject)
    {
        //rb.velocity = Vector3.zero;
        collidedPos = transform.position;
        //transform.position = collidedPos;
        //transform.position = givenGameObject.transform.position;

        if (givenGameObject.tag == "Enemy")
        {
            if (!isCrit)
            {
                if (!isAutoWeak)
                {
                    if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "normalHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(givenGameObject);
                    }

                    givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "weakHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(givenGameObject);
                    }

                    givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                if (!isAutoWeak)
                {
                    if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(givenGameObject);
                    }

                    givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                    {
                        givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                        RunOnHit(givenGameObject);
                    }

                    givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
                }

            }

            if ((givenGameObject.GetComponentInParent<EnemyHealthManager>().curHp / givenGameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                givenGameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBullets > 0)
            {
                if (Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
                {
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
                }
            }

        }
        if (givenGameObject.tag == "EnemyWeakPoint")
        {
            if (!isCrit)
            {
                if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "weakHit", transform.position, whatHandThisComesFrom);
                    RunOnHit(givenGameObject);
                }

                givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
                    RunOnHit(givenGameObject);
                }

                givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }

            if ((givenGameObject.GetComponentInParent<EnemyHealthManager>().curHp / givenGameObject.GetComponentInParent<EnemyHealthManager>().maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
            {
                givenGameObject.GetComponentInParent<EnemyHealthManager>().Die();
            }

            if (nuclearBullets > 0)
            {
                if(Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
                {
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
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
            gameObject.GetComponent<Collider>().enabled = false;

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
                        spawnedBullet.GetComponent<BulletScript>().setStats(damage, isCrit, pierce+1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam, 0, 0, 0, 0, 0, 0, 0, 0);
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
                        spawnedBullet.GetComponent<BulletScript>().setStats(damage, isCrit, pierce+1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam, 0, 0, 0, 0, 0, 0, 0, 0);
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
                if(advTrig > 0) { if(Random.Range(1, 100) > 20) { pierce += 1; } }

                Debug.Log("Begining Rico");
                Ray ricoRay = new Ray(transform.position, transform.forward);
                RaycastHit ricoHit;

                myPos = transform.position;
                //Checkforward
                if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                {
                    Debug.Log("Rico Hit! FORWARDS");
                    Vector3 reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
                    Debug.Log("Reflect Dir found: " + reflectDir);

                    //float ricoRotX = 90f - Mathf.Atan2(reflectDir.z, reflectDir.y) * Mathf.Rad2Deg;
                    //float ricoRotY = 90f - Mathf.Atan2(reflectDir.x, reflectDir.z) * Mathf.Rad2Deg;
                    //float ricoRotZ = 90f - Mathf.Atan2(reflectDir.x, reflectDir.y) * Mathf.Rad2Deg;

                    //transform.eulerAngles = new Vector3(ricoRotX, ricoRotY, ricoRotZ);

                    Vector3 storedVelocity = rb.velocity;

                    rb.velocity = Vector3.zero;
                    if (rb.useGravity == true) { rb.AddForce(((((reflectDir * storedVelocity.magnitude) / 1f) + Vector3.up * 2) + transform.forward * 2), ForceMode.VelocityChange); }
                    if (rb.useGravity == false) { rb.AddForce((((reflectDir * storedVelocity.magnitude) / 1f) + transform.forward * 2),ForceMode.VelocityChange); }
                    Debug.DrawRay(transform.position, (((reflectDir * storedVelocity.magnitude) / 1f) + transform.forward * 2) * Time.deltaTime, Color.green);
                    Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.red);


                    transform.rotation = Quaternion.LookRotation(rb.velocity);
                }
                //Checkbackward
                else
                {
                    ricoRay = new Ray(transform.position, -transform.forward);

                    myPos = transform.position;
                    if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                    {
                        Debug.Log("Rico Hit! BACKWARDS!... Adjusting position for better reflect.");
                        transform.position = transform.position - transform.forward * (rb.velocity * Time.deltaTime).magnitude;
                        ricoRay = new Ray(transform.position, transform.forward);

                        myPos = transform.position;
                        if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 6f))))
                        {
                            Vector3 reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
                            Debug.Log("Reflect Dir found: " + reflectDir);

                            //float ricoRotX = 90f - Mathf.Atan2(reflectDir.z, reflectDir.y) * Mathf.Rad2Deg;
                            //float ricoRotY = 90f - Mathf.Atan2(reflectDir.x, reflectDir.z) * Mathf.Rad2Deg;
                            //float ricoRotZ = 90f - Mathf.Atan2(reflectDir.x, reflectDir.y) * Mathf.Rad2Deg;

                            //transform.eulerAngles = new Vector3(ricoRotX, ricoRotY, ricoRotZ);

                            Vector3 storedVelocity = rb.velocity;

                            rb.velocity = Vector3.zero;
                            if (rb.useGravity == true) { rb.AddForce(((((reflectDir * storedVelocity.magnitude) / 2f) + Vector3.up * 2) + transform.forward * 2), ForceMode.VelocityChange); }
                            if (rb.useGravity == false) { rb.AddForce((((reflectDir * storedVelocity.magnitude) / 2f) + transform.forward * 2), ForceMode.VelocityChange); }
                            Debug.DrawRay(transform.position, (((reflectDir * storedVelocity.magnitude) / 2f) + transform.forward * 2) * Time.deltaTime, Color.green);
                            Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.red);


                            transform.rotation = Quaternion.LookRotation(rb.velocity);
                        }
                    }
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
        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }
        myPos = transform.position;
        if (Physics.Raycast(myPos, rb.velocity, out RaycastHit hit, Vector3.Distance(myPos, (myPos + rb.velocity * 1.5f * Time.fixedDeltaTime))))
        {
            transform.position = hit.point - transform.forward;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged" || hit.collider.gameObject.layer == 0) { RunOnCollide(hit.collider.gameObject); }
        }

        if (rb.freezeRotation && rb.velocity.magnitude > Vector3.zero.magnitude)
        {
            //rb.velocity = Vector3.zero;
            //Debug.Log("Set to 0 " + name);
        }
    }
}