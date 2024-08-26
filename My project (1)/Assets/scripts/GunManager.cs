using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    List<List<int>> rarityList = new List<List<int>>();
    List<int> leftList = new List<int>();
    List<int> rightList = new List<int>();

    HealthManager healthMan;
    List<Vector4> effectList;

    public GameObject leftHand;
    public GameObject rightHand;

    // For bonuses that affect both weapons.
    public float masterAtkSpd = 1f;
    public float masterReSpd = 1f;
    public float masterDmg = 1f;
    public float masterMagSize = 1f;
    public float masterAcc = 1f;
    public float masterBulSpd = 1f;
    public float masterBulSize = 1f;
    public int masterBulPir = 0;
    public float masterCritChance = 1f;
    public float masterCritDamage = 1f;
    public float masterWeakPointChance = 1f;
    public float masterWeakPointDamage = 1f;

    // left weapons base stats
    public float leftAtkSpd = 1f;
    public float leftReSpd = 1f;
    public float leftDmg = 1f;
    public float leftMagSize = 1f;
    public float leftAcc = 1f;
    public float leftBulSpd = 1f;
    public float leftBulSize = 1f;
    public int leftBulPir = 0;
    public float leftCritChance = 1f;
    public float leftCritDamage = 1f;
    public float leftWeakPointChance = 1f;
    public float leftWeakPointDamage = 1f;
    // left item checks
    public float leftHeavyBul = 0f;
    public int leftMutatedCell = 0;
    float leftMutatedCellTimer = 0f;
    public float leftBowAct = 0f;
    public int leftHeavySpirit = 0;

    // right weapons base stats
    public float rightAtkSpd = 1f;
    public float rightReSpd = 1f;
    public float rightDmg = 1f;
    public float rightMagSize = 1f;
    public float rightAcc = 1f;
    public float rightBulSpd = 1f;
    public float rightBulSize = 1f;
    public int rightBulPir = 0;
    public float rightCritChance = 1f;
    public float rightCritDamage = 1f;
    public float rightWeakPointChance = 1f;
    public float rightWeakPointDamage = 1f;
    // right item checks
    public float rightHeavyBul = 0f;
    public int rightMutatedCell = 0;
    float rightMutatedCellTimer = 0f;
    public float rightBowAct = 0f;
    public int rightHeavySpirit = 0;

    private void Start()
    {
        healthMan = GetComponent<HealthManager>();
        effectList = healthMan.activeEffects;
    }

    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
        leftList = givenLeftItems;
        rightList = givenRightItems;
        rarityList = givenRarityList;

        masterAtkSpd = 1f;
        masterReSpd = 1f;
        masterDmg = 1f;
        masterMagSize = 1f;
        masterAcc = 1f;
        masterBulSpd = 1f;
        masterBulSize = 1f;
        masterBulPir = 0;
        masterCritChance = 1f;
        masterCritDamage = 1f;
        masterWeakPointChance = 1f;
        masterWeakPointDamage = 1f;

        //status effect buffs / debuffs

        if (effectList[3].x > 0f) { masterReSpd = masterReSpd * ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[4].x > 0f) { masterCritChance = masterCritChance * ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[5].x > 0f) { masterWeakPointDamage = masterWeakPointDamage * ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[7].x > 0f) { masterAtkSpd = masterAtkSpd * ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[11].x > 0f) { masterDmg = masterDmg * ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[12].x > 0f) { masterDmg = masterDmg / ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }
        if (effectList[13].x > 0f) { masterAtkSpd = masterAtkSpd / ((givenLeftItems[17] + givenRightItems[17]) / 10 + 1f); }

        leftAtkSpd = 1f * masterAtkSpd / (givenLeftItems[16] / 10f + 1f);
        leftReSpd = 1f * (givenLeftItems[7] / 10f + 1f) * masterReSpd;
        leftDmg = 1f * (givenLeftItems[16] / 5f + 1f) * (givenLeftItems[4] / 10f + 1f) * (givenLeftItems[13] / 5f + 1f) * masterDmg / (givenLeftItems[12] / 10f + 1f);
        leftMagSize = 1f * (givenLeftItems[6] / 5f + 1f) * masterMagSize;
        leftAcc = 1f * (givenLeftItems[8] / 5f + 1f) * (givenLeftItems[16] / 10f + 1f) * masterAcc;
        leftBulSpd = 1f * (givenLeftItems[9] / 10f + 1f) * (givenLeftItems[16] / 10f + 1f) * masterBulSpd / (givenLeftItems[11] / 10f + 1f);
        leftBulSize = 1f * (givenLeftItems[11] / 10f + 1f) * masterBulSize;
        leftBulPir = (0 + givenLeftItems[10] + masterBulPir);
        leftCritChance = 1f * masterCritChance;
        leftCritDamage = 1f * masterCritDamage;
        leftWeakPointChance = 1f * masterWeakPointChance;
        leftWeakPointDamage = 1f * masterWeakPointDamage;

        leftHeavyBul = givenLeftItems[11];
        leftMutatedCell = givenLeftItems[14];
        leftBowAct = givenLeftItems[16];
        leftHeavySpirit = givenLeftItems[19];

        rightAtkSpd = 1f * masterAtkSpd / (givenRightItems[16] / 5f + 1f);
        rightReSpd = 1f * (givenRightItems[7] / 10f + 1f) * masterReSpd;
        rightDmg = 1f * (givenRightItems[16] / 5f + 1f) * (givenRightItems[4] / 10f + 1f) * (givenRightItems[13] / 5f + 1f) * masterDmg / (givenRightItems[12] / 10f + 1f);
        rightMagSize = 1f * (givenRightItems[6] / 5f + 1f) * masterMagSize;
        rightAcc = 1f * (givenRightItems[8] / 5f + 1f) * (givenRightItems[16] / 10f + 1f) * masterAcc;
        rightBulSpd = 1f * (givenRightItems[9] / 10f + 1f) * (givenRightItems[16] / 10f + 1f) * masterBulSpd / (givenRightItems[11] / 10f + 1f);
        rightBulSize = 1f * (givenRightItems[11] / 10f + 1f) * masterBulSize;
        rightBulPir = (0 + givenRightItems[10] + masterBulPir) + 1;
        rightCritChance = 1f * masterCritChance;
        rightCritDamage = 1f * masterCritDamage;
        rightWeakPointChance = 1f * masterWeakPointChance;
        rightWeakPointDamage = 1f * masterWeakPointDamage;

        rightHeavyBul = givenRightItems[11];
        rightMutatedCell = givenRightItems[14];
        rightBowAct = givenRightItems[16];
        rightHeavySpirit = givenRightItems[19];

        rightHand.SendMessage("StatUpdateRight", SendMessageOptions.DontRequireReceiver);
        leftHand.SendMessage("StatUpdateLeft", SendMessageOptions.DontRequireReceiver);
    }

    private void Update()
    {
        leftGunUpdate();
        RightGunUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            leftHand.SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
            rightHand.SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
        }

        itemChecks();
    }

    void leftGunUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            leftHand.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftHand.SendMessage("AttemptShootUp", SendMessageOptions.DontRequireReceiver);
        }
    }

    void RightGunUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            rightHand.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightHand.SendMessage("AttemptShootUp", SendMessageOptions.DontRequireReceiver);
        }
    }

    void itemChecks()
    {
        if (leftMutatedCell > 0)
        {
            leftMutatedCellTimer -= Time.deltaTime;
            if (leftMutatedCellTimer <= 0)
            {
                mutatedCellReroll(leftList);
                leftMutatedCellTimer = 500 / (leftMutatedCell / 10f + 1f);
            }
        }

        if (rightMutatedCell > 0)
        {
            rightMutatedCellTimer -= Time.deltaTime;
            if (rightMutatedCellTimer <= 0)
            {
                mutatedCellReroll(rightList);
                rightMutatedCellTimer = 500 / (rightMutatedCell / 10f + 1f);
            }
        }
    }

    void mutatedCellReroll(List<int> itemList)
    {
        int itemsToReroll = 0;

        for (int i = 0; i < rarityList[5].Count; i++)
        {
            if (itemList[rarityList[5][i]] > 0)
            {
                itemsToReroll = itemsToReroll + itemList[rarityList[5][i]];
                itemList[rarityList[5][i]] = 0;
            }
        }

        if (Random.Range(1, 100) < (100 * itemList[14] / 20f))
        {
            itemsToReroll++;
        }

        for (int q = 0; q <= itemsToReroll; q++)
        {
            int rand = Random.Range(0, rarityList[5].Count);
            itemList[rarityList[5][rand]] += 1;
        }
    }
}