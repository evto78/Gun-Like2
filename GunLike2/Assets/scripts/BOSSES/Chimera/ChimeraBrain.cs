using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraBrain : MonoBehaviour
{
    public enum phaseTypes { a, ba, bb, c} public phaseTypes phase;
    public float awpTimer; bool awpActive;
    public float akTimer; bool akActive;
    public float uziTimer; bool uziActive;
    EnemyHealthManager ehm; public float percentage;
    public float uziHitDmg = 0.5f; public float akHitDmg = 0;
    float masterDmg; List<int> milestones;
    TrashOrb trashOrb;
    public GameObject awpHead; Animator awpAnim; public Transform awpFirepoint; public ParticleSystem awpMPS;
    public GameObject ak47Head; Animator ak47Anim; public Transform ak47Firepoint; public ParticleSystem akMPS;
    public GameObject uziHead; Animator uziAnim; public Transform uziFirepoint; public ParticleSystem uziMPS;

    public GameObject droppedAK; float dropAkTimer;
    public GameObject droppedUZI; float dropUziTimer;

    public GameObject uziBullet; public float uziBulSpd; public float uziBulDmg;
    public GameObject ak47Bullet; public float ak47BulSpd; public float ak47BulDmg;
    public GameObject awpBullet; public float awpBulSpd; public float awpBulDmg;

    List<Target> unassignedTargetsAK = new List<Target>();
    List<Target> unassignedTargetsUZI = new List<Target>();
    List<Target> unassignedTargetsAWP = new List<Target>();
    public GameObject telegraph;
    public GameObject awpLaserTelegraphPrefab; Transform awpLaserTelegraph; float awpLaserTelegraphTimer; float awpLaserTelegraphFollowTimer; public LineRenderer awpLaserLR;
    public GameObject target; Transform lineStart1; Transform lineEnd1;
    public GameObject awpBeacon; public GameObject akSmokeGernade;

    public float uziAcc; public float akAcc; public float awpAcc;

    int lastUziAttack = 0; int curUziCycle = 0; float uziBurstTime; float uziCooldownTime; float uziAttackTime; float uziBurstTimer; float uziCooldownTimer; float uziAttackTimer; float uziWait; int uzibulperburst; int uzibulshot;
    int lastAk47Attack = 0; int curAk47Cycle = 0; float akBurstTime; float akCooldownTime; float akAttackTime; float akBurstTimer; float akCooldownTimer; float akAttackTimer; float akWait; int akbulperburst; int akbulshot;
    int lastAwpAttack = 0; int curAwpCycle = 0; float awpCooldownTime; float awpAttackTime; float awpCooldownTimer; float awpAttackTimer; float awpWait;

    Transform player; Transform curUziTarget; Transform curAkTarget; Transform curAwpTarget; public GameObject awpFlare;
    public GameObject akMag; public GameObject orbit; Transform orbitPoint; public GameObject uziBoomerang; public GameObject awpTurretPrefab; List<AWPBossTurret> awpBossTurretList = new List<AWPBossTurret>(); public List<Vector3> potentialTurretSpawns; List<Vector3> avaliableSpawns;
    public GameObject nuke; public GameObject nukeTarget; public GameObject earthquake;
    void Start()
    {
        awpFlare.SetActive(false);
        avaliableSpawns = potentialTurretSpawns;
        milestones = new List<int>();
        milestones.Add(90);
        milestones.Add(75);
        milestones.Add(45);
        milestones.Add(30);
        milestones.Add(10);
        milestones.Add(-1);
        ehm = GetComponent<EnemyHealthManager>();
        player = ehm.gdm.phm.gameObject.transform;
        awpTimer = 12f; akTimer = 8f; uziTimer = 4f; phase = phaseTypes.a;
        masterDmg = ehm.baseDamage * ehm.difficultyScale * ehm.gdm.difficulty;
        trashOrb = GetComponentInChildren<TrashOrb>();

        awpAnim = awpHead.GetComponentInChildren<Animator>();
        ak47Anim = ak47Head.GetComponentInChildren<Animator>();
        uziAnim = uziHead.GetComponentInChildren<Animator>();

        ehm.playerHM.uiMan.bossHealthBars[0].SetActive(true);
        ehm.playerHM.uiMan.bossHealthBars[0].GetComponent<BossHealthBar>().ehm = ehm;

        awpLaserTelegraph = Instantiate(awpLaserTelegraphPrefab).transform;

        orbitPoint = Instantiate(orbit).transform.GetChild(0);
    }
    private void OnDestroy()
    {
        if(ehm.playerHM.uiMan.bossHealthBars[0] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (ehm.playerHM.uiMan.bossHealthBars[0] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(false);
        }
    }
    void Update()
    {
        percentage = (ehm.curHp / ehm.maxHp) * 100f;
        TimerManagement();
        switch (phase)
        {
            case (phaseTypes.a):
                if (uziActive) { UZIActivate(0); }
                if (akActive) { AK47Activate(0); }
                if (awpActive) { AWPActivate(0); }
                if (percentage <= 75 && uziHitDmg >= akHitDmg) { phase = phaseTypes.ba; uziAnim.SetTrigger("Fall"); dropUziTimer = 0.45f; awpAnim.SetTrigger("TakeUzi"); } // Uzi Falls off
                else if (percentage <= 75 && akHitDmg > uziHitDmg) { phase = phaseTypes.bb; ak47Anim.SetTrigger("Fall"); dropAkTimer = 0.45f; awpAnim.SetTrigger("TakeAk"); } // Ak Falls off
                break;
            case (phaseTypes.ba):
                if (akActive) { AK47Activate(1); }
                if (awpActive) { AWPActivate(1); }
                if (percentage <= 30) { phase = phaseTypes.c; ak47Anim.SetTrigger("Fall"); dropAkTimer = 0.45f; awpAnim.SetTrigger("TakeAk"); }
                break;
            case (phaseTypes.bb):
                if (uziActive) { UZIActivate(1); }
                if (awpActive) { AWPActivate(1); }
                if (percentage <= 30) { phase = phaseTypes.c; uziAnim.SetTrigger("Fall"); dropUziTimer = 0.45f; awpAnim.SetTrigger("TakeUzi"); }
                break;
            case (phaseTypes.c):
                if (awpActive) { AWPActivate(2); }
                break;
        }
        if(milestones.Count > 1 && percentage < milestones[0]) { trashOrb.PlayHit(); milestones.RemoveAt(0); }
        if(dropAkTimer > 0)
        {
            dropAkTimer -= Time.deltaTime;
            if(dropAkTimer <= 0) { Instantiate(droppedAK, ak47Anim.transform.position, ak47Anim.transform.rotation); ak47Head.SetActive(false); }
        }
        if (dropUziTimer > 0)
        {
            dropUziTimer -= Time.deltaTime;
            if (dropUziTimer <= 0) { Instantiate(droppedUZI, uziAnim.transform.position, uziAnim.transform.rotation); uziHead.SetActive(false); }
        }
        AttackManagement();

        if(curUziTarget == player)
        {
            uziHead.transform.LookAt(player);
        } else if(curUziTarget == null)
        {
            if (unassignedTargetsUZI.Count > 0) { uziHead.transform.LookAt(unassignedTargetsUZI[0].transform); }
        }
        if (curAkTarget == player)
        {
            ak47Head.transform.LookAt(player);
        }
        else if (curAkTarget == null)
        {
            if (unassignedTargetsAK.Count > 0) { ak47Head.transform.LookAt(unassignedTargetsAK[0].transform); }
        }
        if(curAwpTarget == player)
        {
            awpHead.transform.LookAt(player);
        }
        else if ( curAwpTarget == awpLaserTelegraph.transform)
        {
            awpHead.transform.LookAt(awpLaserTelegraph.transform);
        }
        else if (curAwpTarget == null)
        {
            if(unassignedTargetsAWP.Count > 0) { awpHead.transform.LookAt(unassignedTargetsAWP[0].transform); }
            else { awpHead.transform.localEulerAngles = Vector3.zero; }
        }
        if(awpLaserTelegraphFollowTimer > 0) { awpLaserTelegraphFollowTimer -= Time.deltaTime; awpLaserTelegraph.transform.position = player.position; }
        awpLaserLR.enabled = awpLaserTelegraphTimer > 0; awpLaserTelegraphTimer -= Time.deltaTime;
        awpLaserLR.SetPosition(0, awpFirepoint.position);
        awpLaserLR.SetPosition(1, awpLaserTelegraph.position);
        //GetToPos
        transform.position = orbitPoint.position;
        //SlowlyRotateTowardsPlayer
        Vector3 curAngle = transform.localEulerAngles;
        transform.LookAt(player.position + (Vector3.up * (transform.position.y-player.position.y)));
        Vector3 desiredAngle = transform.localEulerAngles;
        transform.localEulerAngles = Vector3.Lerp(curAngle, desiredAngle, Time.deltaTime);
    }
    void AttackManagement()
    {
        if(uziWait > 0) { uziWait -= Time.deltaTime;} else
        {
            if(uziAttackTimer > 0)
            {
                uziAttackTimer -= Time.deltaTime;
                if (uziCooldownTimer > 0) { uziCooldownTimer -= Time.deltaTime; }
                else
                {
                    if(uzibulshot < uzibulperburst)
                    {
                        if (uziBurstTimer > 0) { uziBurstTimer -= Time.deltaTime; }
                        else
                        {
                            Shoot(0);
                            uziBurstTimer = uziBurstTime;
                            uzibulshot++;
                        }
                    }
                    else
                    {
                        uziCooldownTimer = uziCooldownTime; uzibulshot = 0;
                    }
                }
            }
        }
        if (akWait > 0) { akWait -= Time.deltaTime; }
        else
        {
            if (akAttackTimer > 0)
            {
                akAttackTimer -= Time.deltaTime;
                if (akCooldownTimer > 0) { akCooldownTimer -= Time.deltaTime; }
                else
                {
                    if (akbulshot < akbulperburst)
                    {
                        if (akBurstTimer > 0) { akBurstTimer -= Time.deltaTime; }
                        else
                        {
                            Shoot(1);
                            akBurstTimer = akBurstTime;
                            akbulshot++;
                        }
                    }
                    else
                    {
                        akCooldownTimer = akCooldownTime; akbulshot = 0;
                    }
                }
            }
        }
        if (awpWait > 0) { awpWait -= Time.deltaTime; }
        else
        {
            if (awpAttackTimer > 0)
            {
                awpAttackTimer -= Time.deltaTime;
                if (awpCooldownTimer > 0) { awpCooldownTimer -= Time.deltaTime; }
                else
                {
                    Shoot(2);
                    awpCooldownTimer = awpCooldownTime;
                }
            }
        }
    }
    void UZIActivate(int phase)
    {
        switch (phase)
        {
            case 0:
                lastUziAttack = Random.Range(0, 2);
                switch (lastUziAttack)
                {
                    case 0: uziBurstTime = 0.05f; uziCooldownTime = 0.5f; uziAttackTime = 2f; uziWait = 0f; uzibulperburst = 5; curUziTarget = player; break;
                    case 1: uziBurstTime = 0f; uziCooldownTime = 0.025f; uziAttackTime = 2.5f; uziWait = 1f; uzibulperburst = 1; curUziTarget = null;
                        lineStart1 = Instantiate(telegraph).transform; lineEnd1 = Instantiate(telegraph).transform;
                        lineStart1.transform.position = player.transform.position + player.gameObject.GetComponent<Rigidbody>().velocity + new Vector3(Random.Range(-10f,10f), 0, Random.Range(-10f, 10f));
                        lineEnd1.transform.position = lineStart1.position + (new Vector3(Random.Range(40f,60f), 0, Random.Range(40f, 60f))); StartCoroutine(spawnTargetsUziSpray(0.025f, 1f));
                        Destroy(lineStart1.gameObject,4f); Destroy(lineEnd1.gameObject,4f); break;
                }
                curUziCycle = 0;
                break;
            case 1:
                if (curUziCycle < 2)
                {
                    lastUziAttack = Random.Range(0, 2);
                    switch (lastUziAttack)
                    {
                        case 0: uziBurstTime = 0.025f; uziCooldownTime = 0.5f; uziAttackTime = 2f; uziWait = 0f; uzibulperburst = 10; curUziTarget = player; break;
                        case 1:
                            uziBurstTime = 0f; uziCooldownTime = 0.01f; uziAttackTime = 1.5f; uziWait = 0.5f; uzibulperburst = 2; curUziTarget = null;
                            lineStart1 = Instantiate(telegraph).transform; lineEnd1 = Instantiate(telegraph).transform;
                            lineStart1.transform.position = player.transform.position + player.gameObject.GetComponent<Rigidbody>().velocity + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                            lineEnd1.transform.position = lineStart1.position + (new Vector3(Random.Range(40f, 60f), 0, Random.Range(40f, 60f))); StartCoroutine(spawnTargetsUziSpray(0.01f, 1f));
                            Destroy(lineStart1.gameObject, 2f); Destroy(lineEnd1.gameObject, 2f); break;
                    }
                }
                else
                {
                    uziTimer = 4f;
                    curUziCycle = 0;
                    uziHead.transform.LookAt(player);
                    uziBurstTime = 0f; uziCooldownTime = 0f; uziAttackTime = 0f; uziWait = 0f; uzibulperburst = 0; curUziTarget = null;
                    uziAnim.SetTrigger("Spin");
                    StartCoroutine(uziAdvancedBoomerang(1f, 0.5f, 3f));
                }

                break;
        }
        uziBurstTimer = 0f; uziCooldownTimer = 0f; uziAttackTimer = uziAttackTime;

        curUziCycle++;
    }
    void AK47Activate(int phase)
    {
        switch (phase)
        {
            case 0:
                lastAk47Attack = Random.Range(0, 2);
                switch (lastAk47Attack)
                {
                    case 0: akBurstTime = 0f; akCooldownTime = 0.5f; akAttackTime = 3f; akWait = 2f; akbulperburst = 1; StartCoroutine(spawnTargetsAk(6, 2f)); curAkTarget = null; break;
                    case 1: akBurstTime = 0f; akCooldownTime = 0f; akAttackTime = 0f; akWait = 0f; akbulperburst = 0; curAkTarget = null; Instantiate(akMag, ak47Head.transform.position, ak47Head.transform.rotation); break;
                }
                curAk47Cycle = 0;
                break;
            case 1:
                if(curAk47Cycle < 2)
                {
                    lastAk47Attack = Random.Range(0, 2);
                    switch (lastAk47Attack)
                    {
                        case 0: akBurstTime = 0f; akCooldownTime = 0.25f; akAttackTime = 3f; akWait = 1f; akbulperburst = 1; StartCoroutine(spawnTargetsAk(6, 1f)); curAkTarget = null; break;
                        case 1: akBurstTime = 0f; akCooldownTime = 0f; akAttackTime = 0f; akWait = 0f; akbulperburst = 0; curAkTarget = null; Instantiate(akMag, ak47Head.transform.position, ak47Head.transform.rotation); break;
                    }
                }
                else
                {
                    curAk47Cycle = 0;
                    ak47Head.transform.LookAt(player);
                    akBurstTime = 0f; akCooldownTime = 0f; akAttackTime = 0f; akWait = 0f; akbulperburst = 0; curAkTarget = null;
                    ak47Anim.SetTrigger("Smokes");
                    StartCoroutine(akAdvancedSmokeDeploy(0.5f, 0.2f, 3f));
                }
                
                break;
        }
        akBurstTimer = 0f; akCooldownTimer = 0f; akAttackTimer = akAttackTime;

        curAk47Cycle++;
    }
    void AWPActivate(int phase)
    {
        switch (phase)
        {
            case 0:
                lastAwpAttack = Random.Range(0, 2);
                switch (lastAwpAttack)
                {
                    case 0: awpCooldownTime = 1f; awpAttackTime = 1f; awpWait = 3f; curAwpTarget = awpLaserTelegraph.transform; awpLaserTelegraphFollowTimer=2.5f; awpLaserTelegraphTimer=3f; break;
                    case 1: awpCooldownTime = 1f; awpAttackTime = 3f; awpWait = 1f; curAwpTarget = null; StartCoroutine(spawnTargetsAwpTriangulate(1f)); break;
                }
                curAwpCycle = 0;
                break;
            case 1: 
                if(curAwpCycle < 2)
                {
                    lastAwpAttack = Random.Range(0, 2);
                    switch (lastAwpAttack)
                    {
                        case 0: awpCooldownTime = 0.5f; awpAttackTime = 0.5f; awpWait = 1.5f; curAwpTarget = awpLaserTelegraph.transform; awpLaserTelegraphFollowTimer = 1f; awpLaserTelegraphTimer = 1.5f;
                            for (int i = 0; i < awpBossTurretList.Count; i++)
                            {
                                awpBossTurretList[i].PrepareShoot(awpWait + 0.5f*i, awpLaserTelegraphFollowTimer + 0.5f*i);
                            }
                            break;
                        case 1: awpCooldownTime = 0.5f; awpAttackTime = 1.5f; awpWait = 0.5f; curAwpTarget = null; StartCoroutine(spawnTargetsAwpTriangulate(0.5f)); break;
                    }
                }
                else
                {
                    curAwpCycle = 0;
                    awpCooldownTime = 0f; awpAttackTime = 0f; awpWait = 0f; curAwpTarget = null;
                    awpTimer = 8f;

                    awpAnim.SetTrigger("HiredHelp");
                    StartCoroutine(awpHiredHelp1(2f));
                }
                break;
            case 2:
                switch (curAwpCycle)
                {
                    case 0://BASIC
                        lastAwpAttack = Random.Range(0, 2);
                        switch (lastAwpAttack)
                        {
                            case 0:
                                awpCooldownTime = 0.5f; awpAttackTime = 0.5f; awpWait = 1.5f; curAwpTarget = awpLaserTelegraph.transform; awpLaserTelegraphFollowTimer = 1f; awpLaserTelegraphTimer = 1.5f;
                                for (int i = 0; i < awpBossTurretList.Count; i++)
                                {
                                    awpBossTurretList[i].PrepareShoot(awpWait + 0.5f * i, awpLaserTelegraphFollowTimer + 0.5f * i);
                                }
                                break;
                            case 1: awpCooldownTime = 0.5f; awpAttackTime = 1.5f; awpWait = 0.5f; curAwpTarget = null; StartCoroutine(spawnTargetsAwpTriangulate(0.5f)); break;
                        }
                        break;
                    case 1://BASIC
                        lastAwpAttack = Random.Range(0, 2);
                        switch (lastAwpAttack)
                        {
                            case 0:
                                awpCooldownTime = 0.5f; awpAttackTime = 0.5f; awpWait = 1.5f; curAwpTarget = awpLaserTelegraph.transform; awpLaserTelegraphFollowTimer = 1f; awpLaserTelegraphTimer = 1.5f;
                                for (int i = 0; i < awpBossTurretList.Count; i++)
                                {
                                    awpBossTurretList[i].PrepareShoot(awpWait + 0.5f * i, awpLaserTelegraphFollowTimer + 0.5f * i);
                                }
                                break;
                            case 1: awpCooldownTime = 0.5f; awpAttackTime = 1.5f; awpWait = 0.5f; curAwpTarget = null; StartCoroutine(spawnTargetsAwpTriangulate(0.5f)); break;
                        }
                        break;
                    case 2://ADVANCED
                        switch (Random.Range(0, 2))
                        {
                            case 0:
                                awpAnim.SetTrigger("Greeting");
                                StartCoroutine(awpGreeting());
                                break;
                            case 1:
                                awpAnim.SetTrigger("Earthquake");
                                StartCoroutine(awpEarthQuake());
                                break;
                        }
                        break;
                    case 3://BASIC
                        lastAwpAttack = Random.Range(0, 2);
                        switch (lastAwpAttack)
                        {
                            case 0:
                                awpCooldownTime = 0.5f; awpAttackTime = 0.5f; awpWait = 1.5f; curAwpTarget = awpLaserTelegraph.transform; awpLaserTelegraphFollowTimer = 1f; awpLaserTelegraphTimer = 1.5f;
                                for (int i = 0; i < awpBossTurretList.Count; i++)
                                {
                                    awpBossTurretList[i].PrepareShoot(awpWait + 0.5f * i, awpLaserTelegraphFollowTimer + 0.5f * i);
                                }
                                break;
                            case 1: awpCooldownTime = 0.5f; awpAttackTime = 1.5f; awpWait = 0.5f; curAwpTarget = null; StartCoroutine(spawnTargetsAwpTriangulate(0.5f)); break;
                        }
                        break;
                    case 4://SPECIAL
                        awpAnim.SetTrigger("Overload");
                        StartCoroutine(awpOverload());
                        curAwpCycle = 0;
                        awpTimer = 6f;
                        break;
                }
                break;
        }
        awpCooldownTimer = 0f; awpAttackTimer = awpAttackTime;

        curAwpCycle++;
    }
    void TimerManagement()
    {
        switch (phase) //make sure timer is correct
        {
            case (phaseTypes.a):
                if (awpActive) { awpTimer = 12f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (akActive) { akTimer = 8f; akActive = false; } akTimer -= Time.deltaTime; if (akTimer <= 0) { akActive = true; }
                if (uziActive) { uziTimer = 4f; uziActive = false; } uziTimer -= Time.deltaTime; if (uziTimer <= 0) { uziActive = true; }
                break;
            case (phaseTypes.ba):
                if (awpActive) { awpTimer = 6f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (akActive) { akTimer = 4f; akActive = false; } akTimer -= Time.deltaTime; if (akTimer <= 0) { akActive = true; }
                break;
            case (phaseTypes.bb):
                if (awpActive) { awpTimer = 6f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                if (uziActive) { uziTimer = 2f; uziActive = false; } uziTimer -= Time.deltaTime; if (uziTimer <= 0) { uziActive = true; }
                break;
            case (phaseTypes.c):
                if (awpActive) { awpTimer = 4f; awpActive = false; } awpTimer -= Time.deltaTime; if (awpTimer <= 0) { awpActive = true; }
                break;
        }
    }
    void Shoot(int type)
    {
        switch (type)
        {
            case 0: CreateBullet(0, 1, 1); uziAnim.SetTrigger("Fire");
            break;
            case 1: CreateBullet(1, 1, 1); ak47Anim.SetTrigger("Fire");
            break;
            case 2: CreateBullet(2, 1, 1); awpAnim.SetTrigger("Fire");
            break;
            case 3: CreateBullet(0, 0.5f, 1);
            break;
        }
    }
    void CreateBullet(int type, float speedMod, float dmg)
    {
        EnemyBullet eb = null; GameObject spawnedBul = null;
        switch (type)
        {
            case 0:
                uziMPS.Play();
                spawnedBul = Instantiate(uziBullet, uziFirepoint.position, uziFirepoint.rotation);
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-uziAcc, uziAcc), Random.Range(-uziAcc, uziAcc), Random.Range(-uziAcc, uziAcc)));
                if (unassignedTargetsUZI.Count > 0) { spawnedBul.transform.LookAt(unassignedTargetsUZI[0].transform); unassignedTargetsUZI[0].assignedTar = spawnedBul; unassignedTargetsUZI[0].assigned = true; unassignedTargetsUZI.RemoveAt(0); }
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= uziBulSpd; dmg *= uziBulDmg;
                break;
            case 1:
                akMPS.Play();
                spawnedBul = Instantiate(ak47Bullet, ak47Firepoint.position, ak47Firepoint.rotation);
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-akAcc, akAcc), Random.Range(-akAcc, akAcc), Random.Range(-akAcc, akAcc))); 
                if(unassignedTargetsAK.Count > 0) { spawnedBul.transform.LookAt(unassignedTargetsAK[0].transform); unassignedTargetsAK[0].assignedTar = spawnedBul; unassignedTargetsAK[0].assigned = true; unassignedTargetsAK.RemoveAt(0); }
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= ak47BulSpd; dmg *= ak47BulDmg;
                break;
            case 2:
                awpMPS.Play();
                spawnedBul = Instantiate(awpBullet, awpFirepoint.position, awpFirepoint.rotation);
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-awpAcc, awpAcc), Random.Range(-awpAcc, awpAcc), Random.Range(-awpAcc, awpAcc))); 
                if(curAwpTarget == awpLaserTelegraph) { spawnedBul.transform.LookAt(awpLaserTelegraph); }
                if (unassignedTargetsAWP.Count > 0) { spawnedBul.transform.LookAt(unassignedTargetsAWP[0].transform); unassignedTargetsAWP[0].assignedTar = spawnedBul; unassignedTargetsAWP[0].assigned = true; unassignedTargetsAWP.RemoveAt(0); }
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= awpBulSpd; dmg *= awpBulDmg;
                break;
        }
        eb.gameObject.GetComponent<Rigidbody>().AddForce(eb.transform.forward * speedMod, ForceMode.Impulse);
        eb.SetStats(dmg, ehm);
    }
    private IEnumerator spawnTargetsAk(int targets, float time)
    {
        for(int i = 0; i < targets; i++)
        {
            yield return new WaitForSeconds(time / targets);
            GameObject spawnedTarget = Instantiate(target);
            spawnedTarget.transform.position = player.position + new Vector3(Random.Range(-20f, 20f), 50, Random.Range(-20f, 20f));
            unassignedTargetsAK.Add(spawnedTarget.GetComponent<Target>());
        }
    }
    private IEnumerator spawnTargetsUziSpray(float fireRate, float time)
    {
        for (int i = 0; i < time/fireRate; i++)
        {
            yield return new WaitForSeconds(fireRate);
            if(unassignedTargetsUZI.Count > 100) { break; }
            GameObject spawnedTarget = Instantiate(target);
            spawnedTarget.transform.position = Vector3.Lerp(lineStart1.position, lineEnd1.position, ((float)i/(time/fireRate))) + new Vector3(Random.Range(-5f, 5f), 50, Random.Range(-5f, 5f));
            unassignedTargetsUZI.Add(spawnedTarget.GetComponent<Target>());
        }
    }
    private IEnumerator spawnTargetsAwpTriangulate(float time)
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(time/3);
            GameObject spawnedTarget = Instantiate(awpBeacon);
            spawnedTarget.transform.position = player.transform.position + Vector3.up * 50f;
            switch (i)
            {
                case 0: spawnedTarget.transform.position += new Vector3(18, 0, 18); break;
                case 1: spawnedTarget.transform.position += new Vector3(-18, 0, 18); break;
                case 2: spawnedTarget.transform.position += new Vector3(0, 0, -24); break;
            }
            unassignedTargetsAWP.Add(spawnedTarget.GetComponent<Target>());
        }
    }
    private IEnumerator akAdvancedSmokeDeploy(float wait, float between, float time)
    {
        yield return new WaitForSeconds(wait);
        for (int i = 0; i < time/between; i++)
        {
            yield return new WaitForSeconds(between);
            GameObject spawnedsmoke = Instantiate(akSmokeGernade);
            spawnedsmoke.transform.position = ak47Firepoint.transform.position;
            spawnedsmoke.GetComponent<Rigidbody>().AddForce((ak47Firepoint.transform.forward+new Vector3(Random.Range(-0.25f,0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f))) * 40, ForceMode.Impulse);
        }
    }
    private IEnumerator uziAdvancedBoomerang(float wait, float between, float time)
    {
        yield return new WaitForSeconds(wait);
        for (int i = 0; i < time / between; i++)
        {
            yield return new WaitForSeconds(between);
            GameObject spawnedrang = Instantiate(uziBoomerang);
            spawnedrang.transform.position = uziHead.transform.position;
            spawnedrang.GetComponent<UZIBoomerang>().uziHead = uziHead.transform;
        }
    }
    private IEnumerator awpHiredHelp1(float wait)
    {
        yield return new WaitForSeconds(wait);

        while(awpBossTurretList.Count < 2)
        {
            GameObject spawnedTur = Instantiate(awpTurretPrefab);
            awpBossTurretList.Add(spawnedTur.GetComponent<AWPBossTurret>());
            int i = Random.Range(0, avaliableSpawns.Count);
            spawnedTur.transform.position = avaliableSpawns[i];
            avaliableSpawns.RemoveAt(i);
        }
    }
    private IEnumerator awpHiredHelp2(float wait)
    {
        yield return new WaitForSeconds(wait);
        awpFlare.SetActive(true);

        while (avaliableSpawns.Count > 0)
        {
            GameObject spawnedTur = Instantiate(awpTurretPrefab);
            awpBossTurretList.Add(spawnedTur.GetComponent<AWPBossTurret>());
            int i = Random.Range(0, avaliableSpawns.Count);
            spawnedTur.transform.position = avaliableSpawns[i];
            avaliableSpawns.RemoveAt(i);
        }

        yield return new WaitForSeconds(2);
        awpFlare.SetActive(false);
    }
    private IEnumerator awpOverload()
    {
        curAwpTarget = null; awpHead.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(2);
        GameObject spawnedTarget = Instantiate(nukeTarget);
        spawnedTarget.transform.position = player.transform.position + Vector3.up * 50f;
        unassignedTargetsAWP.Add(spawnedTarget.GetComponent<Target>());
        yield return new WaitForSeconds(2);
        GameObject spawnedNuke = Instantiate(nuke);
        spawnedNuke.transform.position = unassignedTargetsAWP[0].transform.position;
        Shoot(2);
    }
    private IEnumerator awpGreeting()
    {
        curAwpTarget = null; awpHead.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(2);
    }
    private IEnumerator awpEarthQuake()
    {
        curAwpTarget = null; awpHead.transform.localEulerAngles = Vector3.zero;
        yield return new WaitForSeconds(1);
        Shoot(2);
        yield return new WaitForSeconds(1);
        Shoot(2);
        yield return new WaitForSeconds(1);
        Shoot(2);
    }
    public void AWPHIT()
    {

    }
    public void AK47HIT()
    {
        akHitDmg += 1;
    }
    public void UZIHIT()
    {
        uziHitDmg += 1;
    }
}
