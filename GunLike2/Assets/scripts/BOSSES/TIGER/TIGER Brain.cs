using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class TIGERBrain : MonoBehaviour
{
    [Header("References")]
    BossHealthManager ehm;
    TIGERBodyHeightAdjust bha;
    TIGERTailManager tm;
    TIGERNavAI nav;
    NavMeshAgent agent;
    Transform player;
    GameDataManager gdm;
    public Transform headPointer;
    public Transform cannonFirepoint;

    [Header("States and Stats")]
    public Vector2 baseWalkSpeedAccel;
    public Vector2 baseChaseSpeedAccel;
    public Vector2 baseSprintSpeedAccel;
    public enum MoveState { idle, chase, backStep, followTurn, chasePoint, wander, patrol }
    public MoveState curState;
    public enum SpeedState { walk, chase, sprint }
    public SpeedState curMoveState;
    public enum BehaviorState { idle, prepareToFire, growlStance }
    public BehaviorState curAttackState;
    [Header("Internal")]
    float masterDmg;
    float timerSpeedModifier;
    float backAccel = 2; public float curBackSpeed = 0; float backstepTimer = 0;
    float followRotSpeed = 2f;
    public AnimationCurve sineCurve;
    //HeadMovement
    Vector2 manualCamInputDir; Vector2 manualCamDir = Vector2.zero;
    public List<Transform> headJoints;
    List<float> downXVals; List<float> upXVals; List<float> midXVals;
    List<float> leftYVals; List<float> rightYVals; List<float> midYVals;
    float prevX = 0f; float prevY = 0f; bool manualMoving = false;

    [Header("ManualAnim")]
    public bool playIntroOnStart;
    bool pauseUpdate = false;
    public Animator manualAnim;
    public List<Animator> proceduralAnims;
    public Vector3 manualNavPoint;

    [Header("Attacks")]
    public GameObject nuke;
    public float nukeDmg;
    public LineRenderer aimingLR;
    public List<ParticleSystem> muzzleFlash;

    private void Awake()
    {
        playIntroOnStart = false; // <-- DISABLED FOR TESTING
        ehm = GetComponent<BossHealthManager>();
        bha = GetComponent<TIGERBodyHeightAdjust>();
        nav = GetComponent<TIGERNavAI>();
        agent = GetComponent<NavMeshAgent>();
        tm = GetComponentInChildren<TIGERTailManager>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        player = gdm.phm.transform;

        masterDmg = ehm.baseDamage * ehm.difficultyScale * gdm.difficulty;

        InitializeHeadJointVals();
    }
    void Start()
    {
        timerSpeedModifier = 1f;
        if (ehm.gdm.difficultyIDSelected == 0) { timerSpeedModifier = 0.8f; }

        //if (playIntroOnStart) { pauseUpdate = true; StartCoroutine(IntroAnim()); }
        //else { manualAnim.enabled = false; StartPatroling(); pauseUpdate = false; foreach (Animator a in proceduralAnims) { a.enabled = true; } agent.enabled = true; }

        manualAnim.enabled = false; StartPatroling(); pauseUpdate = false; foreach (Animator a in proceduralAnims) { a.enabled = true; }
        agent.enabled = true;
    }
    void StartPatroling()
    {
        ChangeState(MoveState.patrol, SpeedState.walk, BehaviorState.idle);
        gdm.phm.uiMan.bossHealthBars[1].SetActive(false);
    }
    bool combatStarted = false;
    public void StartCombat()
    {
        if (combatStarted) { return; }
        ChangeState(MoveState.followTurn, SpeedState.chase, BehaviorState.idle);
        gdm.phm.uiMan.bossHealthBars[1].SetActive(true);
        gdm.phm.uiMan.bossHealthBars[1].GetComponent<BossHealthBar>().ehm = ehm;
    }
    void InitializeHeadJointVals()
    {
        downXVals = new List<float>() { 10f, 12f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, 3f, -12f, 0f, 0f };
        midXVals = new List<float>() { 10f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
        upXVals = new List<float>() { 10f, -40f, -5f, -5f, -15f, -15f, -5f, 0f, 10f, 10f, -40f, 0f, 0f };
        leftYVals = new List<float>() { -20f, 0f, -5f, -5f, -5f, -5f, -10f, -10f, -5f, -5f, 30f, 0f, 0f };
        midYVals = new List<float>() { 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f };
        rightYVals = new List<float>() { 20f, 0f, 5f, 5f, 5f, 5f, 10f, 10f, 5f, 5f, -30f, 0f, 0f };
    }
    void ChangeState(MoveState newState, SpeedState newMoveState, BehaviorState newAttackState) 
    { 
        curState = newState; curMoveState = newMoveState; curAttackState = newAttackState;
        switch (curState) {
            case MoveState.idle: nav.SetState(TIGERNavAI.state.idle); break;
            case MoveState.chase: nav.SetState(TIGERNavAI.state.chase); break;
            case MoveState.backStep: nav.SetState(TIGERNavAI.state.idle); backstepTimer = 1f; break;
            case MoveState.followTurn: nav.SetState(TIGERNavAI.state.idle); break; 
            case MoveState.chasePoint: nav.SetState(TIGERNavAI.state.chasePoint); break; 
            case MoveState.wander: nav.SetState(TIGERNavAI.state.wander); break; 
            case MoveState.patrol: nav.SetState(TIGERNavAI.state.patrol); break; }
        switch (curMoveState) {
            case SpeedState.walk: agent.speed = baseWalkSpeedAccel.x; agent.acceleration = baseWalkSpeedAccel.y; break;
            case SpeedState.chase: agent.speed = baseChaseSpeedAccel.x; agent.acceleration = baseChaseSpeedAccel.y; break;
            case SpeedState.sprint: agent.speed = baseSprintSpeedAccel.x; agent.acceleration = baseSprintSpeedAccel.y; break; }
        switch (curAttackState) {
            case BehaviorState.idle: break;
            case BehaviorState.prepareToFire: break; }
    }
    void ManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ChangeState(MoveState.idle, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { ChangeState(MoveState.followTurn, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ChangeState(MoveState.chase, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { ChangeState(MoveState.backStep, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Minus)) { ChangeState(MoveState.wander, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Equals)) { ChangeState(MoveState.patrol, curMoveState, curAttackState); }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { ChangeState(curState, SpeedState.walk, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { ChangeState(curState, SpeedState.chase, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { ChangeState(curState, SpeedState.sprint, curAttackState); }

        if (Input.GetKeyDown(KeyCode.Alpha8)) { ChangeState(curState, curMoveState, BehaviorState.idle); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { ChangeState(curState, curMoveState, BehaviorState.prepareToFire); }

        if (Input.GetKeyDown(KeyCode.Alpha0)) { manualMoving = !manualMoving; }

        if (Input.GetKeyDown(KeyCode.P)) { StopAllCoroutines(); StartCoroutine(PrepareToShootAndFire()); }
        if (Input.GetKeyDown(KeyCode.L)) { CannonShoot(); }

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
        if (pauseUpdate) { return; }

        bha.mStepChance = Mathf.Lerp(10f,70f, 1f-(ehm.curHp / ehm.maxHp));
        ManualInput();

        if (curBackSpeed > 0) { curBackSpeed -= backAccel * Time.deltaTime; }
        if (backstepTimer > 0) { backstepTimer -= Time.deltaTime; }

        HeadMovement();

        switch (curAttackState) {
            case BehaviorState.idle: break;
            case BehaviorState.prepareToFire:
                if (prevX < -0.6f) { ChangeState(MoveState.backStep, curMoveState, curAttackState); }
                else { ChangeState(MoveState.followTurn, curMoveState, curAttackState); }
                    break; }
        switch (curState) {
            case MoveState.idle:
                break;
            case MoveState.chase:
                break;
            case MoveState.chasePoint:
                break;
            case MoveState.backStep:
                Quaternion curRot = transform.rotation; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); Quaternion tarRot = transform.rotation;
                transform.rotation = Quaternion.Lerp(curRot, tarRot, Time.deltaTime * followRotSpeed);
                curBackSpeed += backAccel * 2 * Time.deltaTime; if (curBackSpeed > 1) { curBackSpeed = 1; }
                CheckStoppedBackstep();
                break;
            case MoveState.followTurn:
                Quaternion curRota = transform.rotation; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); Quaternion tarRota = transform.rotation;
                transform.rotation = Quaternion.Lerp(curRota, tarRota, Time.deltaTime * followRotSpeed);
                break;
            case MoveState.wander:
                break;
            case MoveState.patrol:
                break; }
        transform.position -= Time.deltaTime * sineCurve.Evaluate(curBackSpeed) * baseWalkSpeedAccel.x * 0.2f * transform.forward;
        if (curBackSpeed > 1) { transform.position -= Time.deltaTime * sineCurve.Evaluate(curBackSpeed) * baseWalkSpeedAccel.x * 0.2f * transform.forward; curBackSpeed = Mathf.Lerp(curBackSpeed, 0, Time.deltaTime*2); }
        else if (curBackSpeed > 0.1 && curState != MoveState.backStep) { curBackSpeed = Mathf.Lerp(curBackSpeed, 0, Time.deltaTime); }
        else if (curState != MoveState.backStep) { curBackSpeed = 0; }

        //Check if damaged, to start combat if combat hasn't already started
        if (!combatStarted && ehm.curHp != ehm.maxHp) 
        {
            StartCombat();
        }

        //Other updates
        bha.BrainUpdate();
        tm.BrainUpdate();
        foreach(TIGERIKFootSolver leg in bha.legs) { leg.BrainUpdate(); }
        nav.BrainUpdate();
    }
    void CheckStoppedBackstep() { if (bha.currentSpeed < 0.2f && backstepTimer <= 0) { ChangeState(MoveState.idle, curMoveState, curAttackState); } }
    void HeadMovement()
    {
        float xAxis;
        float yAxis;

        headPointer.LookAt(cannonFirepoint);
        Debug.DrawRay(headPointer.position, headPointer.forward * 20, Color.red);
        Quaternion curRot = headPointer.localRotation;
        switch (curState)
        {
            case MoveState.idle: headPointer.LookAt(headPointer.position + transform.forward * 10f); break;
            case MoveState.chasePoint: headPointer.LookAt(manualNavPoint); break;
            case MoveState.wander: headPointer.LookAt(headPointer.position + transform.forward * 10f); break;
            case MoveState.patrol: headPointer.LookAt(headPointer.position + transform.forward * 10f); break;
            default: headPointer.LookAt(player); break;
        }
        Debug.DrawRay(headPointer.position, headPointer.forward * 25, Color.yellow);
        Quaternion tarRot = headPointer.localRotation;
        if (tarRot.x > 80f || tarRot.x < -80f) { tarRot.x = 0; }
        if (tarRot.y > 80f || tarRot.y < -80f) { tarRot.y = 0; }
        if (curRot.x > tarRot.x) { prevX += Time.deltaTime * 1f; } else { prevX -= Time.deltaTime * 1f; }
        if (curRot.y > tarRot.y) { prevY -= Time.deltaTime * 1f; } else { prevY += Time.deltaTime * 1f; }

        prevX = Mathf.Clamp(prevX, -1, 1);
        prevY = Mathf.Clamp(prevY, -1, 1);

        xAxis = prevX; yAxis = prevY;

        if (manualMoving)
        {
            xAxis = manualCamDir.y;
            yAxis = manualCamDir.x;
        }
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
    private void OnDestroy()
    {
        if (ehm.playerHM.uiMan.bossHealthBars[1] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[1].gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        if (ehm.playerHM.uiMan.bossHealthBars[1] != null)
        {
            ehm.playerHM.uiMan.bossHealthBars[1].gameObject.SetActive(false);
        }
    }
    Vector3 GetClosestRandPointAtGivenDistanceFromPlayer(float dist)
    {
        Vector3 output = Vector3.zero;

        List<Vector3> options = new List<Vector3>();
        for(int i = 0; i < 50; i++)
        {
            switch (Random.Range(0, 4))
            {
                case 0: options.Add(player.position + new Vector3(Random.Range(dist / 1.1f, dist * 1.1f), 0, Random.Range(dist / 1.1f, dist * 1.1f))); break;
                case 1: options.Add(player.position + new Vector3(-Random.Range(dist / 1.1f, dist * 1.1f), 0, Random.Range(dist / 1.1f, dist * 1.1f))); break;
                case 2: options.Add(player.position + new Vector3(Random.Range(dist / 1.1f, dist * 1.1f), 0, -Random.Range(dist / 1.1f, dist * 1.1f))); break;
                case 3: options.Add(player.position + new Vector3(-Random.Range(dist / 1.1f, dist * 1.1f), 0, -Random.Range(dist / 1.1f, dist * 1.1f))); break;
            }
            Vector3 option = options[i];
            NavMeshHit hit;
            if (NavMesh.SamplePosition(option, out hit, 10f, NavMesh.AllAreas) && (Vector3.Distance(hit.position, transform.position) < Vector3.Distance(output, transform.position)))
            {
                output = option;
            }
        }

        return output;
    }
    void CannonShoot()
    {
        curBackSpeed = 4;
        EnemyBullet spawnedBul = Instantiate(nuke).GetComponent<EnemyBullet>();
        spawnedBul.transform.position = cannonFirepoint.position;
        spawnedBul.transform.rotation = cannonFirepoint.rotation;
        spawnedBul.SetStats(nukeDmg, ehm);
        spawnedBul.GetComponent<Rigidbody>().AddForce(cannonFirepoint.forward * 10, ForceMode.Impulse);
        foreach (ParticleSystem ps in muzzleFlash) { ps.Play(); }
    }
    IEnumerator PrepareToShootAndFire()
    {
        manualNavPoint = GetClosestRandPointAtGivenDistanceFromPlayer(100);
        float waitTimer = 0;
        if (manualNavPoint != Vector3.zero)
        {
            ChangeState(MoveState.chasePoint, SpeedState.sprint, BehaviorState.idle);
            while (waitTimer < 8 && Vector3.Distance(transform.position, manualNavPoint) > 10)
            {
                waitTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        ChangeState(MoveState.backStep, SpeedState.chase, BehaviorState.prepareToFire);
        waitTimer = 0; while (waitTimer < 5) { waitTimer += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        CannonShoot();
        ChangeState(MoveState.idle, SpeedState.chase, BehaviorState.idle);
        waitTimer = 0; while (waitTimer < 3) { waitTimer += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        yield return null;
    }
    IEnumerator BeginCombat()
    {


        yield return null;
    }

    //IEnumerator IntroAnim()
    //{
    //    //Setup for anim
    //    foreach (MonoBehaviour tilt in bha.tilts) { tilt.enabled = false; }
    //    foreach (Animator a in proceduralAnims) { a.enabled = false; }
    //    manualAnim.enabled = true;
    //    pauseUpdate = true;
    //
    //    //Setup for combat
    //    foreach (MonoBehaviour tilt in bha.tilts) { tilt.enabled = true; }
    //    foreach (Animator a in proceduralAnims) { a.enabled = true; }
    //    manualAnim.enabled = false;
    //    pauseUpdate = false;
    //    yield return null;
    //}
}
