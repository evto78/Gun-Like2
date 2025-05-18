using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelScript : GunScript
{
    public override void AttemptShoot()
    {
        if ((bowAct > 0))
        {
            bowCharge += 1 * atkSpd * Time.deltaTime;
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; }
        }
        else
        {
            if (!reloading && !shooting)
            {
                for(int i = 0; i < magSize /2; i++)
                {
                    Shoot(1f);
                }
            }
        }
    }
}
