using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class TIGERFrontToeTilt : MonoBehaviour
{
    public float toeLength;
    public TIGERIKFootSolver linkedFootSolver;
    public bool canOverbend;
    quaternion initRot;
    private void Awake() { initRot = transform.rotation; }
    void Update()
    {
        //Limit Angle
        if (linkedFootSolver.stepping)
        {
            Quaternion curRot = transform.rotation;
            transform.LookAt(linkedFootSolver.staticNextPos + linkedFootSolver.hip.up * toeLength);
            if (!canOverbend) { transform.localEulerAngles = Vector3.right * (transform.localEulerAngles.x + Mathf.Abs(transform.localEulerAngles.y)); }
            Quaternion tarRot = transform.rotation;
            transform.rotation = Quaternion.Lerp(curRot, tarRot, Time.deltaTime * 2f);
        } else { transform.rotation = Quaternion.Lerp(transform.rotation, initRot, Time.deltaTime * 2f); }
    }
}
