using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 axisAndSpeed;
    void Update()
    {
        transform.localEulerAngles += axisAndSpeed * Time.deltaTime;
        Vector3 adjustmentAngle = Vector3.zero;
        if (transform.localEulerAngles.x > 360) { adjustmentAngle -= Vector3.right * 360f; }
        if (transform.localEulerAngles.y > 360) { adjustmentAngle -= Vector3.up * 360f; }
        if (transform.localEulerAngles.z > 360) { adjustmentAngle -= Vector3.forward * 360f; }
        if (transform.localEulerAngles.x < -360) { adjustmentAngle -= Vector3.right * -360f; }
        if (transform.localEulerAngles.y < -360) { adjustmentAngle -= Vector3.up * -360f; }
        if (transform.localEulerAngles.z < -360) { adjustmentAngle -= Vector3.forward * -360f; }
        transform.localEulerAngles += adjustmentAngle;
    }
}
