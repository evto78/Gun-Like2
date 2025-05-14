using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class ItemObject : ScriptableObject
{
    [Header("Functional")]
    public int id;
    public string itemName;
    public string rarity;
    public Sprite itemSprite;
    /// <summary>
    /// General Definition of buff symbols
    /// + = 20%
    /// ++ = 40%
    /// +++ = 60%
    /// + 1/2 = 10%
    /// </summary>
    [Header("Credit")]
    public string ideaCredit;
    public string flavorCredit;
}
