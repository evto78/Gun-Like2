using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class TIGERNavAI : MonoBehaviour
{
    TIGERBrain brain;
    NavMeshAgent agent;
    GameObject target;

    public TIGERDEMOPATROL demoPatrol;

    public float desDistance;

    float targetUpdateTimer;
    public float updateFreq;
    public enum state { idle, wander, chase, chasePoint, patrol }
    public state navState;
    float wanderTimer = 0;
    Vector3 lastSecondPos; float lspTimer;

    void Start()
    {
        brain = GetComponent<TIGERBrain>();
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }
    public void ChangeTarget(GameObject newTarget) { target = newTarget; }
    // Update is called once per frame
    private void LateUpdate()
    {
        lspTimer -= Time.deltaTime;
        if (lspTimer <= 0) { lspTimer = 1f; lastSecondPos = transform.position; }
    }
    public void BrainUpdate()
    {
        switch (navState)
        {
            case state.idle: agent.isStopped = true; break;
            case state.chase:
                agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if (targetUpdateTimer < 0)
                {
                    targetUpdateTimer = updateFreq;
                    if (NavMesh.SamplePosition(target.transform.position, out NavMeshHit hitChase, 10f, NavMesh.AllAreas))
                    {
                        agent.gameObject.GetComponent<NavMeshAgent>().destination = hitChase.position;
                    }

                }
                if (Vector3.Distance(agent.transform.position, target.transform.position) < desDistance)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }
                break;
            case state.chasePoint:
                agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if (targetUpdateTimer < 0)
                {
                    targetUpdateTimer = updateFreq;
                    if (NavMesh.SamplePosition(brain.manualNavPoint, out NavMeshHit hitPoint, 10f, NavMesh.AllAreas))
                    {
                        agent.gameObject.GetComponent<NavMeshAgent>().destination = hitPoint.position;
                    }

                }
                if (Vector3.Distance(agent.transform.position, brain.manualNavPoint) < desDistance)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                }
                break;
            case state.wander:
                agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if (Vector3.Distance(transform.position, lastSecondPos) <= 0.5f) { wanderTimer += Time.deltaTime * Random.Range(0.7f, 1.3f); }
                if (targetUpdateTimer < 0 && wanderTimer > 5f)
                {
                    targetUpdateTimer = updateFreq;

                    GetRandomAvaliablePoint(); wanderTimer = 0f;
                }
                break;
            case state.patrol:
                agent.isStopped = false;
                Vector3 followPoint;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(demoPatrol.followpoint.position, out hit, 10f, 1))
                {
                    followPoint = hit.position; NavMeshPath path = new NavMeshPath();
                    if (!agent.CalculatePath(followPoint, path)) { agent.destination = transform.position; }
                    else { agent.destination = followPoint; }
                    if (Vector3.Distance(transform.position, followPoint) > 40f)
                    {
                        demoPatrol.speed = agent.speed / 10f;
                    }
                    else
                    {
                        demoPatrol.speed = agent.speed / 2f;
                    }
                }
                else { agent.destination = transform.position; }
                break;
        }
    }
    void Update()
    {
        
    }
    public void SetState(state newState)
    {
        if (newState == navState) { return; }
        switch (newState)
        {
            case state.idle: agent.destination = transform.position; break;
            case state.chase: break;
            case state.chasePoint: break;
            case state.wander: wanderTimer = 0; GetRandomAvaliablePoint(); break;
            case state.patrol: demoPatrol.BeginPatrol(); break;
        }
        navState = newState;
    }
    void GetRandomAvaliablePoint()
    {
        Vector3 randPoint = Random.insideUnitCircle * 100f;
        randPoint.z = randPoint.y; randPoint.y = 0;
        randPoint += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randPoint, out hit, 10f, 1))
        {
            randPoint = hit.position; NavMeshPath path = new NavMeshPath();
            if (!agent.CalculatePath(randPoint, path)) { agent.destination = transform.position; }
            else { agent.destination = randPoint; }
        }
        else { agent.destination = transform.position; }
    }
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = false;
    }
    private void OnDisable()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }
}
