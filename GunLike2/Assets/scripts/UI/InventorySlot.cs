using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    public InventoryScript invScript;
    public PlayerItem pi;
    public Image slotRarityBg;
    public Image itemSprite;
    public TextMeshProUGUI quantityText;
    public Selectable btn;
    public int id;
    public string hand;
    bool hovering;
    public void Pressed()
    {
        pi.ItemPressedInInv(id, hand);
    }
    private void Update()
    {
        if (hovering)
        {
            invScript.DisplaySetUp(id);
        }
    }
    public void Hover(bool on)
    {
        hovering = on;
    }
}
    