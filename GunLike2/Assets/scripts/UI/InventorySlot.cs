using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    public PlayerItem pi;
    public Image slotRarityBg;
    public Image itemSprite;
    public TextMeshProUGUI quantityText;
    public int id;
    public string hand;
    public void Pressed()
    {
        pi.ItemPressedInInv(id, hand);
    }
}
    