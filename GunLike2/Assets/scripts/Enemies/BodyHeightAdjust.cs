using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyHeightAdjust : MonoBehaviour
{
    public float offset;
    float avgY;
    public List<IKFootSolver> legs;
    NavMeshAgent agent;
    float initalY;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        initalY = legs[0].transform.localPosition.y;
    }
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * 10f);

        avgY = 0;
        foreach(IKFootSolver solver in legs)
        {
            avgY += solver.transform.localPosition.y;
        }
        avgY = avgY / legs.Count;
        avgY = avgY / initalY;
        //avgY = (avgY / transform.position.y) * legs[0].stepHeight;

        agent.baseOffset = (avgY + offset);
        //transform.position = new Vector3(transform.position.x, avgY + offset, transform.position.z);
    }
}
