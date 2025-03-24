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

    void Start()
    {
        preparing = false;
        healthMan = GetComponent<EnemyHealthManager>();
        player = GameObject.Find("Player");
        mr = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();

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
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity / 10f;
            transform.LookAt(player.transform.position);

            gameObject.GetComponent<Rigidbody>().freezeRotation = true;

            shimmerEffect.Play();

            strikeTimer = 0.5f;
        }

        if (preparing)
        {
            transform.LookAt(player.transform.position);

            strikeTimer -= Time.deltaTime;
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity / 1.1f;

            if(strikeTimer <= 0f) { striking = true; preparing = false; strikeTimer = 2f; }
        }

        if (striking)
        {
            strikeTimer -= Time.deltaTime;
            
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * (50f * Time.deltaTime), ForceMode.Impulse);

            if(strikeTimer <= 0f)
            {
                StopStrike();
            }
        }

        cooldownTimer -= Time.deltaTime;
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
            spawned.GetComponent<flyingEnemyNavController>().player = transform.gameObject;
        }
    }
}
