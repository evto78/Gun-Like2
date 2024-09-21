using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEnemy : MonoBehaviour
{
    private List<GameObject> orbitOptions = new List<GameObject>();
    private GameObject orbitFollow;
    private GameObject player;
    private GameObject target;
    Rigidbody rb;
    
    Vector3 curDir;
    Vector3 tarDir;

    float timer;
    float closestDistance;

    int aiTimer;
    public bool aiTimerActive;
    public int aiTimerIntervals;

    public float maxHp;
    public float curHp;
    public float damage;
    public float armor;
    public float turnSpeed;
    public float speed;

    public bool attacking = false;

    string state;
    // Can be:
    //
    // search
    // orbit
    // charge

    private void Start()
    {
        //Setup
        rb = GetComponent<Rigidbody>();
        state = "search";
        player = GameObject.Find("Player");
        target = player;
        orbitOptions.Add(GameObject.Find("Orbit Point North1"));
        orbitOptions.Add(GameObject.Find("Orbit Point North East1"));
        orbitOptions.Add(GameObject.Find("Orbit Point North West1"));
        orbitOptions.Add(GameObject.Find("Orbit Point East1"));
        orbitOptions.Add(GameObject.Find("Orbit Point West1"));
        orbitOptions.Add(GameObject.Find("Orbit Point South1"));
        orbitOptions.Add(GameObject.Find("Orbit Point South East1"));
        orbitOptions.Add(GameObject.Find("Orbit Point South West1"));

        orbitOptions.Add(GameObject.Find("Orbit Point North2"));
        orbitOptions.Add(GameObject.Find("Orbit Point North East2"));
        orbitOptions.Add(GameObject.Find("Orbit Point North West2"));
        orbitOptions.Add(GameObject.Find("Orbit Point East2"));
        orbitOptions.Add(GameObject.Find("Orbit Point West2"));
        orbitOptions.Add(GameObject.Find("Orbit Point South2"));
        orbitOptions.Add(GameObject.Find("Orbit Point South East2"));
        orbitOptions.Add(GameObject.Find("Orbit Point South West2"));

        orbitOptions.Add(GameObject.Find("Orbit Point North3"));
        orbitOptions.Add(GameObject.Find("Orbit Point North East3"));
        orbitOptions.Add(GameObject.Find("Orbit Point North West3"));
        orbitOptions.Add(GameObject.Find("Orbit Point East3"));
        orbitOptions.Add(GameObject.Find("Orbit Point West3"));
        orbitOptions.Add(GameObject.Find("Orbit Point South3"));
        orbitOptions.Add(GameObject.Find("Orbit Point South East3"));
        orbitOptions.Add(GameObject.Find("Orbit Point South West3"));

        curHp = maxHp;

        aiTimer = aiTimerIntervals;
    }

    void Update()
    {
        aiTimer += 1;

        if (aiTimer >= aiTimerIntervals || aiTimerActive)
        {
            if (player == null)
            {
                player = GameObject.Find("Player");
            }
            else
            {
                if (state == "search")
                {
                    Search();
                }
                if (state == "orbit")
                {
                    Orbit();
                }
                if (state == "charge")
                {
                    Charge();
                }

                if (Vector3.Distance(transform.position, player.transform.position) > 21)
                {
                    state = "search";
                    attacking = false;
                }

            }

            aiTimer = 0;
        }
        
    }

    void Charge()
    {
        ChargeMove();
        if (Vector3.Distance(transform.position, player.transform.position) > 25)
        {
            state = "search";
        }
    }

    void Orbit()
    {
        closestDistance = 9999999f;
        for (int i = 0; i < orbitOptions.Count; i+=1)
        {
            if (Vector3.Distance(orbitOptions[i].transform.position, transform.position) < closestDistance)
            {
                orbitFollow = orbitOptions[i];
                closestDistance = Vector3.Distance(orbitFollow.transform.position, transform.position);
                target = orbitFollow;
            }
        }
        target = orbitFollow;
        Move(target);
        Turn(target);
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            state = "charge";
            target = player;
            Turn(target);
            attacking = true;
        }
    }

    void Search()
    {
        target = player;
        Move(target);
        Turn(target);

        if (Vector3.Distance(transform.position, player.transform.position) < 20)
        {
            state = "orbit";

            closestDistance = 9999999f;
            for (int i = 0; i < orbitOptions.Count; i++)
            {
                if(Vector3.Distance(orbitOptions[i].transform.position, transform.position) < closestDistance)
                {
                    orbitFollow = orbitOptions[i];
                    closestDistance = Vector3.Distance(orbitFollow.transform.position, orbitFollow.transform.position);
                }
            }

            target = orbitFollow;
            timer = Random.Range(10f, 10f);
        }
    }

    void Move(GameObject tar)
    {
        rb.AddRelativeForce(new Vector3(0, 0, speed * Time.deltaTime));
    }
    
    void ChargeMove()
    {
        rb.AddRelativeForce(new Vector3(0, 0, speed * 4 * Time.deltaTime));
    }

    void Turn(GameObject tar)
    {
        curDir = transform.localEulerAngles;
        transform.LookAt(tar.transform.position);
        tarDir = transform.localEulerAngles;
        transform.localEulerAngles = curDir;

        if (curDir.x > tarDir.x)
        {
            curDir.x -= turnSpeed * Time.deltaTime;
            if (curDir.x < tarDir.x)
            {
                curDir.x = tarDir.x;
            }
        }
        else
        {
            curDir.x += turnSpeed * Time.deltaTime;
            if (curDir.x > tarDir.x)
            {
                curDir.x = tarDir.x;
            }
        }

        if (curDir.y > tarDir.y)
        {
            curDir.y -= turnSpeed * Time.deltaTime;
            if (curDir.y < tarDir.y)
            {
                curDir.y = tarDir.y;
            }
        }
        else
        {
            curDir.y += turnSpeed * Time.deltaTime;
            if (curDir.y > tarDir.y)
            {
                curDir.y = tarDir.y;
            }
        }

        if (curDir.z > tarDir.z)
        {
            curDir.z -= turnSpeed * Time.deltaTime;
            if (curDir.z < tarDir.z)
            {
                curDir.z = tarDir.z;
            }
        }
        else
        {
            curDir.z += turnSpeed * Time.deltaTime;
            if (curDir.z > tarDir.z)
            {
                curDir.z = tarDir.z;
            }
        }

        transform.localEulerAngles = curDir;
        transform.LookAt(tar.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && attacking)
        {
            attacking = false;
            player.GetComponent<HealthManager>().TakeDamage(damage, false);

            //fly back after attacking
            transform.LookAt(player.transform.position);
            rb.AddRelativeForce(new Vector3(0, 0, -speed * 20f));
        }
    }
}
