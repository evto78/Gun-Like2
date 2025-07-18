using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProp : MonoBehaviour
{
    GameDataManager gdm;
    HealthManager phm;
    public GameObject enemy;
    public bool usingAnim;
    public Animator anim; //trigger is "go"
    public string finalAnimState;
    ParticleSystem ps;
    public float psTimer;
    public Transform spawnPos;
    [Header("Condition:")]
    public bool distanceBased;
    public float distance;
    public bool timerBased;
    public float timer;
    public bool interaction;
    [Header("Chance to appear")]
    public float chance;
    bool canSpawn; bool animing;
    private void Start()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        phm = gdm.phm;
        if (!usingAnim){ ps = GetComponentInChildren<ParticleSystem>(); }
        canSpawn = Random.Range(1, 100) < chance;
    }
    public void Interact()
    {
        if (interaction)
        {
            Spawn();
        }
    }
    private void Update()
    {
        if (!canSpawn) { this.enabled = false; return; }
        if (animing && usingAnim)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(finalAnimState))
            {
                Instantiate(enemy, spawnPos.position, spawnPos.rotation);
                Destroy(gameObject);
            }
        }
        else if(animing && !usingAnim)
        {
            psTimer -= Time.deltaTime;
            if(psTimer <= 0)
            {
                Instantiate(enemy, spawnPos.position, spawnPos.rotation);
                Destroy(ps.gameObject, 3f);
                ps.transform.SetParent(null);
                Destroy(gameObject);
            }
        }
        else
        {
            if (distanceBased && Vector3.Distance(transform.position, phm.transform.position) < distance)
            {
                Spawn();
            }
            else if (timerBased && gdm.gameTimerActive)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Spawn();
                }
            }
        }
    }
    void Spawn()
    {
        if (usingAnim)
        {
            anim.SetTrigger("go"); animing = true;
        }
        else
        {
            ps.Play(); animing = true;
            if(psTimer == 0)
            {
                animing = false;
                Instantiate(enemy, spawnPos.position, spawnPos.rotation);
                Destroy(ps.gameObject, 3f);
                ps.transform.SetParent(null);
                Destroy(gameObject);
            }
        }
    }
}
