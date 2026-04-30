using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class ItemObject : ScriptableObject
{
    [System.Serializable]
    public class StatData
    {
        public enum Stat { 
            MHP, Armor, PassiveRegen, Speed, SprintSpeed, JumpHeight, JumpCount, AttackSpeed, ReloadSpeed, 
            Damage, MagSize, Accuracy, BulletSpeed, BulletSize, Pierce, CritChance, CritDamage, WeakPointChance, WeakPointDamage
        }
        public Stat stat;
        public float change;
    }

    /// <summary>
    /// General Definition of buff symbols
    /// + <= 20%
    /// ++ <= 40%
    /// +++ <= 60%
    /// +1/2 <= 10%
    /// </summary>

    [Header("Functional")]
    public int id;
    public string itemName;
    public enum rarityType { Common, Uncommon, Rare, Legendary, Mutated, Haunted, Irradiated, Nuclear, Unique }
    public rarityType rarity;
    public Sprite itemSprite;
    public bool needsToBeUnlocked;
    public string unlockCondition;
    public bool cooldownItem;
    public float baseCooldown;
    public bool globalItem; //Does the item effect only affect the gun it is attached to.
    public enum itemType { basic, sponser, classic, fish, unstablePart, boss, horror }
    public itemType subType;
    
    [Header("Display Text")]
    public string effect;
    public string buff;
    public string debuff;
    public string effectSum;
    public string buffSum;
    public string debuffSum;
    public string flavor;
    public List<StatData> statData = new List<StatData>();
    [Header("Credit")]
    public string ideaCredit;
    public string flavorCredit;
}
