using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleGunScript : GunScript
{
    

    public override void LateStatUpdate()
    {
        base.LateStatUpdate();
        atkSpd += littleCharge;
    }

    public override void LateReload()
    {
        base.LateReload();
        littleCharge = 0;
    }

    public override void EarlyShoot(bool requireAmmo)
    {
        base.EarlyShoot(requireAmmo);
        littleCharge += 0.2f;
    }
}
