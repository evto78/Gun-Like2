using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachOnLoa : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
    }
}
