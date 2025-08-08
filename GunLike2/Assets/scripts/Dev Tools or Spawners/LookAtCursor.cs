using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    public Camera mainCamera;
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;

    public bool delay; float delayTimer;

    public bool staticCamera;

    void Start()
    {
        delayTimer = 1f;
    }

    void Update()
    {
        if(delay && delayTimer > 0) { delayTimer -= Time.deltaTime; return; }
        if (staticCamera)
        {
            RaycastHit distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out distance))
            {
                transform.LookAt(distance.point);
            }
        }
        else
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
}