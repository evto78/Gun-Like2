using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    public Transform lookAt;

    // Update is called once per frame
    void Update()
    {
        if (lookAt != null) { transform.LookAt(lookAt); }
    }
}
