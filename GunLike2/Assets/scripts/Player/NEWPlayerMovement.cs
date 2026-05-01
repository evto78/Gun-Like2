using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWPlayerMovement : MonoBehaviour
{
    public Rigidbody rb; bool noGravity;

    // Particle Effects
    public GameObject slamEffect;
    public GameObject slideEffect;
    public GameObject butterSlideEffect;
    public ParticleSystem snailEffect;

    // cam control variables
    public float sensitivity;
    public GameObject cam;
    float yaw = 0.0f;
    float pitch = 0.0f;
    float fov;
    float fps;
    bool dfov;
    //public float minVelFov;
    public float maxVelFov;

    public Transform head;
    float initialHeadHeight;
    public CapsuleCollider myCollider;
    float initialHeight;
    Vector3 slideDir;

    public float friction;

    public float baseMoveSpeed;
    public float baseSprintMoveSpeed;
    public float baseJumpForce;
    public int baseNumberOfJumps;

    public float moveSpeed;
    public float sprintMoveSpeed;
    public float jumpForce;
    public float airStrafeSpeed;
    public float maxSlideVelocity;
    public float slideAccelerationRate;
    public int numberOfJumps;
    int jumpsLeft;
    public float gravityModifier;

    bool onGround;
    public bool isSprinting;
    public bool slamming;
    public bool sliding;
    Vector3 inputDir;
    public float timeSinceGrounded;

    float slidingMod;

    bool buttered = false;
    bool planeMode = false;
    float planeSpeed = 0.0f;
    bool hasBunny = false;
    int beltFed = 0;
    bool hasFightingWings;
    public int wornOutWhip;

    HealthManager healthMan;
    GunManager gunMan;
    GameDataManager gdm;
    public PlayerItem playerItem;
    public GameObject shockwave;

    bool hscSpawned;

    List<Vector4> effectList;
    private void Awake()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
    }
    void Start()
    {
        fov = Mathf.RoundToInt(cam.GetComponent<Camera>().fieldOfView);
        dfov = PlayerPrefs.GetInt("DFOV") == 1;
        if (PlayerPrefs.HasKey("FOV")) { fov = PlayerPrefs.GetFloat("FOV"); }
        if (PlayerPrefs.HasKey("SENS")) { sensitivity = PlayerPrefs.GetFloat("SENS") / 10f; }
        if (PlayerPrefs.HasKey("FPS")) { fps = PlayerPrefs.GetFloat("FPS"); } else { fps = 120; }
        Application.targetFrameRate = Mathf.RoundToInt(fps);
        jumpsLeft = numberOfJumps;
        rb = GetComponent<Rigidbody>();
        healthMan = GetComponent<HealthManager>();
        gunMan = GetComponent<GunManager>();
        effectList = healthMan.activeEffects;
        sliding = false;
        slamming = false;

        initialHeadHeight = head.transform.localPosition.y;
        initialHeight = myCollider.height;
        slideDir = Vector3.zero;
    }
    public void UpdateSettings()
    {
        dfov = PlayerPrefs.GetInt("DFOV") == 1;
        if (PlayerPrefs.HasKey("FOV")) { fov = PlayerPrefs.GetFloat("FOV"); }
        if (PlayerPrefs.HasKey("SENS")) { sensitivity = PlayerPrefs.GetFloat("SENS") / 10f; }
        if (PlayerPrefs.HasKey("FPS")) { fps = PlayerPrefs.GetFloat("FPS"); } else { fps = 120; }
        Application.targetFrameRate = Mathf.RoundToInt(fps);
    }
    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
        effectList = healthMan.activeEffects;

        //base stats
        noGravity = false;
        float rightmoveSpeedMult = 1f; float rightmoveSpeedDiv = 1f; float leftmoveSpeedMult = 1f; float leftmoveSpeedDiv = 1f;
        float sprintMoveSpeedMult = 1f; float sprintMoveSpeedDiv = 1f;
        float jumpForceMult = 1f; float jumpForceDiv = 1f;
        float gravityMult = 1f; float gravityDiv = 1f;

        baseMoveSpeed = 800f;
        baseSprintMoveSpeed = baseMoveSpeed * 1.4f;
        baseJumpForce = 60f;
        baseNumberOfJumps = 1;

        moveSpeed = baseMoveSpeed;
        sprintMoveSpeed = baseSprintMoveSpeed;
        jumpForce = baseJumpForce;
        airStrafeSpeed = moveSpeed * 0.3f;
        maxSlideVelocity = sprintMoveSpeed * 1.4f;
        slideAccelerationRate = sprintMoveSpeed * 1.4f;
        numberOfJumps = baseNumberOfJumps;
        gravityModifier = 1f;

        //status effect buffs / debuffs
        if (effectList[10].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[16].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); }
        if (effectList[18].x > 0f) { moveSpeed = moveSpeed * 1.5f; }
        if (effectList[26].x > 0f) { moveSpeed = moveSpeed * 2f; }
        if (effectList[33].x > 0f) { moveSpeed /= 2f; sprintMoveSpeed /= 2f; }

        //Left Simple Stats
        for (int i = 0; i < givenLeftItems.Count; i++)
        {
            foreach (ItemObject.StatData statData in playerItem.itemData[i].statData)
            {
                if (statData.change > 0)
                {
                    switch (statData.stat)
                    {
                        case ItemObject.StatData.Stat.JumpCount: numberOfJumps += (int)statData.change * givenLeftItems[i]; break;
                        case ItemObject.StatData.Stat.JumpHeight: jumpForceMult += MultAdder(statData.change, givenLeftItems[i]); break;
                        case ItemObject.StatData.Stat.Speed: leftmoveSpeedMult += MultAdder(statData.change, givenLeftItems[i]); break;
                        case ItemObject.StatData.Stat.SprintSpeed: sprintMoveSpeedMult += MultAdder(statData.change, givenLeftItems[i]); break;
                    }
                }
                else
                {
                    switch (statData.stat)
                    {
                        case ItemObject.StatData.Stat.JumpCount: numberOfJumps += (int)statData.change * givenLeftItems[i]; break;
                        case ItemObject.StatData.Stat.JumpHeight: jumpForceDiv += MultAdder(statData.change, givenLeftItems[i]); break;
                        case ItemObject.StatData.Stat.Speed: leftmoveSpeedDiv += MultAdder(statData.change, givenLeftItems[i]); break;
                        case ItemObject.StatData.Stat.SprintSpeed: sprintMoveSpeedDiv += MultAdder(statData.change, givenLeftItems[i]); break;
                    }
                }
            }
        }
        //Right Simple Stats
        for (int i = 0; i < givenRightItems.Count; i++)
        {
            foreach (ItemObject.StatData statData in playerItem.itemData[i].statData)
            {
                if (statData.change > 0)
                {
                    switch (statData.stat)
                    {
                        case ItemObject.StatData.Stat.JumpCount: numberOfJumps += (int)statData.change * givenRightItems[i]; break;
                        case ItemObject.StatData.Stat.JumpHeight: jumpForceMult += MultAdder(statData.change, givenRightItems[i]); break;
                        case ItemObject.StatData.Stat.Speed: rightmoveSpeedMult += MultAdder(statData.change, givenRightItems[i]); break;
                        case ItemObject.StatData.Stat.SprintSpeed: sprintMoveSpeedMult += MultAdder(statData.change, givenRightItems[i]); break;
                    }
                }
                else
                {
                    switch (statData.stat)
                    {
                        case ItemObject.StatData.Stat.JumpCount: numberOfJumps += (int)statData.change * givenRightItems[i]; break;
                        case ItemObject.StatData.Stat.JumpHeight: jumpForceDiv += MultAdder(statData.change, givenRightItems[i]); break;
                        case ItemObject.StatData.Stat.Speed: rightmoveSpeedDiv += MultAdder(statData.change, givenRightItems[i]); break;
                        case ItemObject.StatData.Stat.SprintSpeed: sprintMoveSpeedDiv += MultAdder(statData.change, givenRightItems[i]); break;
                    }
                }
            }
        }
        
        //Gravity
        gravityDiv += MultAdder(-10f, givenLeftItems[15] + givenRightItems[15]);
        //Partial Intagability
        healthMan.evadeChance = 0f;
        if (givenLeftItems[73] > 0) { healthMan.evadeChance += (leftmoveSpeedMult*100f) / 2f; leftmoveSpeedMult /= 2f; }
        if (givenRightItems[73] > 0) { healthMan.evadeChance += (rightmoveSpeedMult*100f) / 2f; rightmoveSpeedMult /= 2f; }
        //Apply mult
        moveSpeed *= leftmoveSpeedMult+rightmoveSpeedMult; moveSpeed /= leftmoveSpeedDiv+rightmoveSpeedDiv;
        sprintMoveSpeed *= sprintMoveSpeedMult; sprintMoveSpeed /= sprintMoveSpeedDiv;
        jumpForce *= jumpForceMult; jumpForce /= jumpForceDiv;
        gravityModifier *= gravityMult; gravityModifier /= gravityDiv;
        //No Gravity Check
        if (healthMan.gdm.mutatedRules.Contains(11)) { moveSpeed /= 4f; sprintMoveSpeed /= 4f; rb.drag = 0.5f; } else { rb.drag = 0.2f; }
        //Item Checks
        if ((givenLeftItems[3] > 0) || (givenRightItems[3] > 0))
        {
            maxSlideVelocity = maxSlideVelocity * 1.5f;
            slideAccelerationRate = slideAccelerationRate * 1.5f;
            buttered = true;
        }
        else
        {
            buttered = false;
        }
        if ((givenLeftItems[5] > 0) || (givenRightItems[5] > 0))
        {
            planeMode = true;
            planeSpeed = (givenRightItems[5] / 5f + 1f) * sprintMoveSpeed / 5f;
        }
        else
        {
            planeMode = false;
        }
        if ((givenLeftItems[20] > 0) || (givenRightItems[20] > 0))
        {
            hasBunny = true;
        }
        else
        {
            hasBunny = false;
        }
        hasFightingWings = (givenLeftItems[46] + givenRightItems[46] > 0);

        beltFed = 0 + givenLeftItems[29] + givenRightItems[29];

        //Irradiated French Pastry
        if (givenLeftItems[22] > 0)
        {
            switch (playerItem.leftIFPStatToBuff)
            {
                case 0: moveSpeed = moveSpeed * (givenLeftItems[22] * 2); break;
                case 1: sprintMoveSpeed = sprintMoveSpeed * (givenLeftItems[22] * 2); break;
                case 2: jumpForce = jumpForce * (givenLeftItems[22] * 2); break;
                case 3: numberOfJumps = Mathf.FloorToInt(numberOfJumps * (givenLeftItems[22] * 2)); break;
            }
            switch (playerItem.leftIFPStatToDeBuff)
            {
                case 0: moveSpeed = moveSpeed * (0.9f / givenLeftItems[22]); break;
                case 1: sprintMoveSpeed = sprintMoveSpeed * (0.9f / givenLeftItems[22]); break;
                case 2: jumpForce = jumpForce * (0.9f / givenLeftItems[22]); break;
                case 3: numberOfJumps = Mathf.FloorToInt(numberOfJumps * (0.9f / givenLeftItems[22])); break;
            }
        }
        if (givenRightItems[22] > 0)
        {
            switch (playerItem.rightIFPStatToBuff)
            {
                case 0: moveSpeed = moveSpeed * (givenRightItems[22] * 2); break;
                case 1: sprintMoveSpeed = sprintMoveSpeed * (givenRightItems[22] * 2); break;
                case 2: jumpForce = jumpForce * (givenRightItems[22] * 2); break;
                case 3: numberOfJumps = Mathf.FloorToInt(numberOfJumps * (givenRightItems[22] * 2)); break;
            }
            switch (playerItem.rightIFPStatToDeBuff)
            {
                case 0: moveSpeed = moveSpeed * (0.9f / givenRightItems[22]); break;
                case 1: sprintMoveSpeed = sprintMoveSpeed * (0.9f / givenRightItems[22]); break;
                case 2: jumpForce = jumpForce * (0.9f / givenRightItems[22]); break;
                case 3: numberOfJumps = Mathf.FloorToInt(numberOfJumps * (0.9f / givenRightItems[22])); break;
            }
        }
        //Mutated Rules Modifiers
        moveSpeed *= healthMan.gdm.mutatedStatModifiers[0];
        sprintMoveSpeed *= healthMan.gdm.mutatedStatModifiers[1];
        jumpForce *= healthMan.gdm.mutatedStatModifiers[2];
        numberOfJumps = Mathf.CeilToInt(numberOfJumps * healthMan.gdm.mutatedStatModifiers[3]);
        if (healthMan.gdm.mutatedRules.Contains(11)) { rb.useGravity = false; noGravity = true; }

        //status effect buffs / debuffs
        if (effectList[9].x > 0f) { jumpForce = jumpForce * ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[15].x > 0f) { sprintMoveSpeed = sprintMoveSpeed / ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[16].x > 0f) { jumpForce = jumpForce * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); moveSpeed = moveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 10f + 1f); sprintMoveSpeed = sprintMoveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 10f + 1f); }
        if (effectList[17].x > 0f) { moveSpeed = moveSpeed + (moveSpeed * (effectList[17].x / 50)); sprintMoveSpeed = sprintMoveSpeed + (sprintMoveSpeed * (effectList[17].x / 50)); }

        //stat caps
        if(moveSpeed > baseMoveSpeed * 5) { moveSpeed = baseMoveSpeed * 5; }
        if(sprintMoveSpeed > baseSprintMoveSpeed * 5) { sprintMoveSpeed = baseSprintMoveSpeed * 5; }
        if(jumpForce > baseJumpForce * 5) { jumpForce = baseJumpForce * 5; }
    }
    float MultAdder(float mult, int amount)
    {
        if (mult >= 0) { return mult * (1f / 100f) * amount; }
        else { return -mult * (1f / 100f) * amount; }
    }
    // Update is called once per frame
    void Update()
    {
        timeSinceGrounded += Time.deltaTime;
        if (healthMan.dead) { rb.freezeRotation = false; rb.AddRelativeTorque(Vector3.one * 3f); return; }

        onGround = GroundCheck();
        if (onGround) { slamming = false; gdm.timeOnGroundThisRoom += Time.deltaTime; } else { gdm.timeInAirThisRoom += Time.deltaTime; }
        if (Cursor.lockState == CursorLockMode.Locked) { CameraMove(); }
        GetInputs();

        if (!onGround && !hscSpawned)
        {
            if (transform.position.y > 200)
            {
                playerItem.SpawnItem(188, true, 8, true);
                hscSpawned = true;
            }
        }

        Effects();
    }
    Vector3 prevFlatVel = new Vector3();
    bool blockSlide;
    private void FixedUpdate()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        onGround = GroundCheck();

        if (!onGround && !noGravity) 
        {
            if (playerItem.leftItems[93] + playerItem.rightItems[93] > 0)
            {
                rb.AddForce(10 * Time.fixedDeltaTime * gravityModifier * ((timeSinceGrounded * 0.5f) + 1) * ((timeSinceGrounded * (0.8f * (playerItem.leftItems[93] + playerItem.rightItems[93]))) + 1) * -Vector3.up);
            }
            else
            {
                rb.AddForce(10 * Time.fixedDeltaTime * gravityModifier * ((timeSinceGrounded * 0.5f) + 1) * -Vector3.up);
            }
        }
        if (healthMan.dead) { return; }
        if (flatVel.magnitude - prevFlatVel.magnitude < -10 && sliding) { blockSlide = true; }
        if (Cursor.lockState == CursorLockMode.Locked) { Move(); } else { Friction(); }
        prevFlatVel = flatVel;
    }
    bool GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - 0f, transform.position.z), transform.localScale * 0.5f, -Vector3.up, out RaycastHit hit, transform.rotation, 1f))
        {
            if (hit.transform.gameObject.CompareTag("Ground") || hit.transform.gameObject.CompareTag("Untagged"))
            {
                if (hasBunny && !onGround) { healthMan.GiveEffect(16, 1f); }
                jumpsLeft = numberOfJumps;
                timeSinceGrounded = 0f;
                if(playerItem.leftItems[93] + playerItem.rightItems[93] > 0 && rb.velocity.y < -25)
                {
                    GameObject spawnedShockwave = Instantiate(shockwave);
                    spawnedShockwave.transform.position = transform.position;
                    spawnedShockwave.GetComponent<Shockwave>().lifetime = 0.5f * (rb.velocity.y / -25f);
                    spawnedShockwave.GetComponent<Shockwave>().damage = 25f * (rb.velocity.y / -25f);
                }
                return true;
            }
        }

        return false;
    }
    private void OnDrawGizmos()
    {
        if (onGround) { Gizmos.color = Color.green; }
        else { Gizmos.color = Color.red; }
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), transform.localScale * 0.5f);
    }
    void CameraMove()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //get mouse input
            yaw += sensitivity * Input.GetAxis("Mouse X");
            pitch -= sensitivity * Input.GetAxis("Mouse Y");

            //limit cam angle
            pitch = Mathf.Clamp(pitch, -85.0f, 85.0f);

            //set cam angle
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yaw, transform.eulerAngles.z);
            cam.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
    void GetInputs()
    {
        if (Input.GetKeyDown(healthMan.gdm.instance.controlsBinds.sprint)) 
        {
            if (isSprinting) { isSprinting = false; } else { isSprinting = true; }
        } 
        if (Input.GetKeyDown(healthMan.gdm.instance.controlsBinds.jump))
        {
            Jump();
        }
        if (Input.GetKey(healthMan.gdm.instance.controlsBinds.jump))
        {
            if (planeMode)
            {
                rb.AddRelativeForce(Vector3.up * planeSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKeyDown(healthMan.gdm.instance.controlsBinds.slam))
        {
            if (!onGround && !sliding)
            {
                Slam();
            }
        }
        if (Input.GetKeyUp(healthMan.gdm.instance.controlsBinds.slide))
        {
            blockSlide = false;
        }
        if ((Input.GetKey(healthMan.gdm.instance.controlsBinds.slide) && !blockSlide) || Input.GetKeyDown(healthMan.gdm.instance.controlsBinds.slide))
        {
            if(onGround || sliding)
            {
                if(slidingMod > 0)
                {
                    if (slideDir == Vector3.zero) { slideDir = transform.forward - (Vector3.up * 0.05f); }
                    Slide();
                }
            }
        }
        else
        {
            if(sliding)
            {
                myCollider.height = initialHeight;
                head.transform.localPosition = Vector3.up * initialHeadHeight;
            }
            slideDir = Vector3.zero;
            sliding = false;
            slidingMod = 2f;
        }
        inputDir = Vector3.zero;
        if (healthMan.gdm.mutatedRules.Contains(11) && !onGround) { return; }
        if (Input.GetKey(healthMan.gdm.instance.controlsBinds.walkForward)) { inputDir = inputDir + Vector3.forward; }
        if (Input.GetKey(healthMan.gdm.instance.controlsBinds.walkBackward)) { inputDir = inputDir - Vector3.forward; }
        if (Input.GetKey(healthMan.gdm.instance.controlsBinds.walkLeft)) { inputDir = inputDir - Vector3.right; }
        if (Input.GetKey(healthMan.gdm.instance.controlsBinds.walkRight)) { inputDir = inputDir + Vector3.right; }
        inputDir = Vector3.Normalize(inputDir);
    }
    void Move()
    {
        float universalSpeedBuff = 2f; //To compisate with changing time.deltatime to time.fixeddeltatime
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (sliding)
        {
            if(rb.velocity.y > 0 && !onGround)
            {
                rb.AddForce(universalSpeedBuff * slideAccelerationRate * slidingMod * Time.fixedDeltaTime * slideDir, ForceMode.Impulse);
                if (buttered) { slidingMod -= Time.fixedDeltaTime * 0.8f; } else { slidingMod -= Time.fixedDeltaTime * 1.5f; }
            }
            else if(rb.velocity.y > 0 && onGround)
            {
                rb.AddForce(universalSpeedBuff * slideAccelerationRate * slidingMod * Time.fixedDeltaTime * slideDir, ForceMode.Impulse);
                if (buttered) { slidingMod -= Time.fixedDeltaTime * 1.2f; } else { slidingMod -= Time.fixedDeltaTime * 2.5f; }
            }
            else if (rb.velocity.y <= -0.5f)
            {
                rb.AddForce(universalSpeedBuff * slideAccelerationRate * slidingMod * Time.fixedDeltaTime * slideDir, ForceMode.Impulse);
                if (buttered) { slidingMod += Time.fixedDeltaTime; } else { slidingMod += Time.fixedDeltaTime * 0.5f; }
            }
            else 
            {
                rb.AddForce(universalSpeedBuff * slideAccelerationRate * slidingMod * Time.fixedDeltaTime * slideDir, ForceMode.Impulse);
                if (buttered) { slidingMod -= Time.fixedDeltaTime * 0.2f; } else { slidingMod -= Time.fixedDeltaTime; }
            }
            if(slidingMod <= 0)
            {
                slidingMod = 0;
                sliding = false;
                myCollider.height = initialHeight;
                head.transform.localPosition = Vector3.up * initialHeadHeight;
            }
            if(slidingMod > 5f) { slidingMod = 5f; }
            
        }
        else if (!onGround)
        {
            rb.AddRelativeForce(universalSpeedBuff * airStrafeSpeed * Time.fixedDeltaTime * inputDir, ForceMode.Impulse);
        }
        else if (isSprinting)
        {
            rb.AddRelativeForce(universalSpeedBuff * sprintMoveSpeed * Time.fixedDeltaTime * inputDir, ForceMode.Impulse);
        }
        else
        {
            rb.AddRelativeForce(universalSpeedBuff * moveSpeed * Time.fixedDeltaTime * inputDir, ForceMode.Impulse);
        }

        //Limit Velocity realative to speed
        if (noGravity) { return; }
        Vector3 limitedVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (isSprinting)
        {
            if(limitedVelocity.magnitude >= sprintMoveSpeed / 100f)
            {
                limitedVelocity = Vector3.Normalize(limitedVelocity);
                limitedVelocity = limitedVelocity * sprintMoveSpeed / 100f;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
        else
        {
            if (limitedVelocity.magnitude >= moveSpeed / 100f)
            {
                limitedVelocity = Vector3.Normalize(limitedVelocity);
                limitedVelocity = limitedVelocity * moveSpeed / 100f;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        Friction();
    }
    void Jump()
    {
        if (jumpsLeft > 0 || hasFightingWings)
        {
            if(hasFightingWings && jumpsLeft <= 0)
            {
                healthMan.TakeDamage(healthMan.curHp / 10f, false, null, "Fighting Wings", null);
            }

            jumpsLeft -= 1;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            if (slamming)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
            if (slamming) { rb.AddForce(transform.up * jumpForce, ForceMode.Force); }
            slamming = false;

            if (playerItem.leftItems[144] + playerItem.rightItems[144] > 0)
            {
                GameObject spawnedShockwave = Instantiate(shockwave);
                spawnedShockwave.transform.position = transform.position;
                spawnedShockwave.GetComponent<Shockwave>().damage = 0f;
                spawnedShockwave.GetComponent<Shockwave>().lifetime = 0.25f;
                if (playerItem.leftItems[144] > 0) { spawnedShockwave.GetComponent<Shockwave>().damage += gunMan.leftGunScript.dmg * playerItem.leftItems[144]; }
                if (playerItem.rightItems[144] > 0) { spawnedShockwave.GetComponent<Shockwave>().damage += gunMan.rightGunScript.dmg * playerItem.rightItems[144]; }
            }
        }
    }
    void Slam()
    {
        slamming = true;
        if (rb.velocity.y > -50f) { rb.velocity = new Vector3(rb.velocity.x, -50f, rb.velocity.z); }
    }
    void Slide()
    {

        sliding = true;
        myCollider.height = initialHeight / 2f;
        head.transform.localPosition = Vector3.zero;
    }
    void Effects()
    {
        //camera effects
        if(dfov)
        {
            float camFOVTar = Camera.main.fieldOfView;
            float t = (rb.velocity.magnitude-10f) / maxVelFov;
            camFOVTar = Mathf.Lerp(fov, fov + 10f, t);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, camFOVTar, Time.deltaTime*50f);
        }

        //particle effects
        slamEffect.SetActive(slamming);
        if (slamEffect.activeSelf) { slamEffect.GetComponent<ParticleSystem>().Play(); }
        if (buttered)
        {
            slideEffect.SetActive(false);
            butterSlideEffect.SetActive(sliding);
            if (butterSlideEffect.activeSelf) { slideEffect.GetComponent<ParticleSystem>().Play(); }
        }
        else
        {
            slideEffect.SetActive(sliding);
            if (slideEffect.activeSelf) { slideEffect.GetComponent<ParticleSystem>().Play(); }
        }
        if(playerItem.leftItems[61] + playerItem.rightItems[61] > 0)
        {
            snailEffect.Play();
        }
        else
        {
            snailEffect.Pause();
        }
    }

    void Friction()
    {
        if (onGround)
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if(inputDir == Vector3.zero && !sliding) { flatVel /= (friction * 4 * (1+Time.fixedDeltaTime)); rb.useGravity = false; } else { rb.useGravity = true; }
            flatVel /= (friction * (1 + Time.fixedDeltaTime));
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y / (1+Time.fixedDeltaTime), flatVel.z); 
        } else if (!noGravity) { rb.useGravity = true; }
    }
}
