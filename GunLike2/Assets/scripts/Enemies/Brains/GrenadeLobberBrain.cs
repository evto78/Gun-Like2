using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadeLobberBrain : MonoBehaviour
{
    EnemyHealthManager hm; HealthManager phm;
    GameObject player;
    public GameObject grenade; public Transform firePointGrenade; public GameObject gunGrenade;
    public enum state { wander, lob }
    public state curState;
    bool jammed;
    public ParticleSystem jamEffect;
    NavAI nav;
    public float shotCooldown;
    public float burstCooldown;
    public int burstAmt;
    int grenadesShotThisBurst;
    float cooldownTimer;
    float burstTimer;
    public float lobSpeed;
    public int maxGrenadesAtOnce;
    List<GameObject> grenadesSpawned = new List<GameObject>();
    List<GameObject> toBeDeleted = new List<GameObject>();
    private void Start()
    {
        hm = GetComponent<EnemyHealthManager>();
        curState = state.wander;
        phm = hm.gdm.phm;
        player = phm.gameObject;
        nav = GetComponent<NavAI>();

        cooldownTimer = 0;
        burstTimer = 0;
        grenadesShotThisBurst = 0;
    }
    private void Update()
    {
        for(int i = 0; i < grenadesSpawned.Count; i++)
        {
            if (grenadesSpawned[i] == null) { toBeDeleted.Add(grenadesSpawned[i]); }
        } foreach(GameObject gm in toBeDeleted)
        {
            grenadesSpawned.Remove(gm);
        } toBeDeleted = new List<GameObject>();

        jammed = hm.activeEffects[3].x > 0;

        if (hm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(player.transform.position, transform.position) > 200 && hm.curHp == hm.maxHp))
        {//Player is invisible. (via circus mask)
            curState = state.wander;
            nav.SetState(NavAI.state.wander);
        }
        else
        {
            curState = state.lob;
            nav.SetState(NavAI.state.chase);
        }

        switch (curState)
        {
            case state.wander: nav.SetState(NavAI.state.wander); 
                break;
            case state.lob: nav.SetState(NavAI.state.chase); 
                if(cooldownTimer <= 0)
                {
                    if(grenadesShotThisBurst <= burstAmt)
                    {
                        if (burstTimer <= 0)
                        {
                            Lob();
                        }
                        else
                        {
                            burstTimer -= Time.deltaTime;
                        }
                    }
                    else
                    {
                        grenadesShotThisBurst = 0;
                        cooldownTimer = shotCooldown;
                    }
                    
                }
                else { cooldownTimer -= Time.deltaTime; }
                break;
        }
    }
    void Lob()
    {
        if(maxGrenadesAtOnce < grenadesSpawned.Count) { return; }
        burstTimer = burstCooldown;
        gunGrenade.GetComponent<Animator>().speed = 1 / (burstCooldown/2f);
        gunGrenade.GetComponent<Animator>().SetTrigger("shoot");
        if (jammed) { jamEffect.Play(); return; }
        GameObject spawned = Instantiate(grenade, firePointGrenade.position, firePointGrenade.rotation);
        spawned.GetComponent<Rigidbody>().AddForce(firePointGrenade.forward * lobSpeed, ForceMode.Impulse);
        grenadesShotThisBurst++;
        grenadesSpawned.Add(spawned);
    }
}
