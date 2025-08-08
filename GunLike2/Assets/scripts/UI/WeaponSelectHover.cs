using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectHover : Selectable
{
    WeaponSelectButton wsb;
    void Update()
    {
        if(wsb == null) { wsb = GetComponentInParent<WeaponSelectButton>(); }
        wsb.Hover(IsHighlighted());
    }
}
