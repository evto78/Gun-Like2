using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGunBarrel : MonoBehaviour
{
    GunScript gs;
    public float spinSpeedMin;
    float spinSpeed;
    public float spinSpeedScale;
    public Transform barrel;
    void Start()
    {
        gs = GetComponent<GunScript>();
    }

    // Update is called once per frame
    void Update()
    {
        spinSpeed = gs.littleCharge * spinSpeedScale;
        spinSpeed = Mathf.Clamp(spinSpeed, spinSpeedMin, 9999f);
        barrel.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
    }
}
