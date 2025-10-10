using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cam;
    public Vector3 offset;
    float angle;
    Vector3 pVector;
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
        if (!limitRotationSpeed) { return; }
        pVector = (cam.transform.position + offset) - transform.position;
        float dotProd = Vector3.Dot(transform.position, pVector);
        Vector3 crossProd = Vector3.Cross(transform.position, pVector);
        switch(dotProd)
        {
            case 0:
                angle = 90;
                break;
            case > 0:
                angle = Mathf.Asin((crossProd.magnitude / (transform.position.magnitude * pVector.magnitude))) * (180 / Mathf.PI);
                break;
            case < 0:
                angle = 180 - (Mathf.Asin((crossProd.magnitude / (transform.position.magnitude * pVector.magnitude))) * (180/Mathf.PI));
                break;
        }
        Debug.Log(angle);
        return;
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
