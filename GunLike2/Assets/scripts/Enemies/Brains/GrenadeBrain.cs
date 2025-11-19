using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeBrain : MonoBehaviour
{
    Transform target;
    Transform secondaryTarget;
    public float speed = 5;
    public bool ticking = false;
    public Rigidbody rb;
    float tickTimer = 3;
    public GameObject explo;
    float bounceTimer;
    public EnemyHealthManager hm;
    float wanderTimer; Vector3 wanderDir;
    public MeshRenderer mr; Vector3 originalScale;
    bool foundSiblings = false;
    public ParticleSystem fuse;

    float subTimer;
    public Color normalColor; public Color explodingColor;

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
        originalScale = mr.transform.localScale;
        fuse.gameObject.SetActive(false);
    }
    void FindSiblings()
    {
        List<EnemyHealthManager> ehms = new List<EnemyHealthManager>();
        foreach(EnemyHealthManager ehm in hm.gdm.activeEhms)
        {
            if (ehm.brains[0].GetType().ToString() == "GrenadeBrain")
            {
                ehms.Add(ehm);
            }
        }
        secondaryTarget = ehms[Random.Range(0, ehms.Count)].transform;
        ehms.Clear();
        foundSiblings = true;
    }
    // Update is called once per frame
    void Update()
    {
        bool sub = false;
        subTimer -= Time.deltaTime; if (subTimer <= 0) { sub = true; }
        if (!foundSiblings || secondaryTarget == null) { FindSiblings(); }
        if(hm.playerHM.activeEffects[22].x > 0 && !hm.gdm.pointsLocked)
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
                Move();
                Bounce();
                break;
            case state.wander:
                Wander();
                break;
        }

        if (ticking) { Blow(); mr.material.color = Color.Lerp(explodingColor, normalColor, tickTimer); if (sub) { mr.material.color = Color.white; } }
        else { mr.material.color = normalColor; }

        if (hm.activeEffects[39].x > 0) { speed = 5f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { speed = 5f; }
        if(subTimer<= -0.1f) { subTimer = 0.1f; }
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
    void Move()
    {
        float modifier = Mathf.Clamp(Vector3.Distance(transform.position, target.position)/100f, 1f, 5f);
        rb.AddForce((target.position - transform.position).normalized * modifier * speed * 40f * Time.deltaTime);//move to player

        modifier = Mathf.Clamp(Vector3.Distance(transform.position, secondaryTarget.position), 1f, 20f);
        rb.AddForce((secondaryTarget.position - transform.position).normalized * modifier * speed * 20f * Time.deltaTime);//move to secondary target
    }
    void Blow()
    {
        fuse.gameObject.SetActive(true);
        mr.transform.localScale = originalScale * (1 + (1-tickTimer));
        tickTimer -= Time.deltaTime;
        if(tickTimer <= 0)
        {

            hm.PlaySound(1, true, true);
            explo.SetActive(true);
            hm.soundEffects[1].source[0].transform.SetParent(explo.transform, false);
            explo.transform.SetParent(null);
            explo.GetComponent<ExplosionHitbox>().damage = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
            Destroy(gameObject);
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
        if (Vector3.Distance(target.position, transform.position) < 5f && !ticking)
        {
            ticking = true;
            subTimer = 0f;
            hm.PlaySound(2,false,true);
            hm.PlaySound(3,false,true);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        hm.PlaySound(0,false,true);
    }
}
