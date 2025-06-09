using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCursor : MonoBehaviour
{
    public Camera mainCamera;
    Ray ray;
    RaycastHit hit;

    void Start()
    {

    }

    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Ray ray = mainCamera.ScreenPointToRay(mouse);
        RaycastHit hit;

        transform.position = Vector3.zero;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = hit.point;

            transform.position = targetPoint;
        }
    }
}