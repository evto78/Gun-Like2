using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;

    public float desDistance;

    float targetUpdateTimer;
    public float updateFreq;
    public enum state { idle, wander, chase} public state navState;
    float wanderTimer = 0;
    Vector3 lastSecondPos; float lspTimer;

    public bool neverIdle;

    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        SetState(state.idle);
        if (neverIdle) { SetState(state.chase); }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        lspTimer -= Time.deltaTime;
        if (lspTimer <= 0) { lspTimer = 1f; lastSecondPos = transform.position; }
    }
    void Update()
    {
        if (neverIdle) { SetState(state.chase); }
        switch (navState)
        {
            case state.idle: agent.isStopped = true; break;
            case state.chase: agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if (targetUpdateTimer < 0)
                {
                    targetUpdateTimer = updateFreq;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(target.transform.position, out hit, 10f, NavMesh.AllAreas))
                    {
                        agent.gameObject.GetComponent<NavMeshAgent>().destination = hit.position;
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
            case state.wander: agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if(Vector3.Distance(transform.position, lastSecondPos) <= 0.5f) { wanderTimer += Time.deltaTime * Random.Range(0.7f,1.3f); }
                if (targetUpdateTimer < 0 && wanderTimer > 5f)
                {
                    targetUpdateTimer = updateFreq;

                    GetRandomAvaliablePoint(); wanderTimer = 0f;
                }
                break;
        }
    }
    public void SetState(state newState)
    {
        if(newState == navState) { return; }
        switch (newState)
        {
            case state.idle: agent.destination = transform.position; break;
            case state.chase: break;
            case state.wander: wanderTimer = 0; GetRandomAvaliablePoint(); break;
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
