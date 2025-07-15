using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;
    public Vector3 offset;
    public bool shake;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.LookAt(cam.transform.position + offset);
        if (shake)
        {
            transform.localEulerAngles += new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f))/2f;
        }
    }
}
