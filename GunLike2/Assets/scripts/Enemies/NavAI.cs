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

    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        SetState(state.idle);
    }

    // Update is called once per frame
    void Update()
    {
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
                    if (Vector3.Distance(agent.transform.position, target.transform.position) < desDistance)
                    {
                        agent.isStopped = true;
                    }
                    else
                    {
                        agent.isStopped = false;
                    }
                } break;
            case state.wander: agent.isStopped = false;
                targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
                if(Vector3.Distance(transform.position, agent.destination) <= 1f) { wanderTimer += Time.deltaTime * Random.Range(0.8f,1.2f); }
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
        navState = newState;
        switch (newState)
        {
            case state.idle: agent.destination = transform.position; break;
            case state.chase: break;
            case state.wander: wanderTimer = 0f; GetRandomAvaliablePoint(); break;
        }
    }
    void GetRandomAvaliablePoint()
    {
        Vector3 randPoint = Random.insideUnitCircle * 25f; randPoint += transform.position;
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
        agent.isStopped = true;
    }
}
