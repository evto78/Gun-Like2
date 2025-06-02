using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryScript : MonoBehaviour
{
    public GameObject player;
    public PlayerItem playerItem;
    List<ItemObject> idata;

    public List<Sprite> itemBg;

    public GameObject slotPrefab;
    public Transform leftHolder;
    public Transform rightHolder;

    private void Start()
    {
        idata = playerItem.itemData;
    }

    public void UpdateInventory()
    {
        //clearslots
        for(int i = 0; i < leftHolder.childCount; i++)
        {
            Destroy(leftHolder.GetChild(i).gameObject);
        }
        for (int i = 0; i < rightHolder.childCount; i++)
        {
            Destroy(rightHolder.GetChild(i).gameObject);
        }
        //prepareinfo
        List<int> li = playerItem.leftItems; 
        List<int> ri = playerItem.rightItems; 
        //addnew
        GameObject temp;
        for(int i = 0; i < li.Count; i++)
        {
            temp = null;
            if (li[i] > 0) { temp = Instantiate(slotPrefab, leftHolder); SetUpSlot(temp, i, li[i], "left"); }
            if (ri[i] > 0) { temp = Instantiate(slotPrefab, rightHolder); SetUpSlot(temp, i, ri[i], "right"); }
        }
        //check if scaling needs changing
        int rightCount = rightHolder.childCount /2;
        int leftCount = leftHolder.childCount / 2;

        if(leftCount < 31) { leftHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(150f,150f); }
        else if (leftCount < 81) { leftHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100f, 100f); }
        else if (leftCount < 201) { leftHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(60f, 60f); }

        if(rightCount < 31) { rightHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(150f,150f); }
        else if (rightCount < 81) { rightHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100f, 100f); }
        else if (rightCount < 201) { rightHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(60f, 60f); }
    }
    void SetUpSlot(GameObject invSlot, int id, int amount, string hand)
    {
        ItemObject data = Resources.Load<ItemObject>("Items/" + id.ToString());
        int temp = 0;
        if(data.rarity.ToString() == "Common") { temp = 0; }
        if(data.rarity.ToString() == "Uncommon") { temp = 1; }
        if(data.rarity.ToString() == "Rare") { temp = 2; }
        if(data.rarity.ToString() == "Legendary") { temp = 3; }
        if(data.rarity.ToString() == "Mutated") { temp = 4; }
        if(data.rarity.ToString() == "Haunted") { temp = 5; }
        if(data.rarity.ToString() == "Irradiated") { temp = 6; }
        if(data.rarity.ToString() == "Nulcear") { temp = 7; }
        if(data.rarity.ToString() == "Unique") { temp = 8; }
        invSlot.GetComponent<InventorySlot>().itemSprite.sprite = data.itemSprite;
        invSlot.GetComponent<InventorySlot>().slotRarityBg.sprite = itemBg[temp];
        invSlot.GetComponent<InventorySlot>().quantityText.text = amount.ToString();
        invSlot.GetComponent<InventorySlot>().id = id;
        invSlot.GetComponent<InventorySlot>().hand = hand;
        invSlot.GetComponent<InventorySlot>().pi = playerItem;
    }
}
