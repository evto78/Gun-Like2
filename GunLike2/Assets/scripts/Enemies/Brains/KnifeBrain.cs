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
    public Vector3 moveCurve;
    Rigidbody rb;

    public bool isLead;
    public int spawnAmount;
    public int spawnVariance;
    public int strikeRange;
    public float dmg;
    bool preparing;
    float strikeTimer;
    bool striking;
    float cooldownTimer;
    public GameObject knifePrefab;

    EnemyHealthManager healthMan;

    bool pauseNagivation;
    bool speedingUp;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        preparing = false;
        healthMan = GetComponent<EnemyHealthManager>();
        dmg = healthMan.baseDamage * healthMan.gdm.difficulty * healthMan.difficultyScale;
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
    }
    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = player;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < strikeRange && !preparing && !striking && cooldownTimer <= 0f)
        {
            preparing = true;
            pauseNagivation = true;
            rb.velocity = rb.velocity / 10f;
            transform.LookAt(player.transform.position);

            rb.freezeRotation = true;

            shimmerEffect.Play();

            strikeTimer = 0.5f;
        }

        if (preparing)
        {
            transform.LookAt(player.transform.position);

            strikeTimer -= Time.deltaTime;
            rb.velocity = rb.velocity / 1.1f;

            if(strikeTimer <= 0f) { striking = true; preparing = false; strikeTimer = 2f; }
        }

        if (striking)
        {
            strikeTimer -= Time.deltaTime;
            //if moving away from player, stop striking sooner
            if (Vector3.Distance(transform.position + rb.velocity, target.transform.position) > Vector3.Distance(transform.position, target.transform.position))
            {
                strikeTimer -= Time.deltaTime * 3f;
            }
            
            rb.AddForce(transform.forward * (50f * Time.deltaTime), ForceMode.Impulse);

            if(strikeTimer <= 0f)
            {
                StopStrike();
            }
        }

        cooldownTimer -= Time.deltaTime;
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HealthManager>().TakeDamage(dmg, false);

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
        striking = false;
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
        }
    }

    void Movement()
    {
        Vector3 moveDir;

        moveDir = target.transform.position - transform.position;
        moveDir.Normalize();

        //add curve to movement
        moveDir = moveDir + moveCurve;
        moveDir.Normalize();

        rb.AddForce(moveDir * speed * Time.fixedDeltaTime);

        //turn in the direction of movement
        if (rb.velocity.sqrMagnitude > 0.01)
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
