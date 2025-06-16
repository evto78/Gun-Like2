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
    bool isCooldown;
    public GameObject cooldownUI;
    public Image cooldownFill;
    public GameObject selected;
    public void Pressed()
    {
        pi.ItemPressedInInv(id, hand);
    }
    private void Start()
    {
        isCooldown = pi.FindObjByID(id).cooldownItem;
        cooldownUI.SetActive(isCooldown);
    }
    private void Update()
    {
        if (hovering)
        {
            invScript.DisplaySetUp(id);
        }
        if (pi.lastItemPressed == 74 && id == 74 && pi.lastItemPressedHand == hand)
        {
            selected.SetActive(true);
            selected.transform.Rotate(Vector3.forward * 60 * Time.deltaTime);
        }
        else
        {
            selected.SetActive(false);
        }
        Vector2 cooldownInfo = pi.GetCooldownInfo(id, hand);
        cooldownFill.fillAmount = cooldownInfo.x / cooldownInfo.y;
    }
    public void Hover(bool on)
    {
        hovering = on;
    }
}
    