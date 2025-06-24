using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnobSpiral : MonoBehaviour
{
    public float spinAmount;
    void Update()
    {
        transform.localEulerAngles += Vector3.forward * spinAmount * Time.deltaTime;
    }
}
