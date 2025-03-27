using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavAI : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject target;
    void Start()
    {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target.transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            agent.gameObject.GetComponent<NavMeshAgent>().destination = hit.position;
        }
    }
}
