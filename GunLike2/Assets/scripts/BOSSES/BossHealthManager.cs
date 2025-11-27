using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : EnemyHealthManager
{
    protected override void Awake()
    {
        base.Awake();
        transform.localScale = Vector3.one;
    }

    public override void TakeDamage(float dmgTaken, bool ignoreArmor, HitType.ht hit, Vector3 hitLocation, string source)
    {
        base.TakeDamage(dmgTaken, ignoreArmor, hit, hitLocation, source);
    }

    public override void Die(bool sentByHm, string source, bool instantKill)
    {
        if (!sentByHm || instantKill) { return; }
        base.Die(sentByHm, source, instantKill);
    }

    protected override void ManageEffects()
    {
        base.ManageEffects();
        foreach (MonoBehaviour brain in brains)
        {
            brain.enabled = true;
        }
        frozenEffect.SetActive(false);
        icons[33].gameObject.SetActive(false);
        icons[37].gameObject.SetActive(false);
    }
}
