using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrateCrabBrain : MonoBehaviour
{
    GameObject player;
    EnemyHealthManager hm;
    NavAI nav;
    NavMeshAgent agent;
    float dmg;
    public enum state { wander, chasing }
    public state curState;
    public GameObject projectile;
    bool jammed;
    public ParticleSystem jamEffect;
    public Transform firepoint;
    public float fireRate; float timer;
    void Start()
    {
        player = GameObject.Find("Player");
        hm = GetComponent<EnemyHealthManager>();
        dmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
        nav = GetComponent<NavAI>();
        agent = GetComponent<NavMeshAgent>();
        curState = state.wander; nav.SetState(NavAI.state.idle);
    }
    void Update()
    {
        if (hm.playerHM.activeEffects[22].x > 0 && !hm.gdm.pointsLocked) 
        {//Player is invisible. (via circus mask)
            curState = state.wander;
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
            case state.wander: break;
            case state.chasing:
                if(timer <= 0)
                {
                    Shoot();

                    timer = 1;
                }
                else { timer -= fireRate * Time.deltaTime * Random.Range(0f,2f); }
                break;
        }

        if (hm.activeEffects[12].x > 0) { agent.speed = 3f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { agent.speed = 3f; }
    }
    void Shoot()
    {
        GameObject spawnedGlob = Instantiate(projectile, firepoint.position, firepoint.rotation);
        spawnedGlob.GetComponent<CrateCrabGlob>().damage = dmg;
        spawnedGlob.GetComponent<CrateCrabGlob>().ehm = hm;
        spawnedGlob.GetComponent<CrateCrabGlob>().lifeTimeTimer = Random.Range(10f,20f);
        spawnedGlob.GetComponent<Rigidbody>().AddForce(transform.forward * 6, ForceMode.Impulse);
    }
}
