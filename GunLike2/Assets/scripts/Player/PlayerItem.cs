using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [Header("Item Categories and data")]
    public List<ItemObject> itemData;

    public List<int> leftItems;
    public List<int> rightItems;

    public List<float> modifierList = new List<float>();
    //Index 0 - 18 what stat is referenced:
    //0 - Speed
    //1 - Sprint Speed
    //2 - Jump Height
    //3 - Number of Jumps

    //LEFT:
    //4 - Crit Chance 
    //5 - Crit Damage
    //6 - Weak Spot Damage
    //7 - Damage
    //8 - Attack Speed
    //9 - Reload Speed
    //10 - Magazine Size
    //11 - Accuracy
    //12 - Bullet Speed
    //13 - Bullet Size
    //14 - Bullet Pierce

    //RIGHT:
    //15 - Crit Chance 
    //16 - Crit Damage
    //17 - Weak Spot Damage
    //18 - Damage
    //19 - Attack Speed
    //20 - Reload Speed
    //21 - Magazine Size
    //22 - Accuracy
    //23 - Bullet Speed
    //24 - Bullet Size
    //25 - Bullet Pierce

    //26 - Max Hp
    //27 - Passive hp regen
    //28 - Armor

    public List<int> commonItems = new List<int>();
    public List<int> uncommonItems = new List<int>();
    public List<int> rareItems = new List<int>();
    public List<int> legendaryItems = new List<int>();
    public List<int> mutatedItems = new List<int>();
    public List<int> hauntedItems = new List<int>();
    public List<int> irradiatedItems = new List<int>();
    public List<int> nuclearItems = new List<int>();
    public List<int> uniqueItems = new List<int>();
    
    public List<List<int>> rarityList = new List<List<int>>();
    public List<ItemObject> lockedItems = new List<ItemObject>();

    public List<int> gunLike1Items = new List<int>();
    public List<int> sponserItems = new List<int>();
    public List<int> fishItems = new List<int>();
    public List<int> unstableItems = new List<int>();
    public List<int> cooldownItems = new List<int>();
    public List<int> horrorItems = new List<int>();

    public int leftCommonItemCount;
    public int leftUncommonItemCount;
    public int leftRareItemCount;
    public int leftLegendaryItemCount;
    public int leftMutatedItemCount;
    public int leftHauntedItemCount;
    public int leftIrradiatedItemCount;
    public int leftNuclearItemCount;
    public int leftUniqueItemCount;
    public int rightCommonItemCount;
    public int rightUncommonItemCount;
    public int rightRareItemCount;
    public int rightLegendaryItemCount;
    public int rightMutatedItemCount;
    public int rightHauntedItemCount;
    public int rightIrradiatedItemCount;
    public int rightNuclearItemCount;
    public int rightUniqueItemCount;

    [Header("Manager scripts")]
    public NEWPlayerMovement playerMvt;
    public HealthManager healthManager;
    public GunManager gunManager;
    public UIManager uiManager;
    public PopupItemUI popupUI;

    public Transform playerCamera;

    public GameObject itemDisplay;
    [Header("Item Checks")]
    //ItemChecks
    public int leftIFPStatToBuff;
    public int leftIFPStatToDeBuff;
    public int rightIFPStatToBuff;
    public int rightIFPStatToDeBuff;

    public int gotchaTickets;
    public int leftLowFreqRes; List<int> leftLFRAffected = new List<int>();
    public int rightLowFreqRes; List<int> rightLFRAffected = new List<int>();
    public int masterCard;
    public float masterCardChance;
    public int copperCoinLeft;
    public int copperCoinRight;

    public int lastItemPressed;
    public string lastItemPressedHand;
    public int itemHeld = -1; public string itemHeldHand = "left";

    public GameObject itemPos;
    public GameObject itemPickupEffectPrefab;

    GameDataManager gdm;
    public bool spentMoneyThisRoom = false;
    private void Awake()
    {
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();

        leftItems = new List<int>(); 
        rightItems = new List<int>(); 
            
        itemData = new List<ItemObject>();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
        SortItemData();

        uiManager = gameObject.GetComponent<UIManager>();

        LoadCategories();
    }
    void SortItemData()
    {
        List<int> comparisonList = new List<int>();
        List<ItemObject> sortedItemData = new List<ItemObject>();
        for(int i = 0; i < itemData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for(int i = 0; i < itemData.Count; i++) 
        {
            sortedItemData[comparisonList.IndexOf(itemData[i].id)] = itemData[i];
        }
        itemData = sortedItemData;
    }
    List<SaveFileReadWrite.UnlockInformation> GetAndSortUnlockData()
    {

        if (gdm.instance.data.UnlockInfo.Count != itemData.Count)
        {
            //Unlock save data is old, remaking file
            gdm.instance.data.UnlockInfo = gdm.instance.UpdateUnlockInfo(gdm.instance.data.UnlockInfo, itemData);
        }

        List<SaveFileReadWrite.UnlockInformation> unlockData = gdm.instance.data.UnlockInfo;

        List<int> comparisonList = new List<int>();
        List<SaveFileReadWrite.UnlockInformation> sortedUnlockData = new List<SaveFileReadWrite.UnlockInformation>();
        for (int i = 0; i < unlockData.Count; i++) { comparisonList.Add(i); sortedUnlockData.Add(null); }
        for (int i = 0; i < unlockData.Count; i++)
        {
            sortedUnlockData[comparisonList.IndexOf(unlockData[i].id)] = unlockData[i];
        }
        unlockData = sortedUnlockData;
        return unlockData;
    }
    void LoadCategories()
    {
        commonItems = new List<int>();
        uncommonItems = new List<int>();
        rareItems = new List<int>();
        legendaryItems = new List<int>();
        mutatedItems = new List<int>();
        hauntedItems = new List<int>();
        irradiatedItems = new List<int>();
        nuclearItems = new List<int>();
        rarityList = new List<List<int>>();
        lockedItems = new List<ItemObject>();

        gunLike1Items = new List<int>();
        sponserItems = new List<int>();
        fishItems = new List<int>();
        unstableItems = new List<int>();
        horrorItems = new List<int>();
        cooldownItems = new List<int>();

        List<SaveFileReadWrite.UnlockInformation> unlockInfo = GetAndSortUnlockData();
        foreach (ItemObject item in itemData)
        {
            leftItems.Add(0); rightItems.Add(0);
            if (unlockInfo[item.id].unlockProgress >= 1)
            {
                switch (item.rarity)
                {
                    case ItemObject.rarityType.Common: commonItems.Add(item.id); break;
                    case ItemObject.rarityType.Uncommon: uncommonItems.Add(item.id); break;
                    case ItemObject.rarityType.Rare: rareItems.Add(item.id); break;
                    case ItemObject.rarityType.Legendary: legendaryItems.Add(item.id); break;
                    case ItemObject.rarityType.Mutated: mutatedItems.Add(item.id); break;
                    case ItemObject.rarityType.Haunted: hauntedItems.Add(item.id); break;
                    case ItemObject.rarityType.Irradiated: irradiatedItems.Add(item.id); break;
                    case ItemObject.rarityType.Nuclear: nuclearItems.Add(item.id); break;
                    case ItemObject.rarityType.Unique: uniqueItems.Add(item.id); break;
                }
                switch (item.subType)
                {
                    case ItemObject.itemType.classic: gunLike1Items.Add(item.id); break;
                    case ItemObject.itemType.sponser: sponserItems.Add(item.id); break;
                    case ItemObject.itemType.fish: fishItems.Add(item.id); break;
                    case ItemObject.itemType.unstablePart: unstableItems.Add(item.id); break;
                    case ItemObject.itemType.horror: horrorItems.Add(item.id); break;
                }
            }
            else { lockedItems.Add(item); }
            if (item.cooldownItem) { cooldownItems.Add(item.id); }
        }
        rarityList.InsertRange(0, new List<int>[] { commonItems, uncommonItems, rareItems, legendaryItems, mutatedItems, hauntedItems, irradiatedItems, nuclearItems, uniqueItems });
    }
    public void TriggerUnlock(int id)
    {
        uiManager.popupNotif.Popup(itemData[id]);
        ItemObject item = itemData[id];
        Debug.Log("UNLOCKED: " + item.itemName);
        switch (item.rarity)
        {
            case ItemObject.rarityType.Common: if (!commonItems.Contains(item.id)) { commonItems.Add(item.id); } break;
            case ItemObject.rarityType.Uncommon: if (!uncommonItems.Contains(item.id)) { uncommonItems.Add(item.id); } break;
            case ItemObject.rarityType.Rare: if (!rareItems.Contains(item.id)) { rareItems.Add(item.id); } break;
            case ItemObject.rarityType.Legendary: if (!legendaryItems.Contains(item.id)) { legendaryItems.Add(item.id); } break;
            case ItemObject.rarityType.Mutated: if (!mutatedItems.Contains(item.id)) { mutatedItems.Add(item.id); } break;
            case ItemObject.rarityType.Haunted: if (!hauntedItems.Contains(item.id)) { hauntedItems.Add(item.id); } break;
            case ItemObject.rarityType.Irradiated: if (!irradiatedItems.Contains(item.id)) { irradiatedItems.Add(item.id); } break;
            case ItemObject.rarityType.Nuclear: if (!nuclearItems.Contains(item.id)) { nuclearItems.Add(item.id); } break;
            case ItemObject.rarityType.Unique: if (!uniqueItems.Contains(item.id)) { uniqueItems.Add(item.id); } break;
        }
    }
    public void ItemsFromMutatedModifcataion(List<int> rules)
    {
        foreach (int rule in rules)
        {
            if (rule == 12)
            {
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "left", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "left", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "left", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "left", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "left", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "right", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "right", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "right", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "right", false);
                AddRandItemsFromRarity(1, ItemRarity.GetWeightedRandRarity(), "right", false);
            }
        }
    }
    private void Update()
    {
        //Stat Update
        ItemUpdate();
        healthManager.StatUpdate(leftItems, rightItems, rarityList);
        playerMvt.StatUpdate(leftItems, rightItems, rarityList);
        gunManager.StatUpdate(leftItems, rightItems, rarityList);
        ItemUpdateCleanup();

        //After Stat Update
        LookForItem();

        UpdateModifierList();

        CheckForMerge();
        UpdateRarityCount();
    }
    void ItemUpdate()
    {
        leftLowFreqRes = leftItems[154]; rightLowFreqRes = rightItems[154];
        LowFreqRes();
        masterCard = leftItems[174] + rightItems[174];
        copperCoinLeft = leftItems[195];
        copperCoinRight = rightItems[195];
    }
    void ItemUpdateCleanup()
    {
        LowFreqResCleanup();
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0)) { itemHeld = -1; }
    }
    public void OnItemDestroy(int id, int amount, string hand)
    {
        popupUI.CreateNotif(amount, FindObjByID(id));
        uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
        if(id == 94) { healthManager.appleBuff += 20; }
        for(int i = 0; i < amount; i++)
        {
            if (Random.Range(1, 100) < 5)
            {
                int rand = unstableItems[Random.Range(0, unstableItems.Count)];
                if (hand == "left") { leftItems[rand]++; }
                if (hand == "right") { rightItems[rand]++; }
            }
        }
    }
    public void OnItemGain(int id, int amount, string hand)
    {
        popupUI.CreateNotif(amount, FindObjByID(id));
        switch (id)
        {
            case 38: gdm.unlockMan.UnlockItem(39); break; // Experimental Implant (39)
            case 69: uiManager.VisionOfGunky(); break;
            case 70: uiManager.VisionOfGunky(); break;
            case 71: uiManager.VisionOfGunky(); break;
            case 72: uiManager.VisionOfGunky(); break;
            case 175:
                if (hand == "left") { for (int i = 0; i < 50 * amount; i++) { gunManager.leftGunScript.addBullet(); } }
                if (hand == "right") { for (int i = 0; i < 50 * amount; i++) { gunManager.rightGunScript.addBullet(); } }
                break;
            case 186: healthManager.GiveEffect(27, 120); break;
            case 188: gdm.unlockMan.UnlockItem(5); break; // Aircraft Grade Metal (5)
            case 191: healthManager.money += Mathf.RoundToInt(50f + (50f * leftItems[177]) + (50f * rightItems[177])); break;
        }

        if (sponserItems.Contains(id)) { gdm.unlockMan.UnlockItem(42); } // Sponsership Deal (42)

        switch (itemData[id].rarity)
        {
            case ItemObject.rarityType.Common: break;
            case ItemObject.rarityType.Uncommon: break;
            case ItemObject.rarityType.Rare: break;
            case ItemObject.rarityType.Legendary: break;
            case ItemObject.rarityType.Mutated: break;
            case ItemObject.rarityType.Haunted: gdm.unlockMan.UnlockItem(40); break; // Possession (40)
            case ItemObject.rarityType.Irradiated: break;
            case ItemObject.rarityType.Nuclear: break;
            case ItemObject.rarityType.Unique: break;
        }

        uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
        
        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(1, 100) < 8 && hand == "left" && leftItems[97] > 0) { leftItems[97]--; }
            if (Random.Range(1, 100) < 8 && hand == "left" && leftItems[98] > 0) { leftItems[98]--; }
            if (Random.Range(1, 100) < 8 && hand == "left" && leftItems[99] > 0) { leftItems[99]--; }
            if (Random.Range(1, 100) < 8 && hand == "left" && leftItems[100] > 0) { leftItems[100]--; }
            if (Random.Range(1, 100) < 8 && hand == "right" && rightItems[97] > 0) { rightItems[97]--; }
            if (Random.Range(1, 100) < 8 && hand == "right" && rightItems[98] > 0) { rightItems[98]--; }
            if (Random.Range(1, 100) < 8 && hand == "right" && rightItems[99] > 0) { rightItems[99]--; }
            if (Random.Range(1, 100) < 8 && hand == "right" && rightItems[100] > 0) { rightItems[100]--; }
        }
        switch (hand)
        {
            case "left": gunManager.leftItemsCollectedDATA+=amount; break;
            case "right": gunManager.rightItemsCollectedDATA+=amount; break;
        }
    }
    int hoverOverId; string hoverOverHand; public void GetHoverOver(int id, string hand) { hoverOverId = id; hoverOverHand = hand; }
    public void LetGoOfHeld() { ItemDroppedOnItem(itemHeld, hoverOverId, itemHeldHand, hoverOverHand);
        if (uiManager.recycleBinUI.GetHighlighted()) { 
            if (itemHeld == -1) { return; }

            uiManager.recycleBinUI.animator.SetTrigger("Deposit");

            int cost = Mathf.CeilToInt((healthManager.baseCost * (int)(healthManager.gdm.difficulty * (healthManager.gdm.roomNumber + 1))) * 0.75f);

            switch (FindRarityByID(itemHeld))
            {
                case 0: cost *= 1; break;
                case 1: cost = Mathf.CeilToInt(cost * 1.5f); break;
                case 2: cost *= 2; break;
                case 3: cost *= 4; break;
                case 4: cost *= 3; break;
                case 5: cost *= 3; break;
                case 6: cost *= 3; break;
                case 7: cost *= 4; break;
                case 8: cost = 0; break;
            }

            if(itemHeldHand == "left") { leftItems[itemHeld]--; } else { rightItems[itemHeld]--; }
            itemHeld = -1;
            healthManager.money += cost;
        }
    }
    public void ItemDroppedOnItem(int heldId, int droppedOnId, string heldHand, string droppedHand)
    {
        if(heldId == droppedOnId) return;
        if (droppedHand == "left")
        {
            if (droppedOnId == 74 && leftItems[74] > 0) { NuclearFission(heldId, heldHand, droppedHand); }
            if (droppedOnId == 171 && leftItems[171] > 0) { OverridenCompressor(heldId, heldHand, droppedHand); }
        }
        else
        {
            if (droppedOnId == 74 && rightItems[74] > 0) { NuclearFission(heldId, heldHand, droppedHand); }
            if (droppedOnId == 171 && rightItems[171] > 0) { OverridenCompressor(heldId, heldHand, droppedHand); }
        }
        uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
    }
    void UpdateRarityCount()
    {
        leftCommonItemCount = 0;
        leftUncommonItemCount = 0;
        leftRareItemCount = 0;
        leftLegendaryItemCount = 0;
        leftMutatedItemCount = 0;
        leftHauntedItemCount = 0;
        leftIrradiatedItemCount = 0;
        leftNuclearItemCount = 0;
        leftUniqueItemCount = 0;

        rightCommonItemCount = 0;
        rightUncommonItemCount = 0;
        rightRareItemCount = 0;
        rightLegendaryItemCount = 0;
        rightMutatedItemCount = 0;
        rightHauntedItemCount = 0;
        rightIrradiatedItemCount = 0;
        rightNuclearItemCount = 0;
        rightUniqueItemCount = 0;

        for(int i = 0; i < leftItems.Count; i++)
        {
            switch (FindRarityByID(i))
            {
                case 0: leftCommonItemCount += leftItems[i]; rightCommonItemCount += rightItems[i]; break;
                case 1: leftUncommonItemCount += leftItems[i]; rightUncommonItemCount += rightItems[i]; break;
                case 2: leftRareItemCount += leftItems[i]; rightRareItemCount += rightItems[i]; break;
                case 3: leftLegendaryItemCount += leftItems[i]; rightLegendaryItemCount += rightItems[i]; break;
                case 4: leftMutatedItemCount += leftItems[i]; rightMutatedItemCount += rightItems[i]; break;
                case 5: leftHauntedItemCount += leftItems[i]; rightHauntedItemCount += rightItems[i]; break;
                case 6: leftIrradiatedItemCount += leftItems[i]; rightIrradiatedItemCount += rightItems[i]; break;
                case 7: leftNuclearItemCount += leftItems[i]; rightNuclearItemCount += rightItems[i]; break;
                case 8: leftUniqueItemCount += leftItems[i]; rightUniqueItemCount += rightItems[i]; break;
            }
        }
    }
    void NuclearFission(int id, string heldHand, string droppedHand)
    {
        if (heldHand == "left") { leftItems[id]--; }
        else { rightItems[id]--; }

        switch (FindRarityByID(id))
        {
            case 0: healthManager.money += Mathf.CeilToInt((healthManager.baseCost * (int)(healthManager.gdm.difficulty * (healthManager.gdm.roomNumber + 1)))/5); break; //common
            case 1: AddRandItemsFromRarity(2, 0, droppedHand, true); break; //uncommon
            case 2: AddRandItemsFromRarity(2, 1, droppedHand, true); break; //rare
            case 3: AddRandItemsFromRarity(2, 2, droppedHand, true); AddRandItemsFromRarity(1, 7, droppedHand, true); break; //legendary
            case 4: AddRandItemsFromRarity(2, 0, droppedHand, true); break; //mutated
            case 5: AddRandItemsFromRarity(2, 0, droppedHand, true); break; //haunted
            case 6: AddRandItemsFromRarity(1, 0, droppedHand, true); AddRandItemsFromRarity(1, 4, droppedHand, true); break; //irradiated
            case 7: AddRandItemsFromRarity(3, 6, droppedHand, true); AddRandItemsFromRarity(1, 5, droppedHand, true); break; //nuclear
        }
    }
    void OverridenCompressor(int id, string heldHand, string droppedHand)
    {
        int raritySelected = FindRarityByID(id);
        int overCompress = 0;
        if (droppedHand == "left") { overCompress = gunManager.leftOverCompress; }
        else { overCompress = gunManager.rightOverCompress; }

        if (heldHand == "left")
        {
            int sumOfItemsFromRarity = 0;
            foreach (int i in rarityList[raritySelected])
            {
                sumOfItemsFromRarity += leftItems[i];
            }
            if (sumOfItemsFromRarity < 3) { return; }

            int itemsRequired;
            if (overCompress > 1 && Random.Range(1, 100) < 20 * (overCompress - 1))
            { itemsRequired = 1; }
            else { itemsRequired = 2; }
            int attempts = 0;
            while (itemsRequired > 0 && attempts < 100)
            {
                int randId = rarityList[raritySelected][Random.Range(0, rarityList[raritySelected].Count)];
                if (leftItems[randId] > 0 && randId != id) { leftItems[randId]--; itemsRequired--; }
                attempts++;
            }
            if (itemsRequired > 0) { return; }
            leftItems[id]++;
        }
        else
        {
            int sumOfItemsFromRarity = 0;
            foreach (int i in rarityList[raritySelected])
            {
                sumOfItemsFromRarity += rightItems[i];
            }
            if (sumOfItemsFromRarity < 3) { return; }

            int itemsRequired;
            if (overCompress > 1 && Random.Range(1, 100) < 20 * (overCompress - 1))
            { itemsRequired = 1; }
            else { itemsRequired = 2; }
            int attempts = 0;
            while (itemsRequired > 0 && attempts < 100)
            {
                int randId = rarityList[raritySelected][Random.Range(0, rarityList[raritySelected].Count)];
                if (rightItems[randId] > 0 && randId != id) { rightItems[randId]--; itemsRequired--; }
                attempts++;
            }
            if (itemsRequired > 0) { return; }
            rightItems[id]++;
        }
    }
    public void AddRandItemsFromRarity(int amount, int rarity, string hand, bool createPopUp)
    {
        if(hand == "left")
        {
            for (int i = 0; i < amount; i++)
            {
                int rand = Random.Range(0, rarityList[rarity].Count);
                leftItems[rarityList[rarity][rand]]++;
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                int rand = Random.Range(0, rarityList[rarity].Count);
                rightItems[rarityList[rarity][rand]]++;
            }
        }
    }
    void CheckForMerge()
    {
        //Angel wings / Imp wings
        //LEFT
        if (leftItems[31] > 0 && leftItems[32] > 0)
        {
            leftItems[46] += 1;
            leftItems[31] -= 1;
            leftItems[32] -= 1;
        }
        //RIGHT
        if (rightItems[31] > 0 && rightItems[32] > 0)
        {
            rightItems[46] += 1;
            rightItems[31] -= 1;
            rightItems[32] -= 1;
        }

        //partnerships
        if(leftItems[34] > 20) { leftItems[48] = 1; }//fire
        if(leftItems[36] > 5) { leftItems[49] = 1; }//silver
        if(leftItems[35] > 20) { leftItems[50] = 1; }//sharp
        if(leftItems[43] > 20) { leftItems[51] = 1; }//help
        if(leftItems[44] > 20) { leftItems[52] = 1; }//cool
        if(leftItems[45] > 20) { leftItems[53] = 1; }//large
        if(leftItems[47] > 10) { leftItems[54] = 1; }//fast

        if (rightItems[34] > 20) { rightItems[48] = 1; }//fire
        if (rightItems[36] > 5) { rightItems[49] = 1; }//silver
        if (rightItems[35] > 20) { rightItems[50] = 1; }//sharp
        if (rightItems[43] > 20) { rightItems[51] = 1; }//help
        if (rightItems[44] > 20) { rightItems[52] = 1; }//cool
        if (rightItems[45] > 20) { rightItems[53] = 1; }//large
        if (rightItems[47] > 10) { rightItems[54] = 1; }//fast

        if (rightItems[69] + leftItems[69] > 0 && rightItems[70] + leftItems[70] > 0 && rightItems[71] + leftItems[71] > 0)
        {
            leftItems[72] = 1; rightItems[72] = 1;
        }

        //Surprise Egg
        if(leftItems[55] > 0) { leftItems[rarityList[0][Random.Range(0, rarityList[0].Count)]] += 1; leftItems[rarityList[0][Random.Range(0, rarityList[0].Count)]] += 1; leftItems[56] += 1; leftItems[55] -= 1; }
        if(rightItems[55] > 0) { rightItems[rarityList[0][Random.Range(0, rarityList[0].Count)]] += 1; rightItems[rarityList[0][Random.Range(0, rarityList[0].Count)]] += 1; rightItems[56] += 1; rightItems[55] -= 1; }
    }
    public void ForcePickup(int id)
    {
        int leftMonkeysPaw = leftItems[176]; int rightMonkeysPaw = rightItems[176];
        if (Random.Range(0, 2) == 0)
        {
            rightItems[id] += 1;
            if (rightMonkeysPaw > 0) { MonkeysPaw(id, "right", rightMonkeysPaw); }
            if (leftItems[86] > 0) { leftItems[id] += 1; leftItems[86]--; }
            if (id == 186) { healthManager.GiveEffect(27, 120); }
        }
        else
        {
            leftItems[id] += 1;
            if (leftMonkeysPaw > 0) { MonkeysPaw(id, "left", leftMonkeysPaw); }
            if (leftItems[86] > 0) { leftItems[id] += 1; leftItems[86]--; }
            if (id == 186) { healthManager.GiveEffect(27, 120); }
        }
    }
    void LookForItem()
    {
        Vector3 camPos = playerCamera.position;
        Ray ray = new Ray(camPos, playerCamera.forward);
        RaycastHit hit;

        int leftMonkeysPaw = leftItems[176]; int rightMonkeysPaw = rightItems[176];

        if (Physics.Raycast(ray, out hit, 7f))
        {
            if(hit.collider.gameObject.tag == "item")
            {
                Vector3 hitItem = hit.collider.gameObject.transform.position;

                itemDisplay.SetActive(true);
                itemDisplay.GetComponent<ItemDisplayScript>().InfoUpdate(hit.collider.gameObject.GetComponentInParent<Item>().itemObj, hitItem);

                if (Input.GetKeyDown(healthManager.gdm.instance.controlsBinds.righInteract) || (Input.GetKey(healthManager.gdm.instance.controlsBinds.righInteract) && Input.GetKey(healthManager.gdm.instance.controlsBinds.sprint)))
                {
                    int id = hit.collider.gameObject.GetComponentInParent<Item>().WhatItem();
                    rightItems[id] += 1;
                    if (rightMonkeysPaw > 0) { MonkeysPaw(id, "right", rightMonkeysPaw); }
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (leftItems[86] > 0) { leftItems[id] += 1; leftItems[86]--; }
                    if (id == 186) { healthManager.GiveEffect(27, 120); }
                    ItemPickupEffect spawnedEffect = Instantiate(itemPickupEffectPrefab).GetComponent<ItemPickupEffect>();
                    spawnedEffect.transform.position = hit.point;
                    switch (itemData[id].rarity)
                    {
                        case ItemObject.rarityType.Common: spawnedEffect.SetUpEffect(0, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Uncommon: spawnedEffect.SetUpEffect(1, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Rare: spawnedEffect.SetUpEffect(2, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Legendary: spawnedEffect.SetUpEffect(3, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Mutated: spawnedEffect.SetUpEffect(4, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Haunted: spawnedEffect.SetUpEffect(5, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Irradiated: spawnedEffect.SetUpEffect(6, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Nuclear: spawnedEffect.SetUpEffect(7, gunManager.rightGunScript.transform); break;
                        case ItemObject.rarityType.Unique: spawnedEffect.SetUpEffect(8, gunManager.rightGunScript.transform); break;
                    }
                }
                if (Input.GetKeyDown(healthManager.gdm.instance.controlsBinds.leftInteract) || (Input.GetKey(healthManager.gdm.instance.controlsBinds.leftInteract) && Input.GetKey(healthManager.gdm.instance.controlsBinds.sprint)))
                {
                    int id = hit.collider.gameObject.GetComponentInParent<Item>().WhatItem();
                    leftItems[id] += 1;
                    if (leftMonkeysPaw > 0) { MonkeysPaw(id, "left", leftMonkeysPaw); }
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (rightItems[86] > 0) { rightItems[id] += 1; rightItems[86]--; }
                    if (id == 186) { healthManager.GiveEffect(27, 120); }
                    ItemPickupEffect spawnedEffect = Instantiate(itemPickupEffectPrefab).GetComponent<ItemPickupEffect>();
                    spawnedEffect.transform.position = hit.point;
                    switch (itemData[id].rarity)
                    {
                        case ItemObject.rarityType.Common: spawnedEffect.SetUpEffect(0, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Uncommon: spawnedEffect.SetUpEffect(1, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Rare: spawnedEffect.SetUpEffect(2, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Legendary: spawnedEffect.SetUpEffect(3, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Mutated: spawnedEffect.SetUpEffect(4, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Haunted: spawnedEffect.SetUpEffect(5, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Irradiated: spawnedEffect.SetUpEffect(6, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Nuclear: spawnedEffect.SetUpEffect(7, gunManager.leftGunScript.transform); break;
                        case ItemObject.rarityType.Unique: spawnedEffect.SetUpEffect(8, gunManager.leftGunScript.transform); break;
                    }
                }
            }
            else if (hit.collider.gameObject.TryGetComponent<ShopCrate>(out ShopCrate sc))
            {
                Vector3 hitItem = hit.collider.gameObject.transform.position;

                itemDisplay.SetActive(true);
                itemDisplay.GetComponent<ItemDisplayScript>().InfoUpdate(FindObjByID(sc.id), hitItem);
            }
            else
            {
                itemDisplay.SetActive(false);
            }
            uiManager.ammoDisplayTextHolder.transform.parent.gameObject.SetActive(!itemDisplay.activeSelf);
            uiManager.crosshair.transform.gameObject.SetActive(!itemDisplay.activeSelf);
            if (hit.collider.gameObject.tag == "Interactable")
            {
                ItemContainer ic;
                if (hit.collider.gameObject.TryGetComponent<ItemContainer>(out ic))
                {
                    if (leftItems[128] + rightItems[128] > 0)
                    {
                        ic.eyeballLooking = true;
                        ic.timeSinceEye = 0;
                    }
                }
                HandleInteractableOutline(hit.collider.gameObject, hit, ic);
                if (Input.GetKeyDown(healthManager.gdm.instance.controlsBinds.righInteract) || Input.GetKeyDown(healthManager.gdm.instance.controlsBinds.leftInteract))
                {
                    hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                    if (leftItems[185] + rightItems[185] > 0) { healthManager.GiveEffect(26, 1f); }
                }
            }
        }
        else
        {
            itemDisplay.SetActive(false);
        }
    }
    void HandleInteractableOutline(GameObject obj, RaycastHit hit, ItemContainer ic)
    {
        OutlineScript outlineS; obj.TryGetComponent<OutlineScript>(out outlineS); if (outlineS == null) { return; }
        if (outlineS.disabledObject) { outlineS.ChangeState(OutlineScript.State.disable); return; }
        if (outlineS.dangerousObject) { outlineS.ChangeState(OutlineScript.State.danger); return; }
        if(ic != null)
        {
            outlineS.ChangeState(OutlineScript.State.interactable);
            if (ic.cost > healthManager.money) { outlineS.ChangeState(OutlineScript.State.noMoney); return; }
        }
        else
        {
            outlineS.ChangeState(OutlineScript.State.interactable);
        }
    }
    void MonkeysPaw(int id, string hand, int amt)
    {
        int rarity = FindRarityByID(id);
        int liquidItems = 0;
        if(hand == "left")
        {
            leftItems[id]+=amt;
            for(int i = 0; i < rarityList[rarity].Count; i++)
            { if(leftItems[rarityList[rarity][i]] > 0) { liquidItems += leftItems[rarityList[rarity][i]]; leftItems[rarityList[rarity][i]] = 0; }
            } leftItems[id] += liquidItems; liquidItems = 0;
        }
        else
        {
            rightItems[id]+=amt;
            for (int i = 0; i < rarityList[rarity].Count; i++)
            { if (rightItems[rarityList[rarity][i]] > 0) { liquidItems += rightItems[rarityList[rarity][i]]; rightItems[rarityList[rarity][i]] = 0; }
            } rightItems[id] += liquidItems; liquidItems = 0;
        }
    }
    void UpdateModifierList()
    {
        modifierList.Clear();

        //Mvt
        modifierList.Add(playerMvt.moveSpeed / playerMvt.baseMoveSpeed);
        modifierList.Add(playerMvt.sprintMoveSpeed / playerMvt.baseSprintMoveSpeed);
        modifierList.Add(playerMvt.jumpForce / playerMvt.baseJumpForce);
        modifierList.Add((playerMvt.numberOfJumps + 1f) / (playerMvt.baseNumberOfJumps + 1f));

        //Guns
        //LEFT:
        modifierList.Add(gunManager.leftCritChance / gunManager.masterCritChance);
        modifierList.Add(gunManager.leftCritDamage / gunManager.masterCritDamage);
        modifierList.Add(gunManager.leftWeakPointDamage / gunManager.masterWeakPointDamage);
        modifierList.Add(gunManager.leftDmg / gunManager.masterDmg);
        modifierList.Add(gunManager.leftAtkSpd / gunManager.masterAtkSpd);
        modifierList.Add(gunManager.leftReSpd / gunManager.masterReSpd);
        modifierList.Add(gunManager.leftMagSize / gunManager.masterMagSize);
        modifierList.Add(gunManager.leftAcc / gunManager.masterAcc);
        modifierList.Add(gunManager.leftBulSpd / gunManager.masterBulSpd);
        modifierList.Add(gunManager.leftBulSize / gunManager.masterBulSize);
        modifierList.Add((gunManager.leftBulPir + 1f) / (gunManager.masterBulPir + 1f));

        //RIGHT:
        modifierList.Add(gunManager.rightCritChance / gunManager.masterCritChance);
        modifierList.Add(gunManager.rightCritDamage / gunManager.masterCritDamage);
        modifierList.Add(gunManager.rightWeakPointDamage / gunManager.masterWeakPointDamage);
        modifierList.Add(gunManager.rightDmg / gunManager.masterDmg);
        modifierList.Add(gunManager.rightAtkSpd / gunManager.masterAtkSpd);
        modifierList.Add(gunManager.rightReSpd / gunManager.masterReSpd);
        modifierList.Add(gunManager.rightMagSize / gunManager.masterMagSize);
        modifierList.Add(gunManager.rightAcc / gunManager.masterAcc);
        modifierList.Add(gunManager.rightBulSpd / gunManager.masterBulSpd);
        modifierList.Add(gunManager.rightBulSize / gunManager.masterBulSize);
        modifierList.Add((gunManager.rightBulPir + 1f) / (gunManager.masterBulPir + 1f));

        //Health
        modifierList.Add(healthManager.maxHp / healthManager.baseMaxHP);
        modifierList.Add(healthManager.healthRegen / healthManager.baseHealthRegen);
        modifierList.Add(healthManager.armor / healthManager.baseArmor);

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        if (leftItems[22] == 0)
        {
            leftIFPStatToBuff = 0;
            leftIFPStatToDeBuff = 28;

            for (int i = 0; i < modifierList.Count; i++)
            {
                if (i < 15 || i > 25)
                {
                    if (modifierList[i] < minValue && modifierList[i] != 1f) { minValue = modifierList[i]; leftIFPStatToBuff = i; }
                    if (modifierList[i] > maxValue && modifierList[i] != 1f) { maxValue = modifierList[i]; leftIFPStatToDeBuff = i; }
                }
            }

            if(Random.Range(0, modifierList.Count - 1) < modifierList.Count/2f)
            {
                if (modifierList[leftIFPStatToBuff] == 1) { leftIFPStatToBuff = Random.Range(0, 14); }
                if (modifierList[leftIFPStatToDeBuff] == 1) { leftIFPStatToDeBuff = Random.Range(0, 14); }
            }
            else
            {
                if (modifierList[leftIFPStatToBuff] == 1) { leftIFPStatToBuff = Random.Range(26, modifierList.Count - 1); }
                if (modifierList[leftIFPStatToDeBuff] == 1) { leftIFPStatToDeBuff = Random.Range(26, modifierList.Count - 1); }
            }
            
        }

        minValue = float.MaxValue;
        maxValue = float.MinValue;

        if (rightItems[22] == 0)
        {
            rightIFPStatToBuff = 0;
            rightIFPStatToDeBuff = 28;

            for (int i = 0; i < modifierList.Count; i++)
            {
                if (i < 4 || i > 14)
                {
                    if (modifierList[i] < minValue && modifierList[i] != 1f) { minValue = modifierList[i]; rightIFPStatToBuff = i; }
                    if (modifierList[i] > maxValue && modifierList[i] != 1f) { maxValue = modifierList[i]; rightIFPStatToDeBuff = i; }
                }
            }

            if (Random.Range(0, modifierList.Count - 1) < modifierList.Count / 2f)
            {
                if (modifierList[rightIFPStatToBuff] == 1) { rightIFPStatToBuff = Random.Range(0, 3); }
                if (modifierList[rightIFPStatToDeBuff] == 1) { rightIFPStatToDeBuff = Random.Range(0, 3); }
            }
            else
            {
                if (modifierList[rightIFPStatToBuff] == 1) { rightIFPStatToBuff = Random.Range(15, modifierList.Count - 1); }
                if (modifierList[rightIFPStatToDeBuff] == 1) { rightIFPStatToDeBuff = Random.Range(15, modifierList.Count - 1); }
            }
        }

    }
    public void LowFreqRes()
    {
        if (leftLowFreqRes + rightLowFreqRes < 1) { return; }
        leftLFRAffected.Clear(); rightLFRAffected.Clear();
        List<ItemObject.rarityType> affectedRaritiesLeft = new List<ItemObject.rarityType>();
        List<ItemObject.rarityType> affectedRaritiesRight = new List<ItemObject.rarityType>();
        switch (leftLowFreqRes)
        {
            case 1: affectedRaritiesLeft.Add(ItemObject.rarityType.Common); 
                break;
            case 2: affectedRaritiesLeft.Add(ItemObject.rarityType.Common);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Uncommon);
                break;
            case 3: affectedRaritiesLeft.Add(ItemObject.rarityType.Common);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Uncommon);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Rare);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Mutated);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Haunted);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Irradiated);
                break;
            case >3: affectedRaritiesLeft.Add(ItemObject.rarityType.Common);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Uncommon);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Rare);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Mutated);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Haunted);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Irradiated);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Legendary);
                affectedRaritiesLeft.Add(ItemObject.rarityType.Nuclear);
                break;
        }
        switch (rightLowFreqRes)
        {
            case 1:
                affectedRaritiesRight.Add(ItemObject.rarityType.Common);
                break;
            case 2:
                affectedRaritiesRight.Add(ItemObject.rarityType.Common);
                affectedRaritiesRight.Add(ItemObject.rarityType.Uncommon);
                break;
            case 3:
                affectedRaritiesRight.Add(ItemObject.rarityType.Common);
                affectedRaritiesRight.Add(ItemObject.rarityType.Uncommon);
                affectedRaritiesRight.Add(ItemObject.rarityType.Rare);
                affectedRaritiesRight.Add(ItemObject.rarityType.Mutated);
                affectedRaritiesRight.Add(ItemObject.rarityType.Haunted);
                affectedRaritiesRight.Add(ItemObject.rarityType.Irradiated);
                break;
            case > 3:
                affectedRaritiesRight.Add(ItemObject.rarityType.Common);
                affectedRaritiesRight.Add(ItemObject.rarityType.Uncommon);
                affectedRaritiesRight.Add(ItemObject.rarityType.Rare);
                affectedRaritiesRight.Add(ItemObject.rarityType.Mutated);
                affectedRaritiesRight.Add(ItemObject.rarityType.Haunted);
                affectedRaritiesRight.Add(ItemObject.rarityType.Irradiated);
                affectedRaritiesRight.Add(ItemObject.rarityType.Legendary);
                affectedRaritiesRight.Add(ItemObject.rarityType.Nuclear);
                break;
        }

        foreach (ItemObject itemObj in itemData)
        {
            ItemObject.rarityType rarity = itemObj.rarity;
            int id = itemObj.id;
            if(leftItems[id] > 0 && affectedRaritiesLeft.IndexOf(rarity) != -1) { leftLFRAffected.Add(id); leftItems[id] += 1+leftLowFreqRes; }
            if(rightItems[id] > 0 && affectedRaritiesRight.IndexOf(rarity) != -1) { rightLFRAffected.Add(id); rightItems[id] += 1+rightLowFreqRes; }
        }
    }
    public void LowFreqResCleanup()
    {
        if (leftLFRAffected.Count + rightLFRAffected.Count < 1) { return; }
        if (leftLFRAffected.Count > 0)
        {
            foreach(int affectedItem in leftLFRAffected)
            {
                leftItems[affectedItem] -= 1+leftLowFreqRes;
            }
        }
        if (rightLFRAffected.Count > 0)
        {
            foreach (int affectedItem in rightLFRAffected)
            {
                rightItems[affectedItem] -= 1+rightLowFreqRes;
            }
        }
        leftLFRAffected.Clear();
        rightLFRAffected.Clear();
    }
    public bool FlipCopperCoin()
    {
        int coinCount = copperCoinLeft + copperCoinRight;

        for (int i = coinCount; i > 0; i--) { if (Random.Range(0, 2) == 0) { return true; } }
        return false;
    }
    public bool RandomItemEffectRoll(bool condition)
    {
        if (copperCoinLeft + copperCoinRight > 0)
        {
            return FlipCopperCoin();
        }
        else
        {
            return condition;
        }
    }
    public void SpawnItem(int id, bool overrideID, int rarity, bool overrideRarity)
    {
        int spawnedRarity = 0;
        if(!overrideID && !overrideRarity)
        {
            int rand = Random.Range(1, 101);
            int rarityID = 0;
            bool limestoneScale = leftItems[178] + rightItems[178] > 0;
            if (limestoneScale)
            {
                rarityID = ItemRarity.GetUnWeightedRandRarity();
            }
            else
            {
                rarityID = ItemRarity.GetWeightedRandRarity();
            }
            spawnedRarity = rarityID;
        }
        else if (overrideRarity) { spawnedRarity = rarity; }

        GameObject spawnedItem = Instantiate(itemPos);
        spawnedItem.transform.position = transform.position + transform.forward * 1.2f;
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(spawnedRarity, false);
        if (overrideID) 
        { 
            spawnedItem.GetComponent<ItemPossibility>().overrideid = true; 
            spawnedItem.GetComponent<ItemPossibility>().idover = id;
            spawnedItem.GetComponent<ItemPossibility>().SetRarity(FindRarityByID(id), false);
        }
        spawnedItem.GetComponent<Rigidbody>().AddForce((Vector3.up * 10f)+(transform.forward), ForceMode.Impulse);
        spawnedItem.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-1f,1f), ForceMode.Impulse);
        spawnedItem.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-1f,1f), ForceMode.Impulse);
    }
    public bool MasterCardCheck()
    {
        if(masterCard < 1) { return false; }
        if (RandomItemEffectRoll(Random.Range(1, 100) < masterCardChance)) { return true; }
        else { masterCardChance += 4f + masterCard; return false; }
    }
    public ItemObject FindObjByID(int id)
    {
        //return Resources.Load<ItemObject>("Items/" + id.ToString()); // <- Old way (expensive)
        return itemData[id]; // <- awesome new way
    }
    public int FindRarityByID(int id)
    {
        return itemData[id].rarity.GetHashCode();
    }
    public Vector2 GetCooldownInfo(int id, string hand)
    {
        //x is cur y is max
        if(hand == "left")
        {
            switch (id)
            {
                case 14: return new Vector2(gunManager.leftMutatedCellTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 10f + 1f));
                case 24: return new Vector2(gunManager.leftHungryParasiteTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 2f + 1f));
                case 33: return new Vector2(gunManager.leftFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * leftItems[id]));
                case 42: return new Vector2(gunManager.leftSponTimer, FindObjByID(id).baseCooldown);
                case 58: return new Vector2(FindObjByID(id).baseCooldown - gunManager.surpriseEggTimer, FindObjByID(id).baseCooldown);
                case 71: return new Vector2(gunManager.axeCooldown, FindObjByID(id).baseCooldown);
                case 88: return new Vector2(FindObjByID(id).baseCooldown - gunManager.leftPrinterTimer, FindObjByID(id).baseCooldown);
                case 103: return new Vector2(gunManager.leftGunScript.sniperTowerCooldown, FindObjByID(id).baseCooldown);
                case 106: return new Vector2(gunManager.leftGunScript.pumpShotgunAttachTimer, FindObjByID(id).baseCooldown);
                case 107: return new Vector2(gunManager.leftGunScript.grenadeAttachTimer, FindObjByID(id).baseCooldown);
                case 108: return new Vector2(gunManager.leftGunScript.gasGrenadeAttachTimer, FindObjByID(id).baseCooldown);
                case 170: return new Vector2(gunManager.centriCheckTimer, FindObjByID(id).baseCooldown);
                case 17: return new Vector2(healthManager.orgGumTimer, FindObjByID(id).baseCooldown);
                case 114: return new Vector2(healthManager.chickenCoopTimer, FindObjByID(id).baseCooldown);
                case 155: return new Vector2(healthManager.divineTimer, (FindObjByID(id).baseCooldown / 2f) + (60f / healthManager.divineInter));
                case 186: return new Vector2(healthManager.sunflowerTimer, FindObjByID(id).baseCooldown);
            }
        }
        else
        {
            switch (id)
            {
                case 14: return new Vector2(gunManager.rightMutatedCellTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 10f + 1f));
                case 24: return new Vector2(gunManager.rightHungryParasiteTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 2f + 1f));
                case 33: return new Vector2(gunManager.rightFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * rightItems[id]));
                case 42: return new Vector2(gunManager.rightSponTimer, FindObjByID(id).baseCooldown);
                case 58: return new Vector2(FindObjByID(id).baseCooldown - gunManager.surpriseEggTimer, FindObjByID(id).baseCooldown);
                case 71: return new Vector2(gunManager.axeCooldown, FindObjByID(id).baseCooldown);
                case 88: return new Vector2(FindObjByID(id).baseCooldown - gunManager.rightPrinterTimer, FindObjByID(id).baseCooldown);
                case 103: return new Vector2(gunManager.rightGunScript.sniperTowerCooldown, FindObjByID(id).baseCooldown);
                case 106: return new Vector2(gunManager.rightGunScript.pumpShotgunAttachTimer, FindObjByID(id).baseCooldown);
                case 107: return new Vector2(gunManager.rightGunScript.grenadeAttachTimer, FindObjByID(id).baseCooldown);
                case 108: return new Vector2(gunManager.rightGunScript.gasGrenadeAttachTimer, FindObjByID(id).baseCooldown);
                case 170: return new Vector2(gunManager.centriCheckTimer, FindObjByID(id).baseCooldown);
                case 17: return new Vector2(healthManager.orgGumTimer, FindObjByID(id).baseCooldown);
                case 114: return new Vector2(healthManager.chickenCoopTimer, FindObjByID(id).baseCooldown);
                case 155: return new Vector2(healthManager.divineTimer, (FindObjByID(id).baseCooldown / 2f) + (60f / healthManager.divineInter));
                case 186: return new Vector2(healthManager.sunflowerTimer, FindObjByID(id).baseCooldown);
            }
        }
        return Vector2.zero;
    }
}

