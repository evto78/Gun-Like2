using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonHoverOver : Selectable
{
    InventorySlot invSlotScript; bool holding = false;
    private void Update()
    {
        if(invSlotScript == null) { invSlotScript = gameObject.GetComponentInParent<InventorySlot>(); }
        invSlotScript.Hover((IsHighlighted() || (IsPressed()&&!holding)));
        if (holding) 
        {
            if (Input.GetMouseButtonUp(0)) { holding = false; invSlotScript.pi.LetGoOfHeld(); return; }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && (IsHighlighted() || IsPressed())) { holding = true; }
        }
        invSlotScript.SelectedAndHeld(holding);
    }

}
