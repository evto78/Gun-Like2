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
    public float speed; float speedModifier;
    enum state { idle, chase} state curState;
    void Start()
    {
        speedModifier = 1f;
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
        if (ehm.activeEffects[12].x > 0) { speedModifier = 0.5f / (1.5f * (1.1f * (ehm.playerHM.playerItem.leftItems[136] + ehm.playerHM.playerItem.rightItems[136]))); } else { speedModifier = 1f; }
        if (ehm.playerHM.activeEffects[22].x > 0 && !phm.gdm.pointsLocked)
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
                rb.AddForce(speed * speedModifier * 20f * Time.deltaTime * (player.transform.position - transform.position).normalized);
                if(Vector3.Distance(player.transform.position, transform.position+rb.velocity)>Vector3.Distance(player.transform.position, transform.position)){
                    rb.AddForce(speedModifier * speed * 20f * Time.deltaTime * (player.transform.position - transform.position).normalized);
                }
                if(player.transform.position.y>transform.position.y && rb.velocity.magnitude <= 15f)
                {
                    rb.AddForce(speed * speedModifier * 10f * Time.deltaTime * Vector3.up);
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
