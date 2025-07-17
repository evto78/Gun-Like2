using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBrain : MonoBehaviour
{
    public float hoverHeight; Ray hoverRay;
    public float hoverSpeed;
    public float speed;
    float curHeight;
    Rigidbody rb;
    public List<PropellerSpin> propellers;
    EnemyHealthManager hm; HealthManager phm;
    EnemyHealthManager grabableTarget;
    GameObject player;
    enum holdType { empty, uzi, grenade, nuke} holdType holding;
    public enum state { wander, seeking, attacking} public state curState;
    [Header("Uzi Walker")]
    public GameObject pickUpUzi; public GameObject uziBullet; public Transform firePointUzi; public GameObject gunUzi; public ParticleSystem jammedUzi;
    [Header("Grenade Lobber")]
    public GameObject pickUpGrenade; public GameObject grenade; public Transform firePointGrenade; public GameObject gunGrenade; public ParticleSystem jammedGrenade;
    [Header("Nukeshell Spider")]
    public GameObject pickUpNuke; public GameObject nuke;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hm = GetComponent<EnemyHealthManager>();
        holding = holdType.empty;
        curState = state.wander;
        phm = hm.gdm.phm;
        grabableTarget = null;
        player = phm.gameObject;
    }
    void Update()
    {
        if (phm.activeEffects[22].x > 0)
        {
            curState = state.wander;
        }
        else if(holding == holdType.empty)
        {
            curState = state.seeking;
        }
        else
        {
            curState = state.attacking;
        }
        switch (curState)
        {
            case state.wander: break;
            case state.seeking:
                //if no target is set, set a target
                if(holding == holdType.empty && grabableTarget == null) { grabableTarget = FindActiveWalker(); }
                //if a target is set, but not holding it yet, move towards the target
                else if(holding == holdType.empty && grabableTarget != null){
                    MoveToTarget(grabableTarget.gameObject, 0);
                    //if above the target, lower and get closer.
                    if (Vector2.Distance(new Vector2(transform.position.x,transform.position.z),new Vector2(grabableTarget.transform.position.x,grabableTarget.transform.position.z)) < 5f) 
                    { hoverHeight = 2; }
                    //if on the target, destroy them, and upgrade this drone.
                    if(Vector3.Distance(transform.position, grabableTarget.transform.position) < 5f)
                    {
                        switch (grabableTarget.data.enemyName)
                        {
                            case "Uzi Walker": holding = holdType.uzi; pickUpUzi.SetActive(true); break;
                            case "Grenade Lobber": holding = holdType.grenade; pickUpGrenade.SetActive(true); break;
                            case "Nukeshell Spider": holding = holdType.nuke; pickUpNuke.SetActive(true); break;
                        }
                        hoverHeight = 50f;
                        Destroy(grabableTarget.gameObject);
                        curState = state.attacking;
                    }
                }
                break;
            case state.attacking:
                switch (holding)
                {
                    case holdType.empty: break;
                    case holdType.uzi:
                        MoveToTarget(player, 30); 
                        break;
                    case holdType.grenade:
                        MoveToTarget(player, 40);
                        break;
                    case holdType.nuke:
                        MoveToTarget(player, 0);
                        break;
                }
                break;
        }
        DistanceToGround();
        foreach (PropellerSpin propeller in propellers) { propeller.speed = 1800f; }
        if(curHeight < hoverHeight)
        {
            rb.AddForce(Vector3.up * hoverSpeed * Time.deltaTime);
            foreach(PropellerSpin propeller in propellers) { propeller.speed = 1800f; }
        }
        else
        {
            rb.AddForce(Vector3.up * hoverSpeed * Time.deltaTime / 2f);
            foreach (PropellerSpin propeller in propellers) { propeller.speed = 800f; }
        }
        if (hm.activeEffects[6].x > 0) { foreach (PropellerSpin propeller in propellers) { propeller.speed = 0f; } }
    }
    private void OnDisable()
    {
        foreach (PropellerSpin propeller in propellers) { propeller.speed = 0f; }
    }
    void Shoot()
    {
        if(holding == holdType.uzi)
        {

        }
        else if(holding == holdType.grenade)
        {

        }
    }
    void MoveToTarget(GameObject target, float desDistance)
    {
        if(holding == holdType.nuke) { speed *= 2f; }
        Debug.DrawRay(transform.position, (target.transform.position - transform.position).normalized * 5f, Color.red);

        if (Vector3.Distance(target.transform.position, transform.position) > desDistance)
        {
            rb.AddForce((target.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
        if(Vector3.Distance(target.transform.position, transform.position) > desDistance && (Vector3.Distance(target.transform.position, transform.position+rb.velocity) > Vector3.Distance(target.transform.position, transform.position)))
        {
            rb.AddForce((target.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
        if (rb.velocity.magnitude > 3f) { transform.LookAt(transform.position + rb.velocity); }
        if (holding == holdType.nuke) { speed /= 2f; }
    }
    void DistanceToGround()
    {
        hoverRay = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(hoverRay, out RaycastHit hit, 200f,1))
        {
            curHeight = hit.distance;
        }
    }
    EnemyHealthManager FindActiveWalker()
    {
        List<EnemyHealthManager> ehms = new List<EnemyHealthManager>();
        foreach(EnemyHealthManager ehm in hm.gdm.activeEhms)
        {
            if(ehm.data.type == Spawnable.Type.walker)
            {
                ehms.Add(ehm);
            }
        }
        if(ehms.Count > 0)
        {
            return ehms[Random.Range(0, ehms.Count)];
        }
        else
        {
            return null;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(holding == holdType.nuke && rb.velocity.magnitude > 20)
        {
            GameObject spawnedNuke = Instantiate(nuke);
            spawnedNuke.transform.position = transform.position;
            spawnedNuke.transform.rotation = transform.rotation;
            spawnedNuke.GetComponent<NuclearExplosion>().damage = 125 * hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
            Destroy(gameObject);
        }
    }
}
