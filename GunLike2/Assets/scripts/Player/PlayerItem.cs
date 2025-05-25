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
    [Header("Manager scripts")]
    public NEWPlayerMovement playerMvt;
    public HealthManager healthManager;
    public GunManager gunManager;
    UIManager uiManager;
    public PopupItemUI popupUI;

    public Transform playerCamera;

    public GameObject itemDisplay;
    [Header("Inventory system :(")]
    public List<Vector4> leftInventory;
    public List<Vector4> rightInventory;
    // X = id of item
    // Y = amount of item
    // z = rarity of item
    [Header("Item Checks")]
    //ItemChecks
    public int leftIFPStatToBuff;
    public int leftIFPStatToDeBuff;
    public int rightIFPStatToBuff;
    public int rightIFPStatToDeBuff;

    List<int> leftSnapshot;
    List<int> rightSnapshot;

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
    }
    public void OnItemGain(int id, int amount, string hand)
    {
        popupUI.CreateNotif(id, amount);
        if(id == 69 || id == 70 || id == 71 || id == 72)
        {
            uiManager.VisionOfGunky();
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

                if (Input.GetKeyDown(KeyCode.E))
                {
                    rightItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    leftItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    hit.collider.gameObject.GetComponentInParent<Item>().Taken();
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
            }
        }
        else
        {
            itemDisplay.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        leftInventory.Clear();
        rightInventory.Clear();

        for (int i = 0; i < leftItems.Count; i++)
        {
            if(leftItems[i] > 0)
            {
                leftInventory.Add(new Vector4(i, leftItems[i], CheckRarity(i)));
            }
        }

        for (int i = 0; i < rightItems.Count; i++)
        {
            if (rightItems[i] > 0)
            {
                rightInventory.Add(new Vector4(i, rightItems[i], CheckRarity(i)));
            }
        }

        GetComponentInChildren<InventoryScript>().leftInventory = leftInventory;
        GetComponentInChildren<InventoryScript>().rightInventory = rightInventory;
    }
    
    int CheckRarity(int id)
    {
        for (int i = 0; i < rarityList.Count; i++)
        {
            if (rarityList[i].Contains(id))
            {
                return i;
            }
        }

        return 0;
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
}

