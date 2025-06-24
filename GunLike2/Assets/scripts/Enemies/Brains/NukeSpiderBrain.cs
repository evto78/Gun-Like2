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
    bool jumping;
    bool diving;
    Vector3 divingAngle;
    public GameObject nuke;
    public float nukeDmg;
    public GameObject tail;
    public GameObject shine;
    EnemyHealthManager hm;
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        nav = GetComponent<NavAI>();
        agent = GetComponent<NavMeshAgent>();
        hm = GetComponent<EnemyHealthManager>();
        nukeDmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
    }

    // Update is called once per frame
    void Update()
    {
        if(!jumping && Vector3.Distance(player.transform.position, transform.position) < jumpDistance && !(hm.playerHM.activeEffects[22].x > 0))
        {
            jumping = true;
            nav.enabled = false;
            agent.enabled = false;
            Jump();
        }
        nav.enabled = !jumping;
        agent.enabled = !jumping;
        tail.SetActive(jumping);
        shine.SetActive(diving);
        if(jumping && rb.velocity.y < 0 && !diving && !(hm.playerHM.activeEffects[22].x > 0))
        {
            diving = true;
            Dive();
        }
        if (diving)
        {
            transform.eulerAngles = divingAngle;
        }
        
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
        divingAngle = transform.eulerAngles;
        rb.AddForce((player.transform.position - transform.position).normalized * 500f, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Untagged") && diving)
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
        nav.enabled = true;
    }
}
