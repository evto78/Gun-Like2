using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Script : GunScript
{
    public override void StatUpdateLeft()
    {
        whatHandThisIsIn = "left";

        magSize = Mathf.Round(baseMagSize * manager.leftMagSize);
        atkSpd = baseAtkSpd * manager.leftAtkSpd;
        reSpd = baseReSpd * manager.leftReSpd;
        bulSpd = baseBulSpd * manager.leftBulSpd;
        dmg = baseDmg * manager.leftDmg;
        acc = baseAcc / manager.leftAcc;
        bulSize = baseBulSize * manager.leftBulSize;
        bulPir = baseBulPir + manager.leftBulPir;
        critChance = baseCritChance * manager.leftCritChance;
        critDamage = baseCritDamage * manager.leftCritDamage;
        weakPointChance = baseWeakPointChance * manager.leftWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.leftWeakPointDamage;

        heavyBul = manager.leftHeavyBul;
        bowAct = manager.leftBowAct;
        heavySpirits = manager.leftHeavySpirit;
        nuclearBul = manager.leftNuclearBul;
        introTrig = manager.leftIntroTrig;
        advTrig = manager.leftAdvTrig;
        masterTrig = manager.leftMasterTrig;
        jam = manager.leftJam;
        fireSpon = manager.leftFireSpon;
        sharperSpon = manager.leftSharperSpon;
        silverSpon = manager.leftSilverSpon;
        helpingSpon = manager.leftHelpingSpon;
        coolSpon = manager.leftCoolSpon;
        fastSpon = manager.leftFastSpon;
        largeSpon = manager.leftLargeSpon;
        possession = manager.leftPossession;

        ricochet = manager.leftRicochet;

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        //vector3 unique
        if (acc > 1f) { acc = 1f; }
        atkSpd += 0.8f / acc;
    }

    public override void StatUpdateRight()
    {
        whatHandThisIsIn = "right";

        magSize = Mathf.Round(baseMagSize * manager.rightMagSize);
        atkSpd = baseAtkSpd * manager.rightAtkSpd;
        reSpd = baseReSpd * manager.rightReSpd;
        bulSpd = baseBulSpd * manager.rightBulSpd;
        dmg = baseDmg * manager.rightDmg;
        acc = baseAcc / manager.rightAcc;
        bulSize = baseBulSize * manager.rightBulSize;
        bulPir = baseBulPir + manager.rightBulPir;
        critChance = baseCritChance * manager.rightCritChance;
        critDamage = baseCritDamage * manager.rightCritDamage;
        weakPointChance = baseWeakPointChance * manager.rightWeakPointChance;
        weakPointDamage = baseWeakPointDamage * manager.rightWeakPointDamage;

        heavyBul = manager.rightHeavyBul;
        bowAct = manager.rightBowAct;
        heavySpirits = manager.rightHeavySpirit;
        nuclearBul = manager.rightNuclearBul;
        introTrig = manager.rightIntroTrig;
        advTrig = manager.rightAdvTrig;
        masterTrig = manager.rightMasterTrig;
        jam = manager.rightJam;
        fireSpon = manager.rightFireSpon;
        sharperSpon = manager.rightSharperSpon;
        silverSpon = manager.rightSilverSpon;
        helpingSpon = manager.rightHelpingSpon;
        coolSpon = manager.rightCoolSpon;
        fastSpon = manager.rightFastSpon;
        largeSpon = manager.rightLargeSpon;
        possession = manager.rightPossession;

        ricochet = manager.rightRicochet;

        //STAT CAPS!
        if (bulSpd > 500f)
        {
            bulSpd = 500f;
        }
        if (acc > 25f)
        {
            acc = 25f;
        }
        //vector3 unique
        if (acc > 1f) { acc = 1f; }
        atkSpd += 0.8f / acc;
    }
}
