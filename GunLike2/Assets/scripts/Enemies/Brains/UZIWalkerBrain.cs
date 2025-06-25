using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UZIWalkerBrain : MonoBehaviour
{
    GameObject player;
    public GameObject turrethead;
    public GameObject firepoint;
    Animator turretAnim;
    EnemyHealthManager hm;
    NavMeshAgent agent;

    public GameObject bullet;
    public float shotCooldown;
    float cooldownTimer;
    public int bulPerBurst;
    int bulShot;
    public float fireRate;
    float fireTimer;
    public float dmg;
    public float bulSpeed;
    public float accuracy;

    bool jammed;
    public ParticleSystem jamEffect;

    void Start()
    {
        player = GameObject.Find("Player");
        hm = GetComponent<EnemyHealthManager>();
        dmg = hm.baseDamage * hm.difficultyScale * hm.gdm.difficulty;
        turretAnim = turrethead.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        jammed = hm.activeEffects[3].x > 0;

        if(hm.playerHM.activeEffects[22].x > 0) { return; }
        turrethead.transform.LookAt(player.transform.position);

        if (Physics.Raycast(turrethead.transform.position, turrethead.transform.forward, out RaycastHit hit, 100f))
        {
            //Debug.Log(hit.transform.gameObject.tag);
            if (hit.transform.gameObject.tag == "Player" || true)
            {
                if (fireTimer <= 0 && cooldownTimer <= 0)
                {
                    Shoot();
                    turretAnim.SetBool("Recharge", false);
                    turretAnim.SetTrigger("Fire");
                    turretAnim.speed = 1f / fireRate;
                    bulShot++;
                    fireTimer = fireRate;
                    if (bulShot >= bulPerBurst)
                    {
                        if (jammed)
                        {
                            hm.activeEffects[3] = new Vector4(hm.activeEffects[3].x - 1, hm.activeEffects[3].y, hm.activeEffects[3].z, hm.activeEffects[3].w);
                        }
                        cooldownTimer = shotCooldown;
                        turretAnim.speed = 1f / cooldownTimer;
                        turretAnim.SetBool("Recharge", true);
                        bulShot = 0;
                    }
                }
            }
            else if(cooldownTimer <= 0f)
            {
                turretAnim.SetBool("Recharge", false);
            }
        }
        else if (cooldownTimer <= 0f)
        {
            turretAnim.SetBool("Recharge", false);
        }
        fireTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
        cooldownTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);

        if (hm.activeEffects[12].x > 0) { agent.speed = 7f / (1.5f * (1.1f*(hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { agent.speed = 7f; }
    }

    void Shoot()
    {
        if (jammed)
        {
            jamEffect.Play();
            return;
        }

        GameObject spawnedBullet = Instantiate(bullet);
        spawnedBullet.transform.position = firepoint.transform.position;
        spawnedBullet.transform.LookAt(player.transform.position + player.GetComponent<Rigidbody>().velocity / 3f);
        spawnedBullet.transform.Rotate(new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0) * accuracy);
        spawnedBullet.GetComponent<EnemyBullet>().SetStats(dmg);
        
        spawnedBullet.GetComponent<Rigidbody>().AddForce(spawnedBullet.transform.forward * bulSpeed, ForceMode.Impulse);
    }
}
