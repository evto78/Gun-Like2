using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGERDEMOPATROL : MonoBehaviour
{
    public Transform followpoint;
    public Transform patrolPoints;
    List<Transform> points = new List<Transform>();
    public float speed;
    int nextPoint;
    void Start()
    {
        for(int i = 0; i < patrolPoints.childCount; i++)
        {
            points.Add(patrolPoints.GetChild(i).transform);
        }
        followpoint.position = points[0].position;
        followpoint.rotation = points[0].rotation;
        nextPoint = 1;
        followpoint.LookAt(points[nextPoint]);
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
}
