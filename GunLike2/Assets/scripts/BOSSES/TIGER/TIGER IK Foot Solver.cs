using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TIGERIKFootSolver : MonoBehaviour
{
    public Transform meshFoot;
    public TIGERBodyHeightAdjust manager;
    public TIGERIKFootSolver pairedLeg;
    public Transform hip;
    public float maxStepHeight;
    public float stepHeight;
    public float stepLength;
    public LayerMask terrainLayer;
    public bool stay;
    public bool stepping;
    public float stepProgress;
    public float stepSpeed;
    Vector3 stayPos;
    public Vector3 activeNextPos;
    public Vector3 staticNextPos;
    public float nextPosOffset;
    float moveDir;

    float initialXOffset;
    public Vector2 maxXOffset;
    
    void Start()
    {
        SnapToGround();
        stay = true;
        stepping = false;
        initialXOffset = transform.localPosition.x;
    }
    void Update()
    {
        StateFluidSteps();
        DrawDebugInfo();
    }
    void StateFluidSteps()
    {
        moveDir = Mathf.Clamp(manager.currentSpeed, -1f, 1f); if (manager.brain.curState == TIGERBrain.State.backStep) { moveDir *= -0.5f; }
        Vector3 hipPlacementOffset = hip.up * (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength);
        hipPlacementOffset *= moveDir;

        bool stepSooner = false; bool notDoneStepping = false; float maxDistMod = 1f;
        switch (manager.brain.curState)
        {
            case TIGERBrain.State.idle: stepSooner = GetHorizontalDist(); notDoneStepping = Vector3.Distance(transform.position, meshFoot.position) > 1; break;
            case TIGERBrain.State.followTurn: stepSooner = GetHorizontalDist(); notDoneStepping = Vector3.Distance(transform.position, meshFoot.position) > 1; maxDistMod = 0.3f; break;
            case TIGERBrain.State.chase: break;
            case TIGERBrain.State.backStep: stepSooner = GetHorizontalDist(); notDoneStepping = Vector3.Distance(transform.position, meshFoot.position) > 1; maxDistMod = 0.5f; break;
        }

        //stepSooner = false;
        //notDoneStepping = false;

        float relativeStepSpeed = (Mathf.Clamp(manager.currentSpeed, 1.5f, 10) * stepSpeed);

        Ray activeRay = new Ray(hip.position + hipPlacementOffset, Vector3.down);
        if (Physics.Raycast(activeRay, out RaycastHit activeInfo, maxStepHeight * 4f, terrainLayer.value)) { activeNextPos = activeInfo.point; }

        if (!stepping)
        {
            if (stay) { transform.position = stayPos; } else { SnapToGround(); }
            Ray ray = new Ray(hip.position + hipPlacementOffset, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit info, maxStepHeight * 4f, terrainLayer.value)) { staticNextPos = info.point; }
            if (stepSooner) {
                if (!pairedLeg.stepping && Vector3.Distance(transform.position, staticNextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) * 0.5f * (maxDistMod/2f))
                { stay = false; stepping = true; stepProgress = 0; } }
            else {
                if (!pairedLeg.stepping && Vector3.Distance(transform.position, staticNextPos) > (Mathf.Lerp(manager.walkStepLengthMod, manager.runStepLengthMod, manager.progressToRun) * stepLength) * 1.5f * maxDistMod)
                { stay = false; stepping = true; stepProgress = 0; } }
            if (notDoneStepping) { stay = false; stepping = true; stepProgress = 0; }
        }
        else
        {
            stepProgress += Time.deltaTime * relativeStepSpeed; if (stepSooner) { stepProgress += Time.deltaTime * relativeStepSpeed; }
            transform.position = Vector3.LerpUnclamped(stayPos, staticNextPos, Mathf.Lerp(manager.walkProgressCurve.Evaluate(stepProgress), manager.runProgressCurve.Evaluate(stepProgress), manager.progressToRun));
            transform.position += Vector3.up * (Mathf.Lerp(manager.walkCurve.Evaluate(stepProgress), manager.runCurve.Evaluate(stepProgress), manager.progressToRun) * stepHeight * Mathf.Lerp(manager.walkStepHeightMod, manager.runStepHeightMod, manager.progressToRun));
            if (stepProgress >= 1) { SnapToGround(); stay = true; stepping = false; }
        }
    }
    bool GetHorizontalDist()
    {
        //Get localPos
        Vector3 curPos = transform.localPosition;
        transform.position = staticNextPos;
        Vector3 nextPos = transform.localPosition;
        transform.localPosition = curPos;

        //Flatten Pos
        curPos = Vector3.right * curPos.x;
        nextPos = Vector3.right * nextPos.x;

        //Calc
        float dist = curPos.x - nextPos.x;

        return (dist < maxXOffset.x) || (dist > maxXOffset.y);
    }
    void SnapToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * maxStepHeight, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, maxStepHeight * 4f, terrainLayer.value))
        {
            transform.position = new Vector3(transform.position.x, info.point.y, transform.position.z);
            stayPos = transform.position;
        }
    }
    void DrawDebugInfo()
    {
        Debug.DrawRay(activeNextPos, Vector3.up, Color.red);
        Debug.DrawRay(staticNextPos, Vector3.up/1.5f, Color.white);
        if (GetHorizontalDist()) {
            Debug.DrawRay(transform.position + transform.forward + transform.right, Vector3.up, Color.red);
            Debug.DrawRay(transform.position + transform.forward - transform.right, Vector3.up, Color.red);
            Debug.DrawRay(transform.position - transform.forward + transform.right, Vector3.up, Color.red);
            Debug.DrawRay(transform.position - transform.forward - transform.right, Vector3.up, Color.red);
            Debug.DrawRay(transform.position + transform.right, Vector3.up, Color.red);
            Debug.DrawRay(transform.position - transform.right, Vector3.up, Color.red);
        } else if (stepping) {
            Debug.DrawRay(transform.position + transform.forward + transform.right, Vector3.up, Color.yellow);
            Debug.DrawRay(transform.position + transform.forward - transform.right, Vector3.up, Color.yellow);
            Debug.DrawRay(transform.position - transform.forward + transform.right, Vector3.up, Color.yellow);
            Debug.DrawRay(transform.position - transform.forward - transform.right, Vector3.up, Color.yellow);
            Debug.DrawRay(transform.position + transform.right, Vector3.up, Color.yellow);
            Debug.DrawRay(transform.position - transform.right, Vector3.up, Color.yellow);
        } else {
            Debug.DrawRay(transform.position + transform.forward + transform.right, Vector3.up, Color.cyan);
            Debug.DrawRay(transform.position + transform.forward - transform.right, Vector3.up, Color.cyan);
            Debug.DrawRay(transform.position - transform.forward + transform.right, Vector3.up, Color.cyan);
            Debug.DrawRay(transform.position - transform.forward - transform.right, Vector3.up, Color.cyan);
            Debug.DrawRay(transform.position + transform.right, Vector3.up, Color.cyan);
            Debug.DrawRay(transform.position - transform.right, Vector3.up, Color.cyan);
        }
        if (Vector3.Distance(transform.position, meshFoot.position) < 1)
        { Debug.DrawRay(transform.position, meshFoot.position - transform.position, Color.white); } 
        else 
        { Debug.DrawRay(transform.position, meshFoot.position - transform.position, Color.magenta); }
    }
}
