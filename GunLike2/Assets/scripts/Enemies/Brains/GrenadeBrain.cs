using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrenadeBrain : MonoBehaviour
{
    Transform target;
    public int speed = 5;
    public bool Ticking = false;
    public Rigidbody rb;
    float tickTimer = 3;
    float explTimer = 0.5f;
    public GameObject explo;
    float bounceTimer;
    // Start is called before the first frame update
    void Start()
    {
        bounceTimer = 2;
        tickTimer = 1;
        explTimer = 0.5f;
        rb = GetComponent<Rigidbody>();
        explo.SetActive(false);
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Ticking)
        {
            Bounce();
        }
        else
        {
            Blow();
        }

    }
        void followPlayer()
    {
        //Vector3 dir = (target.position - transform.position);
        //Quaternion rotation = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        //transform.position += transform.forward * speed * Time.deltaTime;
        //if (Vector3.Distance(target.position, transform.position) < 5)
        //{
            //Ticking = true;

        //}
    }
    void Blow()
    {
        if(tickTimer > 0)
        {
            tickTimer -= Time.deltaTime;
        }
        else
        {
            explo.SetActive(true);
            explTimer -= Time.deltaTime;

        }
        if(explTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Bounce()
    {
        bounceTimer -= Time.deltaTime;
        Vector3 dir = (target.position - transform.position);
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation,  85 * Time.deltaTime);
        if (bounceTimer <= 0)
        {
            rb.AddForce(Vector3.up * 35, ForceMode.Impulse);
            rb.AddForce(transform.forward* 40, ForceMode.Impulse);
            bounceTimer = 3;
        }
         if (Vector3.Distance(target.position, transform.position) < 5)
        {
            Ticking = true;
        }

    }
}
