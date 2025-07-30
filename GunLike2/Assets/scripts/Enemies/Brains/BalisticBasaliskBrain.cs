using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BalisticBasaliskBrain : MonoBehaviour
{
    GameObject player;
    public GameObject turrethead;
    public GameObject firepoint;
    EnemyHealthManager hm;
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

    bool jammed;
    public ParticleSystem jamEffect;

    public enum state { idle, chasing }
    public state curState;

    void Start()
    {
        player = GameObject.Find("Player");
        hm = GetComponent<EnemyHealthManager>();
        dmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
        agent = GetComponent<NavMeshAgent>();
        nav = GetComponent<NavAI>();
        curState = state.idle; nav.SetState(NavAI.state.idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (hm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(player.transform.position, transform.position) > 200 && hm.curHp == hm.maxHp))
        {//Player is invisible. (via circus mask)
            curState = state.idle;
            nav.SetState(NavAI.state.wander);
        }
        else
        {
            curState = state.chasing;
            nav.SetState(NavAI.state.chase);
        }

        jammed = hm.activeEffects[3].x > 0;

        switch (curState)
        {
            case state.idle: break;
            case state.chasing:
                if (Physics.Raycast(turrethead.transform.position, turrethead.transform.forward, out RaycastHit hit, 100f))
                {
                    //Debug.Log(hit.transform.gameObject.tag);
                    if (hit.transform.gameObject.tag == "Player" || true)
                    {
                        if (fireTimer <= 0 && cooldownTimer <= 0 && CanShoot())
                        {
                            Shoot();
                            bulShot++;
                            fireTimer = fireRate;
                            if (bulShot >= bulPerBurst)
                            {
                                if (jammed)
                                {
                                    hm.activeEffects[3] = new Vector4(hm.activeEffects[3].x - 1, hm.activeEffects[3].y, hm.activeEffects[3].z, hm.activeEffects[3].w);
                                }
                                cooldownTimer = shotCooldown;
                                bulShot = 0;
                            }
                        }
                    }
                }
                fireTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                cooldownTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                break;
        }

        if (hm.activeEffects[12].x > 0) { agent.speed = 7f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { agent.speed = 7f; }
    }
    bool CanShoot()
    {
        Ray ray = new Ray(firepoint.transform.position, player.transform.position - firepoint.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 75))
        {
            if (hit.transform.gameObject.layer == 7)
            {
                return true;
            }
        }

        return false;
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
        spawnedBullet.transform.rotation = firepoint.transform.rotation;
        spawnedBullet.transform.Rotate(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0) * accuracy);
        spawnedBullet.GetComponent<EnemyBullet>().SetStats(dmg, hm);

        spawnedBullet.GetComponent<Rigidbody>().AddForce(spawnedBullet.transform.forward * bulSpeed, ForceMode.Impulse);
    }
}
