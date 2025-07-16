using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NukeSpiderBrain : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    public float jumpDistance;
    public float jumpHeight;
    NavAI nav;
    NavMeshAgent agent;
    public List<GameObject> legs;
    Vector3 divingAngle;
    public GameObject nuke;
    public float nukeDmg;
    public GameObject tail;
    public GameObject shine;
    EnemyHealthManager hm;
    public enum state { idle, chaseing, jumping, diving}
    public state curState;
    bool off;
    public GameObject confusionEffect;
    void Start()
    {
        curState = state.idle;
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavAI>();
        agent = GetComponent<NavMeshAgent>();
        hm = GetComponent<EnemyHealthManager>();
        nukeDmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
        off = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!off)
        {
            if ((hm.playerHM.activeEffects[22].x > 0) && (curState != state.jumping && curState != state.diving))
            {//Player is invisible (via circus mask).
                curState = state.idle;
                nav.SetState(NavAI.state.wander);
            }
            else if (curState != state.jumping && curState != state.diving)
            {
                curState = state.chaseing;
                nav.SetState(NavAI.state.chase);
            }

            switch (curState)
            {
                case state.idle: nav.enabled = true; agent.enabled = true; tail.SetActive(false); shine.SetActive(false); rb.useGravity = false; rb.isKinematic = true; break;
                case state.chaseing:
                    nav.enabled = true; agent.enabled = true; tail.SetActive(false); shine.SetActive(false); rb.useGravity = false; rb.isKinematic = true;
                    if (Vector3.Distance(player.transform.position, transform.position) < jumpDistance)
                    {
                        curState = state.jumping;
                        nav.SetState(NavAI.state.idle);
                        nav.enabled = false; agent.enabled = false; tail.SetActive(true); shine.SetActive(false); rb.useGravity = true; rb.isKinematic = false;
                        Jump();
                    }
                    break;
                case state.jumping:
                    nav.enabled = false; agent.enabled = false; tail.SetActive(true); shine.SetActive(false); rb.useGravity = true; rb.isKinematic = false;
                    if (rb.velocity.y < 0) { curState = state.diving; Dive(); }
                    break;
                case state.diving:
                    nav.enabled = false; agent.enabled = false; tail.SetActive(true); shine.SetActive(true); rb.useGravity = true; rb.isKinematic = false;
                    transform.eulerAngles = divingAngle;
                    break;
            }

            if (hm.activeEffects[12].x > 0) { agent.speed = 7f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
            else { agent.speed = 7f; }
        }
        else
        {
            if (rb.velocity.y < 0) { Dive(); shine.SetActive(true); curState = state.diving; }
        }
    }
    public void WeakHit()
    {
        if (curState != state.diving || off) { return; }
        
        off = true;
        shine.SetActive(false);
        curState = state.jumping;
        Jump();
        confusionEffect.SetActive(true);
    }
    void Jump()
    {
        transform.LookAt(transform.position + Vector3.up);
        transform.position += Vector3.up * 6f;
        rb.velocity = Vector3.up * jumpHeight;
        foreach(GameObject leg in legs)
        {
            leg.SetActive(false);
        }
    }
    void Dive()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        transform.LookAt(player.transform);
        if (off) 
        { 
            transform.LookAt(transform.position - Vector3.up);
            divingAngle = transform.eulerAngles;
            rb.AddForce(-Vector3.up.normalized * 30f, ForceMode.Impulse);
        }
        else
        {
            divingAngle = transform.eulerAngles;
            rb.AddForce((player.transform.position - transform.position).normalized * 500f, ForceMode.Impulse);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Untagged") && curState == state.diving)
        {
            GameObject spawnedNuke = Instantiate(nuke);
            spawnedNuke.transform.position = transform.position;
            spawnedNuke.GetComponent<NuclearExplosion>().damage = nukeDmg;
            Destroy(gameObject);
        }
    }
    private void OnDisable()
    {
        nav.enabled = false;
    }
    private void OnEnable()
    {
        if(nav == null) { return; }
        nav.enabled = true;
    }
}
