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
    public float critDamage;
    public float bulSpd;

    public int pierce = 0;
    public bool ricochet = false;

    public bool isFlea;

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
    protected float turbineCharge;
    public int critUnfunny;

    public Collider myCollider;

    public Vector3 myPos;

    public string whatHandThisComesFrom;

    List<Collider> collisions = new List<Collider>();

    public GameObject shockwave;
    public GameObject droppedNerfedBullet;

    protected HealthManager hm;
    protected PlayerItem pi;
    protected GunManager gm;
    protected GunScript gunFiredFrom;

    public GameObject lavaBlob;
    public GameObject sniperTowerAlly;
    public GameObject zipMissle;
    public GameObject web;
    public GameObject darkBranch;
    public GameObject stickyNote;

    public virtual void Awake()
    {
        if(gunFiredFrom != null)
        {
            hm = gunFiredFrom.manager.healthMan;
            pi = gunFiredFrom.manager.playerItem;
            gm = gunFiredFrom.manager;
        }
        else
        {
            hm = GameObject.Find("Player").GetComponent<HealthManager>();
            pi = hm.playerItem;
            gm = pi.gunManager;
        }

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
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }

        if((whatHandThisComesFrom == "left" && pi.leftItems[119] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[119] > 0)) { if (rb.velocity.magnitude > 0.5f) { rb.velocity /= 1f + (1f * Time.deltaTime); } turbineCharge += (rb.velocity.magnitude * Time.deltaTime) / 4f; }
    }
    public void setStats(GunScript firedFrom, float givenDmg, bool isCritHit, int givenPierce, bool isAutoWeakHit, float givenWeakDmg, float givenBulSpd,
        float givenBulSize, bool givenRico, string whatHand, float isHeavy, int givenHeavySpirits, int givenNuclearBul, int givenIntroTrig,
        int givenJam, float chanceForFire, float chanceForSharper, float chanceForSilver, float chanceForHelping, float chanceForCool,
        float chanceForFastFire, float chanceForLarge, int givenAdvTrig, int givenMultistage, int isGunk, int givenGunkClaw)
    {
        if(Random.Range(1, 100) < chanceForFire) { isFireSpon = true; fireSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSharper) { isSharperSpon = true; sharperSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForSilver) { isSilverSpon = true; silverSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForHelping) { isHelpingSpon = true; helpingSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForCool) { isCoolSpon = true; coolSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForFastFire) { isFastFireSpon = true; fastSponEffect.SetActive(true); }
        if(Random.Range(1, 100) < chanceForLarge) { isLargeSpon = true; largeSponEffect.SetActive(true); }
        isGunky = Random.Range(1,100)<isGunk*20f;

        whatHandThisComesFrom = whatHand;
        gunFiredFrom = firedFrom;

        damage = givenDmg;
        isCrit = isCritHit;
        pierce = givenPierce;
        isAutoWeak = isAutoWeakHit;
        weakDamage = givenWeakDmg;
        critDamage = gunFiredFrom.critDamage;
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
        critUnfunny = gunFiredFrom.critUnfunny; if (critUnfunny > 0) { criticallyUnfunny(); }

        ricochet = givenRico;
        bool isOil = (whatHandThisComesFrom == "left" && pi.leftItems[118] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[118] > 0);
        myIsHeavy = isHeavy;
        if (isHeavy != 0f || isLargeSpon || isOil || isFlea)
        {
            rb.useGravity = true;
            rb.mass = isHeavy + 1;
        }
        else
        {
            rb.useGravity = false;
        }
        float bulSize = givenBulSize;
        if (isFlea) { pierce += 10; ricochet = true; bulSize /= 2f; damage = 1; bulSpd = Mathf.Clamp(bulSpd, 5, 25); }
        
        transform.localScale = new Vector3(transform.localScale.x * bulSize, transform.localScale.y * bulSize, transform.localScale.z * bulSize);

        Vector3 forceDir = transform.forward * bulSpd;

        rb.AddForce(forceDir, ForceMode.Impulse);
        if (name == "TRIGBULLET")
        {
            rb.AddForce(forceDir, ForceMode.VelocityChange);
        }
        DetectCollision(forceDir);
    }
    void criticallyUnfunny()
    {
        float critChance = gunFiredFrom.critChance;
        if(critChance <= 100f) { return; }
        isCrit = true;
        critDamage *= Mathf.Pow(2f, Mathf.Floor(critChance / 100f));
        critChance = ((critChance / 100f) - Mathf.Floor(critChance / 100f)) * 100f;
        if (Random.Range(1, 100) < critChance)
        {
            critDamage *= 2f;
        }
        GameObject stckyNote = Instantiate(stickyNote);
        stckyNote.transform.position = gunFiredFrom.firePoint.position + (gunFiredFrom.firePoint.forward/2f);
    }
    public void IntroTrigSetUp(GameObject givenPairedBullet, bool isLead)
    {
        pairedBullet = givenPairedBullet;
        isTrigLead = isLead;
    }

    void RunOnHit(GameObject hit, RaycastHit rayHit)
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
        if (isFireSpon) { ehm.GiveEffect("burn", 1f); }
        if (isSharperSpon) { ehm.GiveEffect("bleed", 1f); }
        if (isSilverSpon) { ehm.GiveEffect("lucky", 1f); }
        if (isHelpingSpon) { ehm.GiveEffect("stiched", 1f); }
        if (isCoolSpon) { ehm.GiveEffect("frozen", 1f); }
        if (isGunky) { ehm.GiveEffect("gunked", 1f); }

        if (pi.leftItems[54] + pi.rightItems[54] > 0){ hm.GiveEffect(PlayerEffectType.effectName.fastFire, 1f); }
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

        if(((whatHandThisComesFrom == "left" && pi.leftItems[122] > 0) ||(whatHandThisComesFrom == "right" && pi.rightItems[122] > 0))&& Random.Range(1, 100) < 11)
        {
            GameObject spawnedZipMissle = Instantiate(zipMissle, gm.transform.position + Vector3.up - gm.transform.forward, gm.transform.rotation);
            spawnedZipMissle.GetComponent<ZipMissle>().damage = damage * 1.5f;
            spawnedZipMissle.GetComponent<ZipMissle>().targetEhm = ehm;
        }

        if(gunFiredFrom.goodies > 0&& Random.Range(1, 100) < 8 + (4 * (gunFiredFrom.goodies - 1)))
        {
            ehm.RandomDebuff(); 
        }
        if(gunFiredFrom.enzymes > 0 && Random.Range(1, 100) < 10 * gunFiredFrom.enzymes)
        {
            ehm.GiveEffect("enzymes", 1);
        }

        if ((gunFiredFrom.darkBranch > 0 && (rayHit.collider != null || rayHit.point != new RaycastHit().point))&& Random.Range(1, 100) < 5 + 5 * gunFiredFrom.darkBranch)
        {
            GameObject spawnedDarkBranch = Instantiate(darkBranch);
            spawnedDarkBranch.transform.position = rayHit.point;
            spawnedDarkBranch.transform.LookAt(transform.position - transform.forward);
            spawnedDarkBranch.transform.SetParent(rayHit.collider.transform);
            spawnedDarkBranch.GetComponent<DarkBranch>().damage = 5 + damage;
            spawnedDarkBranch.GetComponent<DarkBranch>().attachedEHM = ehm;
        }

        //H.E.A.T Rounds
        if (whatHandThisComesFrom == "left" && pi.leftItems[102] > 0 || whatHandThisComesFrom == "right" && pi.rightItems[102] > 0){if (rayHit.point != Vector3.zero){
                for (int i = 0; i < Random.Range(1, 3); i++)
                {
                    GameObject spawnedLava = Instantiate(lavaBlob);
                    spawnedLava.transform.localScale = Vector3.one * Random.Range(0.5f, 1.1f);
                    spawnedLava.transform.position = rayHit.collider.transform.position;
                    spawnedLava.transform.LookAt(rayHit.point);
                    spawnedLava.transform.Rotate(new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), Random.Range(-30f, 30f)));
                    spawnedLava.transform.position = rayHit.point;
                    spawnedLava.transform.position += spawnedLava.transform.forward / 2f;
                    spawnedLava.GetComponent<Rigidbody>().AddForce((spawnedLava.transform.forward * (bulSpd)) + (Vector3.up * 10f)* Random.Range(1f, 1.5f));
                }}
        }
        //Chemical Agent
        if (gunFiredFrom.chemicalAgents > 0)
        {
            int maxA = gunFiredFrom.chemicalAgents; int maxB = gunFiredFrom.chemicalAgents;
            if (ehm.activeEffects[14].x < maxA && (ehm.activeEffects[14].x <= ehm.activeEffects[15].x || ehm.activeEffects[14].x == 0))
            {
                ehm.GiveEffect("chemical A", 1f);
            }
            else if (ehm.activeEffects[15].x < maxB)
            {
                ehm.GiveEffect("chemical B", 1f);
            }
            else
            {
                ehm.TakeDamage(damage * 1.5f * ehm.activeEffects[15].x, false, HitType.ht.special, ehm.transform.position, whatHandThisComesFrom);
                ehm.activeEffects[14] = new Vector4(0, ehm.activeEffects[14].y, ehm.activeEffects[14].z, ehm.activeEffects[14].w);
                ehm.activeEffects[15] = new Vector4(0, ehm.activeEffects[15].y, ehm.activeEffects[15].z, ehm.activeEffects[15].w);
                ehm.ChemicalEffect.Play();
            }
        }
        //Fortify
        if(gunFiredFrom.fority > 0)
        {
            gunFiredFrom.manager.healthMan.fortifyBuff += (1f / (gunFiredFrom.atkSpd*2f))*gunFiredFrom.fority;
        }
    }

    protected void RunOnCollide(GameObject givenGameObject, RaycastHit hit)
    {
        if(Vector3.Distance(transform.position, gunFiredFrom.manager.transform.position) < 20f) { damage = damage * (1f + 0.1f * gunkyClaw); } else if(Vector3.Distance(transform.position, gunFiredFrom.manager.transform.position) > 20f) { damage = damage * (1f + 0.1f * gunkyClaw); }
        collidedPos = transform.position;

        damage = damage * (1+(turbineCharge/4f));

        if (!collided && isLargeSpon && (givenGameObject.tag == "Enemy" || givenGameObject.tag == "Ground" || givenGameObject.tag == "EnemyWeakPoint"))
        {
            GameObject spawnedShockwave = Instantiate(shockwave);
            Shockwave shockScript = spawnedShockwave.GetComponent<Shockwave>();
            spawnedShockwave.transform.position = transform.position;
            shockScript.lifetime = transform.localScale.magnitude / 5f;
            shockScript.damage = damage * 2f;
            shockScript.fireSpon = isFireSpon;
            shockScript.coolSpon = isCoolSpon;
            shockScript.bleedSpon = isSharperSpon;
            shockScript.helpingSpon = isHelpingSpon;
        }
        if (!collided && multistage > 0 && (givenGameObject.tag == "Enemy" || givenGameObject.tag == "Ground" || givenGameObject.tag == "EnemyWeakPoint"))
        {
            GameObject spawnedShockwave = Instantiate(shockwave);
            Shockwave shockScript = spawnedShockwave.GetComponent<Shockwave>();
            spawnedShockwave.transform.position = transform.position;
            shockScript.lifetime = 0.2f * multistage;
            shockScript.damage = damage / 4f;
            shockScript.fireSpon = isFireSpon;
            shockScript.coolSpon = isCoolSpon;
            shockScript.bleedSpon = isSharperSpon;
            shockScript.helpingSpon = isHelpingSpon;
        }
        if(gunFiredFrom.haunt > 0)
        {
            damage /= gunFiredFrom.haunt + 1;
            for(int i = 0; i < gunFiredFrom.haunt+1; i++)
            {
                EnemyCollision(givenGameObject, hit);
            }
            damage *= gunFiredFrom.haunt + 1;
        }
        else
        {
            EnemyCollision(givenGameObject, hit);
        }
        if (!collided && pierce < 1)
        {
            rb.velocity = Vector3.zero;
            rb.freezeRotation = true;
            hitParticle.Play();
            Destroy(mesh);
            collided = true;
            gameObject.GetComponent<Collider>().enabled = false;
            fireSponEffect.transform.parent.SetParent(givenGameObject.transform);
            Destroy(fireSponEffect.transform.parent.gameObject, Random.Range(0.5f, 3f));
            if (gunFiredFrom.nerfedBul)
            {
                GameObject dropped = Instantiate(droppedNerfedBullet);
                DroppedNerfBul droppedScript = dropped.GetComponent<DroppedNerfBul>();
                dropped.transform.position = transform.position;
                dropped.transform.rotation = transform.rotation;
                droppedScript.player = gunFiredFrom.manager.transform;
                droppedScript.firedFrom = gunFiredFrom;
                droppedScript.whatHandThisComesFrom = whatHandThisComesFrom;
                dropped.GetComponent<Rigidbody>().AddForce(-dropped.transform.forward*2f, ForceMode.Impulse);
                dropped.GetComponent<Rigidbody>().AddForce(Vector3.up*4f, ForceMode.Impulse);
                Destroy(dropped, 60f);
            }

            if (introTrig > 0)
            {
                BulletScript pairedScript = null;
                if (pairedBullet != null) { pairedScript = pairedBullet.GetComponent<BulletScript>(); }
                if (isTrigLead)
                {
                    if (pairedScript != null && pairedScript.collided)
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
                    if (pairedScript != null && pairedScript.collided)
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
        else//pierce & ricochet
        {
            pierce -= 1;

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


                    transform.rotation = Quaternion.LookRotation(rb.velocity);
                }
                else
                {
                    ricoRay = new Ray(transform.position, -transform.forward);

                    myPos = transform.position;
                    if (Physics.Raycast(ricoRay, out ricoHit, Vector3.Distance(myPos, (myPos + rb.velocity * Time.fixedDeltaTime * 3f))))
                    {
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


                            transform.rotation = Quaternion.LookRotation(rb.velocity);
                        }
                    }
                }
            }
        }
        if (((whatHandThisComesFrom == "left" && pi.leftItems[136] > 0) || (whatHandThisComesFrom == "right" && pi.rightItems[136] > 0))&& gunFiredFrom.placedWeb == null)
        {
            GameObject spawnedWeb = Instantiate(web);
            spawnedWeb.transform.position = transform.position;
            gunFiredFrom.placedWeb = spawnedWeb;
        }
        damage = damage / (1 + (turbineCharge/4f));
    }

    void EnemyCollision(GameObject givenGameObject, RaycastHit hit)
    {
        if (gunFiredFrom.anatomy > 0)
        {
            if (givenGameObject.tag == "Enemy")
            {
                WeakPointHit(givenGameObject, hit);
            }
            else if (givenGameObject.tag == "EnemyWeakPoint")
            {
                NormalHit(givenGameObject, hit);
            }
        }
        else
        {
            if(givenGameObject.tag == "Enemy")
            {
                NormalHit(givenGameObject, hit);
            }
            else if (givenGameObject.tag == "EnemyWeakPoint")
            {
                WeakPointHit(givenGameObject, hit);
            }
        }
    }
    void WeakPointHit(GameObject givenGameObject, RaycastHit hit)
    {
        EnemyHealthManager ehm = givenGameObject.GetComponentInParent<EnemyHealthManager>();
        if (ehm == null) { return; }
        if (!isCrit){
            ehm.TakeDamage(damage * weakDamage, false, HitType.ht.weak, transform.position, whatHandThisComesFrom);
        }else{
            ehm.TakeDamage(damage * weakDamage * critDamage, false, HitType.ht.critweak, transform.position, whatHandThisComesFrom);
        }
        RunOnHit(givenGameObject, hit);
        givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
        if ((ehm.curHp / ehm.maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
        {ehm.Die();}

        if (nuclearBullets > 0 && Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
        {ehm.GiveEffect("radiation", 1);}
    }
    void NormalHit(GameObject givenGameObject, RaycastHit hit)
    {
        EnemyHealthManager ehm = givenGameObject.GetComponentInParent<EnemyHealthManager>();
        if(ehm == null) { return; }
        if(!isCrit && !isAutoWeak){
            ehm.TakeDamage(damage, false, HitType.ht.normal, transform.position, whatHandThisComesFrom);
        }else if(!isCrit && isAutoWeak){
            ehm.TakeDamage(damage * weakDamage, false, HitType.ht.weak, transform.position, whatHandThisComesFrom);
        }else if (isCrit && !isAutoWeak){
            ehm.TakeDamage(damage * critDamage, false, HitType.ht.crit, transform.position, whatHandThisComesFrom);
        }else if (isCrit && isAutoWeak){
            ehm.TakeDamage(damage * weakDamage * critDamage, false, HitType.ht.critweak, transform.position, whatHandThisComesFrom);
        }
        RunOnHit(givenGameObject, hit); givenGameObject.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);

        if ((ehm.curHp / ehm.maxHp) * 100f <= (50f * (1f - Mathf.Pow(1.2f, -0.5f * heavySpirits))))
        {ehm.Die();}

        if (nuclearBullets > 0 && Random.Range(1, 100) <= (25 + 5 * nuclearBullets))
        {ehm.GiveEffect("radiation", 1);}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collided) { RunOnCollide(collision.gameObject, new RaycastHit()); }
    }
    private void FixedUpdate()
    {
        if (collided) { rb.velocity = Vector3.zero; transform.position = collidedPos; }
        DetectCollision(rb.velocity * 1.5f);
    }

    public virtual void DetectCollision(Vector3 force)
    {
        myPos = transform.position;
        if (Physics.Raycast(myPos, force, out RaycastHit hit, Vector3.Distance(myPos, (myPos + force * Time.fixedDeltaTime))))
        {
            transform.position = hit.point - transform.forward / 10f; string hittag = hit.collider.gameObject.tag;
            if (hittag == "Enemy" || hittag == "EnemyWeakPoint" || hittag == "Ground" || hittag == "Untagged" || hit.collider.gameObject.layer == 0) 
            { RunOnCollide(hit.collider.gameObject, hit); }
        }
    }

    void SetBulletStats(GameObject bullet)
    {
        bullet.GetComponent<BulletScript>().setStats(null, damage, isCrit, pierce + 1, isAutoWeak, weakDamage, bulSpd, 1, ricochet, whatHandThisComesFrom, myIsHeavy, heavySpirits, nuclearBullets, 0, jam
            , 0, 0, 0, 0, 0, 0, 0, 0, multistage, 0, gunkyClaw);
    }
}