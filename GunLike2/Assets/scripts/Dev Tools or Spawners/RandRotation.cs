using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandRotation : MonoBehaviour
{
    void Start()
    {
        transform.localEulerAngles = Vector3.up * Random.Range(0f, 360f);
        transform.localScale = Vector3.one * Random.Range(1f,1.5f);
        transform.localScale += Vector3.up * Random.Range(0f,1f);
    }
}
