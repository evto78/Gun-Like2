using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    public List<string> creditItems;
    float scrollSpeed; bool scroll; bool creditsBuilt = false;
    public Transform creditTextHolder;
    public GameObject creditTextPrefab;
    public List<ItemObject> itemData = new List<ItemObject>();

    void Start()
    {
        if (creditsBuilt) { return; }
        creditsBuilt = true;
        BuildCredits();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
        StartCoroutine(SortData());
    }
    void BuildCredits()
    {
        for(int i = 0; i < creditItems.Count; i++)
        {
            TextMeshProUGUI tmp = Instantiate(creditTextPrefab, creditTextHolder).GetComponent<TextMeshProUGUI>();
            tmp.text = creditItems[i];
        }
    }
    IEnumerator SortData()
    {
        List<int> comparisonList = new List<int>();
        List<ItemObject> sortedItemData = new List<ItemObject>();
        for (int i = 0; i < itemData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for (int i = 0; i < itemData.Count; i++)
        {
            sortedItemData[comparisonList.IndexOf(itemData[i].id)] = itemData[i];
        }
        itemData = sortedItemData;
        WriteData();
        yield return null;
    }
    IEnumerator WriteData()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            TextMeshProUGUI tmp = Instantiate(creditTextPrefab, creditTextHolder).GetComponent<TextMeshProUGUI>();
            string credit = "" + itemData[i].ideaCredit; if (credit == "" || credit == "Gun-Like Classic") { credit = "Evan,V"; }
            string creditFlavor = "" + itemData[i].flavorCredit; if (credit == "" || credit == "Gun-Like Classic") { credit = "Evan,V"; }
            tmp.text = itemData[i].itemName + " (ID:" + itemData[i].id + ") | " + credit + " | Flavor Text by " + creditFlavor;
            yield return null;
        }
    }
}
