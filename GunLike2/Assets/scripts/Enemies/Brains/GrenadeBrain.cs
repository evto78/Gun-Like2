using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeBrain : MonoBehaviour
{
    Transform target;
    public float speed = 5;
    public bool ticking = false;
    public Rigidbody rb;
    float tickTimer = 3;
    public GameObject explo;
    float bounceTimer;
    public EnemyHealthManager hm;
    float wanderTimer; Vector3 wanderDir;
    public enum state { idle, wander, chase} public state curState;
    // Start is called before the first frame update
    void Start()
    {
        bounceTimer = Random.Range(0f, 3f);
        tickTimer = 1;
        rb = GetComponent<Rigidbody>();
        explo.SetActive(false);
        target = GameObject.Find("Player").transform;
        curState = state.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(hm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(target.position, transform.position) > 100 && hm.curHp == hm.maxHp))
        {
            curState = state.wander;
        }
        else
        {
            curState = state.chase;
        }

        switch (curState)
        {
            case state.idle: break;
            case state.chase:
                followPlayer();
                Bounce();
                break;
            case state.wander:
                Wander();
                break;
        }

        if (ticking) { Blow(); }

        if (hm.activeEffects[12].x > 0) { speed = 5f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { speed = 5f; }
    }
    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if(wanderTimer < 0)
        {
            wanderTimer = Random.Range(2f, 7f);
            wanderDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
        rb.AddForce(wanderDir * speed * 20f * Time.deltaTime);
    }
    void followPlayer()
    {
        rb.AddForce((target.position - transform.position).normalized * speed * 40f * Time.deltaTime);
    }
    void Blow()
    {
        if(tickTimer > 0)
        {
            tickTimer -= Time.deltaTime;
            if(tickTimer <= 0)
            {
                explo.SetActive(true);
                explo.transform.SetParent(null);
                explo.GetComponent<ExplosionHitbox>().damage = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
                Destroy(gameObject);
            }
        }
    }

    void Bounce()
    {
        bounceTimer -= Time.deltaTime;
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        if (bounceTimer <= 0)
        {
            rb.AddForce(Vector3.up * speed * 8f, ForceMode.Impulse);
            rb.AddForce(transform.forward * speed * 6f, ForceMode.Impulse);
            bounceTimer = 3;
        }
        if (Vector3.Distance(target.position, transform.position) < 5f)
        {
            ticking = true;
        }

    }
}
