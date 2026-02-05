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
        transform.LookAt(target); 
    }
}
