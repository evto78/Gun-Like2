using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<GameObject> guns;

    List<List<int>> rarityList = new List<List<int>>();
    List<int> leftList = new List<int>();
    List<int> rightList = new List<int>();

    public HealthManager healthMan;
    public PlayerItem playerItem;
    List<Vector4> effectList;

    public GameObject leftHand;
    public GunScript leftGunScript;
    public GameObject rightHand;
    public GunScript rightGunScript;

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
    public float leftMutatedCellTimer = 0f;
    public float leftBowAct = 0f;
    public int leftHeavySpirit = 0;
    public int leftNuclearBul = 0;
    public int leftHungryParasite = 0;
    public float leftHungryParasiteTimer = 0f;
    public int leftIntroTrig = 0;
    public int leftAdvTrig = 0;
    public int leftMasterTrig = 0;
    public int leftJam = 0;
    public int leftBeltFed = 0;
    public float leftFastInserter = 0;
    public float leftFastInserterTimer = 0f;
    public float leftFireSpon;
    public float leftSharperSpon;
    public float leftSilverSpon;
    public float leftHelpingSpon;
    public float leftCoolSpon;
    public float leftFastSpon;
    public float leftLargeSpon;
    public int leftPossession;
    public int leftSponDeal;
    public float leftSponTimer;
    public int leftSponItemsMade;
    public int leftMultistage;
    int leftSurpriseEggLifetime;
    public int leftNerf;
    public int leftSpinach;
    public int leftStickTo;
    public int leftStickToCounters;
    public int leftGunkyBless;
    public int leftGunkyClaw;
    public int leftGunkyAxe;
    public int leftClockwork;
    int leftPrinter;
    int leftMicrowave;
    float leftMicrowaveTimer;
    public int leftSniperTower;
    public float leftPrinterTimer;
    public int leftPerfectedScope;
    public int leftPumpShotgunAttach;
    public int leftGrenadeAttach;
    public int leftGasGrenadeAttach;
    public int leftWarcry;
    public int leftTactReload;
    public int leftCarvedBone;
    public int leftCanineTooth;
    public int leftDoorKnob;
    public int leftHaunt;
    public int leftGoodies;
    public int leftAnatomy;
    public int leftEnzymes;
    public int leftDarkBranch;
    public int leftBrokenPen;
    public int leftRushJob;
    public int leftBrokenInk;
    public int leftChemicalAgents;
    public int left200Fleas;
    public int leftSmokingGun;
    public int leftForkedBarrel;
    public int leftRunicMag;
    public int leftSlots;
    public int leftOverCenti;
    public int leftOverCompress;
    public int leftTriggerHappy;
    public int leftBulletFactory;
    public int leftCritUnfunny;
    public int leftFortify;
    public int leftConfetti;
    public int leftEndless;

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
    public float rightMutatedCellTimer = 0f;
    public float rightBowAct = 0f;
    public int rightHeavySpirit = 0;
    public int rightNuclearBul = 0;
    public int rightHungryParasite = 0;
    public float rightHungryParasiteTimer = 0f;
    public int rightIntroTrig = 0;
    public int rightAdvTrig = 0;
    public int rightMasterTrig = 0;
    public int rightJam = 0;
    public int rightBeltFed = 0;
    public int rightFastInserter = 0;
    public float rightFastInserterTimer = 0f;
    public float rightFireSpon;
    public float rightSharperSpon;
    public float rightSilverSpon;
    public float rightHelpingSpon;
    public float rightCoolSpon;
    public float rightFastSpon;
    public float rightLargeSpon;
    public int rightPossession;
    public int rightSponDeal;
    public float rightSponTimer;
    int rightSponItemsMade;
    public int rightMultistage;
    int rightSurpriseEggLifetime;
    public int rightNerf;
    public int rightSpinach;
    public int rightStickTo;
    public int rightStickToCounters;
    public int rightGunkyBless;
    public int rightGunkyClaw;
    public int rightGunkyAxe;
    public int rightClockwork;
    int rightPrinter;
    int rightMicrowave;
    float rightMicrowaveTimer;
    public float surpriseEggTimer;
    public float rightPrinterTimer;
    public int rightSniperTower;
    public int rightPerfectedScope;
    public int rightPumpShotgunAttach;
    public int rightGrenadeAttach;
    public int rightGasGrenadeAttach;
    public int rightWarcry;
    public int rightTactReload;
    public int rightCarvedBone;
    public int rightCanineTooth;
    public int rightDoorKnob;
    public int rightHaunt;
    public int rightGoodies;
    public int rightAnatomy;
    public int rightEnzymes;
    public int rightDarkBranch;
    public int rightBrokenPen;
    public int rightRushJob;
    public int rightBrokenInk;
    public int rightChemicalAgents;
    public int right200Fleas;
    public int rightSmokingGun;
    public int rightForkedBarrel;
    public int rightRunicMag;
    public int rightSlots;
    public int rightOverCenti;
    public int rightOverCompress;
    public int rightTriggerHappy;
    public int rightBulletFactory;
    public int rightCritUnfunny;
    public int rightFortify;
    public int rightConfetti;
    public int rightEndless;

    public bool rightRicochet = false;
    int leftHandVal;
    int rightHandVal;

    public GameObject gunkyAxe;
    public GameObject microwave;
    public GameObject darkwave;
    float darkwaveTimer;
    public float axeCooldown;
    public float centriCheckTimer;

    public GameObject leftLeg;
    public GameObject rightLeg;
    float leftKickCooldown;
    float rightKickCooldown;

    List<MeshRenderer> leftMRs = new List<MeshRenderer>();
    List<MeshRenderer> rightMRs = new List<MeshRenderer>();
    List<Color> leftColors = new List<Color>();
    List<Color> rightColors = new List<Color>();

    public int totalLiveBullets;
    public int maximumLiveBullets;

    private void Start()
    {
        totalLiveBullets = 0;
        leftHandVal = 0;
        rightHandVal = 1;

        if (PlayerPrefs.HasKey("leftHandGunSelect"))
        {
            leftHandVal = PlayerPrefs.GetInt("leftHandGunSelect");
        }
        if (PlayerPrefs.HasKey("rightHandGunSelect"))
        {
            rightHandVal = PlayerPrefs.GetInt("rightHandGunSelect");
        }
        if(leftHandVal == 9) { leftHandVal = rightHandVal; }
        if(rightHandVal == 9) { rightHandVal = leftHandVal; }

        if(leftHand.transform.childCount > 0) { Destroy(leftHand.transform.GetChild(0).gameObject); }
        if(rightHand.transform.childCount > 0) { Destroy(rightHand.transform.GetChild(0).gameObject); }
        Instantiate(guns[leftHandVal], leftHand.transform);
        Instantiate(guns[rightHandVal], rightHand.transform);
        leftGunScript = leftHand.GetComponentInChildren<GunScript>();
        rightGunScript = rightHand.GetComponentInChildren<GunScript>();

        healthMan = GetComponent<HealthManager>();
        effectList = healthMan.activeEffects;

        GrabMeshRenders();
    }
    void GrabMeshRenders()
    {
        leftMRs.Clear();
        rightMRs.Clear();
        leftColors.Clear();
        rightColors.Clear();
        leftMRs.AddRange(leftHand.GetComponentsInChildren<MeshRenderer>());
        rightMRs.AddRange(rightHand.GetComponentsInChildren<MeshRenderer>());
        foreach(MeshRenderer mr in leftMRs) { leftColors.Add(mr.material.color); }
        foreach(MeshRenderer mr in rightMRs) { rightColors.Add(mr.material.color); }
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
        if(leftGunScript.gunName == "Da Eagle")
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
        if (rightGunScript.gunName == "Da Eagle")
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
        if (effectList[27].x > 0f) { masterDmg *= 2f; }
        //Smoking Gun
        if (effectList[23].x > 0) { masterReSpd *= (1f + (0.2f * givenLeftItems[162] + givenRightItems[162])) * effectList[23].x; }

        //Base Stats
        float leftAtkSpdMult = 1f; float leftAtkSpdDiv = 1f; float rightAtkSpdMult = 1f; float rightAtkSpdDiv = 1f;
        float leftReSpdMult = 1f; float leftReSpdDiv = 1f; float rightReSpdMult = 1f; float rightReSpdDiv = 1f;
        float leftDmgMult = 1f; float leftDmgDiv = 1f; float rightDmgMult = 1f; float rightDmgDiv = 1f;
        float leftMagSizeMult = 1f; float leftMagSizeDiv = 1f; float rightMagSizeMult = 1f; float rightMagSizeDiv = 1f;
        float leftAccMult = 1f; float leftAccDiv = 1f; float rightAccMult = 1f; float rightAccDiv = 1f;
        float leftBulSpdMult = 1f; float leftBulSpdDiv = 1f; float rightBulSpdMult = 1f; float rightBulSpdDiv = 1f;
        float leftBulSizeMult = 1f; float leftBulSizeDiv = 1f; float rightBulSizeMult = 1f; float rightBulSizeDiv = 1f;
        float leftCritDamageMult = 1f; float leftCritDamageDiv = 1f; float rightCritDamageMult = 1f; float rightCritDamageDiv = 1f;
        float leftWeakPointDamageMult = 1f; float leftWeakPointDamageDiv = 1f; float rightWeakPointDamageMult = 1f; float rightWeakPointDamageDiv = 1f;

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
            leftAtkSpdMult += MultAdder(5f, daEagleIgnoredLeft);
            leftReSpdMult += MultAdder(5f, daEagleIgnoredLeft);
            leftDmgMult += MultAdder(5f, daEagleIgnoredLeft);
            leftMagSizeMult += MultAdder(5f, daEagleIgnoredLeft);
            leftAccMult += MultAdder(5f, daEagleIgnoredLeft);
            leftBulSpdMult += MultAdder(5f, daEagleIgnoredLeft);
        }
        if (daEagleIgnoredRight > 0)
        {
            rightAtkSpdMult += MultAdder(5f, daEagleIgnoredRight);
            rightReSpdMult += MultAdder(5f, daEagleIgnoredRight);
            rightDmgMult += MultAdder(5f, daEagleIgnoredRight);
            rightMagSizeMult += MultAdder(5f, daEagleIgnoredRight);
            rightAccMult += MultAdder(5f, daEagleIgnoredRight);
            rightBulSpdMult += MultAdder(5f, daEagleIgnoredRight);
        }
        //Endless Mag
        leftEndless = givenLeftItems[192]; rightEndless = givenRightItems[192];
        //Left Attack Speed
        leftAtkSpdMult += MultAdder(20f, givenLeftItems[62]);
        leftAtkSpdMult += MultAdder(10f, givenLeftItems[97]);
        leftAtkSpdMult += MultAdder(60f, givenLeftItems[118]);
        leftAtkSpdMult += MultAdder(60f, givenLeftItems[132]);
        leftAtkSpdMult += MultAdder(10f, givenLeftItems[143]);
        leftAtkSpdMult += MultAdder(20f, givenLeftItems[153]);
        leftAtkSpdMult += MultAdder(40f, givenLeftItems[157]);
        leftAtkSpdMult += MultAdder(20f, givenLeftItems[161]);
        leftAtkSpdMult += MultAdder(40f, givenLeftItems[169]);
        leftAtkSpdMult += MultAdder(40f, givenLeftItems[184]);

        leftAtkSpdDiv += MultAdder(-50f, givenLeftItems[21]);
        leftAtkSpdDiv += MultAdder(-10f, givenLeftItems[59]);
        leftAtkSpdDiv += MultAdder(-60f, givenLeftItems[91]);
        leftAtkSpdDiv += MultAdder(-40f, givenLeftItems[138]);
        leftAtkSpdDiv += MultAdder(-20f, givenLeftItems[139]);
        leftAtkSpdDiv += MultAdder(-40f, givenLeftItems[151]);
        //Left Reload Speed
        leftReSpdMult += MultAdder(20f, givenLeftItems[7]);
        leftReSpdMult += MultAdder(10f, givenLeftItems[143]);
        leftReSpdMult += MultAdder(20f, givenLeftItems[161]);
        leftReSpdMult += MultAdder(40f, givenLeftItems[184]);

        leftReSpdDiv += MultAdder(-10f, givenLeftItems[59]);
        leftReSpdDiv += MultAdder(-60f, givenLeftItems[90]);
        leftReSpdDiv += MultAdder(-20f, givenLeftItems[113]);
        leftReSpdDiv += MultAdder(-100f, givenLeftItems[172]);
        leftReSpdDiv += MultAdder(-100f, givenLeftItems[180]);
        //Left Damage
        leftDmgMult += MultAdder(40f, givenLeftItems[21]);
        leftDmgMult += MultAdder(40f, givenLeftItems[13]);
        leftDmgMult += MultAdder(20f, givenLeftItems[4]);
        leftDmgMult += MultAdder(20f, givenLeftItems[11]);
        leftDmgMult += MultAdder(20f, givenLeftItems[40]);
        leftDmgMult += MultAdder(40f, givenLeftItems[26]);
        leftDmgMult += MultAdder(40f, givenLeftItems[27]);
        leftDmgMult += MultAdder(20f, givenLeftItems[66]);
        leftDmgMult += MultAdder(20f, givenLeftItems[84]);
        leftDmgMult += MultAdder(40f, givenLeftItems[90]);
        leftDmgMult += MultAdder(40f, givenLeftItems[91]);
        leftDmgMult += MultAdder(40f, givenLeftItems[102]);
        leftDmgMult += MultAdder(10f, givenLeftItems[98]);
        leftDmgMult += MultAdder(40f, givenLeftItems[121]);
        leftDmgMult += MultAdder(20f, givenLeftItems[133]);
        leftDmgMult += MultAdder(10f, givenLeftItems[143]);
        leftDmgMult += MultAdder(40f, givenLeftItems[157]);
        leftDmgMult += MultAdder(500f, givenLeftItems[180]);
        leftDmgMult += MultAdder(40f, givenLeftItems[184]);
        
        leftDmgDiv += MultAdder(-20f, givenLeftItems[12]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[31]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[46]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[59]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[113]);
        leftDmgDiv += MultAdder(-80f, givenLeftItems[118]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[138]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[139]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[149]);
        leftDmgDiv += MultAdder(-20f, givenLeftItems[163]);
        //Left Magazine Size
        leftMagSizeMult += MultAdder(20f, givenLeftItems[6]);
        leftMagSizeMult += MultAdder(10f, givenLeftItems[100]);
        leftMagSizeMult += MultAdder(50f, givenLeftItems[101]);
        leftMagSizeMult += MultAdder(60f, givenLeftItems[118]);
        leftMagSizeMult += MultAdder(10f, givenLeftItems[143]);
        leftMagSizeMult += MultAdder(20f, givenLeftItems[158]);
        leftMagSizeMult += MultAdder(20f, givenLeftItems[169]);
        leftMagSizeMult += MultAdder(40f, givenLeftItems[184]);
        leftMagSizeMult += MultAdder(100f, givenLeftItems[192]);

        leftMagSizeDiv += MultAdder(-50f, givenLeftItems[21]);
        leftMagSizeDiv += MultAdder(-40f, givenLeftItems[25]);
        leftMagSizeDiv += MultAdder(-40f, givenLeftItems[26]);
        leftMagSizeDiv += MultAdder(-10f, givenLeftItems[59]);
        leftMagSizeDiv += MultAdder(-20f, givenLeftItems[113]);
        leftMagSizeDiv += MultAdder(-40f, givenLeftItems[159]);
        leftMagSizeDiv += MultAdder(-20f, givenLeftItems[161]);
        //Left Accuracy
        leftAccMult += MultAdder(20f, givenLeftItems[8]);
        leftAccMult += MultAdder(40f, givenLeftItems[25]);
        leftAccMult += MultAdder(40f, givenLeftItems[27]);
        leftAccMult += MultAdder(40f, givenLeftItems[128]);
        leftAccMult += MultAdder(10f, givenLeftItems[143]);
        leftAccMult += MultAdder(40f, givenLeftItems[184]);

        leftAccDiv += MultAdder(-20f, givenLeftItems[32]);
        leftAccDiv += MultAdder(-20f, givenLeftItems[46]);
        leftAccDiv += MultAdder(-10f, givenLeftItems[59]);
        leftAccDiv += MultAdder(-60f, givenLeftItems[118]);
        leftAccDiv += MultAdder(-40f, givenLeftItems[121]);
        leftAccDiv += MultAdder(-20f, givenLeftItems[163]);
        leftAccDiv += MultAdder(-20f, givenLeftItems[173]);
        //Left Bullet Speed
        leftBulSpdMult += MultAdder(20f, givenLeftItems[9]);
        leftBulSpdMult += MultAdder(20f, givenLeftItems[57]);
        leftBulSpdMult += MultAdder(20f, givenLeftItems[76]);
        leftBulSpdMult += MultAdder(20f, givenLeftItems[119]);
        leftBulSpdMult += MultAdder(10f, givenLeftItems[143]);
        leftBulSpdMult += MultAdder(20f, givenLeftItems[173]);
        leftBulSpdMult += MultAdder(40f, givenLeftItems[184]);

        leftBulSpdDiv += MultAdder(-20f, givenLeftItems[11]);
        leftBulSpdDiv += MultAdder(-20f, givenLeftItems[12]);
        leftBulSpdDiv += MultAdder(-20f, givenLeftItems[59]);
        leftBulSpdDiv += MultAdder(-20f, givenLeftItems[77]);
        leftBulSpdDiv += MultAdder(-60f, givenLeftItems[118]);
        //Left Bullet Size
        leftBulSizeMult += MultAdder(20f, givenLeftItems[11]);
        leftBulSizeMult += MultAdder(20f, givenLeftItems[64]);
        leftBulSizeMult += MultAdder(40f, givenLeftItems[184]);
        //Left Bullet Pierce
        leftBulPir += givenLeftItems[10] + givenLeftItems[26] + givenLeftItems[82] + givenLeftItems[184];
        //Left Weak Point Chance
        leftWeakPointChance += 10f * givenLeftItems[141];
        leftWeakPointChance += 10f * givenLeftItems[150];
        leftWeakPointChance += 20f * givenLeftItems[184];
        //Left Weak Point Damage
        leftWeakPointDamageMult += MultAdder(20f, givenLeftItems[76]);
        leftWeakPointDamageMult += MultAdder(10f, givenLeftItems[141]);
        leftWeakPointDamageMult += MultAdder(20f, givenLeftItems[149]);
        leftWeakPointDamageMult += MultAdder(40f, givenLeftItems[184]);
        //Left Crit Chance
        leftCritChance += 20f * givenLeftItems[77];
        leftCritChance += 10f * givenLeftItems[78];
        leftCritChance += 20f * givenLeftItems[148];
        leftCritChance += 50f * givenLeftItems[179];
        leftCritChance += 20f * givenLeftItems[184];
        //Left Crit Damage
        leftCritDamageMult += MultAdder(20f, givenLeftItems[77]);
        leftCritDamageMult += MultAdder(100f, givenLeftItems[147]);
        leftCritDamageMult += MultAdder(40f, givenLeftItems[184]);
        //Left Other
        leftHeavyBul = givenLeftItems[11] + givenLeftItems[59];
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
        leftNerf = givenLeftItems[59];
        leftSpinach = givenLeftItems[60];
        leftStickTo = givenLeftItems[67];
        leftGunkyBless = givenLeftItems[69];
        leftGunkyClaw = givenLeftItems[70];
        leftGunkyAxe = givenLeftItems[71];
        leftClockwork = givenLeftItems[81];
        leftPrinter = givenLeftItems[88];
        leftMicrowave = givenLeftItems[89];
        leftSniperTower = givenLeftItems[103];
        leftPerfectedScope = givenLeftItems[104];
        leftPumpShotgunAttach = givenLeftItems[106];
        leftGrenadeAttach = givenLeftItems[107];
        leftGasGrenadeAttach = givenLeftItems[108];
        leftWarcry = givenLeftItems[110];
        leftTactReload = givenLeftItems[113];
        leftCarvedBone = givenLeftItems[129];
        leftCanineTooth = givenLeftItems[130];
        leftDoorKnob = givenLeftItems[132];
        leftHaunt = givenLeftItems[138];
        leftGoodies = givenLeftItems[139];
        leftAnatomy = givenLeftItems[141];
        leftEnzymes = givenLeftItems[145];
        leftDarkBranch = givenLeftItems[151];
        leftBrokenPen = givenLeftItems[156];
        leftRushJob = givenLeftItems[157];
        leftBrokenInk = givenLeftItems[158];
        leftChemicalAgents = givenLeftItems[160];
        left200Fleas = givenLeftItems[161];
        leftSmokingGun = givenLeftItems[162];
        leftForkedBarrel = givenLeftItems[163];
        leftRunicMag = givenLeftItems[164];
        leftSlots = givenLeftItems[168];
        leftTriggerHappy = givenLeftItems[169];
        leftOverCenti = givenLeftItems[170];
        leftOverCompress = givenLeftItems[171];
        leftBulletFactory = givenLeftItems[172];
        leftCritUnfunny = givenLeftItems[179];
        leftFortify = givenLeftItems[182];
        leftConfetti = givenLeftItems[187];
        
        leftRicochet = false;

        if (givenLeftItems[16] > 0f) { leftDmg = leftDmg * 1.2f; leftAcc = leftAcc * 1.1f; leftBulSpd = leftBulSpd * 1.1f; leftAtkSpd = leftAtkSpd / 1.2f; }
        if (givenLeftItems[21] > 0f) { leftDmg = leftDmg * 1.1f; leftAtkSpd = leftAtkSpd / 1.1f; leftMagSize = leftMagSize / 1.5f; }
        if (givenLeftItems[26] > 0f) { leftRicochet = true; }
        if (givenLeftItems[82] > 0f) { leftRicochet = true; }
        if (givenLeftItems[29] > 0f) { leftMagSize = (leftMagSize * 3f) * (givenLeftItems[29] * 1.2f); leftReSpd = leftReSpd / 2f; }
        if (givenLeftItems[102] > 0f) { leftBulPir++; }
        if (givenLeftItems[126] > 0f) { leftDmg = Calc(-40f, 1, leftDmg); }
        if (leftAdvTrig > 0 && leftMasterTrig > 0) { leftBulPir += 5; }
        if (leftIntroTrig > 0 && leftAdvTrig > 0 && leftMasterTrig > 0) { leftMagSize = Calc(40f, leftIntroTrig + leftAdvTrig, leftMagSize); }
        if (leftStickToCounters > 0f) { leftDmg = Calc(10f,leftStickToCounters, leftDmg); }
        if (givenLeftItems[135] > 0f) { float upBuff = 1 + ((transform.position.y / 50f) * 2f); if (upBuff > 2f) { upBuff = 2f; }; leftDmg *= upBuff; }

        //Right Attack Speed
        rightAtkSpdMult += MultAdder(20f, givenRightItems[62]);
        rightAtkSpdMult += MultAdder(10f, givenRightItems[97]);
        rightAtkSpdMult += MultAdder(60f, givenRightItems[118]);
        rightAtkSpdMult += MultAdder(60f, givenRightItems[132]);
        rightAtkSpdMult += MultAdder(10f, givenRightItems[143]);
        rightAtkSpdMult += MultAdder(20f, givenRightItems[153]);
        rightAtkSpdMult += MultAdder(40f, givenRightItems[157]);
        rightAtkSpdMult += MultAdder(20f, givenRightItems[161]);
        rightAtkSpdMult += MultAdder(40f, givenRightItems[169]);
        rightAtkSpdMult += MultAdder(40f, givenRightItems[184]);

        rightAtkSpdDiv += MultAdder(-50f, givenRightItems[21]);
        rightAtkSpdDiv += MultAdder(-10f, givenRightItems[59]);
        rightAtkSpdDiv += MultAdder(-60f, givenRightItems[91]);
        rightAtkSpdDiv += MultAdder(-40f, givenRightItems[138]);
        rightAtkSpdDiv += MultAdder(-20f, givenRightItems[139]);
        rightAtkSpdDiv += MultAdder(-40f, givenRightItems[151]);
        //Right Reload Speed
        rightReSpdMult += MultAdder(20f, givenRightItems[7]);
        rightReSpdMult += MultAdder(10f, givenRightItems[143]);
        rightReSpdMult += MultAdder(20f, givenRightItems[161]);
        rightReSpdMult += MultAdder(40f, givenRightItems[184]);

        rightReSpdDiv += MultAdder(-20f, givenRightItems[59]);
        rightReSpdDiv += MultAdder(-60f, givenRightItems[90]);
        rightReSpdDiv += MultAdder(-20f, givenRightItems[113]);
        rightReSpdDiv += MultAdder(-100f, givenRightItems[172]);
        rightReSpdDiv += MultAdder(-100f, givenRightItems[180]);
        //Right Damage
        rightDmgMult += MultAdder(40f, givenRightItems[21]);
        rightDmgMult += MultAdder(40f, givenRightItems[13]);
        rightDmgMult += MultAdder(20f, givenRightItems[11]);
        rightDmgMult += MultAdder(20f, givenRightItems[4]);
        rightDmgMult += MultAdder(20f, givenRightItems[40]);
        rightDmgMult += MultAdder(40f, givenRightItems[26]);
        rightDmgMult += MultAdder(40f, givenRightItems[27]);
        rightDmgMult += MultAdder(20f, givenRightItems[66]);
        rightDmgMult += MultAdder(40f, givenRightItems[90]);
        rightDmgMult += MultAdder(40f, givenRightItems[91]);
        rightDmgMult += MultAdder(40f, givenRightItems[102]);
        rightDmgMult += MultAdder(10f, givenRightItems[98]);
        rightDmgMult += MultAdder(40f, givenRightItems[121]);
        rightDmgMult += MultAdder(20f, givenRightItems[133]);
        rightDmgMult += MultAdder(10f, givenRightItems[143]);
        rightDmgMult += MultAdder(40f, givenRightItems[157]);
        rightDmgMult += MultAdder(500f, givenRightItems[180]);
        rightDmgMult += MultAdder(40f, givenRightItems[184]);

        rightDmgDiv += MultAdder(-20f, givenRightItems[12]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[31]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[46]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[59]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[113]);
        rightDmgDiv += MultAdder(-80f, givenRightItems[118]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[138]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[139]);
        rightDmgDiv += MultAdder(-20f, givenRightItems[149]);
        //Right Magazine Size
        rightMagSizeMult += MultAdder(20f, givenRightItems[6]);
        rightMagSizeMult += MultAdder(10f, givenRightItems[100]);
        rightMagSizeMult += MultAdder(50f, givenRightItems[101]);
        rightMagSizeMult += MultAdder(60f, givenRightItems[118]);
        rightMagSizeMult += MultAdder(10f, givenRightItems[143]);
        rightMagSizeMult += MultAdder(20f, givenRightItems[158]);
        rightMagSizeMult += MultAdder(20f, givenRightItems[169]);
        rightMagSizeMult += MultAdder(40f, givenRightItems[184]);
        rightMagSizeMult += MultAdder(100f, givenRightItems[192]);

        rightMagSizeDiv += MultAdder(-50f, givenRightItems[21]);
        rightMagSizeDiv += MultAdder(-40f, givenRightItems[25]);
        rightMagSizeDiv += MultAdder(-40f, givenRightItems[26]);
        rightMagSizeDiv += MultAdder(-10f, givenRightItems[59]);
        rightMagSizeDiv += MultAdder(-20f, givenRightItems[113]);
        rightMagSizeDiv += MultAdder(-40f, givenRightItems[159]);
        rightMagSizeDiv += MultAdder(-20f, givenRightItems[161]);
        //Right Accuracy
        rightAccMult += MultAdder(20f, givenRightItems[8]);
        rightAccMult += MultAdder(40f, givenRightItems[25]);
        rightAccMult += MultAdder(40f, givenRightItems[27]);
        rightAccMult += MultAdder(40f, givenRightItems[128]);
        rightAccMult += MultAdder(10f, givenRightItems[143]);
        rightAccMult += MultAdder(40f, givenRightItems[184]);

        rightAccDiv += MultAdder(-20f, givenRightItems[32]);
        rightAccDiv += MultAdder(-20f, givenRightItems[46]);
        rightAccDiv += MultAdder(-10f, givenRightItems[59]);
        rightAccDiv += MultAdder(-60f, givenRightItems[118]);
        rightAccDiv += MultAdder(-40f, givenRightItems[121]);
        rightAccDiv += MultAdder(-20f, givenRightItems[173]);
        //Right Bullet Speed
        rightBulSpdMult += MultAdder(20f, givenRightItems[9]);
        rightBulSpdMult += MultAdder(20f, givenRightItems[57]);
        rightBulSpdMult += MultAdder(20f, givenRightItems[76]);
        rightBulSpdMult += MultAdder(20f, givenRightItems[119]);
        rightBulSpdMult += MultAdder(10f, givenRightItems[143]);
        rightBulSpdMult += MultAdder(20f, givenRightItems[173]);
        rightBulSpdMult += MultAdder(40f, givenRightItems[184]);

        rightBulSpdDiv += MultAdder(-20f, givenRightItems[11]);
        rightBulSpdDiv += MultAdder(-20f, givenRightItems[12]);
        rightBulSpdDiv += MultAdder(-20f, givenRightItems[59]);
        rightBulSpdDiv += MultAdder(-20f, givenRightItems[77]);
        rightBulSpdDiv += MultAdder(-60f, givenRightItems[118]);
        //Right Bullet Size
        rightBulSizeMult += MultAdder(20f, givenRightItems[11]);
        rightBulSizeMult += MultAdder(20f, givenRightItems[64]);
        rightBulSizeMult += MultAdder(40f, givenRightItems[184]);
        //Right Pierce
        rightBulPir += givenRightItems[10] + givenRightItems[26] + givenRightItems[82] + givenRightItems[184];
        //Right Weak Point Chance
        rightWeakPointChance += 10f * givenRightItems[141];
        rightWeakPointChance += 10f * givenRightItems[150];
        rightWeakPointChance += 20f * givenRightItems[184];
        //Right Weak Point Damage
        rightWeakPointDamageMult += MultAdder(20f, givenRightItems[76]);
        rightWeakPointDamageMult += MultAdder(10f, givenRightItems[141]);
        rightWeakPointDamageMult += MultAdder(20f, givenRightItems[149]);
        rightWeakPointDamageMult += MultAdder(40f, givenRightItems[184]);
        //Right Crit Chance
        rightCritChance += 20f * givenRightItems[77];
        rightCritChance += 10f * givenRightItems[78];
        rightCritChance += 20f * givenRightItems[148];
        rightCritChance += 50f * givenRightItems[179];
        rightCritChance += 20f * givenRightItems[184];
        //Right Crit Damage
        rightCritDamageMult += MultAdder(20f, givenRightItems[77]);
        rightCritDamageMult += MultAdder(100f, givenRightItems[147]);
        rightCritDamageMult += MultAdder(40f, givenRightItems[184]);
        //Right Other
        rightHeavyBul = givenRightItems[11] + givenRightItems[59];
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
        rightNerf = givenRightItems[59];
        rightSpinach = givenRightItems[60];
        rightStickTo = givenRightItems[67];
        rightGunkyBless = givenRightItems[69];
        rightGunkyClaw = givenRightItems[70];
        rightGunkyAxe = givenRightItems[71];
        rightClockwork = givenRightItems[81];
        rightPrinter = givenRightItems[88];
        rightMicrowave = givenRightItems[89];
        rightSniperTower = givenRightItems[103];
        rightPerfectedScope = givenRightItems[104];
        rightPumpShotgunAttach = givenRightItems[106];
        rightGrenadeAttach = givenRightItems[107];
        rightGasGrenadeAttach = givenRightItems[108];
        rightWarcry = givenRightItems[110];
        rightTactReload = givenRightItems[113];
        rightCarvedBone = givenRightItems[129];
        rightCanineTooth = givenRightItems[130];
        rightDoorKnob = givenRightItems[132];
        rightHaunt = givenRightItems[138];
        rightGoodies = givenRightItems[139];
        rightAnatomy = givenRightItems[141];
        rightEnzymes = givenRightItems[145];
        rightDarkBranch = givenRightItems[151];
        rightBrokenPen = givenRightItems[156];
        rightRushJob = givenRightItems[157];
        rightBrokenInk = givenRightItems[158];
        rightChemicalAgents = givenRightItems[160];
        right200Fleas = givenRightItems[161];
        rightSmokingGun = givenRightItems[162];
        rightForkedBarrel = givenRightItems[163];
        rightRunicMag = givenRightItems[164];
        rightSlots = givenRightItems[168];
        rightTriggerHappy = givenRightItems[169];
        rightOverCenti = givenRightItems[170];
        rightOverCompress = givenRightItems[171];
        rightBulletFactory = givenRightItems[172];
        rightCritUnfunny = givenRightItems[179];
        rightFortify = givenRightItems[182];
        rightConfetti = givenRightItems[187];

        rightRicochet = false;

        if (givenRightItems[16] > 0f) { rightDmg = rightDmg * 1.2f; rightAcc = rightAcc * 1.1f; rightBulSpd = rightBulSpd * 1.1f; rightAtkSpd = rightAtkSpd / 1.2f; }
        if (givenRightItems[21] > 0f) { rightDmg = rightDmg * 1.1f; rightAtkSpd = rightAtkSpd / 1.1f; rightMagSize = rightMagSize / 1.5f; }
        if (givenRightItems[26] > 0f) { rightRicochet = true; }
        if (givenRightItems[82] > 0f) { rightRicochet = true; }
        if (givenRightItems[29] > 0f) { rightMagSize = (rightMagSize * 3f) * (givenRightItems[29] * 1.2f); rightReSpd = rightReSpd / 2f; }
        if (givenRightItems[102] > 0f) { rightBulPir++; }
        if (givenRightItems[126] > 0f) { rightDmg = Calc(-40f, 1, rightDmg); }
        if (rightAdvTrig > 0 && rightMasterTrig > 0) { rightBulPir += 5; }
        if (rightIntroTrig > 0 && rightAdvTrig > 0 && rightMasterTrig > 0) { rightMagSize = Calc(40f, rightIntroTrig + rightAdvTrig, rightMagSize); }
        if (rightStickToCounters > 0f) { rightDmg = Calc(10f, rightStickToCounters, rightDmg); }
        if (givenRightItems[135] > 0f) { float upBuff = 1 + ((transform.position.y / 50f) * 2f); if (upBuff > 2f) { upBuff = 2f; } ; rightDmg *= upBuff; }
        //endless mag
        if(leftEndless > 0) { leftMagSizeMult *= 2f; } if(rightEndless > 0) { rightMagSizeMult *= 2; }
        //Apply Mult
        leftAtkSpd *= leftAtkSpdMult; leftAtkSpd /= leftAtkSpdDiv; rightAtkSpd *= rightAtkSpdMult; rightAtkSpd /= rightAtkSpdDiv;
        leftReSpd *= leftReSpdMult; leftReSpd /= leftReSpdDiv; rightReSpd *= rightReSpdMult; rightReSpd /= rightReSpdDiv;
        leftDmg *= leftDmgMult; leftDmg /= leftDmgDiv; rightDmg *= rightDmgMult; rightDmg /= rightDmgDiv;
        leftMagSize *= leftMagSizeMult; leftMagSize /= leftMagSizeDiv; rightMagSize *= rightMagSizeMult; rightMagSize /= rightMagSizeDiv;
        leftAcc *= leftAccMult; leftAcc /= leftAccDiv; rightAcc *= rightAccMult; rightAcc /= rightAccDiv;
        leftBulSpd *= leftBulSpdMult; leftBulSpd /= leftBulSpdDiv; rightBulSpd *= rightBulSpdMult; rightBulSpd /= rightBulSpdDiv;
        leftBulSize *= leftBulSizeMult; leftBulSize /= leftBulSizeDiv; rightBulSize *= rightBulSizeMult; rightBulSize /= rightBulSizeDiv;
        leftCritDamage *= leftCritDamageMult; leftCritDamage /= leftCritDamageDiv; rightCritDamage *= rightCritDamageMult; rightCritDamage /= rightCritDamageDiv;
        leftWeakPointDamage *= leftWeakPointDamageMult; leftWeakPointDamage /= leftWeakPointDamageDiv; rightWeakPointDamage *= rightWeakPointDamageMult; rightWeakPointDamage /= rightWeakPointDamageDiv;
        //Trigger Happy
        if(leftTriggerHappy > 0) { leftAtkSpd *= 2; }
        if(rightTriggerHappy > 0) { rightAtkSpd *= 2; }
        //Forked Barrel
        if (givenLeftItems[163] > 0) { leftDmg /= 2f; leftAcc /= 2f; }
        if (givenRightItems[163] > 0) { rightDmg /= 2f; rightAcc /= 2f; }
        //Rotation transfer cable
        if (givenLeftItems[153] > 0) { leftAtkSpd /= 2f; leftCritChance += (((100 * leftAtkSpdMult) - (100 * leftAtkSpdDiv))); } 
        if (givenRightItems[153] > 0) { rightAtkSpd /= 2f; rightCritChance += (((100 * rightAtkSpdMult) - (100 * rightAtkSpdDiv))); }
        //Thumb Tack
        if (givenLeftItems[59] > 0 && givenLeftItems[149] > 0) { leftDmg *= 3f; }
        if (givenRightItems[59] > 0 && givenRightItems[149] > 0) { rightDmg *= 3f; }
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

        //One in the chamber
        if (givenLeftItems[180] > 0) { leftMagSize = 0f; }
        if (givenRightItems[180] > 0) { rightMagSize = 0f; }

        rightGunScript.StatUpdateRight();
        leftGunScript.StatUpdateLeft();

        //undo any changes made
        if(leftGunScript.gunName == "Da Eagle")
        {
            givenLeftItems.Clear();
            givenLeftItems.AddRange(leftList);
        }
        if(rightGunScript.gunName == "Da Eagle")
        {
            givenRightItems.Clear();
            givenRightItems.AddRange(rightList);
        }
    }
    float MultAdder(float mult, int amount)
    {
        if (mult > 0) { return mult * (1f / 100f) * amount; }
        if (mult < 0) { return -mult * (1f / 100f) * amount; }
        return 0;
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
        if(leftGunScript == null) { leftGunScript = leftHand.GetComponentInChildren<GunScript>(); }
        if(rightGunScript == null) { rightGunScript = rightHand.GetComponentInChildren<GunScript>(); }

        axeCooldown -= Time.deltaTime * (1+(leftGunkyAxe + rightGunkyAxe)/10f) * (1 + leftClockwork + rightClockwork);
        if (healthMan.dead) { return; }

        if (Cursor.lockState == CursorLockMode.Locked) { leftGunUpdate(); RightGunUpdate(); }
        if (Input.GetKeyDown(KeyCode.R) && Cursor.lockState == CursorLockMode.Locked)
        {
            leftGunScript.AttemptReload();
            rightGunScript.AttemptReload();

            if (axeCooldown <= 0 && leftGunkyAxe + rightGunkyAxe > 0)
            {
                SpawnAxe(Camera.main.transform.forward);
                axeCooldown = 10f;
            }
        }

        itemChecks();
    }

    void leftGunUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            leftGunScript.AttemptShoot();
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftGunScript.AttemptShootUp(false);
        }
    }

    void RightGunUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            rightGunScript.AttemptShoot();
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightGunScript.AttemptShootUp(false);
        }
    }

    void itemChecks()
    {
        if (leftMutatedCell > 0)
        {
            leftMutatedCellTimer -= Time.deltaTime + (Time.deltaTime * leftClockwork);
            if (leftMutatedCellTimer <= 0)
            {
                mutatedCellReroll(playerItem.leftItems);
                leftMutatedCellTimer = playerItem.FindObjByID(14).baseCooldown / (leftMutatedCell / 10f + 1f);
            }
        }

        if (rightMutatedCell > 0)
        {
            rightMutatedCellTimer -= Time.deltaTime + (Time.deltaTime * rightClockwork);
            if (rightMutatedCellTimer <= 0)
            {
                mutatedCellReroll(playerItem.rightItems);
                rightMutatedCellTimer = playerItem.FindObjByID(14).baseCooldown / (rightMutatedCell / 10f + 1f);
            }
        }

        if (leftHungryParasite > 0)
        {
            leftHungryParasiteTimer -= Time.deltaTime + (Time.deltaTime * leftClockwork);
            if (leftHungryParasiteTimer <= 0)
            {
                HungryParasiteReroll(playerItem.leftItems);
                leftHungryParasiteTimer = playerItem.FindObjByID(24).baseCooldown / (leftHungryParasite / 2f + 1f);
            }
        }

        if (rightHungryParasite > 0)
        {
            rightHungryParasiteTimer -= Time.deltaTime + (Time.deltaTime * rightClockwork);
            if (rightHungryParasiteTimer <= 0)
            {
                HungryParasiteReroll(playerItem.rightItems);
                rightHungryParasiteTimer = playerItem.FindObjByID(24).baseCooldown / (rightHungryParasite / 2f + 1f);
            }
        }

        if (leftFastInserter > 0)
        {
            leftFastInserterTimer -= Time.deltaTime + (Time.deltaTime * leftClockwork);
            if (leftFastInserterTimer <= 0 && leftGunScript.currentBullets < rightGunScript.magSize)
            {
                leftGunScript.addBullet();
                leftFastInserterTimer = playerItem.FindObjByID(33).baseCooldown / (0.2f * leftFastInserter);
            }
        }
        if (rightFastInserter > 0)
        {
            rightFastInserterTimer -= Time.deltaTime + (Time.deltaTime * rightClockwork);
            if (rightFastInserterTimer <= 0 && rightGunScript.currentBullets < rightGunScript.magSize)
            {
                rightGunScript.addBullet();
                rightFastInserterTimer = playerItem.FindObjByID(33).baseCooldown / (0.2f * rightFastInserter);
            }
        }

        if(leftSponDeal > 0)
        {
            leftSponTimer += Time.deltaTime + (Time.deltaTime * leftClockwork);
            if(leftSponTimer > playerItem.FindObjByID(42).baseCooldown)
            {
                int rand = Random.Range(0, playerItem.sponserItems.Count);
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
            rightSponTimer += Time.deltaTime + (Time.deltaTime * rightClockwork);
            if (rightSponTimer > playerItem.FindObjByID(42).baseCooldown)
            {
                int rand = Random.Range(0, playerItem.sponserItems.Count);
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

        surpriseEggTimer += Time.deltaTime + (Time.deltaTime * (leftClockwork+rightClockwork));
        if (leftSurpriseEggLifetime > 0)
        {
            if(surpriseEggTimer > playerItem.FindObjByID(58).baseCooldown && healthMan.timeSinceEnemyDied < 120)
            {
                playerItem.leftItems[55] += 1;
            }
        }
        if (rightSurpriseEggLifetime > 0)
        {
            if (surpriseEggTimer > playerItem.FindObjByID(58).baseCooldown && healthMan.timeSinceEnemyDied < 120)
            {
                playerItem.rightItems[55] += 1;
            }
        }
        if (surpriseEggTimer > playerItem.FindObjByID(55).baseCooldown) { surpriseEggTimer = 0; }

        leftPrinterTimer += Time.deltaTime + (Time.deltaTime * leftClockwork);
        rightPrinterTimer += Time.deltaTime + (Time.deltaTime * rightClockwork);
        if(leftPrinter > 0 && healthMan.timeSinceEnemyDied < 60 && leftPrinterTimer > playerItem.FindObjByID(88).baseCooldown)
        {
            leftPrinterTimer = 0;
            List<int> itemsOwned = new List<int>();
            for(int i = 0; i < playerItem.leftItems.Count; i++)
            {
                if (playerItem.leftItems[i] > 0) { itemsOwned.Add(i); }
            }
            playerItem.leftItems[itemsOwned[Random.Range(0, itemsOwned.Count)]]+= leftPrinter;
            if (Random.Range(1, 100) < leftPrinter * 10) { playerItem.leftItems[88]--; }
        }
        if (rightPrinter > 0 && healthMan.timeSinceEnemyDied < 60 && rightPrinterTimer > playerItem.FindObjByID(88).baseCooldown)
        {
            rightPrinterTimer = 0;
            List<int> itemsOwned = new List<int>();
            for (int i = 0; i < playerItem.rightItems.Count; i++)
            {
                if (playerItem.rightItems[i] > 0) { itemsOwned.Add(i); }
            }
            playerItem.rightItems[itemsOwned[Random.Range(0, itemsOwned.Count)]]+= rightPrinter;
            if (Random.Range(1, 100) < rightPrinter * 10) { playerItem.rightItems[88]--; }
        }

        if (leftStickToCounters > leftStickTo * 5) { leftStickToCounters = leftStickTo * 5; }
        if(rightStickToCounters > rightStickTo * 5) { rightStickToCounters = rightStickTo * 5; }

        if(leftGunScript.reloading && leftMicrowave > 0)
        {
            leftMicrowaveTimer -= Time.deltaTime + (Time.deltaTime * leftClockwork);
            if(leftMicrowaveTimer <= 0)
            {
                GameObject spawnedMicrowave = Instantiate(microwave);
                spawnedMicrowave.transform.position = leftHand.transform.position;
                spawnedMicrowave.transform.rotation = leftHand.transform.rotation;
                spawnedMicrowave.GetComponent<Shockwave>().lifetime = 1f;
                spawnedMicrowave.GetComponent<Shockwave>().damage = 5f * leftMicrowave;
                leftMicrowaveTimer = leftReSpd / 4f;
            }
        }
        if (rightGunScript.reloading && rightMicrowave > 0)
        {
            rightMicrowaveTimer -= Time.deltaTime + (Time.deltaTime * rightClockwork);
            if (rightMicrowaveTimer <= 0)
            {
                GameObject spawnedMicrowave = Instantiate(microwave);
                spawnedMicrowave.transform.position = rightHand.transform.position;
                spawnedMicrowave.transform.rotation = rightHand.transform.rotation;
                spawnedMicrowave.GetComponent<Shockwave>().lifetime = 1f;
                spawnedMicrowave.GetComponent<Shockwave>().damage = 5f * rightMicrowave;
                rightMicrowaveTimer = rightReSpd / 4f;
            }
        }

        if (playerItem.leftItems[125] > 0)
        {
            if (leftGunScript.reloading)
            {
                if (darkwaveTimer <= 0)
                {
                    GameObject spawnedMicrowave = Instantiate(darkwave);
                    spawnedMicrowave.transform.position = leftHand.transform.position;
                    spawnedMicrowave.transform.rotation = leftHand.transform.rotation;
                    spawnedMicrowave.GetComponent<Shockwave>().lifetime = 2f;
                    spawnedMicrowave.GetComponent<Shockwave>().damage = 0f;
                    darkwaveTimer = leftReSpd * 2f;
                }
            }
            darkwaveTimer -= Time.deltaTime + (Time.deltaTime * leftClockwork);
        }
        if (playerItem.rightItems[125] > 0)
        {
            if (rightGunScript.reloading)
            {
                if (darkwaveTimer <= 0)
                {
                    GameObject spawnedMicrowave = Instantiate(darkwave);
                    spawnedMicrowave.transform.position = rightHand.transform.position;
                    spawnedMicrowave.transform.rotation = rightHand.transform.rotation;
                    spawnedMicrowave.GetComponent<Shockwave>().lifetime = 2f;
                    spawnedMicrowave.GetComponent<Shockwave>().damage = 0f;
                    darkwaveTimer = rightReSpd * 2f;
                }
            }
            darkwaveTimer -= Time.deltaTime + (Time.deltaTime * rightClockwork);
        }

        leftKickCooldown -= Time.deltaTime; if(leftKickCooldown <= 0) { leftLeg.SetActive(false); }
        rightKickCooldown -= Time.deltaTime; if (rightKickCooldown <= 0) { rightLeg.SetActive(false); }

        if(leftOverCenti > 0 || rightOverCenti > 0)
        {
            centriCheckTimer -= Time.deltaTime;
            if(centriCheckTimer <= 0)
            {
                OverwrittenCentrifugeCheck();
                centriCheckTimer = 5f;
            }
        }

        if (leftSpinach + rightSpinach > 0)
        {
            if(leftMRs.Count == 0 || rightMRs.Count == 0) { GrabMeshRenders(); }
            try
            {
                if (leftSpinach > 0)
                {
                    for (int i = 0; i < leftMRs.Count; i++)
                    { leftMRs[i].material.color = new Color(leftColors[i].r - (leftSpinach / 50f), leftColors[i].g + (leftSpinach / 50f), leftColors[i].b - (leftSpinach / 50f), leftColors[i].a); }
                }
                if (rightSpinach > 0)
                {
                    for (int i = 0; i < rightMRs.Count; i++)
                    { rightMRs[i].material.color = new Color(rightColors[i].r - (rightSpinach / 50f), rightColors[i].g + (rightSpinach / 50f), rightColors[i].b - (rightSpinach / 50f), rightColors[i].a); }
                }
            }
            catch (System.Exception)
            {
                GrabMeshRenders();
                throw;
            }
        }
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
    public void SpawnAxe(Vector3 dir)
    {
        GameObject spawnedAxe = Instantiate(gunkyAxe);
        spawnedAxe.transform.position = transform.position + (dir + Vector3.up) * 2f;
        spawnedAxe.transform.rotation = transform.rotation;
        spawnedAxe.GetComponent<GunkyAxe>().damage = ((leftGunScript.dmg + rightGunScript.dmg) / 2f) * 5f;
        spawnedAxe.GetComponent<Rigidbody>().AddForce((Vector3.up * 4f) + (dir * 20f), ForceMode.Impulse);
    }
    public void Kick(string hand)
    {

        Vector3 camPos = Camera.main.transform.position;
        Ray ray = new Ray(camPos, Camera.main.transform.forward);
        RaycastHit hit;
        if (hand == "left" && leftKickCooldown <= 0) 
        { 
            leftLeg.SetActive(true); leftLeg.GetComponentInChildren<Animator>().speed = leftAtkSpd/2f; leftKickCooldown = leftAtkSpd * 2f;
            if (Physics.Raycast(ray, out hit, 6f))
            {
                string hitTag = hit.collider.gameObject.tag;

                Vector3 force = Camera.main.transform.forward * (leftDmg/2f) * 90f;
                if (force.magnitude > 300f) { force = force.normalized * 300f; }

                if(hitTag == "Untagged" || hitTag == "Ground")
                {
                    //knockback
                    playerItem.playerMvt.rb.AddForce(-force, ForceMode.Impulse);
                }
                else if(hitTag == "Enemy" || hitTag == "EnemyWeakPoint")
                {
                    //damage and knockback them
                    if(hit.transform.parent != null)
                    {
                        if (hit.transform.parent.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
                        {
                            ehm.TakeDamage(leftDmg * 30f, false, HitType.ht.normal, hit.point, hand);
                        }
                        if (hit.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody erb))
                        {
                            erb.AddForce(force, ForceMode.Impulse);
                        }
                    }
                    else
                    {
                        if (hit.transform.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
                        {
                            ehm2.TakeDamage(leftDmg * 30f, false, HitType.ht.normal, hit.point, hand);
                        }
                        if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody erb2))
                        {
                            erb2.AddForce(force, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
        if(hand == "right" && rightKickCooldown <= 0)
        { 
            rightLeg.SetActive(true); rightLeg.GetComponentInChildren<Animator>().speed = rightAtkSpd/2f; rightKickCooldown = rightAtkSpd * 2f;
            if (Physics.Raycast(ray, out hit, 6f))
            {
                string hitTag = hit.collider.gameObject.tag;

                Vector3 force = Camera.main.transform.forward * (rightDmg / 2f) * 90f;
                if (force.magnitude > 300f) { force = force.normalized * 300f; }

                if (hitTag == "Untagged" || hitTag == "Ground")
                {
                    //knockback
                    playerItem.playerMvt.rb.AddForce(-force, ForceMode.Impulse);
                }
                else if (hitTag == "Enemy" || hitTag == "EnemyWeakPoint")
                {
                    //damage and knockback them
                    if (hit.transform.parent != null)
                    {
                        if (hit.transform.parent.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm))
                        {
                            ehm.TakeDamage(rightDmg * 30f, false, HitType.ht.normal, hit.point, hand);
                        }
                        if (hit.transform.parent.TryGetComponent<Rigidbody>(out Rigidbody erb))
                        {
                            erb.AddForce(force, ForceMode.Impulse);
                        }
                    }
                    else
                    {
                        if (hit.transform.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager ehm2))
                        {
                            ehm2.TakeDamage(rightDmg * 30f, false, HitType.ht.normal, hit.point, hand);
                        }
                        if (hit.transform.TryGetComponent<Rigidbody>(out Rigidbody erb2))
                        {
                            erb2.AddForce(force, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
    }

    void OverwrittenCentrifugeCheck()
    {
        if (leftOverCenti > 0)
        {
            for(int i = 0; i < playerItem.leftItems.Count; i++)
            {
                if(playerItem.leftItems[i]>1 && !rarityList[8].Contains(i))
                {
                    playerItem.leftItems[i] -= 1; playerItem.AddRandItemsFromRarity(1, playerItem.FindRarityByID(i), "left", false);
                }
            }
        }
        if (rightOverCenti > 0)
        {
            for (int i = 0; i < playerItem.rightItems.Count; i++)
            {
                if (playerItem.rightItems[i] > 1 && !rarityList[8].Contains(i))
                {
                    playerItem.rightItems[i] -= 1; playerItem.AddRandItemsFromRarity(1, playerItem.FindRarityByID(i), "right", false);
                }
            }
        }
    }
}