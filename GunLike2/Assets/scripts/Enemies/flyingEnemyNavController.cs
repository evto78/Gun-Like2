using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Infohazard.HyperNav;

public class flyingEnemyNavController : MonoBehaviour
{
    [SerializeField] private NavAgent agent;
    [SerializeField] public Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float brakeRange;
    bool paused;

    public GameObject player;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.Find("Player");
        agent = gameObject.GetComponent<NavAgent>();
    }
    
    private void Update()
    {
        if (paused) { return; }

        Vector3 vel = Calc();

        Move(vel);
        Turn(vel);
    }

    void Move(Vector3 vel)
    {
        rb.AddForce(vel * speed * Time.deltaTime);
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity / (1+Time.deltaTime);
        }
    }

    void Turn(Vector3 vel)
    {
        if (rb.velocity.sqrMagnitude > 0.01)
        {
            transform.rotation = Quaternion.LookRotation(rb.velocity, Vector3.up);
        }
    }

    Vector3 Calc()
    {
        if (target == null) { target = player.transform; }
        target = player.transform;

        agent.Destination = target.position;

        Vector3 normTarVel = Vector3.Normalize(agent.DesiredVelocity);
        Vector3 normCurVel = Vector3.Normalize(rb.velocity);


        if (Vector3.Distance(normCurVel, normTarVel) > brakeRange)
        {
            //rb.velocity = rb.velocity / 1.2f;
        }

        return agent.DesiredVelocity;
    }

    public void Pause()
    {
        paused = true;
    }
    public void Unpause()
    {
        paused = false;
    }
}
