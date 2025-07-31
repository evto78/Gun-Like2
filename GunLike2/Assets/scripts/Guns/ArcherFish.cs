using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherFish : GunScript
{
    public override void LateStart()
    {
        if (transform.parent.name == "left holder")
        {
            manager.gameObject.GetComponent<PlayerItem>().leftItems[40] += 1;
        }
        if (transform.parent.name == "right holder")
        {
            manager.gameObject.GetComponent<PlayerItem>().rightItems[40] += 1;
        }

    }
}
