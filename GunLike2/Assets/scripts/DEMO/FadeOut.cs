using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    Image img; float fadeOutProg;
    void Start()
    {
        fadeOutProg = 1;
        img = GetComponent<Image>();
        img.color = Color.black;
    }
    void Update()
    {
        fadeOutProg -= Time.deltaTime;
        img.color = new Color(0, 0, 0, fadeOutProg);
        if(fadeOutProg < 0) { Destroy(gameObject); }
    }
}
