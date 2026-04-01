using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUnlockElement : MonoBehaviour
{
    public Image itemSprite;
    public Image rarityBG;
    public List<Sprite> rarityBGs;
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI progressTxt;
    public GameObject progressObject;
    public TextMeshProUGUI conditionTxt;
    public Image fillGear;
    public Image bgGear;
    public GameObject lockedbg;

    public GameObject unlockDataWindow;
    public List<GameObject> gameObjects;

    public void SetUp(ItemObject myItem, SaveFileReadWrite.UnlockInformation unlockInfo)
    {
        itemSprite.sprite = myItem.itemSprite;
        switch (myItem.rarity)
        {
            case ItemObject.rarityType.Common: rarityBG.sprite = rarityBGs[0]; break;
            case ItemObject.rarityType.Uncommon: rarityBG.sprite = rarityBGs[1]; break;
            case ItemObject.rarityType.Rare: rarityBG.sprite = rarityBGs[2]; break;
            case ItemObject.rarityType.Legendary: rarityBG.sprite = rarityBGs[3]; break;
            case ItemObject.rarityType.Mutated: rarityBG.sprite = rarityBGs[4]; break;
            case ItemObject.rarityType.Haunted: rarityBG.sprite = rarityBGs[5]; break;
            case ItemObject.rarityType.Irradiated: rarityBG.sprite = rarityBGs[6]; break;
            case ItemObject.rarityType.Nuclear: rarityBG.sprite = rarityBGs[7]; break;
            case ItemObject.rarityType.Unique: rarityBG.sprite = rarityBGs[8]; break;
        }
        nameTxt.text = myItem.itemName;
        progressTxt.text = Mathf.FloorToInt(unlockInfo.unlockProgress*100).ToString() + "%";
        conditionTxt.text = myItem.unlockCondition;
        fillGear.fillAmount = unlockInfo.unlockProgress;
        if (fillGear.fillAmount >= 1) 
        { 
            bgGear.enabled = false; fillGear.enabled = false; lockedbg.SetActive(false);
            progressObject.SetActive(false);
            conditionTxt.text = myItem.effect.ToString();
        }
    }

    private void Update()
    {
        unlockDataWindow.SetActive(Vector3.Distance(Input.mousePosition, transform.position) < 50);
    }
}
