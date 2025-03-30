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

    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        targetUpdateTimer -= Time.deltaTime * Random.Range(0.9f, 1.1f);
        if(targetUpdateTimer < 0)
        {
            targetUpdateTimer = updateFreq;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(target.transform.position, out hit, 10f, NavMesh.AllAreas))
            {
                agent.gameObject.GetComponent<NavMeshAgent>().destination = hit.position;
            }
            if(Vector3.Distance(agent.transform.position, target.transform.position) < desDistance)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }
}
