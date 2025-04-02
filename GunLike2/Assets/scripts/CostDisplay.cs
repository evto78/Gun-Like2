using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostDisplay : MonoBehaviour
{
    public float offset;
    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * offset;
    }
}
