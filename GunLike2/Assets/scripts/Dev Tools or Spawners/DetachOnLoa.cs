using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachOnLoa : MonoBehaviour
{
    void Start()
    {
        transform.parent = null;
        LevelBuilder lb = GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>();
        if(lb != null) { lb.placed.Add(gameObject); }
    }
}
