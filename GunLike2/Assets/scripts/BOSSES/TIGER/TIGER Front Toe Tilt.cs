using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TIGERFrontToeTilt : MonoBehaviour
{
    public float toeLength;
    public TIGERIKFootSolver linkedFootSolver;
    public bool canOverbend;
    void Update()
    {
        if (linkedFootSolver.stepping)
        {
            transform.LookAt(linkedFootSolver.staticNextPos + linkedFootSolver.hip.up * toeLength);
            if (!canOverbend) { transform.localEulerAngles = Vector3.right * (transform.localEulerAngles.x + Mathf.Abs(transform.localEulerAngles.y)); }
        }
    }
}
