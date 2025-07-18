using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingOrbitPoint : MonoBehaviour
{
    public float radias; public float speed; Vector3 initialPos;
    public float progress;
    Vector3 lastPos;
    Vector3 targetPos;
    void Start()
    {
        initialPos = transform.position;
        lastPos = initialPos;
        progress = 0;
        newTarPos();
    }
    void Update()
    {
        progress += speed * Time.deltaTime;
        transform.position = Vector3.Lerp(lastPos, targetPos, progress);
        if(progress >= 1)
        {
            transform.position = targetPos;
            lastPos = targetPos;
            newTarPos();
            progress = 0;
        }
    }
    void newTarPos()
    {
        targetPos = initialPos + new Vector3(Random.Range(-radias, radias), 0, Random.Range(-radias, radias));
    }
}
