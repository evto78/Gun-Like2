using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFromAToB : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    float timer;
    public float timerSize;
    bool timerUp;
    private void Start()
    {
        timerUp = false;
        timer = timerSize;
    }
    void Update()
    {
        if (timerUp)
        {
            timer += Time.deltaTime;
            if(timer > timerSize) { timer = timerSize; timerUp = false; }
        }
        else
        {
            timer -= Time.deltaTime;
            if(timer < -timerSize) { timer = -timerSize; timerUp = true; }
        }
        transform.localPosition = Vector3.Lerp(pointA, pointB, (timer+timerSize)/(timerSize*2f));
    }
}
