using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIGERBackPawTilt : MonoBehaviour
{
    public Transform target;
    Quaternion initRot;
    private void Awake() { initRot = transform.rotation; }
    private void OnDisable() { if (initRot == null) { return; } transform.rotation = initRot; }
    void Update()
    {
        //Quaternion curRot = transform.rotation; 
        transform.LookAt(target); 
        //uaternion tarRot = transform.rotation;
        //transform.rotation = Quaternion.Lerp(curRot, tarRot, Time.deltaTime*2f);
        //transform.rotation = new Quaternion(tarRot.x, initRot.y, initRot.z, initRot.w);
        //transform.rotation = new Quaternion(Mathf.Lerp(curRot.x, tarRot.x, Time.deltaTime * 2f), Mathf.Lerp(curRot.y, tarRot.y, Time.deltaTime * 2f), initRot.z, initRot.w);
    }
}
