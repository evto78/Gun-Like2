using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGERTailManager : MonoBehaviour
{
    TIGERBodyHeightAdjust bha;
    public Transform tailStart;
    public Transform tailEnd;
    List<Transform> tailSegments = new List<Transform>();
    public float maxRotationPerJoint;
    public float swaySpeed;
    int segments;
    public AnimationCurve idlePoseXSway;
    public AnimationCurve idlePoseYSway;
    public AnimationCurve alternatingTimerCurve;
    public float variation; public float alternatingTimer = 0f; bool altUp = true; float altSpeed = 1f;
    void Start()
    {
        bha = GetComponent<TIGERBodyHeightAdjust>();

        tailSegments.Add(tailStart);
        FindNextTailSegment(tailStart);
        segments = tailSegments.Count;
    }
    void FindNextTailSegment(Transform parent)
    {
        if (parent.childCount > 0)
        {
            tailSegments.Add(parent.GetChild(0));
            FindNextTailSegment(parent.GetChild(0));
        }
    }
    void Update()
    {
        if (altUp) { alternatingTimer += Time.deltaTime * altSpeed; if (alternatingTimer > 1) { altUp = false; alternatingTimer = 1; } } 
        else { alternatingTimer -= Time.deltaTime * altSpeed; if (alternatingTimer < 0) { altUp = true; alternatingTimer = 0; } } 

        float curVariation = variation * alternatingTimerCurve.Evaluate(alternatingTimer);
        for(int i = 0; i < segments; i++)
        {
            Transform curSegment = tailSegments[i];
            float xSway = (((idlePoseXSway.Evaluate((float)i/(float)segments)-0.5f)*2f)) * maxRotationPerJoint;
            xSway -= curVariation;
            curSegment.localEulerAngles = new Vector3(-xSway, curSegment.localEulerAngles.y, curSegment.localEulerAngles.z);
        }
    }
}
