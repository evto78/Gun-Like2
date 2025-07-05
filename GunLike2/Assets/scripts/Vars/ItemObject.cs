using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class ItemObject : ScriptableObject
{
    [Header("Functional")]
    public int id;
    public string itemName;
    public enum rarityType { Common, Uncommon, Rare, Legendary, Mutated, Haunted, Irradiated, Nuclear, Unique }
    public rarityType rarity;
    public Sprite itemSprite;
    public bool cooldownItem;
    public float baseCooldown;
    public enum itemType { basic, sponser, classic, fish, unstablePart, boss, horror }
    public itemType subType;
    
    /// <summary>
    /// General Definition of buff symbols
    /// + = 20%
    /// ++ = 40%
    /// +++ = 50%/60%
    /// + 1/2 = 10%
    /// </summary>
    [Header("Display Text")]
    public string effect;
    public string buff;
    public string debuff;
    public string effectSum;
    public string buffSum;
    public string debuffSum;
    public string flavor;
    [Header("Credit")]
    public string ideaCredit;
    public string flavorCredit;
}
