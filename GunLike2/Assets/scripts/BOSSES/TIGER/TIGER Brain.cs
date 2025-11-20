using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TIGERBrain : MonoBehaviour
{
    [Header("References")]
    TIGERBodyHeightAdjust bha;
    TIGERTailManager tm;
    TIGERNavAI nav;
    NavMeshAgent agent;
    Transform player;
    GameDataManager gdm;
    public Animator headAnim;
    public Transform headPointer;
    public Transform cannonFirepoint;

    [Header("States and Stats")]
    public Vector2 baseWalkSpeedAccel;
    public Vector2 baseChaseSpeedAccel;
    public Vector2 baseSprintSpeedAccel;
    public enum State { idle, chase, backStep, followTurn }
    public State curState;
    public enum MoveState { walk, chase, sprint }
    public MoveState curMoveState;
    [Header("Internal")]
    float backAccel = 2; float curBackSpeed = 0; float backstepTimer = 0;
    float followRotSpeed = 2f;
    public AnimationCurve sineCurve;
    //HeadMovement
    Vector2 manualCamInputDir; Vector2 manualCamDir = Vector2.zero;
    public List<Transform> headJoints;
    List<float> downXVals; List<float> upXVals; List<float> midXVals;
    List<float> leftYVals; List<float> rightYVals; List<float> midYVals;
    float prevX = 0f; float prevY = 0f;

    private void Awake()
    {
        bha = GetComponent<TIGERBodyHeightAdjust>();
        nav = GetComponent<TIGERNavAI>();
        agent = GetComponent<NavMeshAgent>();
        tm = GetComponentInChildren<TIGERTailManager>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.transform;

        InitializeHeadJointVals();
    }
    void Start()
    {
        ChangeState(curState, curMoveState);
    }
    void InitializeHeadJointVals()
    {
        headAnim.enabled = false;
        downXVals = new List<float>() { 10f, 12f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, -12f, 0f, 0f };
        midXVals = new List<float>() { 10f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
        upXVals = new List<float>() { 10f, -40f, -5f, -5f, -15f, -15f, -5f, 0f, 10f, 10f, -40f, 0f, 0f };
        leftYVals = new List<float>() { -20f, 0f, -5f, -5f, -5f, -5f, -10f, -10f, -5f, -5f, 30f, 0f, 0f };
        midYVals = new List<float>() { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
        rightYVals = new List<float>() { 20f, 0f, 5f, 5f, 5f, 5f, 10f, 10f, 5f, 5f, -30f, 0f, 0f };
    }
    void ChangeState(State newState, MoveState newMoveState) 
    { 
        curState = newState; curMoveState = newMoveState;
        switch (curState) {
            case State.idle: nav.SetState(TIGERNavAI.state.idle); break;
            case State.chase: nav.SetState(TIGERNavAI.state.chase); break;
            case State.backStep: nav.SetState(TIGERNavAI.state.idle); backstepTimer = 1f; break;
            case State.followTurn: nav.SetState(TIGERNavAI.state.idle); break; }
        switch (curMoveState) {
            case MoveState.walk: agent.speed = baseWalkSpeedAccel.x; agent.acceleration = baseWalkSpeedAccel.y; break;
            case MoveState.chase: agent.speed = baseChaseSpeedAccel.x; agent.acceleration = baseChaseSpeedAccel.y; break;
            case MoveState.sprint: agent.speed = baseSprintSpeedAccel.x; agent.acceleration = baseSprintSpeedAccel.y; break; }
    }
    void ManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ChangeState(State.idle, curMoveState); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { ChangeState(State.followTurn, curMoveState); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ChangeState(State.chase, curMoveState); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { ChangeState(State.backStep, curMoveState); }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { ChangeState(curState, MoveState.walk); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { ChangeState(curState, MoveState.chase); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { ChangeState(curState, MoveState.sprint); }

        manualCamInputDir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) { manualCamInputDir += Vector2.up; }
        if (Input.GetKey(KeyCode.DownArrow)) { manualCamInputDir -= Vector2.up; }
        if (Input.GetKey(KeyCode.LeftArrow)) { manualCamInputDir -= Vector2.right; }
        if (Input.GetKey(KeyCode.RightArrow)) { manualCamInputDir += Vector2.right; }
        manualCamDir += Time.deltaTime * manualCamInputDir;
        manualCamDir = new Vector2(Mathf.Clamp(manualCamDir.x, -1, 1), Mathf.Clamp(manualCamDir.y, -1, 1));
    }
    void Update()
    {
        ManualInput();

        if (curBackSpeed > 0) { curBackSpeed -= backAccel * Time.deltaTime; }
        if (backstepTimer > 0) { backstepTimer -= Time.deltaTime; }

        switch (curState) {
            case State.idle:
                break;
            case State.chase:
                break;
            case State.backStep:
                curBackSpeed += backAccel * 2 * Time.deltaTime; if (curBackSpeed > 1) { curBackSpeed = 1; }
                CheckStoppedBackstep();
                break;
            case State.followTurn:
                Quaternion curRot = transform.rotation; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); Quaternion tarRot = transform.rotation;
                transform.rotation = Quaternion.Lerp(curRot, tarRot, Time.deltaTime * followRotSpeed);
                break; }
        transform.position -= Time.deltaTime * sineCurve.Evaluate(curBackSpeed) * baseWalkSpeedAccel.x * 0.2f * transform.forward;

        HeadMovement();
    }
    void CheckStoppedBackstep() { if (bha.currentSpeed < 0.2f && backstepTimer <= 0) { ChangeState(State.idle, curMoveState); } }
    void HeadMovement()
    {
        float xAxis;
        float yAxis;

        headPointer.LookAt(player); //float outy = headPointer.localEulerAngles.y; float outx = -headPointer.localEulerAngles.x;
        //if (outy > -60 && outy < 60 && outx > -60 && outx < 60)
        //{
        //    yAxis = outy / 20f;
        //    if (outx > 0) { xAxis = outx / 12f; }
        //    else { xAxis = outx / 40f; }
        //}
        //else
        //{
        //    xAxis = 0f; yAxis = 0f;
        //}

        //Debug.Log("Current: " + outx + " | " + outy);
        //Debug.Log("Target: " + xAxis + " | " + yAxis);

        //headPointer.LookAt(cannonFirepoint);
        //xAxis = 0.7f;
        //yAxis = -0.2f;

        xAxis = Mathf.Lerp(prevX, xAxis, Time.deltaTime*4f);
        yAxis = Mathf.Lerp(prevY, yAxis, Time.deltaTime*4f);

        prevX = xAxis; prevY = yAxis;

        //xAxis = manualCamDir.y;
        //yAxis = manualCamDir.x;

        Debug.DrawRay(cannonFirepoint.position, cannonFirepoint.forward * 20, Color.red);
        Debug.DrawRay(headPointer.position, headPointer.forward * 20, Color.yellow);
        switch (xAxis)
        {
            case > 0:
                switch (yAxis)
                {
                    case > 0: HeadJointAdjust(xAxis, yAxis, midXVals, upXVals, midYVals, rightYVals); break;
                    case 0: HeadJointAdjust(xAxis, yAxis, midXVals, upXVals, midYVals, midYVals); break;
                    case < 0: HeadJointAdjust(xAxis, 1 + yAxis, midXVals, upXVals, leftYVals, midYVals); break;
                }
                break;
            case 0:
                switch (yAxis)
                {
                    case > 0: HeadJointAdjust(xAxis, yAxis, midXVals, midXVals, midYVals, rightYVals); break;
                    case 0: HeadJointAdjust(xAxis, yAxis, midXVals, midXVals, midYVals, midYVals); break;
                    case < 0: HeadJointAdjust(xAxis, 1 + yAxis, midXVals, midXVals, leftYVals, midYVals); break;
                }
                break;
            case < 0:
                switch (yAxis)
                {
                    case > 0: HeadJointAdjust(1 + xAxis, yAxis, downXVals, midXVals, midYVals, rightYVals); break;
                    case 0: HeadJointAdjust(1 + xAxis, yAxis, downXVals, midXVals, midYVals, midYVals); break;
                    case < 0: HeadJointAdjust(1 + xAxis, 1 + yAxis, downXVals, midXVals, leftYVals, midYVals); break;
                }
                break;
        }
    }
    void HeadJointAdjust(float xAxis, float yAxis, List<float> xMin, List<float> xMax, List<float> yMin, List<float> yMax)
    {
        xAxis = Mathf.Abs(xAxis); yAxis = Mathf.Abs(yAxis);
        if (xMin == xMax && yMin == yMax)
        {
            for (int i = 0; i < headJoints.Count; i++)
            {
                Vector3 newDir = Vector3.zero;
                newDir += Vector3.right * xMin[i];
                newDir += Vector3.up * yMin[i];
                headJoints[i].localEulerAngles = newDir;
            }
        }
        else
        {
            for (int i = 0; i < headJoints.Count; i++)
            {
                Vector3 newDir = Vector3.zero;
                newDir += Vector3.right * Mathf.Lerp(xMin[i], xMax[i], xAxis);
                newDir += Vector3.up * Mathf.Lerp(yMin[i], yMax[i], yAxis);
                headJoints[i].localEulerAngles = newDir;
            }
        }
    }
}
