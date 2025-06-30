using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroRifleScript : GunScript
{
    public override void EarlyShoot()
    {
        base.EarlyShoot();
        if (brokenInk > 0 && inkCounter >= Mathf.Clamp(10 - brokenInk, 1, 9))
        {
            int bulConsumed;
            bulConsumed = Mathf.CeilToInt(magSize);
            currentBullets = 0;
            dmg = 0.5f * bulConsumed * dmg;
        }
        else
        {
            if (carvedBone > 0 && currentBullets < magSize)
            {
                for (int i = 0; i < (magSize - currentBullets); i++)
                {
                    manager.healthMan.TakeDamage(1, false, null);
                    currentBullets++;
                }
            }
            int bulConsumed;
            bulConsumed = currentBullets;
            currentBullets = 1;
            dmg = 0.5f * bulConsumed * dmg;
        }
    }
}
