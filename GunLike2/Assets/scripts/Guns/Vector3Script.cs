using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Script : GunScript
{
    public override void LateStatUpdate()
    {
        base.LateStatUpdate(); // DAMN
        if (acc > 1f) { acc = 1f; }
        atkSpd += 0.8f / acc;
    }
}
