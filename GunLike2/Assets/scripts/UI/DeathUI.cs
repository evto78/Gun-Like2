using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    public Color startColor;
    public Color fadeColor;
    Color curColor;
    public Image deathBg;
    float fadeTimer;
    float shiftTimer;

    float slowdownTimer;

    void Start()
    {
        slowdownTimer = 1f;
        fadeTimer = 0f;
        shiftTimer = -0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        slowdownTimer -= Time.deltaTime;
        if(slowdownTimer < 0.3f) { slowdownTimer = 0.3f; }
        shiftTimer += Time.deltaTime;
        fadeTimer += Time.deltaTime;

        if(fadeTimer > 0.9f) { fadeTimer = 0.9f; }

        curColor = Color.Lerp(startColor, fadeColor, shiftTimer);
        curColor = new Color(curColor.r, curColor.g, curColor.b, fadeTimer);

        deathBg.color = curColor;

        Time.timeScale = slowdownTimer;
    }
}
