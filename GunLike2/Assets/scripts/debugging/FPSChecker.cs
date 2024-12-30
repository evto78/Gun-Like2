using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSChecker : MonoBehaviour
{
    public int fps = 0;
    void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
    }
}
