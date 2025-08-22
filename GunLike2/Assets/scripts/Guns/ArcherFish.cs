using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFish : GunScript
{
    public override void LateStart()
    {
        if (manager.leftGunScript == this && manager.playerItem.leftItems[40] < 1)
        {
            manager.playerItem.leftItems[40] += 1;
        }
        if (manager.rightGunScript == this && manager.playerItem.rightItems[40] < 1)
        {
            manager.playerItem.rightItems[40] += 1;
        }
    }
}
