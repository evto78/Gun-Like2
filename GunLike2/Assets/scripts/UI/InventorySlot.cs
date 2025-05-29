using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventorySlot : MonoBehaviour
{
    public Image slotRarityBg;
    public Image itemSprite;
    public TextMeshProUGUI quantityText;
    public int id;
    public void Pressed()
    {
        Debug.Log(id);
    }
}
    