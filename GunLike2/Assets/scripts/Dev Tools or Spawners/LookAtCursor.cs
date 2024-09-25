using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    public Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    void Start()
    {

    }

    void Update()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        transform.localEulerAngles = Vector3.zero;

        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            Vector3 targetPoint = hit.point;

            transform.LookAt(targetPoint);
        }
    }
}