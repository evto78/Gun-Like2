using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBarrelScript : GunScript
{
    int bulletsQued; static int maxBulletsPerFrame = 100;
    public override void AttemptShoot()
    {
        if ((bowAct > 0))
        {
            bowCharge += ((bowAct / 2f) * Time.deltaTime) + (1.5f * atkSpd * Time.deltaTime);
            if (bowCharge > bowAct + 1f) { bowCharge = bowAct + 1f; AttemptShootUp(true); }
        }
        else
        {
            if (!reloading && !shooting && rushJobTimer <= 0)
            {
                if (rushJob > 0 && Random.Range(1, 100) < Mathf.Clamp(5 + (5 * rushJob), -1, 65))
                {
                    misfireEffect.GetComponent<ParticleSystem>().Play();
                    rushJobTimer = (1f / reSpd) / 2f;
                    return;
                }

                if (Mathf.CeilToInt(magSize / 2f) > maxBulletsPerFrame) { bulletsQued = Mathf.CeilToInt(magSize / 2f); }
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

    public override void AttemptShootUp(bool forcedInput)
    {
        if (!forcedInput) { smokingGunCounter = 0; if (smokingGun > 0) { manager.healthMan.activeEffects[23] = new Vector4(0, manager.healthMan.activeEffects[23].y, manager.healthMan.activeEffects[23].z, manager.healthMan.activeEffects[23].w); } }
        if (bowAct > 0 && !reloading && !shooting)
        {
            if (rushJob > 0 && Random.Range(1, 100) < Mathf.Clamp(5 + (5 * rushJob), -1, 65))
            {
                misfireEffect.GetComponent<ParticleSystem>().Play();
                rushJobTimer = (1f / reSpd) / 2f;
                return;
            }
            if (Mathf.CeilToInt(magSize / 2f) > maxBulletsPerFrame) { bulletsQued = Mathf.CeilToInt(magSize / 2f); }
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
                for (int y = 0; y < maxBulletsPerFrame; y++)
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
                for (int y = 0; y < maxBulletsPerFrame; y++)
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
