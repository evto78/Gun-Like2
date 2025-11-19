using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCBrain : MonoBehaviour
{
    public List<Transform> wheels;
    Rigidbody rb; public float speed; public float turnSpeed; public float articifalCorrectionSpeed; float speedModifier;
    public ParticleSystem fixingEffect;
    Transform player; EnemyHealthManager ehm; float stuckTimer; bool fixing; float timeSinceCollision = 0f;
    void Start()
    {
        speedModifier = 1f;
        ehm = GetComponent<EnemyHealthManager>();
        player = ehm.gdm.phm.transform;
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(timeSinceCollision > 1 && collision.gameObject.TryGetComponent<HealthManager>(out HealthManager hm))
        {
            hm.TakeDamage(ehm.baseDamage * ehm.difficultyScale * ehm.difficultyStatScaling, false, ehm, ehm.data.enemyName, transform);
            timeSinceCollision = 0;
        }
    }
    void Update()
    {
        if (ehm.activeEffects[39].x > 0) { speedModifier = 0.5f / (1.5f * (1.1f * (ehm.playerHM.playerItem.leftItems[136] + ehm.playerHM.playerItem.rightItems[136]))); } else { speedModifier = 1f; }
        if (ehm.playerHM.activeEffects[22].x > 0) { return; }
        timeSinceCollision += Time.deltaTime;
        if (fixing) { return; }
        UpdateTireRoation();
        DetectAndDrive();
        if(rb.velocity.magnitude < 2) { stuckTimer += Time.deltaTime; } if(stuckTimer > 5) { StartCoroutine(ArtificalCorrectAngle()); }
    }
    IEnumerator ArtificalCorrectAngle()
    {
        fixing = true; rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; fixingEffect.Play();
        Vector3 desDir = new Vector3(0, transform.localEulerAngles.y, 0); Vector3 curDir = transform.localEulerAngles;
        Vector3 curPos = transform.position; Vector3 desPos = curPos + Vector3.up;
        float progression = 0;
        while(progression < 1)
        {
            fixing = true; rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero;
            progression += articifalCorrectionSpeed * Time.deltaTime;
            transform.localEulerAngles = Vector3.Lerp(curDir, desDir, progression);
            transform.position = Vector3.Lerp(curPos, desPos, progression);
            yield return new WaitForEndOfFrame();
        }
        fixingEffect.Stop();
        fixing = false;
        stuckTimer = -1f;
        yield return null;
    }
    void DetectAndDrive()
    {
        Vector3 myDir; Vector3 myVel; Vector3 desDir;
        myDir = transform.localEulerAngles; transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z)); desDir = transform.localEulerAngles; transform.localEulerAngles = myDir; myVel = rb.velocity;
        if(desDir.y - myDir.y > 5) { Turn(1); }
        else if(desDir.y - myDir.y < 5) { Turn(-1); }
        if(Vector3.Distance(transform.position + myVel, player.position) > Vector3.Distance(transform.position, player.position)) { Drive(2); }
        else { Drive(2); }
    }
    bool Grounded()
    {
        return (Physics.Raycast(transform.position+(transform.up*0.25f), -transform.up, 1f));
    }
    void Drive(int dir) // 1 fwd, -1 bck
    {
        if (!Grounded()) { return; }
        rb.AddForce(Time.deltaTime * dir * speed * transform.forward);
    }
    void Turn(int dir) // 1 right, -1 left
    {
        rb.AddRelativeTorque(Time.deltaTime * dir * turnSpeed * Vector3.up);
    }
    void UpdateTireRoation()
    {
        foreach(Transform tire in wheels)
        {
            tire.transform.Rotate(Time.deltaTime * rb.velocity.magnitude * 27f * speed * Vector3.right);
        }
    }
}
