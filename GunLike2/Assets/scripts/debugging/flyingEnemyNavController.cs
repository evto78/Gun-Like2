using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infohazard.HyperNav;

public class flyingEnemyNavController : MonoBehaviour
{
    [SerializeField] private NavAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;

    GameObject player;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        agent = gameObject.GetComponent<NavAgent>();
    }

    private void Update()
    {
        Vector3 vel = Calc();

        Move(vel);
        Turn(vel);
    }

    void Move(Vector3 vel)
    {
        //transform.position += vel * speed * Time.deltaTime;

        rb.AddForce(vel * speed * Time.deltaTime);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity / (1+Time.deltaTime);
        }
    }

    void Turn(Vector3 vel)
    {
        if (vel.sqrMagnitude > 0.01)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }
    }

    Vector3 Calc()
    {
        target = player.transform;

        agent.Destination = target.position;

        return agent.DesiredVelocity;
    }
}
