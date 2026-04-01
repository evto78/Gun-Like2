using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupNotif : MonoBehaviour
{
    RectTransform rectTransform;
    public bool state;

    public TextMeshProUGUI unlockText;
    public Image itemSprite;
    public Image rarityBG;
    public List<Sprite> rarityBGs;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void Update()
    {
        if (state)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(0, rectTransform.position.y, rectTransform.position.z), Time.deltaTime*10);
        }
        else
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, new Vector3(-650, rectTransform.position.y, rectTransform.position.z), Time.deltaTime * 10);
        }
    }
    public void Popup(ItemObject data)
    {
        itemSprite.sprite = data.itemSprite;
        unlockText.text = data.itemName;
        switch (data.rarity)
        {
            case ItemObject.rarityType.Common: rarityBG.sprite = rarityBGs[0]; break;
            case ItemObject.rarityType.Uncommon: rarityBG.sprite = rarityBGs[1]; break;
            case ItemObject.rarityType.Rare: rarityBG.sprite = rarityBGs[2]; break;
            case ItemObject.rarityType.Legendary: rarityBG.sprite = rarityBGs[3]; break;
            case ItemObject.rarityType.Mutated: rarityBG.sprite = rarityBGs[4]; break;
            case ItemObject.rarityType.Haunted: rarityBG.sprite = rarityBGs[5]; break;
            case ItemObject.rarityType.Irradiated: rarityBG.sprite = rarityBGs[6]; break;
            case ItemObject.rarityType.Nuclear: rarityBG.sprite = rarityBGs[7]; break;
            case ItemObject.rarityType.Unique: rarityBG.sprite = rarityBGs[8]; break;
        }
        StopAllCoroutines();
        StartCoroutine(PopupRoutine());
    }
    IEnumerator PopupRoutine()
    {
        state = true;
        yield return new WaitForSeconds(5);
        state = false;

        yield return null;
    }
}
