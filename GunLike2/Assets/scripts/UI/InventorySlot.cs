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
    bool hovering; bool draging;
    bool isCooldown;
    public GameObject cooldownUI;
    public Image cooldownFill;
    public GameObject selected;
    public GameObject dimmingPannel;
    public void Pressed()
    {
        //pi.ItemPressedInInv(id, hand);
    }
    private void Start()
    {
        dimmingPannel.SetActive(false);
        isCooldown = pi.FindObjByID(id).cooldownItem;
        cooldownUI.SetActive(isCooldown);
    }
    private void Update()
    {
        if (hovering)
        {
            pi.GetHoverOver(id,hand);
            if(pi.itemHeld == -1) { invScript.DisplaySetUp(id); }
        }
        //if (pi.lastItemPressed == 74 && id == 74 && pi.lastItemPressedHand == hand)
        //{
            //selected.SetActive(true);
            //selected.transform.Rotate(Vector3.forward * 60 * Time.deltaTime);
        //}
        //else
        //{
            //selected.SetActive(false);
        //}
        if (draging)
        {
            pi.itemHeld = id;
            pi.itemHeldHand = hand;
        }
        Vector2 cooldownInfo = pi.GetCooldownInfo(id, hand);
        cooldownFill.fillAmount = cooldownInfo.x / cooldownInfo.y;
    }
    public void Hover(bool on)
    {
        hovering = on;
    }
    public void SelectedAndHeld(bool on)
    {
        if (draging == true && on == false) { pi.itemHeld = -1; }
        if (pi.itemHeld != -1) { return; }
        draging = on;
        dimmingPannel.SetActive(draging);
    }
}
    