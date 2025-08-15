using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearUI : MonoBehaviour
{
    public AnimationCurve spinCurve; public TextMeshProUGUI txt;
    public float spinSpeed; float spinProgress; int roomNum; int nextRoom;
    public bool manualTurning; Vector3 prevTrun; Vector3 tarTurn;
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        roomNum = 1;
        spinProgress = 1;
        prevTrun = transform.localEulerAngles; tarTurn = transform.localEulerAngles;
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
}
