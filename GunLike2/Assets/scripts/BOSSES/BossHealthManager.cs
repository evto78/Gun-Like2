using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : EnemyHealthManager
{
    protected override void Awake()
    {
        base.Awake();
        transform.localScale = Vector3.one;
        gdm.SendDataToEmail("BossReached");
    }

    public override void TakeDamage(float dmgTaken, bool ignoreArmor, HitType.ht hit, Vector3 hitLocation, string source)
    {
        if (hit == HitType.ht.weak || hit == HitType.ht.critweak || hit == HitType.ht.special) { ignoreArmor = true; }
        foreach (MonoBehaviour brain in brains)
        {
            if (hit == HitType.ht.critweak || hit == HitType.ht.weak)
            {
                brain.SendMessage("WeakHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                brain.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
            }
        }
        float tempArmor = armor;
        if (playerHM.ionParticle > 0 && Random.Range(0f, 100f) < 0.5f * playerHM.ionParticle)
        {
            float rand = Random.Range(10f, 1000f);
            dmgTaken *= rand;
        }
        if (activeEffects[0].x > 0) { tempArmor *= 0.25f; }
        if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { ignoreArmor = false; }
        if (activeEffects[7].x > 0) { dmgTaken = dmgTaken * (1f + 0.1f * playerItem.leftItems[69] + playerItem.rightItems[69]); }
        if (activeEffects[9].x > 0) { dmgTaken += dmgTaken * 0.2f; }
        if (playerHM.activeEffects[22].x > 0 && playerItem.leftItems[134] + playerItem.rightItems[134] > 1) { dmgTaken *= 1.25f * (playerItem.leftItems[134] + playerItem.rightItems[134] - 1); }
        if (playerItem.leftItems[146] > 0 && source == "left") { dmgTaken *= (1.05f + 0.05f * playerItem.leftItems[146]) * numOfActiveEffects; }
        if (playerItem.rightItems[146] > 0 && source == "right") { dmgTaken *= (1.05f + 0.05f * playerItem.rightItems[146]) * numOfActiveEffects; }

        if (hit != HitType.ht.crit && hit != HitType.ht.critweak && hit != HitType.ht.special)
        {
            if (activeEffects[11].x > 0 && Random.Range(0, 2) == 0)
            {
                if (hit == HitType.ht.normal) { hit = HitType.ht.crit; }
                if (hit == HitType.ht.weak) { hit = HitType.ht.critweak; }
                if (source == "left") { dmgTaken *= playerItem.gunManager.leftGunScript.critDamage; }
                if (source == "right") { dmgTaken *= playerItem.gunManager.rightGunScript.critDamage; }
                if (playerItem.leftItems[130] + playerItem.rightItems[130] > 1) { dmgTaken *= (1.2f * (playerItem.leftItems[130] + playerItem.rightItems[130] - 1)); }
            }
        }
        else if (activeEffects[11].x > 0 && playerItem.leftItems[130] + playerItem.rightItems[130] > 1)
        {
            dmgTaken *= (1.2f * (playerItem.leftItems[130] + playerItem.rightItems[130] - 1));
        }

        if (activeEffects[6].x > 0 && playerItem.leftItems[52] + playerItem.rightItems[52] > 0)
        {
            dmgTaken = dmgTaken * 2f;
        }

        if (ignoreArmor)
        {
            curHp -= dmgTaken;
            latestDamage = dmgTaken;
        }
        else
        {
            if (tempArmor >= dmgTaken)
            {
                curHp -= 1f;
                latestDamage = 1f;
            }
            else
            {
                curHp -= (dmgTaken - tempArmor);
                latestDamage = dmgTaken - tempArmor;
            }
        }

        if (playerHM.stichedEnemies.Count > 0 && playerItem.leftItems[51] + playerItem.rightItems[51] > 0)
        {
            foreach (EnemyHealthManager ehm in playerHM.stichedEnemies)
            {
                if (dmgTaken * 0.25f > 1f)
                {
                    ehm.QueStandardDamage(dmgTaken * (1f / 4f));
                }
            }
        }

        if (playerItem.leftItems[126] + playerItem.rightItems[126] > 0 && dmgTaken >= 2f)
        {
            GameObject spawnedPapers = Instantiate(burnedPapers, transform.position, transform.rotation);
            spawnedPapers.GetComponent<BurnedPapers>().damage = dmgTaken;
            spawnedPapers.GetComponent<BurnedPapers>().collidedWith.Add(gameObject);
            spawnedPapers.transform.localScale = Vector3.one * ((playerItem.leftItems[126] + playerItem.rightItems[126]) / 2f);
        }

        if (source == "left")
        {
            //irradiated battle plans
            float chance = playerItem.leftItems[80] * 25f; if (chance > 50) { chance = 50; }
            if (Random.Range(1, 100) < chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage, false, this);
                latestDamage = 0;
            }
        }
        else if (source == "right")
        {
            //irradiated battle plans
            float chance = playerItem.rightItems[80] * 25f; if (chance > 50) { chance = 50; }
            if (Random.Range(1, 100) < chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage, false, this);
                latestDamage = 0;
            }
        }
        if (latestDamage != 0)
        {
            PopUpText(latestDamage.ToString(), hit, hitLocation, source);
            gameObject.SendMessage("TookDmg", SendMessageOptions.DontRequireReceiver);
        }

        if (curHp <= 0 && !died)
        {
            died = true;
            if (source == "left" && playerItem.leftItems[133] > 0) { playerItem.gunManager.leftGunScript.echoDmg = dmgTaken / 1.5f; }
            if (source == "right" && playerItem.rightItems[133] > 0) { playerItem.gunManager.rightGunScript.echoDmg = dmgTaken / 1.5f; }
            Die(true);
        }
    }

    public override void Die(bool sentByHm)
    {
        if (!sentByHm) { return; }
        //on death effects
        OnDeath();

        //on actual destruction
        Destroy(gameObject);
    }

    protected override void ManageEffects()
    {
        Vector4 q = new Vector4(0, 0, 0, 0);

        for (int i = 0; i < activeEffects.Count; i++)
        {
            q = activeEffects[i];
            
            if (i == 11 && q.x > 0)
            {
                markedEffect.SetActive(true);
            }
            else if (i == 11 && q.x < 1)
            {
                markedEffect.SetActive(false);
            }

            //if there are any stacks of this effect
            if (q.x > 0)
            {
                numOfActiveEffects++;
                float dotDmgModifer = 1; if (activeEffects[13].x > 0) { dotDmgModifer *= 2; }
                //burn
                if (i == 1 && burnTimer >= 0.2f)
                {
                    if ((playerItem.leftItems[48] + playerItem.rightItems[48]) > 0) { dotDmgModifer *= 2; }
                    QueStandardDamage(1f * dotDmgModifer);
                    burnTimer = 0;
                }

                //progress timer and remove stacks as needed
                if (q.z > 0f)
                {
                    q.z -= Time.deltaTime;
                }
                else
                {
                    //run effects that happen when timer ends
                    if (i == 2)
                    {
                        QueStandardDamage(q.x * 50f * dotDmgModifer);
                    }
                    //If player has anti-antidode chance to not remove stacks when timer runs out
                    int antiAnti = playerItem.leftItems[41] + playerItem.rightItems[41];
                    if (antiAnti > 0)
                    {
                        if (Random.Range(0, 9 + antiAnti) == 0)
                        {
                            q.x -= 1f;
                            if (i == 2) { q.x += 1f; q.x /= 2f; if (q.x < 1f) { q.x = 0f; } }
                        }
                    }
                    else
                    {
                        q.x -= 1f;
                        if (i == 2)
                        {
                            q.x += 1f; q.x /= 2f; if (q.x < 1f) { q.x = 0f; }
                        }
                    }
                    if (q.x! > 0f) { q.z = q.y; }
                }
            }

            activeEffects[i] = q;
        }

        for (int i = 0; i < activeEffects.Count; i++)
        {
            Vector4 effect = activeEffects[i];
            if (effect.x > 0f)
            {
                icons[i].SetActive(true);
                icons[i].GetComponent<EffectIconScript>().UpdateEffectIcon(i, Mathf.RoundToInt(effect.x));
            }
            else
            {
                icons[i].SetActive(false);
            }
        }
    }
}
