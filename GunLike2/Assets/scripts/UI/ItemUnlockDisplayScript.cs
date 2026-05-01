using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUnlockDisplayScript : MonoBehaviour
{
    ItemObject lastObject;

    public Image itemSprite;
    public Image rarityBG;
    public List<Sprite> rarityBGs;
    public Image gearFill;
    public GameObject lockedUI;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemRarity;
    public TextMeshProUGUI unlockProgress;
    public TextMeshProUGUI unlockCondition;
    public TextMeshProUGUI itemBuffs;
    public TextMeshProUGUI itemDebuffs;
    public TextMeshProUGUI itemEffect;

    public void SetUpDisplay(ItemObject itemData, SaveFileReadWrite.UnlockInformation unlockInfo)
    {
        if (lastObject == itemData) { return; }
        lastObject = itemData;
        switch (itemData.rarity)
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
        itemSprite.sprite = itemData.itemSprite;
        itemName.text = itemData.itemName;
        unlockProgress.text = Mathf.FloorToInt(unlockInfo.unlockProgress * 100).ToString() + "%";
        unlockCondition.text = itemData.unlockCondition;
        gearFill.fillAmount = unlockInfo.unlockProgress;
        itemRarity.text = itemData.rarity.ToString();
        itemBuffs.text = BuildSimpleStatTxt(itemData, true);
        itemEffect.text = itemData.effect;
        itemDebuffs.text = BuildSimpleStatTxt(itemData, false);
        if (gearFill.fillAmount >= 1)
        {
            lockedUI.SetActive(false);
        }
        else
        {
            lockedUI.SetActive(true);
        }
    }

    string BuildSimpleStatTxt(ItemObject itemData, bool isBuff)
    {
        string returnTxt = "";
        if(itemData.id == 22)
        {
            if (isBuff) { return "Your Lowest Stat X2"; }
            else { return "Your Highest Stat X0.9"; }
        }
        foreach(ItemObject.StatData statData in itemData.statData)
        {
            if (statData.change >= 0 && isBuff)
            {
                returnTxt += (statData.stat.ToString() + " + " + (int)statData.change + "%, ");
            }
            else if (statData.change < 0 && !isBuff)
            {
                returnTxt += (statData.stat.ToString() + " - " + Mathf.Abs((int)statData.change) + "%, ");
            }
        }
        returnTxt = returnTxt.TrimEnd(' ');
        returnTxt = returnTxt.TrimEnd(',');
        return returnTxt;
    }
}
