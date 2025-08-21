using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GunScript : MonoBehaviour
{
    protected Animator animator;
    public GunManager manager; public Transform bulletReservoir; public int reservoirSize;
    Transform player;
    public GameObject possessionEffect;
    public GameObject misfireEffect;
    public GameObject confettiEffect;
    public string gunName;
    public GunObjectData.GunType gunType;
    public bool isGoo;
    public GooColorShift gooEffect;

    float atkSpeedOverFPSBulQued = 0f;

    public ParticleSystem MuzzleFlash;

    //GunData
    public GunObjectData data;

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
    public int turbine;
    public int oilGun;

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

    public enum BulletType { standard, nerf, oil} public BulletType bulletType;

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
    public Rigidbody target; protected float possessionUpdateTimer = 0f; Vector3 possessionTarOffset;

    int lastShootClipIndex = 0; protected bool shootingThisFrameAudio;
    int lastEmptyClipIndex = 0;
    int lastReloadClipIndex = 0;

    void Start()
    {
        shootingThisFrameAudio = false;
        firePoint = normalFirePoint;
        reservoirSize = 250;
        manager = gameObject.GetComponentInParent<GunManager>();
        bulletType = BulletType.standard;

        magSize = Mathf.CeilToInt(baseMagSize * manager.leftMagSize);
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
        turbine = manager.leftTurbine;
        oilGun = manager.leftOilGun;

        BulletTypeHandler();

        if(bulletReservoir == null) { bulletReservoir = manager.preinstatiatedAmmoBoxLeft; }
        if (firePoint != null) { bulletReservoir.position = firePoint.position; bulletReservoir.rotation = firePoint.rotation; }

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
        if (manager.playerItem.leftItems[109] > 0) { atkSpd += littleCharge; }
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
        turbine = manager.rightTurbine;
        oilGun = manager.rightOilGun;

        BulletTypeHandler();
       
        if(bulletReservoir == null) { bulletReservoir = manager.preinstatiatedAmmoBoxRight; }
        if(firePoint != null) { bulletReservoir.position = firePoint.position; bulletReservoir.rotation = firePoint.rotation; }

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
        if (manager.playerItem.rightItems[109] > 0) { atkSpd += littleCharge; }
    }

    public virtual void LateStatUpdate()
    {
        
    }
    void BulletTypeHandler()
    {
        BulletType prev = bulletType;
        if (nerfedBul) { bulletType = BulletType.nerf; }
        if (oilGun > 0) { bulletType = BulletType.oil; }
        switch (gunName)
        {
            case "Archer Fish": if (bulletType != BulletType.oil) { bulletType = BulletType.standard; } break;
            case "Hand Cannon": bulletType = BulletType.standard; break;
            case "Mutated Knife": bulletType = BulletType.standard; break;
        }
        if (bulletType != prev) { ClearPreInstatiated(); }
    }
    // Update is called once per frame
    public virtual void Update()
    {
        animator.SetBool("IsLeft", whatHandThisIsIn == "left");

        if (doorKnob > 0) { firePoint = doorKnobFirePoint; } else { firePoint = normalFirePoint; }

        timeSinceShot += Time.deltaTime;

        possessionUpdateTimer -= Time.deltaTime;
        if (possession > 0 && timeSinceShot > 5f)
        {
            if (currentBullets <= 0) { AttemptReload(); }

            possessionEffect.SetActive(true);

            if(possessionUpdateTimer <= 0f)
            {
                possessionUpdateTimer = 0.5f;
                List<RaycastHit> hits = new List<RaycastHit>();

                hits.InsertRange(0, Physics.BoxCastAll(cam.transform.position + cam.transform.forward * 10f, Vector3.one * 10f, cam.transform.forward, cam.transform.rotation, 100f));

                EnemyHealthManager ehm;
                target = null;
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform.gameObject.TryGetComponent<EnemyHealthManager>(out ehm))
                    {
                        target = hit.transform.gameObject.GetComponent<Rigidbody>();
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
                        target = ehm.transform.gameObject.GetComponent<Rigidbody>();
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

        //Preinstatiation
        PreInstatiateBullets();
    }
    void PlaySound(string soundType)
    {
        LocalSoundManager lsm = manager.healthMan.lsm; AudioClip selectedClip;
        switch (soundType)
        {
            case "Shoot":
                lastShootClipIndex += Random.Range(1, 3); if (lastShootClipIndex >= data.shootClips.Count) { lastShootClipIndex = 0; }
                selectedClip = data.shootClips[lastShootClipIndex];
                lsm.PlayLocalSound(selectedClip, "gun", 2);
                break;
            case "EmptyShoot":
                lastEmptyClipIndex += Random.Range(1, 3); if (lastEmptyClipIndex >= data.noAmmoClips.Count) { lastEmptyClipIndex = 0; }
                selectedClip = data.noAmmoClips[lastEmptyClipIndex];
                lsm.PlayLocalSound(selectedClip, "gun", 1);
                break;
            case "Reload":
                lastReloadClipIndex += Random.Range(1, 3); if (lastReloadClipIndex >= data.reloadClips.Count) { lastReloadClipIndex = 0; }
                selectedClip = data.reloadClips[lastReloadClipIndex];
                lsm.PlayNonOverlapSound(selectedClip, "gun");
                break;
        }
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
                shootingThisFrameAudio = false;
                Shoot(1f);
                shootingThisFrameAudio = true;
                MuzzleFlash.gameObject.SetActive(currentBullets > 0);
                if (MuzzleFlash != null) { MuzzleFlash.Play(); }
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

            shootingThisFrameAudio = false;
            Shoot(bowCharge);
            shootingThisFrameAudio = true;
            if (pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
            {
                acc = acc * 2f;
                for (int i = 0; i < 9; i++)
                {
                    currentBullets++;
                    Shoot(bowCharge);
                    MuzzleFlash.gameObject.SetActive(currentBullets > 0);
                    if (MuzzleFlash != null) { MuzzleFlash.Play(); }
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
        PlaySound("Reload");
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
    void ClearPreInstatiated()
    {
        if(bulletReservoir.childCount == 0) { return; } int count = bulletReservoir.childCount;
        for (int i = 0; i < count; i++) 
        { 
            GameObject bul = bulletReservoir.GetChild(0).gameObject;
            bul.transform.SetParent(null);
            Destroy(bul);
        }
    }
    void PreInstatiateBullets()
    {
        if(bulletReservoir == null) { return; }
        if (magSize > reservoirSize) { reservoirSize = (int)magSize * 2; } // risk of shooting all bullets in reserve each shot, increase max
        reservoirSize = Mathf.Clamp(reservoirSize, 100, 1000);
        int bulletsToReserveThisFrame = 1; // default recovery speed [1 every frame] (lowest cost)
        switch ((int)(10*((float)bulletReservoir.childCount / (float)reservoirSize)))
        {
            case 0: bulletsToReserveThisFrame = 20; break; // RESERVE IS EMPTY!!! FULL POWER!!!!
            case 1: bulletsToReserveThisFrame = 20; break;
            case 2: bulletsToReserveThisFrame = 15; break;
            case 3: bulletsToReserveThisFrame = 10; break;
            case 4: bulletsToReserveThisFrame = 3; break;
            case 5: bulletsToReserveThisFrame = 3; break;
            case 6: bulletsToReserveThisFrame = 2; break;
            case 7: bulletsToReserveThisFrame = 2; break;
            case 8: bulletsToReserveThisFrame = 1; break;
            case 9: bulletsToReserveThisFrame = 1; break;
            case 10: return; // reserve is full :) we can be chill now. Hell, go home early PreInstantiateBullets() function.
        }

        for (int i = 0; i < bulletsToReserveThisFrame; i++)
        {
            if (bulletReservoir.childCount < reservoirSize)
            {
                GameObject spawned; BulletScript spawnedBS;
                switch (bulletType)
                {
                    case BulletType.oil: spawned = Instantiate(oilBullet, bulletReservoir); break;
                    case BulletType.nerf: spawned = Instantiate(nerfedPistolBullet, bulletReservoir); break;
                    case BulletType.standard: spawned = Instantiate(pistolBullet, bulletReservoir); break;
                    default: spawned = Instantiate(pistolBullet, bulletReservoir); break;
                }
                spawnedBS = spawned.GetComponent<BulletScript>();
                //mini setup
                spawnedBS.gunFiredFrom = this;
                spawnedBS.MiniSetUp();
                spawned.SetActive(false);
            }
            if (manager.preinstatiatedAmmoBoxFleas.childCount < fleas * 10)
            {
                GameObject spawned; BulletScript spawnedBS;
                spawned = Instantiate(fleaBullet, manager.preinstatiatedAmmoBoxFleas);
                spawnedBS = spawned.GetComponent<BulletScript>();
                //mini setup
                spawnedBS.gunFiredFrom = this;
                spawnedBS.MiniSetUp();
                spawned.SetActive(false);
            }
        }
    }
    void QuickAddToReserve()
    {
        GameObject spawned; BulletScript spawnedBS;
        switch (bulletType)
        {
            case BulletType.oil: spawned = Instantiate(oilBullet, bulletReservoir); break;
            case BulletType.nerf: spawned = Instantiate(nerfedPistolBullet, bulletReservoir); break;
            case BulletType.standard: spawned = Instantiate(pistolBullet, bulletReservoir); break;
            default: spawned = Instantiate(pistolBullet, bulletReservoir); break;
        }
        spawnedBS = spawned.GetComponent<BulletScript>();
        //mini setup
        spawnedBS.gunFiredFrom = this;
        spawnedBS.MiniSetUp();
        spawned.SetActive(false);
    }
    Transform PullBulletFromPreInstatiation(bool flea)
    {
        switch (flea)
        {
            case true:
                if (manager.preinstatiatedAmmoBoxFleas.childCount > 0)
                {
                    Transform bulletPulled = manager.preinstatiatedAmmoBoxFleas.GetChild(0); bulletPulled.gameObject.SetActive(true);
                    bulletPulled.SetParent(null);  return bulletPulled;
                }
                else
                {
                    return Instantiate(fleaBullet).transform;
                }
            case false:
                if (bulletReservoir.childCount > 0)
                {
                    Transform bulletPulled = bulletReservoir.GetChild(0); bulletPulled.gameObject.SetActive(true);
                    bulletPulled.SetParent(null); return bulletPulled;
                }
                else
                {
                    QuickAddToReserve();
                    Transform bulletPulled = bulletReservoir.GetChild(0); bulletPulled.gameObject.SetActive(true);
                    bulletPulled.SetParent(null); return bulletPulled;
                }
        }
    }
    public void SpawnBulletAtPos(Vector3 pos)
    {
        //pretty much just for improvised storage
        Transform spawnedBullet = PullBulletFromPreInstatiation(false); BulletScript bs = spawnedBullet.GetComponent<BulletScript>();
        spawnedBullet.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        spawnedBullet.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        SetBulStats(bs, dmg, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), 1);

        if (whatHandThisIsIn == "left") { manager.leftStickToCounters = 0; } else { manager.rightStickToCounters = 0; }
    }
    public virtual void EarlyShoot(bool requireAmmo)
    {
        if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[109] > 0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[109] > 0)) { littleCharge+=0.2f; }
        if (tacticalCompress > 0 && tacticalReload > 0) { dmg = dmg * (1 + (tacticalCompress / (10f / tacticalCompress))); tacticalCompress = 0; }
        if (brokenPen > 0 && brokenPenCounter >= 10) { dmg *= 2f + (1.5f * brokenPen - 1f); }

        if (fleas>0&&Random.Range(1,100)<16){
            for(int i = 0; i < fleas; i++)
            {
                Transform spawnedFlea = PullBulletFromPreInstatiation(true); acc = acc / 1; spawnedFlea.SetPositionAndRotation(firePoint.position,firePoint.rotation);
                if (target != null) { GetPosessionTargetPos(); spawnedFlea.LookAt(target.transform.position + possessionTarOffset); timeSinceShot = 5; }
                acc *= 1.5f; spawnedFlea.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc))); acc /= 1.5f;
                SetBulStats(spawnedFlea.GetComponent<BulletScript>(), dmg * 1, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), 1);
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
        animator.speed = atkSpd * 1.2f;
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
            if (!shootingThisFrameAudio) { PlaySound("Shoot"); }
            animator.SetTrigger("ForceFire");
            if (manager.healthMan.gdm.mutatedRules.Contains(11)) { manager.healthMan.playerMvt.rb.AddForce(-transform.forward * bulSpd / 3f, ForceMode.Impulse); }
            timeSinceShot = 0f;
            if (brokenInk > 0 && inkCounter < Mathf.Clamp(10 - brokenInk, 1, 9)) { inkCounter++; } else if (brokenInk > 0) { inkCounter = 0; requireAmmo = false; }
            EarlyShoot(requireAmmo);
            if (requireAmmo) { currentBullets--; currentBullets -= Mathf.Clamp(triggerHappy, 0, 1); currentBullets = Mathf.Clamp(currentBullets, 0, int.MaxValue); }
            dmg += echoDmg; echoDmg = 0;

            Transform spawnedBullet = PullBulletFromPreInstatiation(false); spawnedBullet.SetPositionAndRotation(firePoint.position, firePoint.rotation); BulletScript bulBS = spawnedBullet.GetComponent<BulletScript>();

            if (target != null) { GetPosessionTargetPos(); spawnedBullet.LookAt(target.transform.position + possessionTarOffset); timeSinceShot = 5f; }
            acc = acc / bowChar;
            spawnedBullet.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));

            SetBulStats(bulBS, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);

            if (introTrig > 0)
            {
                Transform spawnedBulletB = PullBulletFromPreInstatiation(false); spawnedBulletB.SetPositionAndRotation(firePoint.position, firePoint.rotation); BulletScript bulBBS = spawnedBulletB.GetComponent<BulletScript>();
                acc = acc / bowChar; 
                bulBS.IntroTrigSetUp(spawnedBulletB.gameObject, true);
                bulBBS.IntroTrigSetUp(spawnedBullet.gameObject, false);
                if (masterTrig > 0) {acc += 2f;} else {acc += 4;}
                spawnedBulletB.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                SetBulStats(bulBBS, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);
            }
            
            if (forkedBarrel > 0)
            {
                int bulletsSpawned = 1; if (Random.Range(1, 100) < (forkedBarrel - 1f) * 20f) { bulletsSpawned++; }
                for(int i = 0; i < bulletsSpawned; i++)
                {
                    Transform spawnedForkedBullet = PullBulletFromPreInstatiation(false); spawnedForkedBullet.SetPositionAndRotation(firePoint.position, firePoint.rotation); BulletScript forkBS = spawnedForkedBullet.GetComponent<BulletScript>();
                    acc += 4;
                    spawnedForkedBullet.Rotate(new Vector3(Random.Range(-acc, acc), Random.Range(-acc, acc), Random.Range(-acc, acc)));
                    SetBulStats(forkBS, dmg * bowChar, (Random.Range(1, 100) < critChance), (Random.Range(1, 100) < weakPointChance), bowChar);
                }
            }

            if (isFastFiring) { animator.speed = atkSpd * 3f; }

            if(whatHandThisIsIn == "left") { manager.leftStickToCounters = 0; } else{ manager.rightStickToCounters = 0; }

            if(manager.healthMan.activeEffects[20].x > 0) { currentBullets = Mathf.RoundToInt(magSize); }

            if(grenadeAttach > 0 && grenadeAttachTimer < 0)
            {
                Transform spawnedGrenade = Instantiate(grenade).transform; GrenadeAttachment ga = spawnedGrenade.GetComponent<GrenadeAttachment>(); Rigidbody grb = spawnedGrenade.GetComponent<Rigidbody>();
                spawnedGrenade.position = firePoint.transform.position + firePoint.transform.forward;
                ga.damage = dmg * 10f; ga.isGas = false;
                grb.AddForce((Vector3.up * 4f) + (firePoint.transform.forward * 20f), ForceMode.Impulse);
                grb.AddTorque(spawnedGrenade.right * 30f, ForceMode.Impulse);
                grenadeAttachTimer = manager.playerItem.FindObjByID(107).baseCooldown;
            }
            if (gasGrenadeAttach > 0 && gasGrenadeAttachTimer < 0)
            {
                Transform spawnedGrenade = Instantiate(grenade).transform; GrenadeAttachment ga = spawnedGrenade.GetComponent<GrenadeAttachment>(); Rigidbody grb = spawnedGrenade.GetComponent<Rigidbody>();
                spawnedGrenade.position = firePoint.transform.position + firePoint.transform.up / 2f + firePoint.transform.forward;
                ga.damage = dmg * 10f; ga.isGas = true;
                grb.AddForce((Vector3.up * 6f) + (firePoint.transform.forward * 20f), ForceMode.Impulse);
                grb.AddTorque(spawnedGrenade.right * 30f, ForceMode.Impulse);
                gasGrenadeAttachTimer = manager.playerItem.FindObjByID(108).baseCooldown; ;
            }
        }
        else if ((whatHandThisIsIn == "left" && manager.playerItem.leftItems[117]>0) || (whatHandThisIsIn == "right" && manager.playerItem.rightItems[117] > 0))
        {
            manager.Kick(whatHandThisIsIn);
        }
        else
        {
            animator.SetBool("NoAmmo", true);
            animator.SetTrigger("Shooting");
            if (!shootingThisFrameAudio) { PlaySound("EmptyShoot"); }
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
    void GetPosessionTargetPos()
    {
        possessionTarOffset = (Vector3.Distance(target.transform.position, transform.position) * target.velocity) / bulSpd;
    }
    void SetBulStats(BulletScript givenBullet, float givenDmg, bool isCrit, bool isWeakpoint, float givenBowChar)
    {
        givenBullet.mainCamera = cam;
        givenBullet.setStats(this, givenDmg, isCrit, bulPir, isWeakpoint, weakPointDamage, bulSpd * givenBowChar, bulSize, 
            ricochet, whatHandThisIsIn, heavyBul, heavySpirits, nuclearBul, introTrig, jam, fireSpon * 5f, sharperSpon * 5f, silverSpon * 20f, 
            helpingSpon * 5f, coolSpon * 5f, fastSpon * 10f, largeSpon * 5f, advTrig, multistage, gunkyBlessed, gunkyClaw);
    }

    public void addBullet()
    {
        currentBullets += 1;
    }
}