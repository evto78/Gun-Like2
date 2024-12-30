using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSChecker : MonoBehaviour
{
    public int fps = 0;
    public TextMeshProUGUI fpsTxt;

    void Update()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsTxt.text = ""+fps;
    }
}
