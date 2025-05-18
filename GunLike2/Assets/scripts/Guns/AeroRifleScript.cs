using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroRifleScript : GunScript
{
    public override void EarlyShoot()
    {
        base.EarlyShoot();
        int bulConsumed;
        bulConsumed = currentBullets;
        currentBullets = 1;
        dmg = 0.5f * bulConsumed * dmg;
    }
}
