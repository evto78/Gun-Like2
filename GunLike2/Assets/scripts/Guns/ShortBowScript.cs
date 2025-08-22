using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortBowScript : GunScript
{
    public override void LateStart()
    {
        if (manager.leftGunScript == this && manager.playerItem.leftItems[11] < 1)
        {
            manager.playerItem.leftItems[11] += 1;
        }
        if (manager.rightGunScript == this && manager.playerItem.rightItems[11] < 1)
        {
            manager.playerItem.rightItems[11] += 1;
        }

        if (manager.leftGunScript == this && manager.playerItem.leftItems[16] < 1)
        {
            manager.playerItem.leftItems[16] += 1;
        }
        if (manager.rightGunScript == this && manager.playerItem.rightItems[16] < 1)
        {
            manager.playerItem.rightItems[16] += 1;
        }
    }
}
