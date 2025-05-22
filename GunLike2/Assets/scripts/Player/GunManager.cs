using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    List<List<int>> rarityList = new List<List<int>>();
    List<int> leftList = new List<int>();
    List<int> rightList = new List<int>();

    public HealthManager healthMan;
    public PlayerItem playerItem;
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
    public int leftNuclearBul = 0;
    public int leftHungryParasite = 0;
    float leftHungryParasiteTimer = 0f;
    public int leftIntroTrig = 0;
    public int leftAdvTrig = 0;
    public int leftMasterTrig = 0;
    public int leftJam = 0;
    public int leftBeltFed = 0;
    public float leftFastInserter = 0;
    float leftFastInserterTimer = 0f;
    public float leftFireSpon;
    public float leftSharperSpon;
    public float leftSilverSpon;
    public float leftHelpingSpon;
    public float leftCoolSpon;
    public float leftFastSpon;
    public float leftLargeSpon;
    public int leftPossession;
    public int leftSponDeal;
    float leftSponTimer;
    int leftSponItemsMade;
    public int leftMultistage;
    int leftSurpriseEggLifetime;

    public bool leftRicochet = false;

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
    public int rightNuclearBul = 0;
    public int rightHungryParasite = 0;
    float rightHungryParasiteTimer = 0f;
    public int rightIntroTrig = 0;
    public int rightAdvTrig = 0;
    public int rightMasterTrig = 0;
    public int rightJam = 0;
    public int rightBeltFed = 0;
    public int rightFastInserter = 0;
    float rightFastInserterTimer = 0f;
    public float rightFireSpon;
    public float rightSharperSpon;
    public float rightSilverSpon;
    public float rightHelpingSpon;
    public float rightCoolSpon;
    public float rightFastSpon;
    public float rightLargeSpon;
    public int rightPossession;
    public int rightSponDeal;
    float rightSponTimer;
    int rightSponItemsMade;
    public int rightMultistage;
    int rightSurpriseEggLifetime;

    float surpriseEggTimer;

    public bool rightRicochet = false;

    private void Start()
    {
        healthMan = GetComponent<HealthManager>();
        effectList = healthMan.activeEffects;
    }

    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
        leftList.Clear();
        leftList.AddRange(givenLeftItems);
        rightList.Clear();
        rightList.AddRange(givenRightItems);
        rarityList = givenRarityList;

        int daEagleIgnoredLeft = 0;
        int daEagleIgnoredRight = 0;
        //da eagle ignores common
        if(leftHand.transform.GetChild(0).GetComponent<GunScript>().gunName == "Da Eagle")
        {
            for(int i = 0; i < givenLeftItems.Count; i++)
            {
                if (rarityList[0].Contains(i))
                {
                    daEagleIgnoredLeft += givenLeftItems[i];
                    givenLeftItems[i] = 0;
                }
            }
        }
        if (rightHand.transform.GetChild(0).GetComponent<GunScript>().gunName == "Da Eagle")
        {
            for (int i = 0; i < givenRightItems.Count; i++)
            {
                if (rarityList[0].Contains(i))
                {
                    daEagleIgnoredRight += givenRightItems[i];
                    givenRightItems[i] = 0;
                }
            }
        }

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

        if (effectList[3].x > 0f) { masterReSpd = Calc(10f, givenLeftItems[17] + givenRightItems[17], masterReSpd); }
        if (effectList[4].x > 0f) { masterCritChance = Calc(10f, givenLeftItems[17] + givenRightItems[17], masterCritChance); }
        if (effectList[5].x > 0f) { masterWeakPointDamage = Calc(10f, givenLeftItems[17] + givenRightItems[17], masterWeakPointDamage); }
        if (effectList[7].x > 0f) { masterAtkSpd = Calc(10f, givenLeftItems[17] + givenRightItems[17], masterAtkSpd); }
        if (effectList[11].x > 0f) { masterDmg = Calc(10f, givenLeftItems[17] + givenRightItems[17], masterDmg); }
        if (effectList[12].x > 0f) { masterDmg = Calc(-10f, givenLeftItems[17] + givenRightItems[17], masterDmg); }
        if (effectList[13].x > 0f) { masterAtkSpd = Calc(-10f, givenLeftItems[17] + givenRightItems[17], masterAtkSpd); }
        if (effectList[18].x > 0f) { masterAtkSpd = masterAtkSpd * 1.5f; }
        if (effectList[19].x > 0f) { masterAtkSpd = masterAtkSpd * 2f; }

        leftAtkSpd = 1f * masterAtkSpd;
        leftReSpd = 1f * masterReSpd;
        leftDmg = 1f * masterDmg;
        leftMagSize = 1f * masterMagSize;
        leftAcc = 1f * masterAcc;
        leftBulSpd = 1f * masterBulSpd;
        leftBulSize = 1f * masterBulSize;
        leftBulPir = 0 + masterBulPir;
        leftCritChance = 1f * masterCritChance;
        leftCritDamage = 1f * masterCritDamage;
        leftWeakPointChance = 1f * masterWeakPointChance;
        leftWeakPointDamage = 1f * masterWeakPointDamage;

        rightAtkSpd = 1f * masterAtkSpd;
        rightReSpd = 1f * masterReSpd;
        rightDmg = 1f * masterDmg;
        rightMagSize = 1f * masterMagSize;
        rightAcc = 1f * masterAcc;
        rightBulSpd = 1f * masterBulSpd;
        rightBulSize = 1f * masterBulSize;
        rightBulPir = 0 + masterBulPir;
        rightCritChance = 1f * masterCritChance;
        rightCritDamage = 1f * masterCritDamage;
        rightWeakPointChance = 1f * masterWeakPointChance;
        rightWeakPointDamage = 1f * masterWeakPointDamage;

        // da eagle special treatment
        if(daEagleIgnoredLeft > 0)
        {
            leftAtkSpd = Calc(5f, daEagleIgnoredLeft, leftAtkSpd);
            leftReSpd = Calc(5f, daEagleIgnoredLeft, leftReSpd);
            leftDmg = Calc(5f, daEagleIgnoredLeft, leftDmg);
            leftMagSize = Calc(5f, daEagleIgnoredLeft, leftMagSize);
            leftAcc = Calc(5f, daEagleIgnoredLeft, leftAcc);
            leftBulSpd = Calc(5f, daEagleIgnoredLeft, leftBulSpd);
            leftBulSize = Calc(5f, daEagleIgnoredLeft, leftBulSize);
        }
        if (daEagleIgnoredRight > 0)
        {
            rightAtkSpd = Calc(5f, daEagleIgnoredRight, rightAtkSpd);
            rightReSpd = Calc(5f, daEagleIgnoredRight, rightReSpd);
            rightDmg = Calc(5f, daEagleIgnoredRight, rightDmg);
            rightMagSize = Calc(5f, daEagleIgnoredRight, rightMagSize);
            rightAcc = Calc(5f, daEagleIgnoredRight, rightAcc);
            rightBulSpd = Calc(5f, daEagleIgnoredRight, rightBulSpd);
            rightBulSize = Calc(5f, daEagleIgnoredRight, rightBulSize);
        }

        leftAtkSpd = Calc(-50f, givenLeftItems[21], leftAtkSpd);
        leftReSpd = Calc(20f, givenLeftItems[7], leftReSpd);
        leftDmg = Calc(40f, givenLeftItems[21], leftDmg);
        leftDmg = Calc(40f, givenLeftItems[13], leftDmg);
        leftDmg = Calc(20f, givenLeftItems[4], leftDmg);
        leftDmg = Calc(20f, givenLeftItems[11], leftDmg);
        leftDmg = Calc(20f, givenLeftItems[40], leftDmg);
        leftDmg = Calc(40f, givenLeftItems[26], leftAcc);
        leftDmg = Calc(40f, givenLeftItems[27], leftAcc);
        leftDmg = Calc(-10f, givenLeftItems[12], leftDmg);
        leftDmg = Calc(-10f, givenLeftItems[31], leftDmg);
        leftDmg = Calc(-10f, givenLeftItems[46], leftDmg);
        leftMagSize = Calc(20f, givenLeftItems[6], leftMagSize);
        leftMagSize = Calc(-50f, givenLeftItems[21], leftMagSize);
        leftMagSize = Calc(-20f, givenLeftItems[25], leftMagSize);
        leftMagSize = Calc(-20f, givenLeftItems[26], leftMagSize);
        leftAcc = Calc(20f, givenLeftItems[8], leftAcc);
        leftAcc = Calc(40f, givenLeftItems[25], leftAcc);
        leftAcc = Calc(40f, givenLeftItems[27], leftAcc);
        leftAcc = Calc(-10f, givenLeftItems[32], leftAcc);
        leftAcc = Calc(-10f, givenLeftItems[46], leftAcc);
        leftBulSpd = Calc(20f, givenLeftItems[9], leftBulSpd);
        leftBulSpd = Calc(20f, givenLeftItems[57], leftBulSpd);
        leftBulSpd = Calc(-10f, givenLeftItems[11], leftBulSpd);
        leftBulSpd = Calc(-10f, givenLeftItems[12], leftBulSpd);
        leftBulSize = Calc(20f, givenLeftItems[11], leftBulSize);
        leftBulPir += givenLeftItems[10] + givenLeftItems[26];

        leftHeavyBul = givenLeftItems[11];
        leftMutatedCell = givenLeftItems[14];
        leftBowAct = givenLeftItems[16];
        leftHeavySpirit = givenLeftItems[19];
        leftNuclearBul = givenLeftItems[21];
        leftHungryParasite = givenLeftItems[24];
        leftIntroTrig = givenLeftItems[25];
        leftAdvTrig = givenLeftItems[26];
        leftMasterTrig = givenLeftItems[27];
        leftJam = givenLeftItems[28];
        leftBeltFed = givenLeftItems[29];
        leftFastInserter = givenLeftItems[33];
        leftFireSpon = givenLeftItems[34];
        leftSharperSpon = givenLeftItems[35];
        leftSilverSpon = givenLeftItems[36];
        leftHelpingSpon = givenLeftItems[43];
        leftCoolSpon = givenLeftItems[44];
        leftLargeSpon = givenLeftItems[45];
        leftFastSpon = givenLeftItems[47];
        leftPossession = givenLeftItems[40];
        leftSponDeal = givenLeftItems[42];
        leftMultistage = givenLeftItems[57];
        leftSurpriseEggLifetime = givenLeftItems[58];

        leftRicochet = false;

        if (givenLeftItems[16] > 0f) { leftDmg = leftDmg * 1.2f; leftAcc = leftAcc * 1.1f; leftBulSpd = leftBulSpd * 1.1f; leftAtkSpd = leftAtkSpd / 1.2f; }
        if (givenLeftItems[21] > 0f) { leftDmg = leftDmg * 1.1f; leftAtkSpd = leftAtkSpd / 1.1f; leftMagSize = leftMagSize / 1.5f; }
        if (givenLeftItems[26] > 0f) { leftRicochet = true; }
        if (givenLeftItems[29] > 0f) { leftMagSize = (leftMagSize * 3f) * (givenLeftItems[29] * 1.2f); leftReSpd = leftReSpd / 2f; }
        if (leftAdvTrig > 0 && leftMasterTrig > 0) { leftBulPir += 5; }
        if (leftIntroTrig > 0 && leftAdvTrig > 0 && leftMasterTrig > 0) { leftMagSize = Calc(40f, leftIntroTrig + leftAdvTrig, leftMagSize); }

        rightAtkSpd = Calc(-50f, givenRightItems[21], rightAtkSpd);
        rightReSpd = Calc(20f, givenRightItems[7], rightReSpd);
        rightReSpd = Calc(20f, givenRightItems[33], rightReSpd);
        rightDmg = Calc(40f, givenRightItems[21], rightDmg);
        rightDmg = Calc(40f, givenRightItems[13], rightDmg);
        rightDmg = Calc(20f, givenRightItems[11], rightDmg);
        rightDmg = Calc(20f, givenRightItems[4], rightDmg);
        rightDmg = Calc(20f, givenRightItems[40], rightDmg);
        rightDmg = Calc(40f, givenRightItems[26], rightDmg);
        rightDmg = Calc(40f, givenRightItems[27], rightDmg);
        rightDmg = Calc(-10f, givenRightItems[12], rightDmg);
        rightDmg = Calc(-10f, givenRightItems[31], rightDmg);
        rightDmg = Calc(-10f, givenRightItems[46], rightDmg);
        rightMagSize = Calc(20f, givenRightItems[6], rightMagSize);
        rightMagSize = Calc(-50f, givenRightItems[21], rightMagSize);
        rightMagSize = Calc(-20f, givenRightItems[25], rightMagSize);
        rightMagSize = Calc(-20f, givenRightItems[26], rightMagSize);
        rightAcc = Calc(20f, givenRightItems[8], rightAcc);
        rightAcc = Calc(40f, givenRightItems[25], rightAcc);
        rightAcc = Calc(40f, givenRightItems[27], rightAcc);
        rightAcc = Calc(-10f, givenRightItems[32], rightAcc);
        rightAcc = Calc(-10f, givenRightItems[46], rightAcc);
        rightBulSpd = Calc(20f, givenRightItems[9], rightBulSpd);
        rightBulSpd = Calc(20f, givenRightItems[57], rightBulSpd);
        rightBulSpd = Calc(-10f, givenRightItems[11], rightBulSpd);
        rightBulSpd = Calc(-10f, givenRightItems[12], rightBulSpd);
        rightBulSize = Calc(20f, givenRightItems[11], rightBulSize);
        rightBulPir += givenRightItems[10] + givenRightItems[26];

        rightHeavyBul = givenRightItems[11];
        rightMutatedCell = givenRightItems[14];
        rightBowAct = givenRightItems[16];
        rightHeavySpirit = givenRightItems[19];
        rightNuclearBul = givenRightItems[21];
        rightHungryParasite = givenRightItems[24];
        rightIntroTrig = givenRightItems[25];
        rightAdvTrig = givenRightItems[26];
        rightMasterTrig = givenRightItems[27];
        rightJam = givenRightItems[28];
        rightBeltFed = givenRightItems[29];
        rightFastInserter = givenRightItems[33];
        rightFireSpon = givenRightItems[34];
        rightSharperSpon = givenRightItems[35];
        rightSilverSpon = givenRightItems[36];
        rightHelpingSpon = givenRightItems[43];
        rightCoolSpon = givenRightItems[44];
        rightLargeSpon = givenRightItems[45];
        rightFastSpon = givenRightItems[47];
        rightPossession = givenRightItems[40];
        rightSponDeal = givenRightItems[42];
        rightMultistage = givenRightItems[57];
        rightSurpriseEggLifetime = givenRightItems[58];

        rightRicochet = false;

        if (givenRightItems[16] > 0f) { rightDmg = rightDmg * 1.2f; rightAcc = rightAcc * 1.1f; rightBulSpd = rightBulSpd * 1.1f; rightAtkSpd = rightAtkSpd / 1.2f; }
        if (givenRightItems[21] > 0f) { rightDmg = rightDmg * 1.1f; rightAtkSpd = rightAtkSpd / 1.1f; rightMagSize = rightMagSize / 1.5f; }
        if (givenRightItems[26] > 0f) { rightRicochet = true; }
        if (givenRightItems[29] > 0f) { rightMagSize = (rightMagSize * 3f) * (givenRightItems[29] * 1.2f); rightReSpd = rightReSpd / 2f; }
        if (rightAdvTrig > 0 && rightMasterTrig > 0) { rightBulPir += 5; }
        if (rightIntroTrig > 0 && rightAdvTrig > 0 && rightMasterTrig > 0) { rightMagSize = Calc(40f, rightIntroTrig + rightAdvTrig, rightMagSize); }

        //Irradiated French Pastry
        if (givenLeftItems[22] > 0)
        {
            if (playerItem.leftIFPStatToBuff == 4) { leftCritChance = leftCritChance * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 5) { leftCritDamage = leftCritDamage * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 6) { leftWeakPointDamage = leftWeakPointDamage * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 7) { leftDmg = leftDmg * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 8) { leftAtkSpd = leftAtkSpd * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 9) { leftReSpd = leftReSpd * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 10) { leftMagSize = leftMagSize * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 11) { leftAcc = leftAcc * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 12) { leftBulSpd = leftBulSpd * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 13) { leftBulSize = leftBulSize * (givenLeftItems[22] * 2); }
            if (playerItem.leftIFPStatToBuff == 14) { leftBulPir = leftBulPir * (givenLeftItems[22] * 2); }

            if (playerItem.leftIFPStatToDeBuff == 4) { leftCritChance = leftCritChance * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 5) { leftCritDamage = leftCritDamage * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 6) { leftWeakPointDamage = leftWeakPointDamage * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 7) { leftDmg = leftDmg * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 8) { leftAtkSpd = leftAtkSpd * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 9) { leftReSpd = leftReSpd * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 10) { leftMagSize = leftMagSize * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 11) { leftAcc = leftAcc * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 12) { leftBulSpd = leftBulSpd * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 13) { leftBulSize = leftBulSize * (0.9f / givenLeftItems[22]); }
            if (playerItem.leftIFPStatToDeBuff == 14) { leftBulPir = Mathf.FloorToInt(leftBulPir * (0.9f / givenLeftItems[22])); }
        }
        if (givenRightItems[22] > 0)
        {
            if (playerItem.rightIFPStatToBuff == 15) { rightCritChance = rightCritChance * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 16) { rightCritDamage = rightCritDamage * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 17) { rightWeakPointDamage = rightWeakPointDamage * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 18) { rightDmg = rightDmg * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 19) { rightAtkSpd = rightAtkSpd * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 20) { rightReSpd = rightReSpd * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 21) { rightMagSize = rightMagSize * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 22) { rightAcc = rightAcc * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 23) { rightBulSpd = rightBulSpd * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 24) { rightBulSize = rightBulSize * (givenRightItems[22] * 2); }
            if (playerItem.rightIFPStatToBuff == 25) { rightBulPir = rightBulPir * (givenRightItems[22] * 2); }

            if (playerItem.rightIFPStatToDeBuff == 15) { rightCritChance = rightCritChance * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 16) { rightCritDamage = rightCritDamage * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 17) { rightWeakPointDamage = rightWeakPointDamage * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 18) { rightDmg = rightDmg * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 19) { rightAtkSpd = rightAtkSpd * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 20) { rightReSpd = rightReSpd * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 21) { rightMagSize = rightMagSize * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 22) { rightAcc = rightAcc * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 23) { rightBulSpd = rightBulSpd * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 24) { rightBulSize = rightBulSize * (0.9f / givenRightItems[22]); }
            if (playerItem.rightIFPStatToDeBuff == 25) { rightBulPir = Mathf.FloorToInt(rightBulPir * (0.9f / givenRightItems[22])); }
        }

        rightHand.transform.GetChild(0).SendMessage("StatUpdateRight", SendMessageOptions.DontRequireReceiver);
        leftHand.transform.GetChild(0).SendMessage("StatUpdateLeft", SendMessageOptions.DontRequireReceiver);

        //undo any changes made
        if(leftHand.transform.GetChild(0).GetComponent<GunScript>().gunName == "Da Eagle")
        {
            givenLeftItems.Clear();
            givenLeftItems.AddRange(leftList);
        }
        if(rightHand.transform.GetChild(0).GetComponent<GunScript>().gunName == "Da Eagle")
        {
            givenRightItems.Clear();
            givenRightItems.AddRange(rightList);
        }
        
    }

    float Calc(float modifier, int amount, float baseVal)
    {
        float result = baseVal;

        if (amount <= 0) { return result; }

        if (modifier > 0)
        {
            //Buff

            for (int i = 0; i <= amount; i++)
            {
                result = result + result * (modifier / 100);
            }
        }
        else if(modifier < 0)
        {
            //Debuff
            modifier = modifier * -1f;

            for (int i = 0; i <= amount; i++)
            {
                result = result - result * (modifier / 100);
            }
        }

        return result;
    }

    private void Update()
    {
        if (healthMan.dead) { return; }

        leftGunUpdate();
        RightGunUpdate();
        if (Input.GetKeyDown(KeyCode.R))
        {
            leftHand.transform.GetChild(0).SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
            rightHand.transform.GetChild(0).SendMessage("AttemptReload", SendMessageOptions.DontRequireReceiver);
        }

        itemChecks();
    }

    void leftGunUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            leftHand.transform.GetChild(0).SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftHand.transform.GetChild(0).SendMessage("AttemptShootUp", SendMessageOptions.DontRequireReceiver);
        }
    }

    void RightGunUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            rightHand.transform.GetChild(0).SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightHand.transform.GetChild(0).SendMessage("AttemptShootUp", SendMessageOptions.DontRequireReceiver);
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

        if (leftHungryParasite > 0)
        {
            leftHungryParasiteTimer -= Time.deltaTime;
            if (leftHungryParasiteTimer <= 0)
            {
                HungryParasiteReroll(leftList);
                leftHungryParasiteTimer = 60 / (leftHungryParasite / 2f + 1f);
            }
        }

        if (rightHungryParasite > 0)
        {
            rightHungryParasiteTimer -= Time.deltaTime;
            if (rightHungryParasiteTimer <= 0)
            {
                HungryParasiteReroll(rightList);
                rightHungryParasiteTimer = 60 / (rightHungryParasite / 2f + 1f);
            }
        }

        if (leftFastInserter > 0)
        {
            leftFastInserterTimer -= Time.deltaTime;
            if (leftFastInserterTimer <= 0 && leftHand.GetComponentInChildren<GunScript>().currentBullets < leftHand.GetComponentInChildren<GunScript>().magSize)
            {
                leftHand.transform.GetChild(0).SendMessage("addBullet", SendMessageOptions.DontRequireReceiver);
                leftFastInserterTimer = 1 / (0.2f * leftFastInserter);
            }
        }
        if (rightFastInserter > 0)
        {
            rightFastInserterTimer -= Time.deltaTime;
            if (rightFastInserterTimer <= 0 && rightHand.GetComponentInChildren<GunScript>().currentBullets < rightHand.GetComponentInChildren<GunScript>().magSize)
            {
                rightHand.transform.GetChild(0).SendMessage("addBullet", SendMessageOptions.DontRequireReceiver);
                rightFastInserterTimer = 1 / (0.2f * rightFastInserter);
            }
        }

        if(leftSponDeal > 0)
        {
            leftSponTimer += Time.deltaTime;
            if(leftSponTimer > 20f)
            {
                int rand = Random.Range(0, playerItem.sponserItems.Count);
                //Debug.Log("Giving item: " + playerItem.sponserItems[rand]);
                playerItem.leftItems[playerItem.sponserItems[rand]] += 1;

                leftSponItemsMade++;
                leftSponTimer = 0f;
                if(leftSponItemsMade >= 5)
                {
                    leftSponItemsMade = 0;
                    playerItem.leftItems[42] -= 1;
                }
            }
        }
        if (rightSponDeal > 0)
        {
            rightSponTimer += Time.deltaTime;
            if (rightSponTimer > 20f)
            {
                int rand = Random.Range(0, playerItem.sponserItems.Count);
                //Debug.Log("Giving item: " + playerItem.sponserItems[rand]);
                playerItem.rightItems[playerItem.sponserItems[rand]] += 1;

                rightSponItemsMade++;
                rightSponTimer = 0f;
                if (rightSponItemsMade >= 5)
                {
                    rightSponItemsMade = 0;
                    playerItem.rightItems[42] -= 1;
                }
            }
        }

        surpriseEggTimer += Time.deltaTime;
        if (leftSurpriseEggLifetime > 0)
        {
            if(surpriseEggTimer > 120 && healthMan.timeSinceEnemyDied < 120)
            {
                playerItem.leftItems[55] += 1;
            }
        }
        if (rightSurpriseEggLifetime > 0)
        {
            if (surpriseEggTimer > 120 && healthMan.timeSinceEnemyDied < 120)
            {
                playerItem.rightItems[55] += 1;
            }
        }
        if (surpriseEggTimer > 120) { surpriseEggTimer = 0; }
    }

    void mutatedCellReroll(List<int> itemList)
    {
        int itemsToReroll = 0;

        for (int i = 0; i < rarityList[4].Count; i++)
        {
            if (itemList[rarityList[4][i]] > 0)
            {
                itemsToReroll = itemsToReroll + itemList[rarityList[4][i]];
                itemList[rarityList[4][i]] = 0;
            }
        }

        if (Random.Range(1, 100) < (100 * itemList[14] / 20f))
        {
            itemsToReroll++;
        }

        for (int q = 0; q <= itemsToReroll; q++)
        {
            int rand = Random.Range(0, rarityList[4].Count);
            itemList[rarityList[4][rand]] += 1;
        }
    }

    void HungryParasiteReroll(List<int> itemList)
    {
        List<int> rerollOptions = new List<int>();

        for (int i = 0; i < rarityList[0].Count; i++)
        {
            if (itemList[rarityList[0][i]] > 0)
            {
                rerollOptions.Add(rarityList[0][i]);
            }
        }

        if (Random.Range(1, 100) < (100 * itemList[24] / 20f))
        {
            for (int i = 0; i < rarityList[1].Count; i++)
            {
                if (itemList[rarityList[1][i]] > 0)
                {
                    rerollOptions.Add(rarityList[1][i]);
                }
            }
        }

        if(rerollOptions.Count > 0)
        {
            int rerolledItem = rerollOptions[Random.Range(0, rerollOptions.Count - 1)];
            itemList[rerolledItem]--;
            itemList[rarityList[4][Random.Range(0, rarityList.Count - 1)]]++;
        }
    }
}