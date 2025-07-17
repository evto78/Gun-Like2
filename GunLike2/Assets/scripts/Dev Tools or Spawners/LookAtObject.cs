using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform lookAt;
    public bool direction; public Vector3 dir;
    public bool spin; public float spinSpeed; float spinamount;
    public Vector3 spinAxis;

    // Update is called once per frame
    void Update()
    {
        spinamount += spinSpeed * Time.deltaTime * 360; if (spinamount > 360) { spinamount -= 360; }
        if (lookAt != null) { transform.LookAt(lookAt); }
        if (direction) { transform.LookAt(transform.position + dir); }
        transform.Rotate(spinAxis * spinamount);
    }
}
