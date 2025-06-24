using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelScript : GunScript
{
    int bulletsQued;
    public override void AttemptShoot()
    {
        if ((bowAct > 0))
        {
            bowCharge += 1 * atkSpd * Time.deltaTime;
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; }
        }
        else
        {
            if (!reloading && !shooting)
            {
                if(Mathf.CeilToInt(magSize / 2f) > 25) { bulletsQued = Mathf.CeilToInt(magSize / 2f); }
                else
                {
                    for (int i = 0; i < Mathf.CeilToInt(magSize / 2f); i++)
                    {
                        Shoot(1f);
                        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(1f); }
                        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(1f); }
                    }
                }
                if (pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
                {
                    acc = acc * 2f;
                    for (int i = 0; i < 9; i++)
                    {
                        currentBullets++;
                        Shoot(1f);
                        if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(1f); }
                        if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(1f); }
                    }

                    pumpShotgunAttachTimer = 30f;
                }
            }
        }
    }

    public override void AttemptShootUp()
    {
        if (bowAct > 0)
        {
            if (Mathf.CeilToInt(magSize / 2f) > 25) { bulletsQued = Mathf.CeilToInt(magSize / 2f); }
            else
            {
                for (int i = 0; i < Mathf.CeilToInt(magSize / 2f); i++)
                {
                    Shoot(bowCharge);
                    if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(bowCharge); }
                    if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(bowCharge); }
                }
            }
            if (pumpShotgunAttach > 0 && pumpShotgunAttachTimer < 0)
            {
                acc = acc * 2f;
                for (int i = 0; i < 9; i++)
                {
                    currentBullets++;
                    Shoot(bowCharge);
                    if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(bowCharge); }
                    if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(bowCharge); }
                }

                pumpShotgunAttachTimer = 30f;
            }
            bowCharge = 0f;
        }
    }
    private void LateUpdate()
    {
        if(bulletsQued > 0)
        {
            if(bowAct > 0)
            {
                for (int y = 0; y < 30; y++)
                {
                    if(bulletsQued < 1) { break; }
                    bulletsQued--;
                    Shoot(bowCharge);
                    if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(bowCharge); }
                    if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(bowCharge); }
                }
            }
            else
            {
                for (int y = 0; y < 30; y++)
                {
                    if(bulletsQued < 1) { break; }
                    bulletsQued--;
                    Shoot(1f);
                    if (whatHandThisIsIn == "left" && manager.playerItem.leftItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.leftItems[111]) { Shoot(1f); }
                    if (whatHandThisIsIn == "right" && manager.playerItem.rightItems[111] > 0 && Random.Range(1, 100) < 40 + 10 * manager.playerItem.rightItems[111]) { Shoot(1f); }
                }
            }

        }
    }
}
