using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWPlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    // cam control variables;
    public float sensitivity;
    public GameObject cam;
    float yaw = 0.0f;
    float pitch = 0.0f;

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
    bool isSprinting;

    bool buttered = false;
    bool planeMode = false;
    float planeSpeed = 0.0f;
    bool hasBunny = false;
    int beltFed = 0;

    HealthManager healthMan;
    public PlayerItem playerItem;

    List<Vector4> effectList;

    // Start is called before the first frame update
    void Start()
    {
        jumpsLeft = numberOfJumps;
        rb = GetComponent<Rigidbody>();
        healthMan = GetComponent<HealthManager>();
        effectList = healthMan.activeEffects;
    }

    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
        baseMoveSpeed = 1000f;
        baseSprintMoveSpeed = baseMoveSpeed * 1.6f;
        baseJumpForce = 2000;
        baseNumberOfJumps = 1;

        moveSpeed = baseMoveSpeed;
        sprintMoveSpeed = baseSprintMoveSpeed;
        jumpForce = baseJumpForce;
        airStrafeSpeed = moveSpeed;
        maxSlideVelocity = moveSpeed * 3f;
        slideAccelerationRate = moveSpeed * 2f;
        numberOfJumps = baseNumberOfJumps;
        gravityModifier = 1f;

        //status effect buffs / debuffs
        if (effectList[10].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[17] + givenRightItems[17]) / 10f + 1f); }
        if (effectList[16].x > 0f) { moveSpeed = moveSpeed * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); }
        if (effectList[18].x > 0f) { moveSpeed = moveSpeed * 1.5f; }

        moveSpeed = Calc(-10f, givenLeftItems[20] + givenRightItems[20], moveSpeed);
        sprintMoveSpeed = Calc(10f, givenLeftItems[0] + givenRightItems[0], sprintMoveSpeed);
        jumpForce = Calc(10f, givenLeftItems[1] + givenRightItems[1], jumpForce);
        jumpForce = Calc(10f, givenLeftItems[20] + givenRightItems[20], jumpForce);
        jumpForce = Calc(-10f, givenLeftItems[23] + givenRightItems[23], jumpForce);
        numberOfJumps += givenLeftItems[15] + givenRightItems[15];
        numberOfJumps += givenLeftItems[31] + givenRightItems[31];
        numberOfJumps += givenLeftItems[32] + givenRightItems[32];
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
        if (effectList[16].x > 0f) { jumpForce = jumpForce * ((givenLeftItems[20] + givenRightItems[20]) / 2.5f + 1f); }
        if (effectList[17].x > 0f) { moveSpeed = moveSpeed + (moveSpeed * (effectList[17].x / 50)); sprintMoveSpeed = sprintMoveSpeed + (sprintMoveSpeed * (effectList[17].x / 50)); }
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
        onGround = GroundCheck();
        CameraMove();
        GetInputs();
        Move();
    }
    bool GroundCheck()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1.26f))
        {
            if(hit.transform.gameObject.tag == "Ground")
            {
                jumpsLeft = numberOfJumps;
                return true;
            }
        }

        return false;
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
    }
    void Move()
    {
        if (isSprinting)
        {
            if (Input.GetKey(KeyCode.W)) { rb.AddForce(transform.forward * sprintMoveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.S)) { rb.AddForce(-transform.forward * sprintMoveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.A)) { rb.AddForce(-transform.right * sprintMoveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.D)) { rb.AddForce(transform.right * sprintMoveSpeed * Time.deltaTime, ForceMode.Acceleration); }
        }
        else
        {
            if (Input.GetKey(KeyCode.W)) { rb.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.S)) { rb.AddForce(-transform.forward * moveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.A)) { rb.AddForce(-transform.right * moveSpeed * Time.deltaTime, ForceMode.Acceleration); }
            if (Input.GetKey(KeyCode.D)) { rb.AddForce(transform.right * moveSpeed * Time.deltaTime, ForceMode.Acceleration); }
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
    }
    void Jump()
    {
        if(jumpsLeft > 0)
        {
            jumpsLeft -= 1;
            rb.AddForce(transform.up * jumpForce, ForceMode.Force);
        }
    }
    void Slam()
    {

    }
    void Slide()
    {

    }
}
