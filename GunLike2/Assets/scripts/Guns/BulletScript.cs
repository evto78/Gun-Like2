using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    protected Rigidbody rb;
    public ParticleSystem hitParticle;
    public GameObject mesh;

    public GameObject bulletPrefab;

    protected bool collided = false;
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
    bool isGunky;
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
    public int multistage;
    public int gunkyClaw;
    public int storage;
    float turbineCharge;

    public Collider myCollider;

    public Vector3 myPos;

    public string whatHandThisComesFrom;

    List<Collider> collisions = new List<Collider>();

    public GameObject shockwave;

    protected HealthManager hm;
    protected PlayerItem pi;
    protected GunManager gm;
    protected GunScript gunFiredFrom;

    public GameObject lavaBlob;
    public GameObject sniperTowerAlly;
    public GameObject zipMissle;
    public GameObject web;

    void Awake()
    {
        hm = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerItem>();
        gm = GameObject.FindGameObjectWithTag("Player").GetComponent<GunManager>();

        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 30f);
        collided = false;
        turbineCharge = 0;

        gm.totalLiveBullets++;
    }
    private void OnDestroy()
    {
        gm.totalLiveBullets--;
    }
    private void Update()
    {
        if (gm.totalLiveBullets > gm.maximumLiveBullets) { Destroy(gameObject, 0.6f); }
        if(rb.velocity != Vector3.zero) { transform.rotation = Quaternion.LookRotation(rb.velocity); }
        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }

        Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.cyan);

        if((whatHandThisComesFrom == "left" && pi.leftItems[119] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[119] > 0)) { if (rb.velocity.magnitude > 0.5f) { rb.velocity /= 1f + (1f * Time.deltaTime); } turbineCharge += (rb.velocity.magnitude * Time.deltaTime) / 4f; }
    }
    public void setStats(GunScript firedFrom, float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd,
        float givenBulSize, bool givenRico, string whatHand, float isHeavy, int givenHeavySpirits, int givenNuclearBul, int givenIntroTrig,
        int givenJam, float chanceForFire, float chanceForSharper, float chanceForSilver, float chanceForHelping, float chanceForCool,
        float chanceForFastFire, float chanceForLarge, int givenAdvTrig, int givenMultistage, bool isGunk, int givenGunkClaw)
    {
        if(Random.Range(1, 100) < chanceForFire) { isFireSpon = true; fireSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSharper) { isSharperSpon = true; sharperSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSilver) { isSilverSpon = true; silverSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForHelping) { isHelpingSpon = true; helpingSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForCool) { isCoolSpon = true; coolSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForFastFire) { isFastFireSpon = true; fastSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForLarge) { isLargeSpon = true; largeSponEffect.SetActive(true); }
        isGunky = isGunk;

        whatHandThisComesFrom = whatHand;
        gunFiredFrom = firedFrom;

        damage = givenDmg;
        isCrit = isCritHit;
        pierce = givenPierce;
        isAutoWeak = isAutoWeakHit;
        weakDamage = givenWeakDmg;
        bulSpd = givenBulSpd;
        if (isLargeSpon) { bulSpd = bulSpd / 2f; }
        if (isLargeSpon) { transform.localScale = transform.localScale * 3f; }
        if (isFastFireSpon && firedFrom != null) { firedFrom.isFastFiring = true; }

        heavySpirits = givenHeavySpirits;
        nuclearBullets = givenNuclearBul;
        introTrig = givenIntroTrig;
        advTrig = givenAdvTrig;
        jam = givenJam;
        multistage = givenMultistage;
        gunkyClaw = givenGunkClaw;
        if(whatHandThisComesFrom == "left") { storage = pi.leftItems[95]; }
        if(whatHandThisComesFrom == "right") { storage = pi.rightItems[95]; }

        ricochet = givenRico;
        bool isOil = (whatHandThisComesFrom == "left" && pi.leftItems[118] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[118] > 0);
        myIsHeavy = isHeavy;
        if (isHeavy != 0f || isLargeSpon || isOil)
        {
            rb.useGravity = true;
            rb.mass = isHeavy + 1;
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
        DetectCollision(forceDir);
    }
    public void IntroTrigSetUp(GameObject givenPairedBullet, bool isLead)
    {
        pairedBullet = givenPairedBullet;
        isTrigLead = isLead;
    }

    void RunOnHit(GameObject hit)
    {
        EnemyHealthManager ehm;
        if (hit.transform.parent != null)
        {
            ehm = hit.GetComponentInParent<EnemyHealthManager>();
        }
        else
        {
            ehm = hit.GetComponent<EnemyHealthManager>();
        }
        ehm.OnHitEffect(jam);
        if (isFireSpon) { ehm.GiveEffect("burn", 3f); }
        if (isSharperSpon) { ehm.GiveEffect("bleed", 3f); }
        if (isSilverSpon) { ehm.GiveEffect("lucky", 1f); }
        if (isHelpingSpon) { ehm.GiveEffect("stiched", 1f); }
        if (isCoolSpon) { ehm.GiveEffect("frozen", 1f); }
        if (isGunky) { ehm.GiveEffect("gunked", 1f); }

        if (pi.leftItems[54] + pi.rightItems[54] > 0){ hm.GiveEffect("fast fire", 1f); }
        if (gunFiredFrom.stickTo && whatHandThisComesFrom == "left") { gm.rightStickToCounters++; }
        if (gunFiredFrom.stickTo && whatHandThisComesFrom == "right") { gm.leftStickToCounters++; }

        if (storage > 0 && ehm.activeEffects[8].x < 20 + 10 * storage) { ehm.GiveEffect("storage", 1f); }

        if(hm.curHp / hm.maxHp <= 0.5f)
        {
            if(whatHandThisComesFrom == "left" && pi.leftItems[96] > 0) { ehm.didOnDeath = false; ehm.OnDeath(); ehm.didOnDeath = false; }
            if(whatHandThisComesFrom == "right" && pi.rightItems[96] > 0) { ehm.didOnDeath = false; ehm.OnDeath(); ehm.didOnDeath = false; }
        }

        if(gunFiredFrom.sniperTower > 0 && gunFiredFrom.sniperTowerCooldown <= 0)
        {
            GameObject spawnedSniperTower = Instantiate(sniperTowerAlly);
            spawnedSniperTower.transform.position = pi.transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * 3f;
            spawnedSniperTower.GetComponent<SniperTurretAlly>().damage = damage * 10f;
            spawnedSniperTower.GetComponent<SniperTurretAlly>().target = ehm;
            gunFiredFrom.sniperTowerCooldown = pi.FindObjByID(103).baseCooldown;
        }

        if((whatHandThisComesFrom == "left" && pi.leftItems[122] > 0) ||(whatHandThisComesFrom == "right" && pi.rightItems[122] > 0))
        {
            int temp = Random.Range(1, 100);
            if(temp < 11)
            {
                GameObject spawnedZipMissle = Instantiate(zipMissle, gm.transform.position + Vector3.up - gm.transform.forward, gm.transform.rotation);
                spawnedZipMissle.GetComponent<ZipMissle>().damage = damage * 1.5f;
                spawnedZipMissle.GetComponent<ZipMissle>().targetEhm = ehm;
            }
            
        }

        if(gunFiredFrom.goodies > 0)
        {
            if (Random.Range(1, 100) < 8 + (4 * (gunFiredFrom.goodies - 1))) { ehm.RandomDebuff(); }
        }
    }

    protected void RunOnCollide(GameObject givenGameObject)
    {
        if(Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < 20f) { damage = damage * (1f + 0.1f * gunkyClaw); } else if(Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) > 20f) { damage = damage * (1f + 0.1f * gunkyClaw); }
        collidedPos = transform.position;

        damage = damage * (1+(turbineCharge/4f));

        if (!collided && isLargeSpon && (givenGameObject.tag == "Enemy" || givenGameObject.tag == "Ground" || givenGameObject.tag == "EnemyWeakPoint"))
        {
            GameObject spawnedShockwave = Instantiate(shockwave);
            spawnedShockwave.transform.position = transform.position;
            spawnedShockwave.GetComponent<Shockwave>().lifetime = transform.localScale.magnitude / 5f;
            spawnedShockwave.GetComponent<Shockwave>().damage = damage * 2f;
            spawnedShockwave.GetComponent<Shockwave>().fireSpon = isFireSpon;
            spawnedShockwave.GetComponent<Shockwave>().coolSpon = isCoolSpon;
            spawnedShockwave.GetComponent<Shockwave>().bleedSpon = isSharperSpon;
            spawnedShockwave.GetComponent<Shockwave>().helpingSpon = isHelpingSpon;
        }
        if (!collided && multistage > 0 && (givenGameObject.tag == "Enemy" || givenGameObject.tag == "Ground" || givenGameObject.tag == "EnemyWeakPoint"))
        {
            GameObject spawnedShockwave = Instantiate(shockwave);
            spawnedShockwave.transform.position = transform.position;
            spawnedShockwave.GetComponent<Shockwave>().lifetime = 0.2f * multistage;
            spawnedShockwave.GetComponent<Shockwave>().damage = damage / 4f;
            spawnedShockwave.GetComponent<Shockwave>().fireSpon = isFireSpon;
            spawnedShockwave.GetComponent<Shockwave>().coolSpon = isCoolSpon;
            spawnedShockwave.GetComponent<Shockwave>().bleedSpon = isSharperSpon;
            spawnedShockwave.GetComponent<Shockwave>().helpingSpon = isHelpingSpon;
        }
        if(gunFiredFrom.haunt > 0)
        {
            damage /= gunFiredFrom.haunt + 1;
            for(int i = 0; i < gunFiredFrom.haunt+1; i++)
            {
                EnemyCollision(givenGameObject);
            }
            damage *= gunFiredFrom.haunt + 1;
        }
        else
        {
            EnemyCollision(givenGameObject);
        }
        if (!collided && pierce < 1)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            if(gameObject.name != "NerfedBullet" && gameObject.name != "NerfedBullet(Clone)")
            {
                Destroy(mesh);
            }
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
                        SetBulletStats(spawnedBullet);
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
                        SetBulletStats(spawnedBullet);
                        spawnedBullet.GetComponent<BulletScript>().mainCamera = Camera.main;

                        spawnedBullet.GetComponent<BulletScript>().collided = false;
                    }
                }
            }
        }
        else
        {
            pierce -= 1;

            //H.E.A.T Rounds
            if(whatHandThisComesFrom == "left" && pi.leftItems[102] > 0 || whatHandThisComesFrom == "right" && pi.rightItems[102] > 0)
            {
                Ray ricoRay = new Ray(transform.position, transform.forward);
                RaycastHit ricoHit;

                myPos = transform.position;
                Vector3 reflectDir = Vector3.zero;
                Vector3 hitPos = Vector3.zero;
                if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                {
                    reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
                    hitPos = ricoHit.point;
                }
                else
                {
                    ricoRay = new Ray(transform.position, -transform.forward);

                    myPos = transform.position;
                    if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                    {
                        //Debug.Log("Rico Hit! BACKWARDS!... Adjusting position for better reflect.");
                        myPos = transform.position - transform.forward * (rb.velocity * Time.deltaTime).magnitude;
                        ricoRay = new Ray(myPos, transform.forward);
                        if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 6f))))
                        {
                            reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
                            hitPos = ricoHit.point;

                        }
                    }
                }
                if(hitPos != Vector3.zero)
                {
                    for (int i = 0; i < Random.Range(1, 2); i++)
                    {
                        GameObject spawnedLava = Instantiate(lavaBlob);
                        //reflectDir += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * -5f;
                        spawnedLava.transform.position = hitPos;
                        spawnedLava.transform.rotation = Quaternion.LookRotation(reflectDir);
                        spawnedLava.transform.position += spawnedLava.transform.forward;
                        spawnedLava.GetComponent<Rigidbody>().AddForce(spawnedLava.transform.forward * bulSpd * 2f);
                    }
                }
            }

            if (ricochet)
            {
                if(whatHandThisComesFrom == "left" && pi.leftItems[82] > 0) { damage *= 1.25f; }
                if(whatHandThisComesFrom == "right" && pi.rightItems[82] > 0) { damage *= 1.25f; }

                if(advTrig > 0) { if(Random.Range(1, 100) > 20) { pierce += 1; } }

                Ray ricoRay = new Ray(transform.position, transform.forward);
                RaycastHit ricoHit;

                myPos = transform.position;
                if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                {
                    Vector3 reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
                    Vector3 storedVelocity = rb.velocity;

                    rb.velocity = Vector3.zero;
                    if (rb.useGravity == true) { rb.AddForce(((((reflectDir * storedVelocity.magnitude) / 1f) + Vector3.up * 2) + transform.forward * 2), ForceMode.VelocityChange); }
                    if (rb.useGravity == false) { rb.AddForce((((reflectDir * storedVelocity.magnitude) / 1f) + transform.forward * 2),ForceMode.VelocityChange); }
                    Debug.DrawRay(transform.position, (((reflectDir * storedVelocity.magnitude) / 1f) + transform.forward * 2) * Time.deltaTime, Color.green);
                    Debug.DrawRay(transform.position, rb.velocity * Time.deltaTime, Color.red);


                    transform.rotation = Quaternion.LookRotation(rb.velocity);
                }
                else
                {
                    ricoRay = new Ray(transform.position, -transform.forward);

                    myPos = transform.position;
                    if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                    {
                        //Debug.Log("Rico Hit! BACKWARDS!... Adjusting position for better reflect.");
                        transform.position = transform.position - transform.forward * (rb.velocity * Time.deltaTime).magnitude;
                        ricoRay = new Ray(transform.position, transform.forward);

                        myPos = transform.position;
                        if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 6f))))
                        {
                            Vector3 reflectDir = Vector3.Reflect(ricoRay.direction, ricoHit.normal);
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
        if ((whatHandThisComesFrom == "left" && pi.leftItems[136] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[136] > 0))
        {
            if (gunFiredFrom.placedWeb == null)
            {
                GameObject spawnedWeb = Instantiate(web);
                spawnedWeb.transform.position = transform.position;
                gunFiredFrom.placedWeb = spawnedWeb;
            }
        }
        damage = damage / (1 + (turbineCharge/4f));
    }

    void EnemyCollision(GameObject givenGameObject)
    {
        if (gunFiredFrom.anatomy > 0)
        {
            if (givenGameObject.tag == "Enemy")
            {
                WeakPointHit(givenGameObject);
            }
            if (givenGameObject.tag == "EnemyWeakPoint")
            {
                NormalHit(givenGameObject);
            }
        }
        else
        {
            if(givenGameObject.tag == "Enemy")
            {
                NormalHit(givenGameObject);
            }
            if (givenGameObject.tag == "EnemyWeakPoint")
            {
                WeakPointHit(givenGameObject);
            }
        }
    }
    void WeakPointHit(GameObject givenGameObject)
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
                givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage * gunFiredFrom.critDamage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
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
            if (Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
            {
                givenGameObject.GetComponentInParent<EnemyHealthManager>().TakePercentDamage(0.15f);
            }
        }
    }
    void NormalHit(GameObject givenGameObject)
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
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * weakDamage, false, "weakHit", transform.position, whatHandThisComesFrom);
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
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * gunFiredFrom.critDamage, false, "critHit", transform.position, whatHandThisComesFrom);
                    RunOnHit(givenGameObject);
                }

                givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                if (givenGameObject.GetComponentInParent<EnemyHealthManager>() != null)
                {
                    givenGameObject.GetComponentInParent<EnemyHealthManager>().TakeDamage(damage * gunFiredFrom.critDamage * weakDamage, false, "critWeakHit", transform.position, whatHandThisComesFrom);
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
        if(collided && (gameObject.name == "NerfedBullet" || gameObject.name == "NerfedBullet(Clone)") && Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < 2f)
        {
            if(whatHandThisComesFrom == "left") { GameObject.Find("Player").GetComponent<GunManager>().leftHand.transform.GetChild(0).gameObject.SendMessage("addBullet", SendMessageOptions.DontRequireReceiver); }
            if(whatHandThisComesFrom == "right") { GameObject.Find("Player").GetComponent<GunManager>().rightHand.transform.GetChild(0).gameObject.SendMessage("addBullet", SendMessageOptions.DontRequireReceiver); }
            Destroy(gameObject);
        }

        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }
        DetectCollision(rb.velocity * 1.5f);
    }

    public virtual void DetectCollision(Vector3 force)
    {
        myPos = transform.position;
        if (Physics.Raycast(myPos, force, out RaycastHit hit, Vector3.Distance(myPos, (myPos + force * Time.fixedDeltaTime))))
        {
            transform.position = hit.point - transform.forward / 10f;
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "EnemyWeakPoint" || hit.collider.gameObject.tag == "Ground" || hit.collider.gameObject.tag == "Untagged" || hit.collider.gameObject.layer == 0) { RunOnCollide(hit.collider.gameObject); }
        }
    }

    void SetBulletStats(GameObject bullet)
    {
        bullet.GetComponent<BulletScript>().setStats(null, damage, isCrit, pierce + 1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam
            , 0, 0, 0, 0, 0, 0, 0, 0, multistage, isGunky, gunkyClaw);
    }
}