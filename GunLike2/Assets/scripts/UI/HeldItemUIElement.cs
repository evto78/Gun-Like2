using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeldItemUIElement : MonoBehaviour
{
    public Image sprite;
    public Image bg;
    public List<Color> rarityColors;
    void Update()
    {
        transform.position = Input.mousePosition;
    }
    public void SetItem(ItemObject obj)
    {
        sprite.sprite = obj.itemSprite;
        switch (obj.rarity) 
        {
            case ItemObject.rarityType.Common: bg.color = rarityColors[0]; break;
            case ItemObject.rarityType.Uncommon: bg.color = rarityColors[1]; break;
            case ItemObject.rarityType.Rare: bg.color = rarityColors[2]; break;
            case ItemObject.rarityType.Legendary: bg.color = rarityColors[3]; break;
            case ItemObject.rarityType.Mutated: bg.color = rarityColors[4]; break;
            case ItemObject.rarityType.Haunted: bg.color = rarityColors[5]; break;
            case ItemObject.rarityType.Irradiated: bg.color = rarityColors[6]; break;
            case ItemObject.rarityType.Nuclear: bg.color = rarityColors[7]; break;
            case ItemObject.rarityType.Unique: bg.color = rarityColors[8]; break;
        }
    }
}
