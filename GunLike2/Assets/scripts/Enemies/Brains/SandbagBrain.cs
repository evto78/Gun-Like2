using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandbagBrain : MonoBehaviour
{
    Rigidbody rb;
    GameObject player;
    PlayerItem pi;
    HealthManager phm;
    public EnemyHealthManager ehm;
    GameDataManager gdm;
    public float dmg;
    public float speed;
    enum state { idle, chase} state curState;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        phm = gdm.phm;
        player = phm.gameObject;
        pi = phm.playerItem;
        ehm = GetComponent<EnemyHealthManager>();
        if(ehm == null)
        {
            ehm = GetComponentInParent<EnemyHealthManager>();
        }
        dmg = ehm.baseDamage * ehm.gdm.difficulty * ehm.difficultyScale;
        curState = state.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (ehm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(player.transform.position, transform.position) > 100 && ehm.curHp == ehm.maxHp))
        {//Player is invisible. (via circus mask)
            curState = state.idle;
        }
        else
        {
            curState = state.chase;
        }

        switch (curState)
        {
            case state.idle: break;
            case state.chase:
                rb.AddForce((player.transform.position - transform.position).normalized * speed * 20f * Time.deltaTime);
                if(Vector3.Distance(player.transform.position, transform.position+rb.velocity)>Vector3.Distance(player.transform.position, transform.position)){
                    rb.AddForce((player.transform.position - transform.position).normalized * speed * 20f * Time.deltaTime);
                }
                break;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(dmg, false, ehm);
        }
    }
}
