using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
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
    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
    {
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

        leftAtkSpd = 1f * masterAtkSpd;
        leftReSpd = 1f * (givenLeftItems[7] / 10f + 1f) * masterReSpd;
        leftDmg = 1f * (givenLeftItems[4] / 10f + 1f) * masterDmg;
        leftMagSize = 1f * (givenLeftItems[6] / 5f + 1f) * masterMagSize;
        leftAcc = 1f * (givenLeftItems[8] / 5f + 1f) * masterAcc;
        leftBulSpd = 1f * (givenLeftItems[9] / 10f + 1f) * masterBulSpd;
        leftBulSize = 1f * masterBulSize;
        leftBulPir = (0 + givenLeftItems[10] + masterBulPir);
        leftCritChance = 1f * masterCritChance;
        leftCritDamage = 1f * masterCritDamage;
        leftWeakPointChance = 1f * masterWeakPointChance;
        leftWeakPointDamage = 1f * masterWeakPointDamage;

        rightAtkSpd = 1f * masterAtkSpd;
        rightReSpd = 1f * (givenRightItems[7] / 10f + 1f) * masterReSpd;
        rightDmg = 1f * (givenRightItems[4] / 10f + 1f) * masterDmg;
        rightMagSize = 1f * (givenRightItems[6] / 5f + 1f) * masterMagSize;
        rightAcc = 1f * (givenRightItems[8] / 5f + 1f) * masterAcc;
        rightBulSpd = 1f * (givenRightItems[9] / 10f + 1f) * masterBulSpd;
        rightBulSize = 1f * masterBulSize;
        rightBulPir = (0 + givenRightItems[10] + masterBulPir) + 1;
        rightCritChance = 1f * masterCritChance;
        rightCritDamage = 1f * masterCritDamage;
        rightWeakPointChance = 1f * masterWeakPointChance;
        rightWeakPointDamage = 1f * masterWeakPointDamage;

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
    }

    void leftGunUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            leftHand.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
    }

    void RightGunUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            rightHand.SendMessage("AttemptShoot", SendMessageOptions.DontRequireReceiver);
        }
    }
}