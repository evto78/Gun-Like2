using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFish : GunScript
{
    public override void LateStart()
    {
        
        if (transform.parent.name == "left holder" && manager.playerItem.leftItems[40] < 1)
        {
            manager.playerItem.leftItems[40] += 1;
        }
        if (transform.parent.name == "right holder" && manager.playerItem.rightItems[40] < 1)
        {
            manager.playerItem.rightItems[40] += 1;
        }
    }
}
