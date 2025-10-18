using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearUI : MonoBehaviour
{
    public AnimationCurve spinCurve; public TextMeshProUGUI txt;
    public float spinSpeed; float spinProgress; int roomNum; int nextRoom;
    public bool manualTurning; Vector3 prevTrun; Vector3 tarTurn;

    float flashTimer = 0;
    Color baseColor; Image img;
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        roomNum = 1;
        spinProgress = 1;
        prevTrun = transform.localEulerAngles; tarTurn = transform.localEulerAngles;

        img = GetComponent<Image>();
        baseColor = img.color;
    }

    // Update is called once per frame
    void Update()
    {
        spinProgress += Time.deltaTime * spinSpeed;
        if(txt != null) { txt.text = roomNum.ToString(); }
        if (manualTurning)
        {
            transform.localEulerAngles = Vector3.Lerp(prevTrun, tarTurn, spinCurve.Evaluate(spinProgress));
        }
        else
        {
            transform.localEulerAngles = Vector3.LerpUnclamped(Vector3.zero, Vector3.forward * 360f, spinCurve.Evaluate(spinProgress));
            if (spinProgress > 0.1f) { roomNum = nextRoom; }
        }

        flashTimer -= Time.deltaTime;
        transform.localScale = Vector3.one * Mathf.Lerp(1f, 1.2f, flashTimer);
        img.color = Color.Lerp(baseColor, (baseColor/2f)+(Color.white/2f), flashTimer);
    }
    public void Turn(float newVal)
    {
        if (manualTurning)
        {
            prevTrun = transform.localEulerAngles; tarTurn = Vector3.forward*newVal * 120f;
        }
        else
        {
            spinProgress = 0;
            transform.localEulerAngles = Vector3.zero; nextRoom = (int)newVal;
        }
    }
    public void ResetSpin()
    {
        spinProgress = 0;
    }
    public void Flash(float intensity)
    {
        intensity = Mathf.Clamp(intensity, 0.33f, 1f);
        if(flashTimer < intensity) { flashTimer = intensity; }
    }
}
