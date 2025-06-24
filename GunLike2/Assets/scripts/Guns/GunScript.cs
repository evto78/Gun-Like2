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
    public int multistage;
    public bool nerfedBul;
    public bool stickTo;
    public bool gunkyBlessed;
    public int gunkyClaw;
    public int sniperTower;
    public float sniperTowerCooldown;
    public int perfectedScope;
    public int pumpShotgunAttach;
    public float pumpShotgunAttachTimer;
    public int grenadeAttach;
    public float grenadeAttachTimer;
    public int gasGrenadeAttach;
    public float gasGrenadeAttachTimer;
    public int tacticalReload;
    int tacticalCompress;
    public int carvedBone;
    public int canineTooth;
    public int doorKnob;

    public float echoDmg;

    public bool isFastFiring;

    //Status
    protected float reloadTimer = 0;
    protected float attackTimer = 0;
    public int currentBullets;
    public bool reloading = false;
    protected bool shooting = false;
    protected float bowCharge;

    protected bool ricochet = false;


    public GameObject pistolBullet;
    public GameObject nerfedPistolBullet;
    public GameObject oilBullet;
    public Transform firePoint;
    public Transform normalFirePoint;
    public Transform doorKnobFirePoint;

    public GameObject grenade;

    public Camera cam;

    Ray ray;
    RaycastHit hit;

    public string whatHandThisIsIn;
    public float littleCharge;
    protected float timeSinceShot;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        manager = gameObject.GetComponentInParent<GunManager>();

        currentBullets = Mathf.RoundToInt(magSize);
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        cam = Camera.main;

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
        critChance = baseCritChance + manager.leftCritChance;
        critDamage = baseCritDamage * manager.leftCritDamage;
        weakPointChance = baseWeakPointChance + manager.leftWeakPointChance;
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
        multistage = manager.leftMultistage;
        nerfedBul = manager.leftNerf > 0;
        stickTo = manager.leftStickTo > 0;
        gunkyBlessed = Random.Range(0, 100) < manager.leftGunkyBless * 20f;
        gunkyClaw = manager.leftGunkyClaw;
        sniperTower = manager.leftSniperTower;
        perfectedScope = manager.leftPerfectedScope;
        pumpShotgunAttach = manager.leftPumpShotgunAttach;
        grenadeAttach = manager.leftGrenadeAttach;
        gasGrenadeAttach = manager.leftGasGrenadeAttach;
        tacticalReload = manager.leftTactReload;
        carvedBone = manager.leftCarvedBone;
        canineTooth = manager.leftCanineTooth;
        doorKnob = manager.leftDoorKnob;

        ricochet = manager.leftRicochet;

        if (perfectedScope > 0 && acc < baseAcc)
        {
            acc = 0.001f;
            critDamage = critDamage * 2f;
            weakPointDamage = weakPointDamage * 2f;
        }

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        if(bulSize > 10f)
        {
            bulSize = 10f;
        }
        LateStatUpdate();
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { atkSpd += littleCharge; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { atkSpd += littleCharge; }
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
        critChance = baseCritChance + manager.rightCritChance;
        critDamage = baseCritDamage * manager.rightCritDamage;
        weakPointChance = baseWeakPointChance + manager.rightWeakPointChance;
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
        multistage = manager.rightMultistage;
        nerfedBul = manager.rightNerf > 0;
        stickTo = manager.rightStickTo > 0;
        gunkyBlessed = Random.Range(0, 100) < manager.rightGunkyBless * 20f;
        gunkyClaw = manager.rightGunkyClaw;
        sniperTower = manager.rightSniperTower;
        perfectedScope = manager.rightPerfectedScope;
        pumpShotgunAttach = manager.rightPumpShotgunAttach;
        grenadeAttach = manager.rightGrenadeAttach;
        gasGrenadeAttach = manager.rightGasGrenadeAttach;
        tacticalReload = manager.rightTactReload;
        carvedBone = manager.rightCarvedBone;
        canineTooth = manager.rightCanineTooth;
        doorKnob = manager.rightDoorKnob;

        ricochet = manager.rightRicochet;

        if (perfectedScope > 0 && acc < baseAcc)
        {
            acc = 0.001f;
            critDamage = critDamage * 2f;
            weakPointDamage = weakPointDamage * 2f;
        }

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        if (bulSize > 10f)
        {
            bulSize = 10f;
        }
        LateStatUpdate();
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { atkSpd += littleCharge; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { atkSpd += littleCharge; }
    }

    public virtual void LateStatUpdate()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(doorKnob > 0) { firePoint = doorKnobFirePoint; } else { firePoint = normalFirePoint; }

        timeSinceShot += Time.deltaTime;

        if(possession > 0 && timeSinceShot > 5f)
        {
            if(currentBullets <= 0) { AttemptReload(); }

            possessionEffect.SetActive(true);

            List<RaycastHit> hits = new List<RaycastHit>();

            hits.InsertRange(0, Physics.BoxCastAll(cam.transform.position + cam.transform.forward * 10f, Vector3.one * 10f, cam.transform.forward, cam.transform.rotation, 100f));

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
        //laserPointer
        LineRenderer laser = transform.parent.GetComponent<LineRenderer>();
        if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[105] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[105] > 0))
        {
            int laserPointer = 0;
            if(whatHandThisIsIn == "left" && manager.playerItem.leftItems[105] > 0) { laserPointer = manager.playerItem.leftItems[105]; }
            if(whatHandThisIsIn == "right" && manager.playerItem.rightItems[105] > 0) { laserPointer = manager.playerItem.rightItems[105]; }
            laser.enabled = true;
            laser.SetPosition(0, firePoint.position);

            List<RaycastHit> hits = new List<RaycastHit>();

            hits.InsertRange(0, Physics.BoxCastAll(cam.transform.position + cam.transform.forward * 5f * laserPointer, Vector3.one * 5f * laserPointer, cam.transform.forward, cam.transform.rotation, 100f));
            //Debug.Log("Num of hits: " + hits.Count);
            EnemyHealthManager ehm;
            target = null;
            float dist = 99999f;
            foreach (RaycastHit hit in hits)
            {
                ehm = null;

                if(hit.transform.gameObject.TryGetComponent<EnemyHealthManager>(out ehm)) { }
                else if (hit.transform.parent != null) { if (hit.transform.parent.gameObject.TryGetComponent<EnemyHealthManager>(out ehm)) { } }

                if (ehm != null)
                {
                    //Debug.Log("Found target with emh : " + ehm.gameObject.name);
                    if(Vector3.Distance(player.position, ehm.transform.position) < dist)
                    {
                        //Debug.Log("Setting target as: " + ehm.gameObject.name);
                        target = ehm.transform;
                        dist = Vector3.Distance(player.position, ehm.transform.position);
                    }
                }
            }
            if(target != null) { laser.SetPosition(1, target.position); }
            else if(target == null) { laser.SetPosition(1, firePoint.position + firePoint.forward * 20f); }
        }
        else { laser.enabled = false; }

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
        else if (carvedBone < 1)
        {
            animator.SetBool("NoAmmo", true);
        }

        if(sniperTower > 0)
        {
            sniperTowerCooldown -= Time.deltaTime * (1 + (sniperTower * 0.5f));
            if(whatHandThisIsIn == "left") { sniperTowerCooldown -= Time.deltaTime * (manager.leftClockwork); }
            if(whatHandThisIsIn == "right") { sniperTowerCooldown -= Time.deltaTime * (manager.rightClockwork); }
        }

        if (pumpShotgunAttach > 0)
        {
            pumpShotgunAttachTimer -= Time.deltaTime * (1 + (pumpShotgunAttach * 0.5f));
            if (whatHandThisIsIn == "left") { pumpShotgunAttachTimer -= Time.deltaTime * (manager.leftClockwork); }
            if (whatHandThisIsIn == "right") { pumpShotgunAttachTimer -= Time.deltaTime * (manager.rightClockwork); }
        }

        if (grenadeAttach > 0)
        {
            grenadeAttachTimer -= Time.deltaTime * (1 + (grenadeAttach * 0.5f));
            if (whatHandThisIsIn == "left") { grenadeAttachTimer -= Time.deltaTime * (manager.leftClockwork); }
            if (whatHandThisIsIn == "right") { grenadeAttachTimer -= Time.deltaTime * (manager.rightClockwork); }
        }

        if (gasGrenadeAttach > 0)
        {
            gasGrenadeAttachTimer -= Time.deltaTime * (1 + (gasGrenadeAttach * 0.5f));
            if (whatHandThisIsIn == "left") { gasGrenadeAttachTimer -= Time.deltaTime * (manager.leftClockwork); }
            if (whatHandThisIsIn == "right") { gasGrenadeAttachTimer -= Time.deltaTime * (manager.rightClockwork); }
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
                if(pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
                {
                    acc = acc * 2f;
                    for(int i = 0; i < 9; i++)
                    {
                        currentBullets++;
                        Shoot(1f);
                        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(1f); }
                        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(1f); }
                    }

                    pumpShotgunAttachTimer = manager.playerItem.FindObjByID(106).baseCooldown;
                }
                if(whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1,100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(1f); }
                if(whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1,100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(1f); }
            }
        }
    }

    public virtual void AttemptShootUp()
    {
        if (bowAct > 0)
        {
            Shoot(bowCharge);
            if (pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
            {
                acc = acc * 2f;
                for (int i = 0; i < 9; i++)
                {
                    currentBullets++;
                    Shoot(bowCharge);
                    if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(bowCharge); }
                    if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(bowCharge); }
                }

                pumpShotgunAttachTimer = manager.playerItem.FindObjByID(106).baseCooldown;
            }
            if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(bowCharge); }
            if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(bowCharge); }
            bowCharge = 0f;
        }
    }

    public void AttemptReload()
    {
        if (!reloading && (currentBullets != magSize || tacticalReload > 0))
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

        if(tacticalReload > 0) { tacticalCompress = currentBullets; currentBullets = 1; }

        if(manager.leftBeltFed + manager.rightBeltFed > 0)
        {
            manager.healthMan.GiveEffect("pants falling", 50);
        }

        if(manager.leftWarcry > 0 && whatHandThisIsIn == "left") { manager.healthMan.GiveEffect("warcry", 1f); }
        if(manager.rightWarcry > 0 && whatHandThisIsIn == "right") { manager.healthMan.GiveEffect("warcry", 1f); }

        LateReload();
    }
    public virtual void LateReload()
    {
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { littleCharge = 0f; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { littleCharge = 0f; }
    }
    public void SpawnBulletAtPos(Vector3 pos)
    {
        //pretty much just for improvised storage
        GameObject spawnedBullet;
        if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[118] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[118] > 0)) { spawnedBullet = Instantiate(oilBullet); }
        else if (nerfedBul) { spawnedBullet = Instantiate(nerfedPistolBullet); }
        else { spawnedBullet = Instantiate(pistolBullet); }
        spawnedBullet.transform.position = pos;
        spawnedBullet.transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;

        SetBulStats(spawnedBullet, dmg, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), 1);

        if (whatHandThisIsIn == "left") { player.GetComponent<GunManager>().leftStickToCounters = 0; }
        if (whatHandThisIsIn == "right") { player.GetComponent<GunManager>().rightStickToCounters = 0; }
    }
    public virtual void EarlyShoot()
    {
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { littleCharge+=0.2f; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { littleCharge+=0.2f; }
        if (tacticalCompress > 0 && tacticalReload > 0) { dmg = dmg * (1 + (tacticalCompress / (10f / tacticalCompress))); tacticalCompress = 0; }
    }
    public virtual void Shoot(float bowChar)
    {
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd * 1.5f;
        isFastFiring = false;
        shooting = true;
        attackTimer = 1;
        if(carvedBone > 0 && currentBullets < 1)
        {
            manager.healthMan.TakeDamage(1, false);
            currentBullets++;
        }
        if (currentBullets > 0)
        {
            timeSinceShot = 0f;
            EarlyShoot();
            currentBullets--;
            if(echoDmg > 0) { dmg += echoDmg; echoDmg = 0; }
            GameObject spawnedBullet;
            if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[118] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[118] > 0)) { spawnedBullet = Instantiate(oilBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else if (nerfedBul) { spawnedBullet = Instantiate(nerfedPistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else { spawnedBullet = Instantiate(pistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
            if (target != null) { spawnedBullet.transform.LookAt(target); timeSinceShot = 5f; }
            acc = acc / bowChar;
            spawnedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
            spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;
            GameObject spawnedBulletB = Instantiate(pistolBullet, firePoint.position, firePoint.rotation);
            if (introTrig > 0)
            {
                spawnedBullet.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBulletB, true);
                spawnedBulletB.GetComponent<BulletScript>().IntroTrigSetUp(spawnedBullet, false);
            }
            else{Destroy(spawnedBulletB);}

            SetBulStats(spawnedBullet, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);

            if (introTrig > 0)
            {
                acc = acc / bowChar;
                if(masterTrig > 0) {acc += 2f;}
                else {acc += 4;}
                spawnedBulletB.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                spawnedBulletB.GetComponent<BulletScript>().mainCamera = cam;
                SetBulStats(spawnedBulletB, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);
            }

            if (isFastFiring)
            {
                animator.speed = atkSpd * 3f;
            }
            if(whatHandThisIsIn == "left") { player.GetComponent<GunManager>().leftStickToCounters = 0; }
            if(whatHandThisIsIn == "right") { player.GetComponent<GunManager>().rightStickToCounters = 0; }

            if(manager.healthMan.activeEffects[20].x > 0)
            {
                currentBullets = Mathf.RoundToInt(magSize);
            }

            if(grenadeAttach > 0 && grenadeAttachTimer < 0)
            {
                GameObject spawnedGrenade = Instantiate(grenade);
                spawnedGrenade.transform.position = firePoint.transform.position + firePoint.transform.forward;
                spawnedGrenade.GetComponent<GrenadeAttachment>().damage = dmg * 10f;
                spawnedGrenade.GetComponent<GrenadeAttachment>().isGas = false;
                spawnedGrenade.GetComponent<Rigidbody>().AddForce((Vector3.up * 4f) + (firePoint.transform.forward * 20f), ForceMode.Impulse);
                spawnedGrenade.GetComponent<Rigidbody>().AddTorque(spawnedGrenade.transform.right * 30f, ForceMode.Impulse);
                grenadeAttachTimer = manager.playerItem.FindObjByID(107).baseCooldown;
            }
            if (gasGrenadeAttach > 0 && gasGrenadeAttachTimer < 0)
            {
                GameObject spawnedGrenade = Instantiate(grenade);
                spawnedGrenade.transform.position = firePoint.transform.position + firePoint.transform.up / 2f + firePoint.transform.forward;
                spawnedGrenade.GetComponent<GrenadeAttachment>().damage = dmg * 10f;
                spawnedGrenade.GetComponent<GrenadeAttachment>().isGas = true;
                spawnedGrenade.GetComponent<Rigidbody>().AddForce((Vector3.up * 6f) + (firePoint.transform.forward * 20f), ForceMode.Impulse);
                spawnedGrenade.GetComponent<Rigidbody>().AddTorque(spawnedGrenade.transform.right * 30f, ForceMode.Impulse);
                gasGrenadeAttachTimer = manager.playerItem.FindObjByID(108).baseCooldown; ;
            }
        }
        else if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[117]>0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[117] > 0))
        {
            manager.Kick(whatHandThisIsIn);
        }
    }

    void SetBulStats(GameObject givenBullet, float givenDmg, bool isCrit, bool isWeakpoint, float givenBowChar)
    {
        //"What?"
        //"Now, I know that sounds bad"
        givenBullet.GetComponent<BulletScript>().setStats(this, givenDmg, isCrit, bulPir, isWeakpoint, weakPointDamage, bulSpd * givenBowChar, bulSize, 
            ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, 
            helpingSpon * 5f, coolSpon * 5f, fastSpon * 10f, largeSpon * 5f, advTrig, multistage, gunkyBlessed, gunkyClaw);
    }

    public void addBullet()
    {
        currentBullets += 1;
    }
}