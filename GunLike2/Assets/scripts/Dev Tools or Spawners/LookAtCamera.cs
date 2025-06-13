using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;
    public Vector3 offset;

    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.LookAt(cam.transform.position + offset);
    }
}
