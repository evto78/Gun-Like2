using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemEffect : MonoBehaviour
{
    public enum EffectType { OnHitChance, PassiveCooldown }
    public EffectType type;
}
