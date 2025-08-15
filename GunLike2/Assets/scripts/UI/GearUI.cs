using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearUI : MonoBehaviour
{
    public AnimationCurve spinCurve; TextMeshProUGUI txt;
    public float spinSpeed; float spinProgress; int roomNum; int nextRoom;
    void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
        roomNum = 1;
        spinProgress = 1;
    }

    // Update is called once per frame
    void Update()
    {
        txt.text = roomNum.ToString();
        spinProgress += Time.deltaTime * spinSpeed;
        transform.localEulerAngles = Vector3.LerpUnclamped(Vector3.zero, Vector3.forward*360f, spinCurve.Evaluate(spinProgress));
        if(spinProgress > 0.1f) { roomNum = nextRoom; }
    }
    public void Turn(int newRoom)
    {
        spinProgress = 0; transform.localEulerAngles = Vector3.zero; nextRoom = newRoom;
    }
}
