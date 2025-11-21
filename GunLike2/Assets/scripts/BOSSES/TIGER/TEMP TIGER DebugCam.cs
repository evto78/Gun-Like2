using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPTIGERDebugCam : MonoBehaviour
{
    public Transform xAxis;
    void Update()
    {
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) { inputDir -= Vector3.right; }
        if (Input.GetKey(KeyCode.S)) { inputDir += Vector3.right; }
        if (Input.GetKey(KeyCode.A)) { inputDir -= Vector3.up; }
        if (Input.GetKey(KeyCode.D)) { inputDir += Vector3.up; }
        transform.Rotate((Vector3.up * inputDir.y) * (Time.deltaTime * 90f));
        xAxis.Rotate((Vector3.right * inputDir.x) * (Time.deltaTime * 90f));
    }
}
