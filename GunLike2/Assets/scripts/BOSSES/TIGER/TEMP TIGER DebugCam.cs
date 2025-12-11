using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMPTIGERDebugCam : MonoBehaviour
{
    public Transform xAxis;
    bool followPlayer;
    Quaternion preFollowRotation;
    Quaternion initialCamRot;
    Transform player;
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        initialCamRot = transform.GetChild(0).localRotation;
        preFollowRotation = transform.rotation;
    }
    void Update()
    {
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.B)) { followPlayer = !followPlayer; transform.rotation = preFollowRotation; transform.GetChild(0).localRotation = initialCamRot; }
        if (!followPlayer)
        {
            if (Input.GetKey(KeyCode.T)) { inputDir -= Vector3.right; }
            if (Input.GetKey(KeyCode.G)) { inputDir += Vector3.right; }
            if (Input.GetKey(KeyCode.F)) { inputDir += Vector3.up; }
            if (Input.GetKey(KeyCode.H)) { inputDir -= Vector3.up; }
            transform.Rotate((Vector3.up * inputDir.y) * (Time.deltaTime * 90f));
            xAxis.Rotate((Vector3.right * inputDir.x) * (Time.deltaTime * 90f));
            preFollowRotation = transform.rotation;
        }
        else
        {
            transform.GetChild(0).LookAt(player);
        }
    }
}
