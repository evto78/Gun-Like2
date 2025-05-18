using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortBowScript : GunScript
{
    public override void LateStart()
    {
        if(transform.parent.name == "left holder")
        {
            manager.gameObject.GetComponent<PlayerItem>().leftItems[11] += 1;
            manager.gameObject.GetComponent<PlayerItem>().leftItems[16] += 1;
        }
        if (transform.parent.name == "right holder")
        {
            manager.gameObject.GetComponent<PlayerItem>().rightItems[11] += 1;
            manager.gameObject.GetComponent<PlayerItem>().rightItems[16] += 1;
        }
    }
}
