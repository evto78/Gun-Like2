using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatedKnifeScript : GunScript
{
    private void LateUpdate()
    {
        currentBullets = Mathf.RoundToInt(magSize);
        littleCharge = 0f;
    }
}
