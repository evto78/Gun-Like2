using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonHoverOver : Selectable
{
    InventorySlot invSlotScript;
    private void Update()
    {
        if(invSlotScript == null) { invSlotScript = gameObject.GetComponentInParent<InventorySlot>(); }
        invSlotScript.Hover((IsHighlighted() || IsPressed()));
    }

}
