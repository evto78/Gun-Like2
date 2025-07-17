using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBrain : MonoBehaviour
{
    //public flyingEnemyNavController navController;
    public GameObject player;
    public GameObject target;
    
    MeshRenderer mr;
    public ParticleSystem shimmerEffect;

    public Material leadMat;
    public Material followMat;

    public float speed;
    float baseSpeed;
    public Vector3 moveCurve;
    Rigidbody rb;

    public GameObject leadKnife;
    public bool isLead;
    public int spawnAmount;
    public int spawnVariance;
    public int strikeRange;
    public float dmg;
    float strikeTimer;
    float cooldownTimer;
    public GameObject knifePrefab;

    EnemyHealthManager hm;

    public enum state { idle, wander, chase, prepare, strike} public state curState;

    bool pauseNagivation;
    bool speedingUp;

    GameObject fop;

    void Start()
    {
        fop = GameObject.Find("FlyingOrbitPoint");
        baseSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        hm = GetComponent<EnemyHealthManager>();
        hm.gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        dmg = hm.baseDamage * hm.gdm.difficulty * hm.difficultyScale;
        player = GameObject.Find("Player");
        mr = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>();

        moveCurve = Vector3.one * Random.Range(-1f, 1f);
        moveCurve = moveCurve / 2f;

        if (isLead)
        {
            target = player;

            SpawnFollowers(Random.Range(spawnAmount-spawnVariance, spawnAmount+spawnVariance));

            mr.material = leadMat;
        }
        else
        {
            mr.material = followMat;
        }

        curState = state.wander;
    }
    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = player;
        }

        if(hm.playerHM.activeEffects[22].x > 0 || (Vector3.Distance(player.transform.position, transform.position) > 100 && hm.curHp == hm.maxHp))
        {//Player is invisible. (via circus mask)
            curState = state.wander;
        } else if(curState == state.wander)
        {
            curState = state.chase;
        }

        switch (curState)
        {
            case state.idle: break;
            case state.wander:
                pauseNagivation = false;
                if(target == player || isLead)
                {
                    target = fop; if(target == null) { target = gameObject; }
                }
                break;
            case state.chase:
                if (isLead) { target = player; } else { target = leadKnife; } if(target == null) { target = player; }
                if(Vector3.Distance(transform.position, player.transform.position) < strikeRange && cooldownTimer <= 0f)
                {
                    curState = state.prepare;
                    pauseNagivation = true;
                    rb.velocity = rb.velocity / 10f;
                    transform.LookAt(player.transform.position);
                    rb.freezeRotation = true;
                    shimmerEffect.Play();
                    strikeTimer = 0.5f;
                }
                break;
            case state.prepare:
                if (!(hm.playerHM.activeEffects[22].x < 0))
                {
                    transform.LookAt(player.transform.position);

                    strikeTimer -= Time.deltaTime;
                    rb.velocity = rb.velocity / 1.1f;

                    if (strikeTimer <= 0f) { curState = state.strike; strikeTimer = 2f; }
                }
                break;
            case state.strike:
                strikeTimer -= Time.deltaTime;
                //if moving away from player, stop striking sooner
                if (Vector3.Distance(transform.position + rb.velocity, target.transform.position) > Vector3.Distance(transform.position, target.transform.position))
                {
                    strikeTimer -= Time.deltaTime * 3f;
                }
                if (hm.activeEffects[12].x > 0) { rb.AddForce(transform.forward * ((50f / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136])))) * Time.deltaTime), ForceMode.Impulse); }
                else { rb.AddForce(transform.forward * (50f * Time.deltaTime), ForceMode.Impulse); }


                if (strikeTimer <= 0f)
                {
                    StopStrike();
                }
                break;
        }

        cooldownTimer -= Time.deltaTime;

        if (hm.activeEffects[12].x > 0) { speed = baseSpeed / (1.5f * (1.1f * (hm.playerHM.playerItem.leftItems[136] + hm.playerHM.playerItem.rightItems[136]))); }
        else { speed = baseSpeed; }
    }
    private void FixedUpdate()
    {
        if (target == null)
        {
            target = player;
        }
        if (!pauseNagivation)
        {
            Movement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && curState == state.strike)
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(dmg, false, hm);

            gameObject.GetComponent<Rigidbody>().freezeRotation = false;
            gameObject.GetComponent<Rigidbody>().AddForce((transform.position - player.transform.position) * 5f, ForceMode.Impulse);
            StopStrike();
        }
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Untagged")
        {
            StopStrike();
        }
    }

    void StopStrike()
    {
        target = player;

        cooldownTimer = 3f;
        curState = state.chase;
        pauseNagivation = false;
        gameObject.GetComponent<Rigidbody>().freezeRotation = false;
    }

    void SpawnFollowers(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject spawned = Instantiate(knifePrefab);
            spawned.transform.position = new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y + Random.Range(-2f, 2f), transform.position.z + Random.Range(-2f, 2f));
            spawned.GetComponent<KnifeBrain>().isLead = false;
            spawned.GetComponent<KnifeBrain>().target = gameObject;
            spawned.GetComponent<KnifeBrain>().leadKnife = gameObject;
        }
    }

    void Movement()
    {
        if (hm.playerHM != null && hm.playerHM.activeEffects[22].x > 0) { return; }
        Vector3 moveDir;

        moveDir = target.transform.position - transform.position;
        moveDir.Normalize();

        //add curve to movement
        moveDir = moveDir + moveCurve;
        moveDir.Normalize();

        rb.AddForce(moveDir * speed * Time.fixedDeltaTime);

        //turn in the direction of movement
        if (rb.velocity.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }

        //if movement would make it go further from the player, add move movement toward the player
        if(Vector3.Distance(transform.position + rb.velocity, target.transform.position) > Vector3.Distance(transform.position, target.transform.position))
        {
            moveDir = target.transform.position - transform.position;
            moveDir.Normalize();

            rb.AddForce(moveDir * speed * Time.fixedDeltaTime);
        }
        //if really far from target, get increased speed to the target
        if(Vector3.Distance(transform.position, target.transform.position) > 75f)
        {
            moveDir = target.transform.position - transform.position;
            moveDir.Normalize();

            rb.AddForce(moveDir * speed * 2f * Time.fixedDeltaTime);
            speedingUp = true;
        }
        else if(Vector3.Distance(transform.position, target.transform.position) < 50f)
        {
            if (speedingUp)
            {
                speedingUp = false;
                rb.velocity = rb.velocity / 2f;
            }
        }
    }
}
