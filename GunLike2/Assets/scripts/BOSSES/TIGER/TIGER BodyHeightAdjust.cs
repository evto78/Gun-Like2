using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TIGERBodyHeightAdjust : MonoBehaviour
{
    public float runOffset; public float crouchOffset; float actingOffset;
    NavMeshAgent agent;
    float initalY;
    public Transform backJoint; float initialBackY; Vector3 initialBackPos;
    public Transform frontJoint; float initialFrontY; Vector3 initialFrontPos;

    [Header("Management")]
    public List<TIGERIKFootSolver> legs; // (0,1) is back legs, (2,3) is front legs
    public enum state { walk, run }
    public state curState;
    public bool handlePairs;
    public float walkStepHeightMod; public float runStepHeightMod;
    public float walkStepLengthMod; public float runStepLengthMod;
    public float walkStepSpeedMod; public float runStepSpeedMod; float baseStepSpeed;
    float internalTimer = 0f;
    public AnimationCurve walkCurve; float walkResult;
    public AnimationCurve runCurve; float frontRunResult; float backRunResult;
    public AnimationCurve walkProgressCurve;
    public AnimationCurve runProgressCurve;
    public float currentSpeed; float posSampleTimer; Vector3 sampledPos;
    public Vector2 walkRunSpeedThreshold; public float progressToRun;

    private void Awake() { foreach (TIGERIKFootSolver solver in legs) { solver.manager = this; solver.managerHandlesPairs = handlePairs; } }

    private void Start()
    {
        actingOffset = crouchOffset;
        agent = GetComponent<NavMeshAgent>();
        initalY = legs[0].transform.localPosition.y;
        initialBackY = backJoint.localPosition.y; initialBackPos = backJoint.localPosition;
        initialFrontY = frontJoint.localPosition.y; initialFrontPos = frontJoint.localPosition;
        sampledPos = transform.position;
        posSampleTimer = 0f;
        baseStepSpeed = legs[0].stepSpeed;
    }
    void Update()
    {
        internalTimer += Time.deltaTime; if (internalTimer > 1) { internalTimer -= Mathf.Floor(internalTimer); }
        walkResult = walkCurve.Evaluate(internalTimer); frontRunResult = runCurve.Evaluate(internalTimer); backRunResult = runCurve.Evaluate(1f - internalTimer);

        posSampleTimer -= Time.deltaTime; if (posSampleTimer < 0f) { currentSpeed = Vector3.Distance(transform.position, sampledPos); posSampleTimer = 0.1f; sampledPos = transform.position; }
        progressToRun = Mathf.Clamp((currentSpeed - walkRunSpeedThreshold.x) / walkRunSpeedThreshold.y, 0f, 1f);

        //auto state change
        if (progressToRun == 1) { ChangeState(state.run); } else { ChangeState(state.walk); }
        actingOffset = Mathf.Lerp(crouchOffset, runOffset, progressToRun);

        foreach(TIGERIKFootSolver solver in legs) { solver.stepSpeed = baseStepSpeed * Mathf.Lerp(walkStepSpeedMod, runStepSpeedMod, progressToRun); }
        if (handlePairs) { ForceStepProgressBetweenPairs(); }
        HeightAdjustment();
    }
    public void HeightAdjustment()
    {
        float backHeight; float frontHeight; float avgHeight;

        //Avg
        float avgY = 0f;
        foreach (TIGERIKFootSolver solver in legs)
        {
            avgY += solver.transform.localPosition.y;
        }
        avgY /= legs.Count;
        avgY /= initalY;
        avgHeight = avgY;
        agent.baseOffset = avgHeight + actingOffset;

        //Back
        avgY = 0f;
        avgY += legs[0].transform.localPosition.y; avgY += legs[1].transform.localPosition.y;
        avgY /= 2f; avgY /= initalY;
        backHeight = avgY;

        //Front
        avgY = 0f;
        avgY += legs[2].transform.localPosition.y; avgY += legs[3].transform.localPosition.y;
        avgY /= 2f; avgY /= initalY;
        frontHeight = avgY;

        backJoint.localPosition = initialBackPos + (Vector3.up * (((backHeight * 1.5f) - 1) / 75f));
        frontJoint.localPosition = initialFrontPos + (Vector3.up * (((frontHeight * 1.5f) - 1) / 75f));
        if (frontJoint.localPosition.y < initialFrontY - 0.01f) { frontJoint.localPosition = initialFrontPos - Vector3.up * 0.01f; }
    }
    public void ChangeState(state newState)
    {
        curState = newState;
        foreach (TIGERIKFootSolver solver in legs)
        {
            switch (curState)
            {
                case state.walk: solver.curState = TIGERIKFootSolver.state.walk; break;
                case state.run: solver.curState = TIGERIKFootSolver.state.run; break;
            }
        }
    }
    void ForceStepProgressBetweenPairs()
    {
        float walkPairDifference = 1f; float runPairDifference = 0.2f;
        float walkOppositeDifference = 0.2f; float runOppositeDifference = 1f;

        TIGERIKFootSolver br = legs[0];
        TIGERIKFootSolver bl = legs[1];
        TIGERIKFootSolver fr = legs[2];
        TIGERIKFootSolver fl = legs[3];

        float tarPairDifference = Mathf.Lerp(walkPairDifference, runPairDifference, progressToRun); 
        float tarOppositeDifference = Mathf.Lerp(walkOppositeDifference, runOppositeDifference, progressToRun);

        if (progressToRun < 0.75f)
        {
            br.pairedLeg = bl; bl.pairedLeg = br;
            fr.pairedLeg = fl; fl.pairedLeg = fr;
        }
        else
        {
            br.pairedLeg = fr; fr.pairedLeg = br;
            fl.pairedLeg = bl; bl.pairedLeg = fl;
        }

            ApplyChangeToStepProgress(br, bl, tarPairDifference, 1f);
        ApplyChangeToStepProgress(fr, fl, tarPairDifference, 1f);
        ApplyChangeToStepProgress(bl, fr, tarOppositeDifference, 1.5f);
        ApplyChangeToStepProgress(br, fl, tarOppositeDifference, 1.5f);
    }
    void ApplyChangeToStepProgress(TIGERIKFootSolver a, TIGERIKFootSolver b, float tarDiff, float force)
    {
        float diff; float diffToTar;
        if (a.stepProgress > b.stepProgress)
        {
            diff = a.stepProgress - b.stepProgress;
            diffToTar = diff - tarDiff;
            if (Mathf.Abs(diffToTar) < 0.1f) { return; }
            a.stepProgress -= Time.deltaTime * (currentSpeed * a.stepSpeed) * diffToTar * force;

            //if (a.stepProgress >= 0.5f) { a.stepProgress += Time.deltaTime * (currentSpeed * a.stepSpeed); }
            //else { a.stepProgress -= Time.deltaTime * (currentSpeed * a.stepSpeed); }
        }
        else
        {
            diff = b.stepProgress - a.stepProgress;
            diffToTar = diff - tarDiff;
            if (Mathf.Abs(diffToTar) < 0.1f) { return; }
            b.stepProgress -= Time.deltaTime * (currentSpeed * b.stepSpeed) * diffToTar * force;

            //if (b.stepProgress >= 0.5f) { b.stepProgress += Time.deltaTime * (currentSpeed * b.stepSpeed); }
            //else { b.stepProgress -= Time.deltaTime * (currentSpeed * b.stepSpeed); }
        }
    }
}
