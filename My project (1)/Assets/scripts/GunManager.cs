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

    // left weapons base stats
    public float leftAtkSpd = 1f;
    public float leftReSpd = 1f;
    public float leftDmg = 1f;
    public float leftMagSize = 1f;
    public float leftAcc = 1f;
    public float leftBulSpd = 1f;
    public float leftBulSize = 1f;
    public int leftBulPir = 0;

    // right weapons base stats
    public float rightAtkSpd = 1f;
    public float rightReSpd = 1f;
    public float rightDmg = 1f;
    public float rightMagSize = 1f;
    public float rightAcc = 1f;
    public float rightBulSpd = 1f;
    public float rightBulSize = 1f;
    public int rightBulPir = 0;
    public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems)
    {
        masterAtkSpd = 1f;
        masterReSpd = 1f;
        masterDmg = 1f;
        masterMagSize = 1f;
        masterAcc = 1f;
        masterBulSpd = 1f;
        masterBulSize = 1f;
        masterBulPir = 0;

        leftAtkSpd = 1f * masterAtkSpd;
        leftReSpd = 1f * (givenLeftItems[7] / 10f + 1f) * masterReSpd;
        leftDmg = 1f * (givenLeftItems[4] / 10f + 1f) * masterDmg;
        leftMagSize = 1f * (givenLeftItems[6] / 5f + 1f) * masterMagSize;
        leftAcc = 1f * (givenLeftItems[8] / 5f + 1f) * masterAcc;
        leftBulSpd = 1f * (givenLeftItems[9] / 10f + 1f) * masterBulSpd;
        leftBulSize = 1f * masterBulSize;
        leftBulPir = (0 + givenLeftItems[10] + masterBulPir);

        rightAtkSpd = 1f * masterAtkSpd;
        rightReSpd = 1f * (givenRightItems[7] / 10f + 1f) * masterReSpd;
        rightDmg = 1f * (givenRightItems[4] / 10f + 1f) * masterDmg;
        rightMagSize = 1f * (givenRightItems[6] / 5f + 1f) * masterMagSize;
        rightAcc = 1f * (givenRightItems[8] / 5f + 1f) * masterAcc;
        rightBulSpd = 1f * (givenRightItems[9] / 10f + 1f) * masterBulSpd;
        rightBulSize = 1f * masterBulSize;
        rightBulPir = (0 + givenRightItems[10] + masterBulPir);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
