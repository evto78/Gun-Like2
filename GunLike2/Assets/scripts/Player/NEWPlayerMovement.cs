using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWPlayerMovement : MonoBehaviour
{
    Rigidbody rb;

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

    HealthManager healthMan;
    public PlayerItem playerItem;

    List<Vector4> effectList;

    // Start is called before the first frame update
    void Start()
    {
        fov = Mathf.RoundToInt(cam.GetComponent<Camera>().fieldOfView);
        if (PlayerPrefs.HasKey("FOV")) { fov = PlayerPrefs.GetFloat("FOV"); }
        if (PlayerPrefs.HasKey("SENS")) { sensitivity = PlayerPrefs.GetFloat("SENS") / 10f; }
        if (PlayerPrefs.HasKey("FPS")) { fps = PlayerPrefs.GetFloat("FPS"); } else { fps = 120; }
        Application.targetFrameRate = Mathf.RoundToInt(fps);
        jumpsLeft = numberOfJumps;
        rb = GetComponent<Rigidbody>();
        healthMan = GetComponent<HealthManager>();
        effectList = healthMan.activeEffects;
        sliding = false;
        slamming = false;

        initialHeadHeight = head.transform.localPosition.y;
        initialHeight = myCollider.height;
        slideDir = Vector3.zero;
    }

    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
        baseMoveSpeed = 600f;
        baseSprintMoveSpeed = baseMoveSpeed * 1.6f;
        baseJumpForce = 2000f;
        baseNumberOfJumps = 1;

        moveSpeed = baseMoveSpeed;
        sprintMoveSpeed = baseSprintMoveSpeed;
        jumpForce = baseJumpForce;
        airStrafeSpeed = moveSpeed * 0.5f;
        maxSlideVelocity = moveSpeed * 3f;
        slideAccelerationRate = sprintMoveSpeed * 1.5f;
        numberOfJumps = baseNumberOfJumps;
        gravityModifier = 1f;

        //status effect buffs / debuffs
        if (effectList[10].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[16].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); }
        if (effectList[18].x > 0f) { moveSpeed = moveSpeed * 1.5f; }

        moveSpeed = Calc(-20f, givenLeftItems[20] + givenRightItems[20], moveSpeed);
        moveSpeed = Calc(-20f, givenLeftItems[61] + givenRightItems[61], moveSpeed);
        moveSpeed = Calc(20f, givenLeftItems[59] + givenRightItems[59], moveSpeed);
        moveSpeed = Calc(20f, givenLeftItems[73] + givenRightItems[73], moveSpeed);
        sprintMoveSpeed = Calc(20f, givenLeftItems[0] + givenRightItems[0], sprintMoveSpeed);
        sprintMoveSpeed = Calc(40f, givenLeftItems[59] + givenRightItems[59], sprintMoveSpeed);
        jumpForce = Calc(20f, givenLeftItems[1] + givenRightItems[1], jumpForce);
        jumpForce = Calc(20f, givenLeftItems[20] + givenRightItems[20], jumpForce);
        jumpForce = Calc(20f, givenLeftItems[59] + givenRightItems[59], jumpForce);
        jumpForce = Calc(-20f, givenLeftItems[23] + givenRightItems[23], jumpForce);
        numberOfJumps += givenLeftItems[15] + givenRightItems[15];
        numberOfJumps += givenLeftItems[31] + givenRightItems[31];
        numberOfJumps += givenLeftItems[32] + givenRightItems[32];
        numberOfJumps += (givenLeftItems[46]*2) + (givenRightItems[46]*2);
        gravityModifier = Calc(-10f, givenLeftItems[15] + givenRightItems[15], gravityModifier);

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
        if (beltFed > 0) { moveSpeed = moveSpeed / 2f; sprintMoveSpeed = sprintMoveSpeed / 2f; }

        //Irradiated French Pastry
        if (givenLeftItems[22] > 0)
        {
            if (playerItem.leftIFPStatToBuff == 0) { moveSpeed = moveSpeed * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 1) { sprintMoveSpeed = sprintMoveSpeed * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 2) { jumpForce = jumpForce * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 3) { numberOfJumps = Mathf.FloorToInt(numberOfJumps * (givenLeftItems[22] * 2)); }

            if (playerItem.leftIFPStatToDeBuff == 0) { moveSpeed = moveSpeed * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 1) { sprintMoveSpeed = sprintMoveSpeed * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 2) { jumpForce = jumpForce * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 3) { numberOfJumps = Mathf.FloorToInt(numberOfJumps * (0.9f / givenLeftItems[22])); }
        }
        if (givenRightItems[22] > 0)
        {
            if (playerItem.rightIFPStatToBuff == 0) { moveSpeed = moveSpeed * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 1) { sprintMoveSpeed = sprintMoveSpeed * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 2) { jumpForce = jumpForce * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 3) { numberOfJumps = Mathf.FloorToInt(numberOfJumps * (givenRightItems[22] * 2)); }

            if (playerItem.rightIFPStatToDeBuff == 0) { moveSpeed = moveSpeed * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 1) { sprintMoveSpeed = sprintMoveSpeed * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 2) { jumpForce = jumpForce * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 3) { numberOfJumps = Mathf.FloorToInt(numberOfJumps * (0.9f / givenRightItems[22])); }
        }

        //status effect buffs / debuffs

        if (effectList[9].x > 0f) { jumpForce = jumpForce * ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[15].x > 0f) { sprintMoveSpeed = sprintMoveSpeed / ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[16].x > 0f) { jumpForce = jumpForce * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); moveSpeed = moveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 10f + 1f); sprintMoveSpeed = sprintMoveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 10f + 1f); }
        if (effectList[17].x > 0f) { moveSpeed = moveSpeed + (moveSpeed * (effectList[17].x / 50)); sprintMoveSpeed = sprintMoveSpeed + (sprintMoveSpeed * (effectList[17].x / 50)); }

        //stat caps
        if(moveSpeed > baseMoveSpeed * 10) { moveSpeed = baseMoveSpeed * 10; }
        if(jumpForce > baseJumpForce * 10) { jumpForce = baseJumpForce * 10; }
    }

    float Calc(float modifier, int amount, float baseVal)
    {
        float result = baseVal;

        if (amount <= 0) { return result; }

        if (modifier > 0)
        {
            //Buff

            for (int i = 0; i <= amount; i++)
            {
                result = result + result * (modifier / 100);
            }
        }
        else if (modifier < 0)
        {
            //Debuff
            modifier = modifier * -1f;

            for (int i = 0; i <= amount; i++)
            {
                result = result - result * (modifier / 100);
            }
        }

        if (Mathf.FloorToInt(result) <= 0)
        {
            result = Mathf.CeilToInt(result);
        }

        return result;
    }
    // Update is called once per frame
    void Update()
    {
        timeSinceGrounded += Time.deltaTime;
        if (healthMan.dead) { rb.freezeRotation = false; rb.AddRelativeTorque(Vector3.one * 3f); return; }

        onGround = GroundCheck();
        if (onGround) { slamming = false; }
        if (Cursor.lockState == CursorLockMode.Locked) { CameraMove(); }
        GetInputs();

        Effects();
    }
    private void FixedUpdate()
    {
        if (healthMan.dead) { return; }
        if (Cursor.lockState == CursorLockMode.Locked) { Move(); } else { Friction(); }
    }
    bool GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y - 0f, transform.position.z), transform.localScale * 0.5f, -Vector3.up, out RaycastHit hit, transform.rotation, 1f))
        {
            if (hit.transform.gameObject.tag == "Ground")
            {
                if (hasBunny && !onGround) { healthMan.GiveEffect("bunny hop buff", 1f); }
                jumpsLeft = numberOfJumps;
                timeSinceGrounded = 0f;
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
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            if (isSprinting) { isSprinting = false; } else { isSprinting = true; }
        } 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (planeMode)
            {
                rb.AddRelativeForce(Vector3.up * planeSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!onGround && !sliding)
            {
                if(Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Slam();
                }
            }
            else
            {
                if(slidingMod > 0)
                {
                    if (slideDir == Vector3.zero) { slideDir = transform.forward; }
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
        if (Input.GetKey(KeyCode.W)) { inputDir = inputDir + Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { inputDir = inputDir - Vector3.forward; }
        if (Input.GetKey(KeyCode.A)) { inputDir = inputDir - Vector3.right; }
        if (Input.GetKey(KeyCode.D)) { inputDir = inputDir + Vector3.right; }
        inputDir = Vector3.Normalize(inputDir);
        //Debug.Log(inputDir);

    }
    void Move()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        if (sliding)
        {
            if(rb.velocity.y > 0)
            {
                rb.AddForce(slideDir * slideAccelerationRate * slidingMod * Time.deltaTime, ForceMode.Impulse);
                if (buttered) { slidingMod -= Time.fixedDeltaTime * 0.8f; } else { slidingMod -= Time.fixedDeltaTime * 1.5f; }
            }
            else if (rb.velocity.y < 0)
            {
                rb.AddForce(slideDir * slideAccelerationRate * slidingMod * Time.deltaTime, ForceMode.Impulse);
                if (buttered) { slidingMod += Time.fixedDeltaTime; } else { slidingMod += Time.fixedDeltaTime * 0.5f; }
            }
            else if (rb.velocity.y <= 0)
            {
                rb.AddForce(slideDir * slideAccelerationRate * slidingMod * Time.deltaTime, ForceMode.Impulse);
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
        else if (onGround)
        {
            rb.AddRelativeForce(inputDir * airStrafeSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else if (isSprinting)
        {
            rb.AddRelativeForce(inputDir * sprintMoveSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            rb.AddRelativeForce(inputDir * moveSpeed * Time.deltaTime, ForceMode.Impulse);
        }

        //Limit Velocity realative to speed
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
                healthMan.TakeDamage(healthMan.curHp / 10f, false);
            }

            jumpsLeft -= 1;
            rb.AddForce(transform.up * jumpForce, ForceMode.Force);
            if (slamming)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            }
            if (slamming) { rb.AddForce(transform.up * jumpForce, ForceMode.Force); }
            slamming = false;
        }
    }
    void Slam()
    {
        slamming = true;
        rb.velocity = new Vector3(rb.velocity.x, -50f, rb.velocity.z);
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
        if(rb.velocity.magnitude > 10)
        {
            float t = rb.velocity.magnitude / maxVelFov;
            Camera.main.fieldOfView = Mathf.Lerp(fov, fov + 10f, t);
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
            if(inputDir == Vector3.zero && !sliding) { flatVel = flatVel / (friction * (1 + Time.deltaTime)); }
            flatVel = flatVel / (friction * (1 + Time.deltaTime));
            rb.velocity = new Vector3(flatVel.x, rb.velocity.y, flatVel.z);
        }
    }
}
