using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    protected Animator animator;
    public GunManager manager;
    Transform player;
    public GameObject possessionEffect;
    public string gunName;

    //Base stats for this gun
    public float baseMagSize = 15;
    public float baseAtkSpd = 2f;
    public float baseReSpd = 1f;
    public float baseBulSpd = 100f;
    public float baseDmg = 10f;
    public float baseAcc = 1f;
    public float baseBulSize = 1f;
    public int baseBulPir = 0;
    public float baseCritChance = 0f;
    public float baseCritDamage = 2f;
    public float baseWeakPointChance = 0f;
    public float baseWeakPointDamage = 1.5f;

    //Modified Stats
    public float magSize;
    public float atkSpd;
    public float reSpd;
    public float bulSpd;
    public float dmg;
    public float acc;
    public float bulSize;
    public int bulPir;
    public float critChance;
    public float critDamage;
    public float weakPointChance;
    public float weakPointDamage;

    //item checks
    public float heavyBul;
    public float bowAct;
    public int heavySpirits;
    public int nuclearBul;
    public int introTrig;
    public int advTrig;
    public int masterTrig;
    public int jam;
    public float fireSpon;
    public float sharperSpon;
    public float silverSpon;
    public float helpingSpon;
    public float coolSpon;
    public float fastSpon;
    public float largeSpon;
    public int possession;

    public bool isFastFiring;

    //Status
    protected float reloadTimer = 0;
    protected float attackTimer = 0;
    public int currentBullets;
    protected bool reloading = false;
    protected bool shooting = false;
    protected float bowCharge;

    protected bool ricochet = false;


    public GameObject pistolBullet;
    public Transform firePoint;

    public Camera cam;

    Ray ray;
    RaycastHit hit;

    public string whatHandThisIsIn;

    protected float timeSinceShot;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameObject.GetComponentInParent<GunManager>();

        currentBullets = Mathf.RoundToInt(magSize);
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;

        LateStart();
    }
    public virtual void LateStart()
    {

    }
    public virtual void StatUpdateLeft()
    {
        whatHandThisIsIn = "left";

        magSize = Mathf.Round(baseMagSize * manager.leftMagSize);
        atkSpd = baseAtkSpd * manager.leftAtkSpd;
        reSpd = baseReSpd * manager.leftReSpd;
        bulSpd = baseBulSpd * manager.leftBulSpd;
        dmg = baseDmg * manager.leftDmg;
        acc = baseAcc / manager.leftAcc;
        bulSize = baseBulSize * manager.leftBulSize;
        bulPir = baseBulPir + manager.leftBulPir;
        critChance = baseCritChance * manager.leftCritChance;
        critDamage = baseCritDamage * manager.leftCritDamage;
        weakPointChance = baseWeakPointChance * manager.leftWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.leftWeakPointDamage;

        heavyBul = manager.leftHeavyBul;
        bowAct = manager.leftBowAct;
        heavySpirits = manager.leftHeavySpirit;
        nuclearBul = manager.leftNuclearBul;
        introTrig = manager.leftIntroTrig;
        advTrig = manager.leftAdvTrig;
        masterTrig = manager.leftMasterTrig;
        jam = manager.leftJam;
        fireSpon = manager.leftFireSpon;
        sharperSpon = manager.leftSharperSpon;
        silverSpon = manager.leftSilverSpon;
        helpingSpon = manager.leftHelpingSpon;
        coolSpon = manager.leftCoolSpon;
        fastSpon = manager.leftFastSpon;
        largeSpon = manager.leftLargeSpon;
        possession = manager.leftPossession;

        ricochet = manager.leftRicochet;

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        LateStatUpdate();
    }

    public virtual void StatUpdateRight()
    {
        whatHandThisIsIn = "right";

        magSize = Mathf.Round(baseMagSize * manager.rightMagSize);
        atkSpd = baseAtkSpd * manager.rightAtkSpd;
        reSpd = baseReSpd * manager.rightReSpd;
        bulSpd = baseBulSpd * manager.rightBulSpd;
        dmg = baseDmg * manager.rightDmg;
        acc = baseAcc / manager.rightAcc;
        bulSize = baseBulSize * manager.rightBulSize;
        bulPir = baseBulPir + manager.rightBulPir;
        critChance = baseCritChance * manager.rightCritChance;
        critDamage = baseCritDamage * manager.rightCritDamage;
        weakPointChance = baseWeakPointChance * manager.rightWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.rightWeakPointDamage;

        heavyBul = manager.rightHeavyBul;
        bowAct = manager.rightBowAct;
        heavySpirits = manager.rightHeavySpirit;
        nuclearBul = manager.rightNuclearBul;
        introTrig = manager.rightIntroTrig;
        advTrig = manager.rightAdvTrig;
        masterTrig = manager.rightMasterTrig;
        jam = manager.rightJam;
        fireSpon = manager.rightFireSpon;
        sharperSpon = manager.rightSharperSpon;
        silverSpon = manager.rightSilverSpon;
        helpingSpon = manager.rightHelpingSpon;
        coolSpon = manager.rightCoolSpon;
        fastSpon = manager.rightFastSpon;
        largeSpon = manager.rightLargeSpon;
        possession = manager.rightPossession;

        ricochet = manager.rightRicochet;

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        LateStatUpdate();
    }

    public virtual void LateStatUpdate()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        timeSinceShot += Time.deltaTime;

        if(possession > 0 && timeSinceShot > 5f)
        {
            if(currentBullets <= 0) { AttemptReload(); }

            possessionEffect.SetActive(true);

            List<RaycastHit> hits = new List<RaycastHit>();

            hits.InsertRange(0, Physics.BoxCastAll(cam.transform.position + cam.transform.forward * 25f, Vector3.one * 10f, cam.transform.forward, cam.transform.rotation, 100f));

            EnemyHealthManager eHealthMan;
            target = null;
            foreach(RaycastHit hit in hits)
            {
                //Debug.Log(hit.transform.gameObject.name);
                if (hit.transform.gameObject.TryGetComponent<EnemyHealthManager>(out eHealthMan))
                {
                    if ((eHealthMan.curHp + eHealthMan.armor) <= dmg)
                    {
                        target = hit.transform;
                        break;
                    }
                }
                
            }
            if(target != null)
            {
                AttemptShoot();
            }
        }
        else { target = null; possessionEffect.SetActive(false); }

        if (reloading)
        {
            reloadTimer -= Time.deltaTime * reSpd;
            if (reloadTimer <= 0)
            {
                reloading = false;
                currentBullets = Mathf.RoundToInt(magSize);
            }
        }
        if (shooting)
        {
            attackTimer -= Time.deltaTime * atkSpd;
            if (isFastFiring) { attackTimer -= Time.deltaTime * atkSpd; }
            if (attackTimer <= 0)
            {
                shooting = false;
            }
        }


        if (currentBullets > 0)
        {
            animator.SetBool("NoAmmo", false);
        }
        else
        {
            animator.SetBool("NoAmmo", true);
        }

    }

    public virtual void AttemptShoot()
    {
        if ((bowAct > 0))
        {
            bowCharge += 1 * atkSpd * Time.deltaTime;
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; }
        }
        else
        {
            if (!reloading && !shooting)
            {
                Shoot(1f);
            }
        }
    }

    public void AttemptShootUp()
    {
        if (bowAct > 0)
        {
            Shoot(bowCharge);
            bowCharge = 0f;
        }
    }

    public void AttemptReload()
    {
        if (!reloading && currentBullets != magSize)
        {
            Reload();
        }
    }

    public void Reload()
    {
        animator.SetTrigger("Reloading");
        animator.speed = reSpd;
        reloading = true;
        reloadTimer = 1;
        shooting = false;
        attackTimer = 0;

        if(manager.leftBeltFed + manager.rightBeltFed > 0)
        {
            manager.healthMan.GiveEffect("pants falling", 50);
        }
        LateReload();
    }
    public virtual void LateReload()
    {

    }
    public virtual void EarlyShoot()
    {

    }
    public virtual void Shoot(float bowChar)
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd * 1.5f;
        isFastFiring = false;
        shooting = true;
        attackTimer = 1;
        if (currentBullets > 0)
        {
            timeSinceShot = 0f;

            EarlyShoot();
            currentBullets--;

            GameObject spawnedBullet = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if(target != null) { spawnedBullet.transform.LookAt(target); timeSinceShot = 5f; }
            acc = acc / bowChar;
            spawnedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
            //spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulSpd, ForceMode.Impulse);

            spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;

            GameObject spawnedBulletB = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if (introTrig > 0)
            {
                spawnedBullet.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBulletB, true);
                spawnedBulletB.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBullet, false);
            }
            else
            {
                Destroy(spawnedBulletB);
            }

            if (Random.Range(1, 100) < critChance)
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(this, dmg * critDamage * weakPointDamage * bowChar, true, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon*5f, sharperSpon*5f, silverSpon*20f, helpingSpon*5f, coolSpon*5f, fastSpon*10f, largeSpon*5f, advTrig);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(this, dmg * critDamage * bowChar, true, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
            }
            else
            {
                if (Random.Range(1, 100) < weakPointChance)
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(this, dmg * weakPointDamage * bowChar, false, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
                else
                {
                    spawnedBullet.GetComponent<BulletScript>().setStats(this, dmg * bowChar, false, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                }
            }

            if (introTrig > 0)
            {
                acc = acc / bowChar;
                if(masterTrig > 0)
                {
                    acc += 2f;
                }
                else
                {
                    acc += 4;
                }
                spawnedBulletB.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                //spawnedBullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * bulSpd, ForceMode.Impulse);

                spawnedBulletB.GetComponent<BulletScript>().mainCamera = cam;
                if (Random.Range(1, 100) < critChance)
                {
                    if (Random.Range(1, 100) < weakPointChance)
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(this, dmg * critDamage * weakPointDamage * bowChar, true, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                    else
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(this, dmg * critDamage * bowChar, true, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                }
                else
                {
                    if (Random.Range(1, 100) < weakPointChance)
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(this, dmg * weakPointDamage * bowChar, false, bulPir, true, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                    else
                    {
                        spawnedBulletB.GetComponent<BulletScript>().setStats(this, dmg * bowChar, false, bulPir, false, weakPointDamage, bulSpd * bowChar, bulSize, ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, helpingSpon * 5f, coolSpon*5f, fastSpon * 10f, largeSpon * 5f, advTrig);
                    }
                }
            }

            if (isFastFiring)
            {
                animator.speed = atkSpd * 3f;
            }

        }
    }

    public void addBullet()
    {
        currentBullets += 1;
    }
}