using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;
    public Vector3 offset;
    public bool shake;
    public bool limitRotationSpeed;
    public float maxRotationSpeed;
    Vector3 desRotation; Vector3 curRotation;
    void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        if (limitRotationSpeed)
        {
            curRotation = transform.localEulerAngles;
            transform.LookAt(cam.transform.position + offset);
            desRotation = transform.localEulerAngles;
            transform.localEulerAngles = Vector3.Lerp(curRotation, desRotation, maxRotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.LookAt(cam.transform.position + offset);
        }
        if (shake)
        {
            transform.localEulerAngles += new Vector3(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f))/2f;
        }
    }
}
