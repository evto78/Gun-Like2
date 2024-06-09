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
        orbitOptions.Add(GameObject.Find("Orbit Point North"));
        orbitOptions.Add(GameObject.Find("Orbit Point North East"));
        orbitOptions.Add(GameObject.Find("Orbit Point North West"));
        orbitOptions.Add(GameObject.Find("Orbit Point East"));
        orbitOptions.Add(GameObject.Find("Orbit Point West"));
        orbitOptions.Add(GameObject.Find("Orbit Point South"));
        orbitOptions.Add(GameObject.Find("Orbit Point South East"));
        orbitOptions.Add(GameObject.Find("Orbit Point South West"));

        curHp = maxHp;
    }

    void Update()
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
            orbitFollow = orbitOptions[Random.Range(0, orbitOptions.Count)];
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
            player.GetComponent<HealthManager>().TakeDamage(damage);

            //fly back after attacking
            transform.LookAt(player.transform.position);
            rb.AddRelativeForce(new Vector3(0, 0, -speed * 20f));
        }
    }
}
