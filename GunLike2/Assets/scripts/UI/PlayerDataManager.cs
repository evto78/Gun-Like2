using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public GameObject itemUnlockElementPrefab;
    public Transform itemUnlockWindow;

    SaveFileReadWrite saveDataReader;
    public List<ItemObject> itemData;

    private void Awake()
    {
        saveDataReader = GameObject.Find("SaveDataReader").GetComponent<SaveFileReadWrite>();

        if(itemUnlockWindow.childCount < 1) { StartCoroutine(BuildUnlocks()); }
    }
    List<ItemObject> SortItemDataByRarity(List<ItemObject> inputItemData)
    {
        List<int> comparisonList = new List<int>();
        List<ItemObject> sortedItemData = new List<ItemObject>();
        for (int i = 0; i < inputItemData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for (int i = 0; i < inputItemData.Count; i++)
        {
            sortedItemData[comparisonList.IndexOf(inputItemData[i].id)] = inputItemData[i];
        }
        List<ItemObject> commonItems = new List<ItemObject>();
        List<ItemObject> uncommonItems = new List<ItemObject>();
        List<ItemObject> rareItems = new List<ItemObject>();
        List<ItemObject> legendaryItems = new List<ItemObject>();
        List<ItemObject> mutatedItems = new List<ItemObject>();
        List<ItemObject> hauntedItems = new List<ItemObject>();
        List<ItemObject> irradiatedItems = new List<ItemObject>();
        List<ItemObject> nuclearItems = new List<ItemObject>();
        List<ItemObject> uniqueItems = new List<ItemObject>();
        foreach (ItemObject item in sortedItemData)
        {
            switch (item.rarity)
            {
                case ItemObject.rarityType.Common: commonItems.Add(item); break;
                case ItemObject.rarityType.Uncommon: uncommonItems.Add(item); break;
                case ItemObject.rarityType.Rare: rareItems.Add(item); break;
                case ItemObject.rarityType.Legendary: legendaryItems.Add(item); break;
                case ItemObject.rarityType.Mutated: mutatedItems.Add(item); break;
                case ItemObject.rarityType.Haunted: hauntedItems.Add(item); break;
                case ItemObject.rarityType.Irradiated: irradiatedItems.Add(item); break;
                case ItemObject.rarityType.Nuclear: nuclearItems.Add(item); break;
                case ItemObject.rarityType.Unique: uniqueItems.Add(item); break;
            }
        }
        sortedItemData.Clear();
        sortedItemData.AddRange(commonItems);
        sortedItemData.AddRange(uncommonItems);
        sortedItemData.AddRange(rareItems);
        sortedItemData.AddRange(legendaryItems);
        sortedItemData.AddRange(mutatedItems);
        sortedItemData.AddRange(hauntedItems);
        sortedItemData.AddRange(irradiatedItems);
        sortedItemData.AddRange(nuclearItems);
        sortedItemData.AddRange(uniqueItems);
        return sortedItemData;
    }
    IEnumerator BuildUnlocks()
    {
        itemData = new List<ItemObject>();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
        itemData = SortItemDataByRarity(itemData);

        List<SaveFileReadWrite.UnlockInformation> unlockData = saveDataReader.data.UnlockInfo;

        int counter = 0;
        foreach(ItemObject item in itemData)
        {
            ItemUnlockElement unlockElement = Instantiate(itemUnlockElementPrefab, itemUnlockWindow).GetComponent<ItemUnlockElement>();
            unlockElement.SetUp(item, unlockData[item.id]);
            counter++;
            if(counter == 10) { counter = 0; yield return new WaitForEndOfFrame(); }
        }

        yield return null;
    }
}
