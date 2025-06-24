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

    public int lastItemPressed;
    public string lastItemPressedHand;

    public GameObject itemPos;
    private void Awake()
    {
        itemData = new List<ItemObject>();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));

        uiManager = gameObject.GetComponent<UIManager>();

        LoadRarites();
        LoadCategories();
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
        leftSnapshot = new List<int>();
        leftSnapshot.AddRange(leftItems);
        rightSnapshot = new List<int>();
        rightSnapshot.AddRange(rightItems);

        playerMvt.StatUpdate(leftItems, rightItems, rarityList);
        healthManager.StatUpdate(leftItems, rightItems, rarityList);
        gunManager.StatUpdate(leftItems, rightItems, rarityList);


        LookForItem();

        UpdateModifierList();

        CheckForMerge();
    }
    private void LateUpdate()
    {
        for(int i = 0; i < leftItems.Count; i++)
        {
            leftSnapshot[i] = leftItems[i] - leftSnapshot[i];
            if (leftSnapshot[i] < 0) { OnItemDestroy(i, leftSnapshot[i], "left"); }
            if (leftSnapshot[i] > 0) { OnItemGain(i, leftSnapshot[i], "left"); }
        }
        for (int i = 0; i < rightItems.Count; i++)
        {
            rightSnapshot[i] = rightItems[i] - rightSnapshot[i];
            if(rightSnapshot[i] < 0) { OnItemDestroy(i, rightSnapshot[i], "right");  }
            if(rightSnapshot[i] > 0) { OnItemGain(i, rightSnapshot[i], "right");  }
        }

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
    }
    public void ItemPressedInInv(int id, string hand)
    {
        if(lastItemPressed == 74 && lastItemPressedHand == hand)
        {
            if(hand == "left" && leftItems[74] < 1) { return; }
            if(hand == "right" && rightItems[74] < 1) { return; }
            NuclearFission(id, hand);
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
            if (rarityList[1].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//uncommon
            if (rarityList[2].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 1, hand); }//rare
            if (rarityList[3].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 2, hand); AddRandItemsFromRarity(1, 7, hand); }//legendary
            if (rarityList[4].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//mutated
            if (rarityList[5].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//haunted
            if (rarityList[6].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(1, 0, hand); AddRandItemsFromRarity(1, 4, hand); }//irradiated
            if (rarityList[7].Contains(id)) { leftItems[id]--; AddRandItemsFromRarity(3, 6, hand); AddRandItemsFromRarity(1, 5, hand); }//nuclear
            if (rarityList[8].Contains(id)) { leftItems[id]--; }//unique
        }
        else
        {
            if (rarityList[0].Contains(id)) { rightItems[id]--; }//common
            if (rarityList[1].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//uncommon
            if (rarityList[2].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 1, hand); }//rare
            if (rarityList[3].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 2, hand); AddRandItemsFromRarity(1, 7, hand); }//legendary
            if (rarityList[4].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//mutated
            if (rarityList[5].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(2, 0, hand); }//haunted
            if (rarityList[6].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(1, 0, hand); AddRandItemsFromRarity(1, 4, hand); }//irradiated
            if (rarityList[7].Contains(id)) { rightItems[id]--; AddRandItemsFromRarity(3, 6, hand); AddRandItemsFromRarity(1, 5, hand); }//nuclear
            if (rarityList[8].Contains(id)) { rightItems[id]--; }//unique
        }
    }
    void AddRandItemsFromRarity(int amount, int rarity, string hand)
    {
        if(hand == "left")
        {
            for (int i = 0; i < amount; i++)
            {
                int rand = Random.Range(0, rarityList[rarity].Count);
                leftItems[rarityList[rarity][rand]]++;
                OnItemGain(rarityList[rarity][rand], 1, hand);
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                int rand = Random.Range(0, rarityList[rarity].Count);
                rightItems[rarityList[rarity][rand]]++;
                OnItemGain(rarityList[rarity][rand], 1, hand);
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
                    rightItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (leftItems[86] > 0) { leftItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1; leftItems[86]--; }
                }
                if (Input.GetKeyDown(KeyCode.Q) || (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.LeftShift)))
                {
                    leftItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                    if (rightItems[86] > 0) { rightItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1; rightItems[86]--; }
                }
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
    public void SpawnItem(int id, bool overrideID, int rarity, bool overrideRarity)
    {
        int spawnedRarity = 0;
        if(!overrideID && !overrideRarity)
        {
            int rand = Random.Range(1, 101);
            if (rand < 71) { spawnedRarity = 0; }
            if (rand < 91 && rand > 70) { spawnedRarity = 0; }
            if (rand == 91 || rand == 92) { spawnedRarity = 2; }
            if (rand == 93 || rand == 94) { spawnedRarity = 4; }
            if (rand == 95 || rand == 96) { spawnedRarity = 5; }
            if (rand == 97 || rand == 98) { spawnedRarity = 6; }
            if (rand == 99) { spawnedRarity = 3; }
            if (rand == 100) { spawnedRarity = 7; }
        }
        else if (overrideRarity) { spawnedRarity = rarity; }

        GameObject spawnedItem = Instantiate(itemPos);
        spawnedItem.transform.position = transform.position + transform.forward * 1.2f;
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(spawnedRarity);
        if (overrideID) 
        { 
            spawnedItem.GetComponent<ItemPossibility>().overrideid = true; 
            spawnedItem.GetComponent<ItemPossibility>().idover = id;
            for(int i = 0; i < rarityList.Count; i++)
            {
                if (rarityList[i].Contains(id)) { spawnedItem.GetComponent<ItemPossibility>().SetRarity(i); }
            }
        }
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }
    public ItemObject FindObjByID(int id)
    {
        return Resources.Load<ItemObject>("Items/" + id.ToString());
    }
    public Vector2 GetCooldownInfo(int id, string hand)
    {
        //x is cur y is max
        Vector2 info = Vector2.zero;

        //gun manager
        if(id == 14 && hand == "left") { info = new Vector2(gunManager.leftMutatedCellTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 10f + 1f)); }//mutatedCell
        if(id == 14 && hand == "right") { info = new Vector2(gunManager.rightMutatedCellTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 10f + 1f)); }//mutatedCell
        if(id == 24 && hand == "left") { info = new Vector2(gunManager.leftHungryParasiteTimer, FindObjByID(id).baseCooldown / (leftItems[id] / 2f + 1f)); }//hungryhungryparasite
        if(id == 24 && hand == "right") { info = new Vector2(gunManager.rightHungryParasiteTimer, FindObjByID(id).baseCooldown / (rightItems[id] / 2f + 1f)); }//hungryhungryparasite
        if(id == 33 && hand == "left") { info = new Vector2(gunManager.leftFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * leftItems[id])); }//fastinserter
        if(id == 33 && hand == "right") { info = new Vector2(gunManager.rightFastInserterTimer, FindObjByID(id).baseCooldown / (0.2f * rightItems[id])); }//fastinserter
        if(id == 42 && hand == "left") { info = new Vector2(gunManager.leftSponTimer, FindObjByID(id).baseCooldown); }//spondeal
        if(id == 42 && hand == "right") { info = new Vector2(gunManager.rightSponTimer, FindObjByID(id).baseCooldown); }//spondeal
        if(id == 58) { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.surpriseEggTimer, FindObjByID(id).baseCooldown); }//lifetime egg
        if(id == 71) { info = new Vector2(gunManager.axeCooldown, FindObjByID(id).baseCooldown); }//gunky axe
        if(id == 88 && hand == "left") { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.leftPrinterTimer, FindObjByID(id).baseCooldown); }//printer
        if(id == 88 && hand == "right") { info = new Vector2(FindObjByID(id).baseCooldown - gunManager.rightPrinterTimer, FindObjByID(id).baseCooldown); }//printer
        if(id == 103 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().sniperTowerCooldown, FindObjByID(id).baseCooldown); }//sniper
        if(id == 103 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().sniperTowerCooldown, FindObjByID(id).baseCooldown); }//sniper
        if(id == 106 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().pumpShotgunAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        if(id == 106 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().pumpShotgunAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        if(id == 107 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().grenadeAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        if(id == 107 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().grenadeAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        if(id == 108 && hand == "left") { info = new Vector2(gunManager.leftHand.transform.GetChild(0).GetComponent<GunScript>().gasGrenadeAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        if(id == 108 && hand == "right") { info = new Vector2(gunManager.rightHand.transform.GetChild(0).GetComponent<GunScript>().gasGrenadeAttachTimer, FindObjByID(id).baseCooldown); }//sniper
        // health manager
        if(id == 17) { info = new Vector2(healthManager.orgGumTimer, FindObjByID(id).baseCooldown); }//organic gumball
        if(id == 114) { info = new Vector2(healthManager.chickenCoopTimer, FindObjByID(id).baseCooldown); }//chickencoop

        //Debug.Log("ID: " + id + " | " + info);
        return info;
    }
}

