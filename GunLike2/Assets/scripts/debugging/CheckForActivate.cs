using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForActivate : MonoBehaviour
{
    public Material on;
    public Material off;
    
    public void Activate()
    {
        gameObject.GetComponent<MeshRenderer>().material = on;
    }
}
