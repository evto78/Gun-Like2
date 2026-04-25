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
    public bool needsToBeUnlocked;
    public string unlockCondition;
    public bool cooldownItem;
    public float baseCooldown;
    public bool globalItem; //Does the item effect only affect the gun it is attached to.
    public enum itemType { basic, sponser, classic, fish, unstablePart, boss, horror }
    public itemType subType;

    [Header("Effects")]
    public float[] statEffects = new float[18]; //float is percentage multiplier (ie: 20 = 20% = x1.2 | -20 = -20% = x/1.2)
    //Index 0 - 17 what stat is referenced:
    //0 - Speed
    //1 - Sprint Speed
    //2 - Jump Height
    //3 - Number of Jumps

    //4 - Crit Chance 
    //5 - Crit Damage
    //6 - Weak Spot Damage
    //7 - Damage
    //8 - Attack Speed
    //9 - Reload Speed
    //10 - Magazine Size
    //11 - Accuracy
    //12 - Bullet Speed
    //13 - Bullet Size
    //14 - Bullet Pierce

    //15 - Max Hp
    //16 - Passive hp regen
    //17 - Armor
    public List<ItemEffect> otherEffects = new List<ItemEffect>();

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
