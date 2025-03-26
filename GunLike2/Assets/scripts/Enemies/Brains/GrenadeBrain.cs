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
    public GameObject explo;
    float bounceTimer;
    // Start is called before the first frame update
    void Start()
    {
        bounceTimer = Random.Range(0f, 3f);
        tickTimer = 1;
        rb = GetComponent<Rigidbody>();
        explo.SetActive(false);
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        followPlayer();
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
        rb.AddForce((target.position - transform.position).normalized * speed * 40f * Time.deltaTime);
    }
    void Blow()
    {
        if(tickTimer > 0)
        {
            tickTimer -= Time.deltaTime;
            if(tickTimer <= 0)
            {
                explo.SetActive(true);
                explo.transform.SetParent(null);
                Destroy(gameObject);
            }
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
            rb.AddForce(Vector3.up * speed * 3f, ForceMode.Impulse);
            rb.AddForce(transform.forward * speed * 3f, ForceMode.Impulse);
            bounceTimer = 3;
        }
         if (Vector3.Distance(target.position, transform.position) < 5)
        {
            Ticking = true;
        }

    }
}
