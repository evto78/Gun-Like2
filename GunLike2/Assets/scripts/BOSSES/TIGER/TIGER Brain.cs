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
    public enum State { idle, chase, backStep, followTurn, chasePoint }
    public State curState;
    public enum MoveState { walk, chase, sprint }
    public MoveState curMoveState;
    public enum AttackState { idle, prepareToFire }
    public AttackState curAttackState;
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
        if (playIntroOnStart) { pauseUpdate = true; StartCoroutine(IntroAnim()); }
        else { manualAnim.enabled = false; StartCombat(); pauseUpdate = false; foreach (Animator a in proceduralAnims) { a.enabled = true; } agent.enabled = true; }
    }
    void StartCombat()
    {
        ChangeState(curState, curMoveState, curAttackState);
        gdm.phm.uiMan.bossHealthBars[1].SetActive(true);
        gdm.phm.uiMan.bossHealthBars[1].GetComponent<BossHealthBar>().ehm = ehm;

        timerSpeedModifier = 1f;
        if (ehm.gdm.difficultyIDSelected == 0) { timerSpeedModifier = 0.8f; }
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
    void ChangeState(State newState, MoveState newMoveState, AttackState newAttackState) 
    { 
        curState = newState; curMoveState = newMoveState; curAttackState = newAttackState;
        switch (curState) {
            case State.idle: nav.SetState(TIGERNavAI.state.idle); break;
            case State.chase: nav.SetState(TIGERNavAI.state.chase); break;
            case State.backStep: nav.SetState(TIGERNavAI.state.idle); backstepTimer = 1f; break;
            case State.followTurn: nav.SetState(TIGERNavAI.state.idle); break; 
            case State.chasePoint: nav.SetState(TIGERNavAI.state.chasePoint); break; }
        switch (curMoveState) {
            case MoveState.walk: agent.speed = baseWalkSpeedAccel.x; agent.acceleration = baseWalkSpeedAccel.y; break;
            case MoveState.chase: agent.speed = baseChaseSpeedAccel.x; agent.acceleration = baseChaseSpeedAccel.y; break;
            case MoveState.sprint: agent.speed = baseSprintSpeedAccel.x; agent.acceleration = baseSprintSpeedAccel.y; break; }
        switch (curAttackState) {
            case AttackState.idle: break;
            case AttackState.prepareToFire: break; }
    }
    void ManualInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ChangeState(State.idle, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { ChangeState(State.followTurn, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ChangeState(State.chase, curMoveState, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { ChangeState(State.backStep, curMoveState, curAttackState); }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { ChangeState(curState, MoveState.walk, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { ChangeState(curState, MoveState.chase, curAttackState); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { ChangeState(curState, MoveState.sprint, curAttackState); }

        if (Input.GetKeyDown(KeyCode.Alpha8)) { ChangeState(curState, curMoveState, AttackState.idle); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { ChangeState(curState, curMoveState, AttackState.prepareToFire); }

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
            case AttackState.idle: break;
            case AttackState.prepareToFire:
                if (prevX < -0.6f) { ChangeState(State.backStep, curMoveState, curAttackState); }
                else { ChangeState(State.followTurn, curMoveState, curAttackState); }
                    break; }
        switch (curState) {
            case State.idle:
                break;
            case State.chase:
                break;
            case State.chasePoint:
                break;
            case State.backStep:
                Quaternion curRot = transform.rotation; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); Quaternion tarRot = transform.rotation;
                transform.rotation = Quaternion.Lerp(curRot, tarRot, Time.deltaTime * followRotSpeed);
                curBackSpeed += backAccel * 2 * Time.deltaTime; if (curBackSpeed > 1) { curBackSpeed = 1; }
                CheckStoppedBackstep();
                break;
            case State.followTurn:
                Quaternion curRota = transform.rotation; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); Quaternion tarRota = transform.rotation;
                transform.rotation = Quaternion.Lerp(curRota, tarRota, Time.deltaTime * followRotSpeed);
                break; }
        transform.position -= Time.deltaTime * sineCurve.Evaluate(curBackSpeed) * baseWalkSpeedAccel.x * 0.2f * transform.forward;
        if (curBackSpeed > 1) { transform.position -= Time.deltaTime * sineCurve.Evaluate(curBackSpeed) * baseWalkSpeedAccel.x * 0.2f * transform.forward; curBackSpeed = Mathf.Lerp(curBackSpeed, 0, Time.deltaTime*2); }
        else if (curBackSpeed > 0.1 && curState != State.backStep) { curBackSpeed = Mathf.Lerp(curBackSpeed, 0, Time.deltaTime); }
        else if (curState != State.backStep) { curBackSpeed = 0; }

            //Other updates
            bha.BrainUpdate();
        tm.BrainUpdate();
        foreach(TIGERIKFootSolver leg in bha.legs) { leg.BrainUpdate(); }
        nav.BrainUpdate();
    }
    void CheckStoppedBackstep() { if (bha.currentSpeed < 0.2f && backstepTimer <= 0) { ChangeState(State.idle, curMoveState, curAttackState); } }
    void HeadMovement()
    {
        float xAxis;
        float yAxis;

        headPointer.LookAt(cannonFirepoint);
        Debug.DrawRay(headPointer.position, headPointer.forward * 20, Color.red);
        Quaternion curRot = headPointer.localRotation;
        switch (curState)
        {
            case State.idle: headPointer.LookAt(headPointer.position + transform.forward * 10f); break;
            case State.chasePoint: headPointer.LookAt(manualNavPoint); break;
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
    IEnumerator PrepareToShootAndFire()
    {
        manualNavPoint = GetClosestRandPointAtGivenDistanceFromPlayer(100);
        Debug.Log(manualNavPoint);
        float waitTimer = 0;
        if (manualNavPoint != Vector3.zero)
        {
            ChangeState(State.chasePoint, MoveState.sprint, AttackState.idle);
            while (waitTimer < 8 && Vector3.Distance(transform.position, manualNavPoint) > 10)
            {
                waitTimer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        ChangeState(State.backStep, MoveState.chase, AttackState.prepareToFire);
        waitTimer = 0; while (waitTimer < 5) { waitTimer += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        CannonShoot();
        ChangeState(State.idle, MoveState.chase, AttackState.idle);
        waitTimer = 0; while (waitTimer < 3) { waitTimer += Time.deltaTime; yield return new WaitForEndOfFrame(); }
        yield return null;
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
    IEnumerator IntroAnim()
    {
        //Setup for anim
        foreach (MonoBehaviour tilt in bha.tilts) { tilt.enabled = false; }
        foreach (Animator a in proceduralAnims) { a.enabled = false; }
        manualAnim.enabled = true;
        pauseUpdate = true;

        //Setup for combat
        foreach (MonoBehaviour tilt in bha.tilts) { tilt.enabled = true; }
        foreach (Animator a in proceduralAnims) { a.enabled = true; }
        manualAnim.enabled = false;
        pauseUpdate = false;
        yield return null;
    }
}
