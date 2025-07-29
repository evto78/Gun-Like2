using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    protected Animator animator;
    public GunManager manager;
    Transform player;
    public GameObject possessionEffect;
    public GameObject misfireEffect;
    public GameObject confettiEffect;
    public string gunName;
    public bool isGoo;
    public GooColorShift gooEffect;

    float atkSpeedOverFPSBulQued = 0f;

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
    public int heavyBul;
    public float bowAct;
    public int heavySpirits;
    public int nuclearBul;
    public int introTrig;
    public int advTrig;
    public int masterTrig;
    public int jam;
    public int fireSpon;
    public int sharperSpon;
    public int silverSpon;
    public int helpingSpon;
    public int coolSpon;
    public int fastSpon;
    public int largeSpon;
    public int possession;
    public int multistage;
    public bool nerfedBul;
    public bool stickTo;
    public int gunkyBlessed;
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
    public int haunt;
    public int goodies;
    public int anatomy;
    public int enzymes;
    public int darkBranch;
    public int brokenPen;
    int brokenPenCounter;
    public int rushJob;
    public float rushJobTimer;
    public int brokenInk;
    public int inkCounter;
    public int chemicalAgents;
    public int fleas;
    public int smokingGun;
    protected int smokingGunCounter;
    public int forkedBarrel;
    public int runicMag;
    int runicMagsStored;
    public int slots;
    public int triggerHappy;
    public int bulletFactory;
    public int critUnfunny;
    public int fority;
    public int confetti;
    public int storage;

    public float echoDmg;

    public bool isFastFiring;

    //Status
    protected float reloadTimer = 0;
    protected float attackTimer = 0;
    public int currentBullets;
    public bool reloading = false;
    protected bool shooting = false;
    public float bowCharge;

    protected bool ricochet = false;



    public GameObject pistolBullet;
    public GameObject nerfedPistolBullet;
    public GameObject oilBullet;
    public GameObject fleaBullet;
    public Transform firePoint;
    public Transform normalFirePoint;
    public Transform doorKnobFirePoint;

    public GameObject grenade;
    public GameObject placedWeb;

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

        if(gooEffect != null) { gooEffect.enabled = isGoo; }
        
        LateStart();
    }
    public virtual void LateStart()
    {

    }
    public virtual void StatUpdateLeft()
    {
        whatHandThisIsIn = "left";

        magSize = Mathf.CeilToInt(baseMagSize * manager.leftMagSize);
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
        gunkyBlessed = manager.leftGunkyBless;
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
        haunt = manager.leftHaunt;
        goodies = manager.leftGoodies;
        anatomy = manager.leftAnatomy;
        enzymes = manager.leftEnzymes;
        darkBranch = manager.leftDarkBranch;
        brokenPen = manager.leftBrokenPen;
        rushJob = manager.leftRushJob;
        brokenInk = manager.leftBrokenInk;
        chemicalAgents = manager.leftChemicalAgents;
        fleas = manager.left200Fleas;
        smokingGun = manager.leftSmokingGun;
        forkedBarrel = manager.leftForkedBarrel;
        runicMag = manager.leftRunicMag;
        slots = manager.leftSlots;
        triggerHappy = manager.leftTriggerHappy;
        bulletFactory = manager.leftBulletFactory;
        critUnfunny = manager.leftCritUnfunny;
        fority = manager.leftFortify;
        confetti = manager.leftConfetti;
        storage = manager.leftStorage;

        ricochet = manager.leftRicochet;

        if (perfectedScope > 0 && acc < baseAcc)
        {
            acc = 0.001f;
            critDamage = critDamage * 2f;
            weakPointDamage = weakPointDamage * 2f;
        }

        //STAT CAPS!
        bulSpd = Mathf.Clamp(bulSpd, 0, 500);
        acc = Mathf.Clamp(acc, 0, 25);
        bulSize = Mathf.Clamp(bulSize, 0, 10);
        magSize = Mathf.Clamp(magSize, 1, float.PositiveInfinity);
        LateStatUpdate();
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { atkSpd += littleCharge; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { atkSpd += littleCharge; }
    }

    public virtual void StatUpdateRight()
    {
        whatHandThisIsIn = "right";

        magSize = Mathf.CeilToInt(baseMagSize * manager.rightMagSize);
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
        gunkyBlessed = manager.rightGunkyBless;
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
        haunt = manager.rightHaunt;
        goodies = manager.rightGoodies;
        anatomy = manager.rightAnatomy;
        enzymes = manager.rightEnzymes;
        darkBranch = manager.rightDarkBranch;
        brokenPen = manager.rightBrokenPen;
        rushJob = manager.rightRushJob;
        brokenInk = manager.rightBrokenInk;
        chemicalAgents = manager.rightChemicalAgents;
        fleas = manager.right200Fleas;
        smokingGun = manager.rightSmokingGun;
        forkedBarrel = manager.rightForkedBarrel;
        runicMag = manager.rightRunicMag;
        slots = manager.rightSlots;
        triggerHappy = manager.rightTriggerHappy;
        bulletFactory = manager.rightBulletFactory;
        critUnfunny = manager.rightCritUnfunny;
        fority = manager.rightFortify;
        confetti = manager.rightConfetti;
        storage = manager.rightStorage;

        ricochet = manager.rightRicochet;

        if (perfectedScope > 0 && acc < baseAcc)
        {
            acc = 0.001f;
            critDamage = critDamage * 2f;
            weakPointDamage = weakPointDamage * 2f;
        }

        //STAT CAPS!
        bulSpd = Mathf.Clamp(bulSpd, 0, 500);
        acc = Mathf.Clamp(acc, 0, 25);
        bulSize = Mathf.Clamp(bulSize, 0, 10);
        magSize = Mathf.Clamp(magSize, 1, float.PositiveInfinity);
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
                if(runicMag > 0 && runicMagsStored > 0) { currentBullets = Mathf.Clamp(Mathf.RoundToInt(magSize) + runicMagsStored, 1, Mathf.RoundToInt(magSize)*(runicMag+1)); runicMagsStored = 0; }
                else{currentBullets = Mathf.RoundToInt(magSize);}
            }
        }
        if (shooting)
        {
            attackTimer -= Time.deltaTime * atkSpd;
            if (isFastFiring) { attackTimer -= Time.deltaTime * atkSpd; }
            if (attackTimer <= 0)
            {
                shooting = false;
                atkSpeedOverFPSBulQued = 0f - attackTimer;
            }
        }
        else { atkSpeedOverFPSBulQued = 0f; }

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

        rushJobTimer -= Time.deltaTime;
    }

    public virtual void AttemptShoot()
    {
        if ((bowAct > 0))
        {
            bowCharge += ((bowAct/2f) * Time.deltaTime) + (1.5f * atkSpd * Time.deltaTime);
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; AttemptShootUp(true); }
        }
        else
        {
            if (!reloading && !shooting && rushJobTimer <= 0)
            {
                if (rushJob > 0 && Random.Range(1, 100) < Mathf.Clamp(5 + (5 * rushJob), -1, 65))
                {
                    misfireEffect.GetComponent<ParticleSystem>().Play();
                    rushJobTimer = (1f / reSpd)/2f;
                    return;
                }

                Shoot(1f);
                if(pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
                {
                    acc = acc * 2f;
                    for(int i = 0; i < 9; i++)
                    {
                        currentBullets++;
                        Shoot(1f);
                        if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111])
                            || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111])) { Shoot(1f); }
                    }

                    pumpShotgunAttachTimer = manager.playerItem.FindObjByID(106).baseCooldown;
                }
                if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111])
                            || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111])) { Shoot(1f); }
                if (brokenPen > 0) { brokenPenCounter++; }
            }
        }
    }

    public virtual void AttemptShootUp(bool forcedInput)
    {
        if (!forcedInput) { smokingGunCounter = 0; if (smokingGun > 0) { manager.healthMan.activeEffects[23] = new Vector4(0, manager.healthMan.activeEffects[23].y, manager.healthMan.activeEffects[23].z, manager.healthMan.activeEffects[23].w); } }
        if (bowAct > 0 && !reloading && !shooting)
        {
            if (rushJob > 0 && Random.Range(1, 100) < Mathf.Clamp(5 + (5 * rushJob), -1, 65))
            {
                misfireEffect.GetComponent<ParticleSystem>().Play();
                rushJobTimer = (1f / reSpd) / 2f;
                return;
            }

            Shoot(bowCharge);
            if (pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
            {
                acc = acc * 2f;
                for (int i = 0; i < 9; i++)
                {
                    currentBullets++;
                    Shoot(bowCharge);
                    if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111])
                            || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111])) { Shoot(bowCharge); }
                }

                pumpShotgunAttachTimer = manager.playerItem.FindObjByID(106).baseCooldown;
            }
            if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111])
                            || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111])) { Shoot(bowCharge); }
            if (brokenPen > 0) { brokenPenCounter++; }
            bowCharge = 0f;
        }
    }

    public void AttemptReload()
    {
        if (!reloading && (currentBullets != magSize || tacticalReload > 0 || runicMag > 0 ))
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

        if(runicMag > 0) { runicMagsStored += currentBullets; }
        if(tacticalReload > 0) { tacticalCompress = currentBullets; currentBullets = 1; }

        if(manager.leftBeltFed + manager.rightBeltFed > 0)
        {
            manager.healthMan.GiveEffect(PlayerEffectType.effectName.pantsFalling, 50);
        }

        if(manager.leftWarcry > 0 && whatHandThisIsIn == "left") { manager.healthMan.GiveEffect(PlayerEffectType.effectName.warcry, 1f); }
        if(manager.rightWarcry > 0 && whatHandThisIsIn == "right") { manager.healthMan.GiveEffect(PlayerEffectType.effectName.warcry, 1f); }

        if(manager.playerItem.leftItems[180] > 0 && whatHandThisIsIn == "left") { manager.healthMan.TakeDamage(manager.healthMan.curHp/2f, false, null); }
        if(manager.playerItem.rightItems[180] > 0 && whatHandThisIsIn == "right") { manager.healthMan.TakeDamage(manager.healthMan.curHp/2f, false, null); }

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
        else { player.GetComponent<GunManager>().rightStickToCounters = 0; }
    }
    public virtual void EarlyShoot(bool requireAmmo)
    {
        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) { littleCharge+=0.2f; }
        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0) { littleCharge+=0.2f; }
        if (tacticalCompress > 0 && tacticalReload > 0) { dmg = dmg * (1 + (tacticalCompress / (10f / tacticalCompress))); tacticalCompress = 0; }
        if (brokenPen > 0 && brokenPenCounter >= 10) { dmg *= 2f + (1.5f * brokenPen - 1f); }

        if (fleas>0&&Random.Range(1,100)<16){
            for(int i = 0; i < fleas; i++)
            {
                GameObject spawnedFlea = Instantiate(fleaBullet, firePoint.position, firePoint.rotation); acc = acc / 1;
                if (target != null) { spawnedFlea.transform.LookAt(target); timeSinceShot = 5f; }
                acc *= 1.5f; spawnedFlea.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc))); acc /= 1.5f;
                spawnedFlea.GetComponent<BulletScript>().mainCamera = cam;
                SetBulStats(spawnedFlea, dmg * 1, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), 1);
            }
        }

        if (slots > 0) { dmg = RiggedSlotMachine(dmg); }
    }
    public virtual void Shoot(float bowChar)
    {
        if (atkSpeedOverFPSBulQued >= 1) { atkSpeedOverFPSBulQued -= 1f; Shoot(bowChar); }
        if (currentBullets < 1 && smokingGun > 0) { smokingGunCounter++; if (smokingGunCounter >= 2) { manager.healthMan.GiveEffect(PlayerEffectType.effectName.smokingGun, 1f); } AttemptReload(); return; }
        if (currentBullets > 0 && carvedBone <= 0) { animator.SetBool("NoAmmo", false); }
        if (confetti > 0) { confettiEffect.GetComponent<ParticleSystem>().Play(); }
        animator.SetTrigger("Shooting");
        animator.speed = atkSpd * 1.5f;
        isFastFiring = false;
        shooting = true;
        attackTimer = 1;
        bool requireAmmo = true;
        if (bulletFactory > 0) { requireAmmo = false; if(Random.Range(1, Mathf.RoundToInt(1 + bulletFactory + (magSize - currentBullets))) == 1) { requireAmmo = true; } }
        if (brokenInk > 0 && inkCounter >= Mathf.Clamp(10 - brokenInk, 1, 9)) { requireAmmo = false; }

        if(carvedBone > 0 && currentBullets < 1 && requireAmmo)
        {
            manager.healthMan.TakeDamage(1, false, null);
            currentBullets++;
            if (triggerHappy > 0) { currentBullets++; manager.healthMan.TakeDamage(1, false, null); }
        }
        if (currentBullets > 0 || !requireAmmo)
        {
            timeSinceShot = 0f;
            if (brokenInk > 0 && inkCounter < Mathf.Clamp(10 - brokenInk, 1, 9)) { inkCounter++; } else if (brokenInk > 0) { inkCounter = 0; requireAmmo = false; }
            EarlyShoot(requireAmmo);
            if (requireAmmo)
            {
                currentBullets--;
                if (triggerHappy > 0) { currentBullets--; }
                if (currentBullets < 0) { currentBullets = 0; }
            }
            if (echoDmg > 0) { dmg += echoDmg; echoDmg = 0; }
            GameObject spawnedBullet;
            if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[118] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[118] > 0)) { spawnedBullet = Instantiate(oilBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else if (nerfedBul) { spawnedBullet = Instantiate(nerfedPistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else { spawnedBullet = Instantiate(pistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
            if (target != null) { spawnedBullet.transform.LookAt(target); timeSinceShot = 5f; }
            acc = acc / bowChar;
            spawnedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
            spawnedBullet.GetComponent<BulletScript>().mainCamera = cam;
            GameObject spawnedBulletB;
            if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[118] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[118] > 0)) { spawnedBulletB = Instantiate(oilBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else if (nerfedBul) { spawnedBulletB = Instantiate(nerfedPistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
            else { spawnedBulletB = Instantiate(pistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
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
            
            if (forkedBarrel > 0)
            {
                int bulletsSpawned = 1; if (Random.Range(1, 100) < (forkedBarrel - 1f) * 20f) { bulletsSpawned++; }
                for(int i = 0; i < bulletsSpawned; i++)
                {
                    GameObject spawnedForkedBullet;
                    if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[118] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[118] > 0)) { spawnedForkedBullet = Instantiate(oilBullet, firePoint.transform.position, firePoint.transform.rotation); }
                    else if (nerfedBul) { spawnedForkedBullet = Instantiate(nerfedPistolBullet, firePoint.transform.position, firePoint.transform.rotation); }
                    else { spawnedForkedBullet = Instantiate(pistolBullet, firePoint.transform.position, firePoint.transform.rotation); }

                    acc += 4;
                    spawnedForkedBullet.transform.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                    spawnedForkedBullet.GetComponent<BulletScript>().mainCamera = cam;
                    SetBulStats(spawnedForkedBullet, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);
                }
            }

            if (isFastFiring)
            {
                animator.speed = atkSpd * 3f;
            }
            if(whatHandThisIsIn == "left") { manager.leftStickToCounters = 0; }
            else{ manager.rightStickToCounters = 0; }

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
    float RiggedSlotMachine(float incomingDamage)
    {
        int dmgGiven = Mathf.CeilToInt(incomingDamage);
        List<int> dmgChars = new List<int>();
        int temp;
        Debug.Log(dmgGiven);
        foreach (char digit in dmgGiven.ToString()) // Build list
        {
            int.TryParse(digit.ToString(), out temp);
            dmgChars.Add(temp);
        }

        var count = dmgChars.Count; // Shuffle order
        var last = count - 1;
        for (var i = 0; i < last; i++)
        {
            var r = Random.Range(i, count);
            var tmp = dmgChars[i];
            dmgChars[i] = dmgChars[r];
            dmgChars[r] = tmp;
        }

        int lowID = 0; int highID = 0; // Set lowest and highest
        for(int i = 0; i < dmgChars.Count; i++)
        {
            if(dmgChars[i] > dmgChars[highID]) { highID = i; }
            if(dmgChars[i] < dmgChars[lowID]) { lowID = i; }
        }
        dmgChars[lowID] = 1;
        dmgChars[highID] = 0 + (slots-1);
        if (dmgChars[highID] > 9) { dmgChars[highID] = 9; }

        string constructing = ""; // Reconstruct int
        foreach(int digit in dmgChars)
        {
            constructing += digit.ToString();
        }
        int.TryParse(constructing, out dmgGiven);
        Debug.Log(dmgGiven);
        return dmgGiven;
    }

    void SetBulStats(GameObject givenBullet, float givenDmg, bool isCrit, bool isWeakpoint, float givenBowChar)
    {
        givenBullet.GetComponent<BulletScript>().setStats(this, givenDmg, isCrit, bulPir, isWeakpoint, weakPointDamage, bulSpd * givenBowChar, bulSize, 
            ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, 
            helpingSpon * 5f, coolSpon * 5f, fastSpon * 10f, largeSpon * 5f, advTrig, multistage, gunkyBlessed, gunkyClaw);
    }

    public void addBullet()
    {
        currentBullets += 1;
    }
}