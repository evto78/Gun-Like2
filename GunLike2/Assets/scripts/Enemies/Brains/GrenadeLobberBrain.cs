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
    float cooldownTimer;
    public float lobSpeed;
    private void Start()
    {
        hm = GetComponent<EnemyHealthManager>();
        curState = state.wander;
        phm = hm.gdm.phm;
        player = phm.gameObject;
        nav = GetComponent<NavAI>();
    }
    private void Update()
    {
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
                    Lob();
                }
                else { cooldownTimer -= Time.deltaTime; }
                break;
        }
    }
    void Lob()
    {
        cooldownTimer = shotCooldown;
        gunGrenade.GetComponent<Animator>().SetTrigger("shoot");
        if (jammed) { jamEffect.Play(); return; }
        GameObject spawned = Instantiate(grenade, firePointGrenade.position, firePointGrenade.rotation);
        spawned.GetComponent<Rigidbody>().AddForce(firePointGrenade.forward * lobSpeed, ForceMode.Impulse);
    }
}
