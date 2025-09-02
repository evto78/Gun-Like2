using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    EnemyHealthManager ehm;
    public IKFootSolver pairedLeg;
    public Transform hip;
    public float maxStepHeight;
    public float stepHeight;
    public float stepLength;
    public LayerMask terrainLayer;
    public bool stay;
    public bool stepping;
    float stepProgress;
    public float stepSpeed;
    Vector3 stayPos;
    public Vector3 nextPos;
    void Start()
    {
        SnapToGround();
        stay = true;
        stepping = false;

        ehm = GetComponentInParent<EnemyHealthManager>();
    }
    void Update()
    {
        if (!stepping)
        {
            if (stay)
            {
                transform.position = stayPos;
            }
            else
            {
                SnapToGround();
            }
            Ray ray = new Ray(hip.position + hip.up * stepLength, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit info, maxStepHeight * 2f, terrainLayer.value))
            {
                nextPos = info.point;
                Debug.DrawRay(info.point, Vector3.up);
            }
            if (Vector3.Distance(transform.position, nextPos) > stepLength * 1.5f)
            {
                if (!pairedLeg.stepping)
                {
                    stay = false;
                    stepping = true;
                    stepProgress = 0;
                }
            }
        }
        else
        {
            stepProgress += Time.deltaTime * stepSpeed;
            transform.position = Vector3.Lerp(stayPos, nextPos, stepProgress);
            transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(stepProgress * Mathf.PI) * stepHeight, transform.position.z);
            if(stepProgress >= 1)
            {
                SnapToGround();
                stay = true;
                stepping = false;
                if(ehm != null) { if(ehm.data.enemyName == "Uzi Walker" || ehm.data.enemyName == "Grenade Lobber") { ehm.PlaySound(0, false, true); } }
            }
        }
    }
    void SnapToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * maxStepHeight, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit info, maxStepHeight*2f, terrainLayer.value))
        {
            transform.position = new Vector3(transform.position.x, info.point.y, transform.position.z);
            stayPos = transform.position;
        }
    }
}
