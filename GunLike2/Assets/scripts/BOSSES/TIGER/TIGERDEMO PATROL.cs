using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGERDEMOPATROL : MonoBehaviour
{
    public bool hidePoints;
    public Transform tiger;
    public Transform followpoint;
    public Transform patrolPoints;
    List<Transform> points = new List<Transform>();
    public float speed;
    int nextPoint;
    private void Awake()
    {
        for (int i = 0; i < patrolPoints.childCount; i++)
        {
            points.Add(patrolPoints.GetChild(i).transform);
            points[i].GetComponent<MeshRenderer>().enabled = !hidePoints;
        }
        followpoint.GetComponent<MeshRenderer>().enabled = !hidePoints;
        followpoint.position = points[0].position;
        followpoint.rotation = points[0].rotation;
        nextPoint = 1;
    }
    void Start()
    {
        followpoint.LookAt(points[nextPoint]);
        BeginPatrol();
    }
    void Update()
    {
        followpoint.position += followpoint.forward * Time.deltaTime * speed;
        if(Vector3.Distance(followpoint.position, points[nextPoint].position) < Vector3.Distance(followpoint.position + followpoint.forward * Time.deltaTime * speed, points[nextPoint].position))
        {
            nextPoint++; if(nextPoint >= points.Count) { nextPoint = 0; }
            followpoint.LookAt(points[nextPoint]);
        }
    }
    public void BeginPatrol()
    {
        float minDist = float.PositiveInfinity; int minDistIndex = 0;

        foreach(Transform t in points)
        {
            float dist = Vector3.Distance(t.position, tiger.position);
            if (dist < minDist) { minDist = dist; minDistIndex = points.IndexOf(t); }
        }

        followpoint.position = points[minDistIndex].position;
        nextPoint = minDistIndex + 1;
        if (nextPoint >= points.Count) {nextPoint = 0; }
        followpoint.LookAt(points[nextPoint]);
    }
}
