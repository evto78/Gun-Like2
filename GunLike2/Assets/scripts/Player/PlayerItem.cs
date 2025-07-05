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

    public List<int> gunLike1Items = new List<int>();
    public List<int> sponserItems = new List<int>();
    public List<int> fishItems = new List<int>();
    public List<int> unstableItems = new List<int>();
    public List<int> cooldownItems = new List<int>();
    public List<int> horrorItems = new List<int>();
    [Header("Manager scripts")]
    public NEWPlayerMovement playerMvt;
    public HealthManager healthManager;
    public GunManager gunManager;
    UIManager uiManager;
    public PopupItemUI popupUI;

    public Transform playerCamera;

    public GameObject itemDisplay;
    [Header("Item Checks")]
    //ItemChecks
    public int leftIFPStatToBuff;
    public int leftIFPStatToDeBuff;
    public int rightIFPStatToBuff;
    public int rightIFPStatToDeBuff;

    List<int> leftSnapshot;
    List<int> rightSnapshot;

    public int gotchaTickets;
    public int leftLowFreqRes; List<int> leftLFRAffected = new List<int>();
    public int rightLowFreqRes; List<int> rightLFRAffected = new List<int>();
    public int masterCard;
    public float masterCardChance;

    public int lastItemPressed;
    public string lastItemPressedHand;

    public GameObject itemPos;

    bool changedLastFrame;
    private void Awake()
    {
        itemData = new List<ItemObject>();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
        SortItemData();

        uiManager = gameObject.GetComponent<UIManager>();

        LoadRarites();
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
    void LoadRarites()
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
        foreach(ItemObject item in itemData)
        {
            if(item.rarity.ToString() == "Common") { commonItems.Add(item.id); }
            if(item.rarity.ToString() == "Uncommon") { uncommonItems.Add(item.id); }
            if(item.rarity.ToString() == "Rare") { rareItems.Add(item.id); }
            if(item.rarity.ToString() == "Legendary") { legendaryItems.Add(item.id); }
            if(item.rarity.ToString() == "Mutated") { mutatedItems.Add(item.id); }
            if(item.rarity.ToString() == "Haunted") { hauntedItems.Add(item.id); }
            if(item.rarity.ToString() == "Irradiated") { irradiatedItems.Add(item.id); }
            if(item.rarity.ToString() == "Nuclear") { nuclearItems.Add(item.id); }
            if(item.rarity.ToString() == "Unique") { uniqueItems.Add(item.id); }
        }
        rarityList.InsertRange(0, new List<int>[] { commonItems, uncommonItems, rareItems, legendaryItems, mutatedItems, hauntedItems, irradiatedItems, nuclearItems, uniqueItems });
    }
    void LoadCategories()
    {
        foreach(ItemObject item in itemData)
        {
            if(item.subType.ToString() == "classic") { gunLike1Items.Add(item.id); }
            if(item.subType.ToString() == "sponser") { sponserItems.Add(item.id); }
            if(item.subType.ToString() == "fish") { fishItems.Add(item.id); }
            if(item.subType.ToString() == "unstablePart") { unstableItems.Add(item.id); }
            if (item.cooldownItem) { cooldownItems.Add(item.id); }
            if(item.subType.ToString() == "horror") { horrorItems.Add(item.id); }
        }
    }
    private void Update()
    {
        //Stat Update
        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(rightItems);
        masterCard = leftItems[174] + rightItems[174];
        leftLowFreqRes = leftItems[154];rightLowFreqRes = rightItems[154];
        LowFreqRes();
        playerMvt.StatUpdate(leftItems, rightItems, rarityList);
        healthManager.StatUpdate(leftItems, rightItems, rarityList);
        gunManager.StatUpdate(leftItems, rightItems, rarityList);
        LowFreqResCleanup();
        //After Stat Update
        LookForItem();

        UpdateModifierList();

        CheckForMerge();
    }
    private void LateUpdate()
    {
        bool change = false;
        for(int i = 0; i < leftItems.Count; i++)
        {
            leftSnapshot[i] = leftItems[i] - leftSnapshot[i];
            if (leftSnapshot[i] < 0) { OnItemDestroy(i, leftSnapshot[i], "left"); change = true; }
            if (leftSnapshot[i] > 0) { OnItemGain(i, leftSnapshot[i], "left"); change = true; }
        }
        for (int i = 0; i < rightItems.Count; i++)
        {
            rightSnapshot[i] = rightItems[i] - rightSnapshot[i];
            if(rightSnapshot[i] < 0) { OnItemDestroy(i, rightSnapshot[i], "right"); change = true; }
            if(rightSnapshot[i] > 0) { OnItemGain(i, rightSnapshot[i], "right"); change = true; }
        }
        if (change) { changedLastFrame = true; }

        if (change || (!change && changedLastFrame)) { uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();  if (!change) { changedLastFrame = false; } }
    }
    public void OnItemDestroy(int id, int amount, string hand)
    {
        popupUI.CreateNotif(id, amount);
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
        popupUI.CreateNotif(id, amount);
        if(id == 69 || id == 70 || id == 71 || id == 72)
        {
            uiManager.VisionOfGunky();
        }
        uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
        for (int i = 0; i < amount; i++)
        {
            if (Random.Range(1, 100) < 5 && leftItems[97] > 0 && hand == "left") { leftItems[97]--; }
            if (Random.Range(1, 100) < 5 && leftItems[98] > 0 && hand == "left") { leftItems[98]--; }
            if (Random.Range(1, 100) < 5 && leftItems[99] > 0 && hand == "left") { leftItems[99]--; }
            if (Random.Range(1, 100) < 5 && leftItems[100] > 0 && hand == "left") { leftItems[100]--; }
            if (Random.Range(1, 100) < 5 && rightItems[97] > 0 && hand == "right") { rightItems[97]--; }
            if (Random.Range(1, 100) < 5 && rightItems[98] > 0 && hand == "right") { rightItems[98]--; }
            if (Random.Range(1, 100) < 5 && rightItems[99] > 0 && hand == "right") { rightItems[99]--; }
            if (Random.Range(1, 100) < 5 && rightItems[100] > 0 && hand == "right") { rightItems[100]--; }
        }
        if(id == 175)
        {
            if(hand == "left"){for(int i = 0; i < 50 * amount; i++) { gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().addBullet(); }}
            if(hand == "right"){for(int i = 0; i < 50 * amount; i++) { gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().addBullet(); }}
        }
    }
    public void ItemPressedInInv(int id, string hand)
    {
        if(lastItemPressed == 74 && lastItemPressedHand == hand)
        {
            if(hand == "left" && leftItems[74] < 1) { return; }
            if(hand == "right" && rightItems[74] < 1) { return; }
            NuclearFission(id, hand);
        }
        if(lastItemPressed == 171 && lastItemPressedHand == hand)
        {
            if(hand == "left" && gunManager.leftOverCompress < 1) { return; }
            if(hand == "right" && gunManager.rightOverCompress < 1) { return; }
            OverridenCompressor(id, hand);
        }
        lastItemPressed = id;
        lastItemPressedHand = hand;
        uiManager.inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
    }
    void NuclearFission(int id, string hand)
    {
        OnItemDestroy(id, 1, hand);
        if(hand == "left")
        {
            if (rarityList[0].Contains(id)) { leftItems[id]--; }//common
            if (rarityList[1].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//uncommon
            if (rarityList[2].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 1, hand, true); }//rare
            if (rarityList[3].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 2, hand, true); AddRandItemsFromRarity(1, 7, hand, true); }//legendary
            if (rarityList[4].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//mutated
            if (rarityList[5].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//haunted
            if (rarityList[6].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(1, 0, hand, true); AddRandItemsFromRarity(1, 4, hand, true); }//irradiated
            if (rarityList[7].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(3, 6, hand, true); AddRandItemsFromRarity(1, 5, hand, true); }//nuclear
        }
        else
        {
            if (rarityList[0].Contains(id)) { rightItems[id]--; }//common
            if (rarityList[1].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//uncommon
            if (rarityList[2].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 1, hand, true); }//rare
            if (rarityList[3].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 2, hand, true); AddRandItemsFromRarity(1, 7, hand, true); }//legendary
            if (rarityList[4].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//mutated
            if (rarityList[5].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand, true); }//haunted
            if (rarityList[6].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(1, 0, hand, true); AddRandItemsFromRarity(1, 4, hand, true); }//irradiated
            if (rarityList[7].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(3, 6, hand, true); AddRandItemsFromRarity(1, 5, hand, true); }//nuclear
        }
    }
    void OverridenCompressor(int id, string hand)
    {
        int raritySelected = FindRarityByID(id);
        
        if(hand == "left")
        {
            int sumOfItemsFromRarity = 0;
            foreach (int i in rarityList[raritySelected])
            {
                sumOfItemsFromRarity += leftItems[i];
            }
            if (sumOfItemsFromRarity < 3) { return; }

            int itemsRequired;
            if(gunManager.leftOverCompress>1 && Random.Range(1, 100) < 20 * (gunManager.leftOverCompress - 1))
            {itemsRequired = 1;} else{itemsRequired = 2;}
            int attempts = 0;
            while (itemsRequired > 0 && attempts < 100)
            {
                int randId = rarityList[raritySelected][Random.Range(0, rarityList[raritySelected].Count)];
                if (leftItems[randId] > 0 && randId != id) { leftItems[randId]--; itemsRequired--; }
                attempts++;
            }
            if(itemsRequired > 0) { return; }
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
            if (gunManager.rightOverCompress > 1 && Random.Range(1, 100) < 20 * (gunManager.rightOverCompress - 1))
            { itemsRequired = 1; }
            else { itemsRequired = 2; }
            int attempts = 0;
            while (itemsRequired > 0 && attempts < 100)
            {
                int randId = rarityList[raritySelected][Random.Range(0, rarityList[raritySelected].Count)];
                if (rightItems[randId] > 0 && randId != id) { rightItems[randId]--; itemsRequired--; }
                attempts++;
            }
            if(itemsRequired > 0) { return; }
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
                if (createPopUp) { OnItemGain(rarityList[rarity][rand], 1, hand); }
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                int rand = Random.Range(0, rarityList[rarity].Count);
                rightItems[rarityList[rarity][rand]]++;
                if (createPopUp) { OnItemGain(rarityList[rarity][rand], 1, hand); }
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

                hit.collider.gameObject.GetComponentInParent<Item>().StayStill();

                if (Input.GetKeyDown(KeyCode.E) || (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.LeftShift)))
                {
                    int id = hit.collider.gameObject.GetComponentInParent<Item>().WhatItem();
                    rightItems[id] += 1;
                    if (rightMonkeysPaw > 0) { MonkeysPaw(id, "right", rightMonkeysPaw); }
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (leftItems[86] > 0) { leftItems[id] += 1; leftItems[86]--; }
                }
                if (Input.GetKeyDown(KeyCode.Q) || (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)))
                {
                    int id = hit.collider.gameObject.GetComponentInParent<Item>().WhatItem();
                    leftItems[id] += 1;
                    if (leftMonkeysPaw > 0) { MonkeysPaw(id, "left", leftMonkeysPaw); }
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (rightItems[86] > 0) { rightItems[id] += 1; rightItems[86]--; }
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
            if(hit.collider.gameObject.tag == "Interactable")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                    healthManager.GiveEffect("chaosEngine", 1f);
                }
                if(hit.collider.gameObject.TryGetComponent<ItemContainer>(out ItemContainer ic))
                {
                    if (leftItems[128] + rightItems[128] > 0)
                    {
                        ic.eyeballLooking = true;
                        ic.timeSinceEye = 0;
                    }
                }
            }
        }
        else
        {
            itemDisplay.SetActive(false);
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
        if(leftLowFreqRes + rightLowFreqRes < 1) { return; }
        leftLFRAffected.Clear();
        rightLFRAffected.Clear();
        foreach(ItemObject itemObj in itemData)
        {
            ItemObject.rarityType rarity = itemObj.rarity;
            if (leftItems[itemObj.id] > 0)
            {
                
                if (leftLowFreqRes > 0)
                {
                    if (leftLowFreqRes > 0 && rarity == ItemObject.rarityType.Common) { leftLFRAffected.Add(itemObj.id); leftItems[itemObj.id] += leftLowFreqRes; }
                    if (leftLowFreqRes > 1 && rarity == ItemObject.rarityType.Uncommon) { leftLFRAffected.Add(itemObj.id); leftItems[itemObj.id] += leftLowFreqRes; }
                    if (leftLowFreqRes > 2 && (rarity == ItemObject.rarityType.Rare || rarity == ItemObject.rarityType.Unique)) { leftLFRAffected.Add(itemObj.id); leftItems[itemObj.id] += leftLowFreqRes; }
                    if (leftLowFreqRes > 3 && (rarity == ItemObject.rarityType.Mutated || rarity == ItemObject.rarityType.Haunted || rarity == ItemObject.rarityType.Irradiated)) { leftLFRAffected.Add(itemObj.id); leftItems[itemObj.id] += leftLowFreqRes; }
                    if (leftLowFreqRes > 4 && (rarity == ItemObject.rarityType.Legendary || rarity == ItemObject.rarityType.Nuclear) && itemObj.id != 154) { leftLFRAffected.Add(itemObj.id); leftItems[itemObj.id] += leftLowFreqRes; }
                }
            }
            if (rightItems[itemObj.id] > 0) 
            {
                if (rightLowFreqRes > 0)
                {
                    if (rightLowFreqRes > 0 && rarity == ItemObject.rarityType.Common) { rightLFRAffected.Add(itemObj.id); rightItems[itemObj.id] += rightLowFreqRes; }
                    if (rightLowFreqRes > 1 && rarity == ItemObject.rarityType.Uncommon) { rightLFRAffected.Add(itemObj.id); rightItems[itemObj.id] += rightLowFreqRes; }
                    if (rightLowFreqRes > 2 && (rarity == ItemObject.rarityType.Rare || rarity == ItemObject.rarityType.Unique)) { rightLFRAffected.Add(itemObj.id); rightItems[itemObj.id] += rightLowFreqRes; }
                    if (rightLowFreqRes > 3 && (rarity == ItemObject.rarityType.Mutated || rarity == ItemObject.rarityType.Haunted || rarity == ItemObject.rarityType.Irradiated)) { rightLFRAffected.Add(itemObj.id); rightItems[itemObj.id] += rightLowFreqRes; }
                    if (rightLowFreqRes > 4 && (rarity == ItemObject.rarityType.Legendary || rarity == ItemObject.rarityType.Nuclear) && itemObj.id != 154) { rightLFRAffected.Add(itemObj.id); rightItems[itemObj.id] += rightLowFreqRes; }
                }
            }
        }
    }
    public void LowFreqResCleanup()
    {
        if (leftLFRAffected.Count + rightLFRAffected.Count < 1) { return; }
        if (leftLFRAffected.Count > 0)
        {
            foreach(int affectedItem in leftLFRAffected)
            {
                leftItems[affectedItem] -= leftLowFreqRes;
            }
        }
        if (rightLFRAffected.Count > 0)
        {
            foreach (int affectedItem in rightLFRAffected)
            {
                rightItems[affectedItem] -= rightLowFreqRes;
            }
        }
        leftLFRAffected.Clear();
        rightLFRAffected.Clear();
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
                rand = Random.Range(0, 8);
                rarityID = rand;
            }
            else
            {
                if (rand < 71) { rarityID = 0; }
                if (rand < 91 && rand > 70) { rarityID = 1; }
                if (rand == 91 || rand == 92) { rarityID = 2; }
                if (rand == 93 || rand == 94) { rarityID = 4; }
                if (rand == 95 || rand == 96) { rarityID = 5; }
                if (rand == 97 || rand == 98) { rarityID = 6; }
                if (rand == 99) { rarityID = 3; }
                if (rand == 100) { rarityID = 7; }
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
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }
    public bool MasterCardCheck()
    {
        if(masterCard < 1) { return false; }
        if (Random.Range(1, 100) < masterCardChance) { return true; }
        else { masterCardChance += 4f + masterCard; return false; }
    }
    public ItemObject FindObjByID(int id)
    {
        //return Resources.Load<ItemObject>("Items/" + id.ToString()); // <- Old way (expensive)
        return itemData[id]; // <- awesome new way
    }
    public int FindRarityByID(int id)
    {
        for(int i = 0; i < rarityList.Count; i++)
        {
            if (rarityList[i].Contains(id)) { return i; }
        }
        return 0;
    }
    public Vector2 GetCooldownInfo(int id, string hand)
    {
        //x is cur y is max
        Vector2 info = Vector2.zero;

        //gun manager
        if(id == 14 && hand == "left") { info = new Vector2(gunManager.leftMutatedCellTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 10f + 1f)); }//mutatedCell
        else if(id == 14 && hand == "right") { info = new Vector2(gunManager.rightMutatedCellTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 10f + 1f)); }//mutatedCell
        else if (id == 24 && hand == "left") { info = new Vector2(gunManager.leftHungryParasiteTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 2f + 1f)); }//hungryhungryparasite
        else if (id == 24 && hand == "right") { info = new Vector2(gunManager.rightHungryParasiteTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 2f + 1f)); }//hungryhungryparasite
        else if (id == 33 && hand == "left") { info = new Vector2(gunManager.leftFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * leftItems[id])); }//fastinserter
        else if (id == 33 && hand == "right") { info = new Vector2(gunManager.rightFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * rightItems[id])); }//fastinserter
        else if (id == 42 && hand == "left") { info = new Vector2(gunManager.leftSponTimer, FindObjByID(id).baseCooldown); }//spondeal
        else if (id == 42 && hand == "right") { info = new Vector2(gunManager.rightSponTimer, FindObjByID(id).baseCooldown); }//spondeal
        else if (id == 58) { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.surpriseEggTimer, FindObjByID(id).baseCooldown); }//lifetime egg
        else if (id == 71) { info = new Vector2(gunManager.axeCooldown, FindObjByID(id).baseCooldown); }//gunky axe
        else if (id == 88 && hand == "left") { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.leftPrinterTimer, FindObjByID(id).baseCooldown); }//printer
        else if (id == 88 && hand == "right") { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.rightPrinterTimer, FindObjByID(id).baseCooldown); }//printer
        else if (id == 103 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().sniperTowerCooldown, FindObjByID(id).baseCooldown); }//sniper
        else if (id == 103 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().sniperTowerCooldown, FindObjByID(id).baseCooldown); }//sniper
        else if (id == 106 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().pumpShotgunAttachTimer, FindObjByID(id).baseCooldown); }//pumpshotgun
        else if (id == 106 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().pumpShotgunAttachTimer, FindObjByID(id).baseCooldown); }//pumpshotgun
        else if (id == 107 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().grenadeAttachTimer, FindObjByID(id).baseCooldown); }//grenade
        else if (id == 107 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().grenadeAttachTimer, FindObjByID(id).baseCooldown); }//grenade
        else if (id == 108 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().gasGrenadeAttachTimer, FindObjByID(id).baseCooldown); }//gasGrenade
        else if (id == 108 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().gasGrenadeAttachTimer, FindObjByID(id).baseCooldown); }//gasGrenade
        else if (id == 170) { info = new Vector2(gunManager.centriCheckTimer, FindObjByID(id).baseCooldown); }//overCentri
        // health manager
        else if (id == 17) { info = new Vector2(healthManager.orgGumTimer, FindObjByID(id).baseCooldown); }//organic gumball
        else if (id == 114) { info = new Vector2(healthManager.chickenCoopTimer, FindObjByID(id).baseCooldown); }//chickencoop
        else if (id == 155) { info = new Vector2(healthManager.divineTimer, (FindObjByID(id).baseCooldown/2f) +(60f/healthManager.divineInter)); }//divine intervention

        //Debug.Log("ID: " + id + " | " + info);
        return info;
    }
}

