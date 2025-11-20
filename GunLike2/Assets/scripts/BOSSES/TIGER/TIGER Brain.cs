using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TIGERBrain : MonoBehaviour
{
    // Refrences
    TIGERBodyHeightAdjust bha;
    TIGERTailManager tm;
    TIGERNavAI nav;
    NavMeshAgent agent;
    Transform player;
    GameDataManager gdm;
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

    private void Awake()
    {
        bha = GetComponent<TIGERBodyHeightAdjust>();
        nav = GetComponent<TIGERNavAI>();
        agent = GetComponent<NavMeshAgent>();
        tm = GetComponentInChildren<TIGERTailManager>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.transform;
    }
    void Start()
    {
        ChangeState(curState, curMoveState);
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
    }
    void Update()
    {
        //ManualInput();

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
    }
    void CheckStoppedBackstep() { if (bha.currentSpeed < 0.2f && backstepTimer <= 0) { ChangeState(State.idle, curMoveState); } }
}
