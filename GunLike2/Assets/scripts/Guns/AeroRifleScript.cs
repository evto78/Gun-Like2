using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AeroRifleScript : GunScript
{
    public override void EarlyShoot(bool requireAmmo)
    {
        base.EarlyShoot(requireAmmo);
        if (requireAmmo)
        {
            if (carvedBone > 0 && currentBullets < magSize)
            {
                for (int i = 0; i < (magSize - currentBullets); i++)
                {
                    manager.healthMan.TakeDamage(1, false, null, "Carved Bone");
                    currentBullets++;
                }
            }
            int bulConsumed;
            bulConsumed = currentBullets;
            currentBullets = 1;
            dmg = 0.5f * bulConsumed * dmg;
        }
        else
        {
            int bulConsumed;
            bulConsumed = Mathf.CeilToInt(magSize);
            dmg = 0.5f * bulConsumed * dmg;
        }
    }
}
