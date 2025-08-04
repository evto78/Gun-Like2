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
    public GameObject awpHead; Animator awpAnim; public Transform awpFirepoint;
    public GameObject ak47Head; Animator ak47Anim; public Transform ak47Firepoint;
    public GameObject uziHead; Animator uziAnim; public Transform uziFirepoint;

    public GameObject droppedAK; float dropAkTimer;
    public GameObject droppedUZI; float dropUziTimer;

    public GameObject uziBullet; public float uziBulSpd; public float uziBulDmg;
    public GameObject ak47Bullet; public float ak47BulSpd; public float ak47BulDmg;
    public GameObject awpBullet; public float awpBulSpd; public float awpBulDmg;

    public float uziAcc; public float akAcc; public float awpAcc;

    int lastUziAttack = 0; int curUziCycle = 0; float uziBurstTime; float uziCooldownTime; float uziAttackTime; float uziBurstTimer; float uziCooldownTimer; float uziAttackTimer; float uziWait; int uzibulperburst; int uzibulshot;
    int lastAk47Attack = 0; int curAk47Cycle = 0; float akBurstTime; float akCooldownTime; float akAttackTime; float akBurstTimer; float akCooldownTimer; float akAttackTimer; float akWait; int akbulperburst; int akbulshot;
    int lastAwpAttack = 0; int curAwpCycle = 0; float awpCooldownTime; float awpAttackTime; float awpCooldownTimer; float awpAttackTimer; float awpWait;
    void Start()
    {
        milestones = new List<int>();
        milestones.Add(90);
        milestones.Add(75);
        milestones.Add(45);
        milestones.Add(30);
        milestones.Add(10);
        milestones.Add(-1);
        ehm = GetComponent<EnemyHealthManager>();
        awpTimer = 12f; akTimer = 8f; uziTimer = 4f; phase = phaseTypes.a;
        masterDmg = ehm.baseDamage * ehm.difficultyScale * ehm.gdm.difficulty;
        trashOrb = GetComponentInChildren<TrashOrb>();

        awpAnim = awpHead.GetComponentInChildren<Animator>();
        ak47Anim = ak47Head.GetComponentInChildren<Animator>();
        uziAnim = uziHead.GetComponentInChildren<Animator>();

        ehm.playerHM.uiMan.bossHealthBars[0].gameObject.SetActive(true);
        ehm.playerHM.uiMan.bossHealthBars[0].ehm = ehm;
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
        switch (phase) //make sure phase change milestones are correct
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
                    case 0: uziBurstTime = 0.05f; uziCooldownTime = 0.5f; uziAttackTime = 2f; uziWait = 0f; uzibulperburst = 5; break;
                    case 1: uziBurstTime = 0f; uziCooldownTime = 0.025f; uziAttackTime = 2.5f; uziWait = 1f; uzibulperburst = 1; break;
                }
                curUziCycle = 0;
                break;
            case 1: break;
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
                    case 0: akBurstTime = 0f; akCooldownTime = 0.5f; akAttackTime = 3f; akWait = 2f; akbulperburst = 1; break;
                    case 1: akBurstTime = 0f; akCooldownTime = 0f; akAttackTime = 0f; akWait = 0f; akbulperburst = 0; break;
                }
                curAk47Cycle = 0;
                break;
            case 1: break;
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
                    case 0: awpCooldownTime = 1f; awpAttackTime = 1f; awpWait = 3f;break;
                    case 1: awpCooldownTime = 1f; awpAttackTime = 3f; awpWait = 1f;break;
                }
                curAwpCycle = 0;
                break;
            case 1: break;
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
            case 1: CreateBullet(1, 1, 1);
            break;
            case 2: CreateBullet(2, 1, 1);
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
                spawnedBul = Instantiate(uziBullet, uziFirepoint.position, uziFirepoint.rotation);
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= uziBulSpd; dmg *= uziBulDmg;
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-uziAcc, uziAcc), Random.Range(-uziAcc, uziAcc), Random.Range(-uziAcc, uziAcc))); break;
            case 1:
                spawnedBul = Instantiate(ak47Bullet, ak47Firepoint.position, ak47Firepoint.rotation);
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= ak47BulSpd; dmg *= ak47BulDmg;
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-akAcc, akAcc), Random.Range(-akAcc, akAcc), Random.Range(-akAcc, akAcc))); break;
            case 2:
                spawnedBul = Instantiate(awpBullet, awpFirepoint.position, awpFirepoint.rotation);
                eb = spawnedBul.GetComponent<EnemyBullet>(); speedMod *= awpBulSpd; dmg *= awpBulDmg;
                spawnedBul.transform.Rotate(new Vector3(Random.Range(-awpAcc, awpAcc), Random.Range(-awpAcc, awpAcc), Random.Range(-awpAcc, awpAcc))); break;
        }
        eb.gameObject.GetComponent<Rigidbody>().AddForce(eb.transform.forward * speedMod, ForceMode.Impulse);
        eb.SetStats(dmg, ehm);
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
