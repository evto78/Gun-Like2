using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerItem : MonoBehaviour
{
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

    public NEWPlayerMovement playerMvt;
    public HealthManager healthManager;
    public GunManager gunManager;

    public Transform playerCamera;

    public GameObject itemDisplay;

    public List<Vector4> leftInventory;
    public List<Vector4> rightInventory;
    // X = id of item
    // Y = amount of item
    // z = rarity of item

    //ItemChecks
    public int leftIFPStatToBuff;
    public int leftIFPStatToDeBuff;
    public int rightIFPStatToBuff;
    public int rightIFPStatToDeBuff;

    private void Awake()
    {
        commonItems.InsertRange(0, new int[] { 0, 1, 2, 4, 6, 7, 8, 9, 60, 61, 62, 63, 64, 65, 78, 94, 173, 174, 175, 177 });
        uncommonItems.InsertRange(0, new int[] { 3, 10, 11, 12, 25, 28, 31, 32, 34, 35, 45, 47, 55, 114, 122, 148, 149, 150, 162, 165 });
        rareItems.InsertRange(0, new int[] { 5, 16, 26, 33, 36, 44, 57, 67, 76, 77, 82, 83, 86, 95, 101, 102, 119, 147, 156, 158 });
        legendaryItems.InsertRange(0, new int[] { 27, 38, 41, 42, 58, 75, 81, 88, 112, 116, 137, 154, 155, 164, 167, 178, 179, 182, 183, 184 });
        mutatedItems.InsertRange(0, new int[] { 17, 18, 23, 24, 69, 70, 71, 79, 117, 124, 129, 139, 140, 141, 144, 145, 146, 160, 161, 168 });
        hauntedItems.InsertRange(0, new int[] { 15, 19, 40, 43, 73, 96, 123, 125, 126, 127, 128, 130, 131, 132, 133, 134, 135, 136, 138, 151 });
        irradiatedItems.InsertRange(0, new int[] { 13, 14, 20, 22, 29, 39, 59, 80, 89, 90, 91, 92, 113, 118, 121, 153, 157, 159, 163, 166 });
        nuclearItems.InsertRange(0, new int[] { 21, 30, 37, 68, 74, 87, 93, 115, 120, 142, 152, 169, 170, 171, 172, 176, 180, 181, 185, 186 });
        uniqueItems.InsertRange(0, new int[] { 46, 48, 49, 50, 51, 52, 53, 54, 56, 66, 72, 84, 85, 97, 98, 99, 100, 103, 104, 105, 106, 107, 108, 109, 110, 111, 143, 187, 188, 189, 190 });

        rarityList.InsertRange(0, new List<int>[] { commonItems, uncommonItems, rareItems, legendaryItems, mutatedItems, hauntedItems, irradiatedItems, nuclearItems, uniqueItems });
    }

    private void Update()
    {
        playerMvt.StatUpdate(leftItems, rightItems, rarityList);
        healthManager.StatUpdate(leftItems, rightItems, rarityList);
        gunManager.StatUpdate(leftItems, rightItems, rarityList);


        LookForItem();

        UpdateModifierList();

        CheckForMerge();
    }

    void CheckForMerge()
    {
        //trigger-nometry
        //LEFT
        if(leftItems[25] > 0 && leftItems[26] > 0)
        {
            leftItems[26] += 1;
            leftItems[25] -= 1;
        }
        if (leftItems[26] > 0 && leftItems[27] > 0)
        {
            leftItems[27] += 1;
            leftItems[26] -= 1;
        }
        //RIGHT
        if (rightItems[25] > 0 && rightItems[26] > 0)
        {
            rightItems[26] += 1;
            rightItems[25] -= 1;
        }
        if (rightItems[26] > 0 && rightItems[27] > 0)
        {
            rightItems[27] += 1;
            rightItems[26] -= 1;
        }

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
    }

    void LookForItem()
    {
        Vector3 camPos = playerCamera.position;
        Ray ray = new Ray(camPos, playerCamera.forward);
        RaycastHit hit;

        //Debug.DrawLine(camPos, camPos + playerCamera.forward * 7f);
        if (Physics.Raycast(ray, out hit, 7f))
        {
            if(hit.collider.gameObject.tag == "item")
            {
                Vector3 hitItem = hit.collider.gameObject.transform.position;

                itemDisplay.SetActive(true);
                itemDisplay.GetComponent<ItemDisplayScript>().InfoUpdate(hit.collider.gameObject.GetComponentInParent<Item>().WhatItem(), hitItem);

                hit.collider.gameObject.GetComponentInParent<Item>().StayStill();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    rightItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    //Debug.Log("Item of Item ID " + hit.collider.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                    Destroy(hit.collider.gameObject);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    leftItems[hit.collider.gameObject.GetComponentInParent<Item>().WhatItem()] += 1;
                    //Debug.Log("Item of Item ID " + hit.collider.gameObject.GetComponentInParent<Item>().WhatItem() + " added to inventory");
                    Destroy(hit.collider.gameObject);
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
                    hit.transform.gameObject.GetComponent<InteractableButton>().Interact();
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

