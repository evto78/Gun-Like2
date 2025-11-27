using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class TIGERBodyHeightAdjust : MonoBehaviour
{
    public TIGERBrain brain;
    public float runOffset; public float crouchOffset; float actingOffset;
    NavMeshAgent agent;
    float initalY;
    public Transform backJoint; float initialBackY; Vector3 initialBackPos;
    public Transform frontJoint; float initialFrontY; Vector3 initialFrontPos;

    [Header("Walking/Running Management")]
    public List<TIGERIKFootSolver> legs; // (0,1) is back legs, (2,3) is front legs
    public List<MonoBehaviour> tilts;
    public enum state { walk, run }
    public state curState;
    public bool handlePairs;
    public float walkStepHeightMod; public float runStepHeightMod;
    public float walkStepLengthMod; public float runStepLengthMod;
    public float walkStepSpeedMod; public float runStepSpeedMod; float baseStepSpeed;
    public AnimationCurve walkCurve;
    public AnimationCurve runCurve;
    public AnimationCurve walkProgressCurve;
    public AnimationCurve runProgressCurve;
    public float currentSpeed; float posSampleTimer; Vector3 sampledPos; Vector3 sampledForward; public float unscaledCurrentSpeed;
    public Vector2 walkRunSpeedThreshold; public float progressToRun;
    public Transform backLegsHolder; public Transform frontLegsHolder; float initialBackLegsHolderY; float initialFrontLegsHolderY;
    static Vector2 backLegsVerticalDisplacementMinMax = new Vector2(-0.03f, 0.015f); // manualy inputed
    static Vector2 frontLegsVerticalDisplacementMinMax = new Vector2(-0.01f, 0.015f); // manualy inputed
    float backDisplace = 0.5f; float frontDisplace = 0.5f;
    static Vector2 backHeightMinMax = new Vector2(0.19f, 1.22f); // manualy calculated and inputed
    static Vector2 frontHeightMinMax = new Vector2(0.58f, 1.36f); // manualy calculated and inputed
    public Transform pevlisRotationPoint; public Transform pelvisSholderPointer;

    [Header("Effects")]
    [Tooltip("Chance for each step to be a miss-step. Accepts a value from 0-100")]
    public float mStepChance; //2 legs cannot miss-step at the same time, the same leg cannot miss-step 2 times in a row.

    [Header("External Variables")]
    public Vector3 turnDirVel;

    private void Awake() { brain = GetComponent<TIGERBrain>(); foreach (TIGERIKFootSolver solver in legs) { solver.manager = this; } }

    private void Start()
    {
        actingOffset = crouchOffset;
        agent = GetComponent<NavMeshAgent>();
        initalY = legs[0].transform.localPosition.y;
        initialBackY = backJoint.localPosition.y; initialBackPos = backJoint.localPosition;
        initialFrontY = frontJoint.localPosition.y; initialFrontPos = frontJoint.localPosition;
        initialBackLegsHolderY = backLegsHolder.localPosition.y;
        initialFrontLegsHolderY = frontLegsHolder.localPosition.y;
        sampledPos = transform.position; sampledForward = transform.forward;
        posSampleTimer = 0f;
        baseStepSpeed = legs[0].stepSpeed;

        tilts = new List<MonoBehaviour>();
        tilts.AddRange(GetComponentsInChildren<TIGERBackPawTilt>());
        tilts.AddRange(GetComponentsInChildren<TIGERFrontToeTilt>());
        foreach(MonoBehaviour tilt in tilts) { tilt.enabled = false; }
    }
    void Update()
    {
        posSampleTimer -= Time.deltaTime; if (posSampleTimer < 0f) 
        { currentSpeed = Vector3.Distance(transform.position, sampledPos); posSampleTimer = 0.1f; sampledPos = transform.position; GetTurnDirVel(); sampledForward = transform.forward; }
        progressToRun = Mathf.Clamp((currentSpeed - walkRunSpeedThreshold.x) / walkRunSpeedThreshold.y, 0f, 1f);

        //auto state change
        if (progressToRun == 1) { ChangeState(state.run); } else { ChangeState(state.walk); }
        actingOffset = Mathf.Lerp(crouchOffset, runOffset, progressToRun);

        foreach(TIGERIKFootSolver solver in legs) { solver.stepSpeed = baseStepSpeed * Mathf.Lerp(walkStepSpeedMod, runStepSpeedMod, progressToRun); }
        if (handlePairs) { ForceStepProgressBetweenPairs(); }
        HeightAdjustment();
        ManageStepping();

        pevlisRotationPoint.transform.LookAt(pelvisSholderPointer);
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

        ManageHipSholderHeightDisplacement(backHeight, frontHeight);

        backJoint.localPosition = initialBackPos + (Vector3.up * (((backHeight * 1.5f) - 1) / 75f));
        frontJoint.localPosition = initialFrontPos + (Vector3.up * (((frontHeight * 1.5f) - 1) / 75f));
        if (frontJoint.localPosition.y < initialFrontY - 0.01f) { frontJoint.localPosition = initialFrontPos - Vector3.up * 0.01f; }
    }
    void ManageHipSholderHeightDisplacement(float backAvgY, float frontAvgY)
    {
        backDisplace = 0.5f; frontDisplace = 0.5f;

        backDisplace = (backAvgY - backHeightMinMax.x) / (backHeightMinMax.y - backHeightMinMax.x);
        frontDisplace = (frontAvgY - frontHeightMinMax.x) / (frontHeightMinMax.y - frontHeightMinMax.x);

        float intensity = Mathf.Clamp(progressToRun, 0.5f, 1f);

        if (legs[0].goingToMStep || legs[1].goingToMStep) { backDisplace /= 2f; intensity = 1.2f; }
        if (legs[2].goingToMStep || legs[3].goingToMStep) { frontDisplace /= 2f; intensity = 1.2f; }

        float backYAmt = Mathf.Lerp(backLegsVerticalDisplacementMinMax.x, backLegsVerticalDisplacementMinMax.y, backDisplace) * intensity;
        float frontYAmt = Mathf.Lerp(frontLegsVerticalDisplacementMinMax.x, frontLegsVerticalDisplacementMinMax.y, frontDisplace) * intensity;

        //backLegsHolder.transform.localPosition = new Vector3(backLegsHolder.transform.localPosition.x, initialBackLegsHolderY + backYAmt, backLegsHolder.transform.localPosition.z);
        //frontLegsHolder.transform.localPosition = new Vector3(frontLegsHolder.transform.localPosition.x, initialFrontLegsHolderY + frontYAmt, frontLegsHolder.transform.localPosition.z);

        float adjustSpeed = 6f;

        backLegsHolder.transform.localPosition = Vector3.Lerp(backLegsHolder.transform.localPosition, 
            new Vector3(backLegsHolder.transform.localPosition.x, initialBackLegsHolderY + backYAmt, backLegsHolder.transform.localPosition.z), Time.deltaTime * adjustSpeed);
        frontLegsHolder.transform.localPosition = Vector3.Lerp(frontLegsHolder.transform.localPosition,
            new Vector3(frontLegsHolder.transform.localPosition.x, initialFrontLegsHolderY + frontYAmt, frontLegsHolder.transform.localPosition.z), Time.deltaTime * adjustSpeed);
    }
    void ManageStepping()
    {
        int index = 0;
        foreach(TIGERIKFootSolver leg in legs)
        {
            bool enableTilt = leg.goingToMStep;
            enableTilt = false;
            switch (index)
            {
                case 0: tilts[1].enabled = enableTilt; tilts[5].enabled = enableTilt; break;
                case 1: tilts[0].enabled = enableTilt; tilts[4].enabled = enableTilt; break;
                case 2: tilts[3].enabled = enableTilt; break;
                case 3: tilts[2].enabled = enableTilt; break;
            }
            index++;
        }
    }
    public bool CheckBeforeStep(TIGERIKFootSolver sender)
    {
        bool mStep = false;

        if (sender.goingToMStep) { return true; }
        if (sender.pairedLeg.goingToMStep) { return false; }
        mStep = Random.Range(1, 100) < mStepChance;

        return mStep;
    }
    public void ChangeState(state newState)
    {
        curState = newState;
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
    void GetTurnDirVel()
    {
        Vector3 forwardDir = transform.forward;
        Vector3 movementDir = sampledForward;
        turnDirVel = forwardDir - movementDir;
    }
}
