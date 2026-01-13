using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UZIWalkerBrain : MonoBehaviour
{
    GameObject player;
    public GameObject turrethead;
    public GameObject firepoint;
    Animator turretAnim;
    EnemyHealthManager hm;
    HealthManager phm;
    UIManager uiMan;
    NavMeshAgent agent;
    NavAI nav;

    public GameObject bullet;
    public float shotCooldown;
    float cooldownTimer;
    public int bulPerBurst;
    int bulShot;
    public float fireRate;
    float fireTimer;
    public float dmg;
    public float bulSpeed;
    public float accuracy;
    public float warnTime; bool sentWarningThisBurst;

    bool jammed;
    public ParticleSystem jamEffect;

    public enum state { idle, chasing} public state curState;

    void Start()
    {
        player = GameObject.Find("Player");
        phm = player.GetComponent<HealthManager>();
        uiMan = phm.uiMan;
        hm = GetComponent<EnemyHealthManager>();
        dmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
        turretAnim = turrethead.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        nav = GetComponent<NavAI>();
        curState = state.idle; nav.SetState(NavAI.state.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if((hm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(player.transform.position, transform.position) > 200 && hm.curHp == hm.maxHp)) && !hm.gdm.pointsLocked)
        {//Player is invisible. (via circus mask)
            curState = state.idle;
            nav.SetState(NavAI.state.wander);
        }
        else
        {
            curState = state.chasing;
            nav.SetState(NavAI.state.chase);
        }

        jammed = hm.activeEffects[30].x > 0;

        switch (curState)
        {
            case state.idle: break;
            case state.chasing:
                turrethead.transform.LookAt(player.transform.position);

                if (CanShoot())
                {
                    if (fireTimer + cooldownTimer <= 0)
                    {
                        if (!sentWarningThisBurst) { cooldownTimer = warnTime; break; }
                        Shoot();
                        turretAnim.SetBool("Recharge", false);
                        turretAnim.SetTrigger("Fire");
                        turretAnim.speed = 1f / fireRate;
                        bulShot++;
                        fireTimer = fireRate;
                        if (bulShot >= bulPerBurst)
                        {
                            if (jammed) { hm.activeEffects[30] -= new Vector4(1,0,0,0); }
                            cooldownTimer = shotCooldown;
                            turretAnim.speed = 1f / cooldownTimer;
                            turretAnim.SetBool("Recharge", true);
                            bulShot = 0;
                            sentWarningThisBurst = false;
                        }
                    }
                    else if (cooldownTimer <= 0f)
                    {
                        turretAnim.SetBool("Recharge", false);
                    }
                    if (cooldownTimer <= warnTime && !sentWarningThisBurst) { uiMan.AddDangerWarnSource(transform, transform.position, false, warnTime); }
                }
                else if (cooldownTimer <= 0f)
                {
                    turretAnim.SetBool("Recharge", false);
                }
                fireTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                cooldownTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                break;
        }

        if (hm.activeEffects[39].x > 0) { agent.speed = 7f / (1.5f * (1.1f*(hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { agent.speed = 7f; }
    }
    bool CanShoot()
    {
        Ray ray = new Ray(firepoint.transform.position, player.transform.position - firepoint.transform.position);
        return Physics.Raycast(ray, out RaycastHit hit, 75) && hit.transform.gameObject.layer == 7;
    }
    void Shoot()
    {
        if (jammed)
        {
            jamEffect.Play();
            return;
        }

        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = firepoint.transform.position;
        spawnedBullet.transform.LookAt(player.transform.position + (player.GetComponent<Rigidbody>().velocity * 0f));
        spawnedBullet.transform.Rotate(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0) * accuracy);
        spawnedBullet.GetComponent<EnemyBullet>().SetStats(dmg, hm);
        
        spawnedBullet.GetComponent<Rigidbody>().AddForce(spawnedBullet.transform.forward * bulSpeed, ForceMode.Impulse);
    }
}
