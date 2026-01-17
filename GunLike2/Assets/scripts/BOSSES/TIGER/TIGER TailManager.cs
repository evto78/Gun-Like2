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
    public AnimationCurve idlePoseYSway1;
    public AnimationCurve idlePoseYSway2; float yAltState = 0f; float yAltStateTimerSpeed = 0.5f; int yAltDir = 1;
    public AnimationCurve sineCurve; //smoothInSmoothOut
    public AnimationCurve alternatingTimerCurve;
    public float variation; public float alternatingTimer = 0f; bool altUp = true; float altSpeed = 0.5f;
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
    public void BrainUpdate()
    {
        yAltState += yAltStateTimerSpeed * Time.deltaTime * yAltDir;
        if (yAltState < 0 && yAltDir == -1) { yAltDir = 1; yAltState = 0; }
        else if (yAltState > 1 && yAltDir == 1) { yAltDir = -1; yAltState = 1; }

        if (altUp) { alternatingTimer += Time.deltaTime * altSpeed; if (alternatingTimer > 1) { altUp = false; alternatingTimer = 1; } }
        else { alternatingTimer -= Time.deltaTime * altSpeed; if (alternatingTimer < 0) { altUp = true; alternatingTimer = 0; } }

        float curVariation = variation * alternatingTimerCurve.Evaluate(alternatingTimer);
        for (int i = 0; i < segments; i++)
        {
            Transform curSegment = tailSegments[i];
            float xSway = (((idlePoseXSway.Evaluate((float)i / (float)segments) - 0.5f) * 2f)) * maxRotationPerJoint;
            xSway -= curVariation;
            float ySway1 = (((idlePoseYSway1.Evaluate((float)i / (float)segments) - 0.5f) * 2f)) * maxRotationPerJoint;
            float ySway2 = (((idlePoseYSway2.Evaluate((float)i / (float)segments) - 0.5f) * 2f)) * maxRotationPerJoint;
            float ySway = Mathf.Lerp(ySway1, ySway2, sineCurve.Evaluate(yAltState));
            ySway -= curVariation;
            curSegment.localRotation = new Quaternion(-xSway, ySway, curSegment.localRotation.z, curSegment.localRotation.w);
        }
    }
}
