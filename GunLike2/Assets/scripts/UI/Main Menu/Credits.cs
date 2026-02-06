using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    public List<string> creditItems;
    float scrollSpeed; bool scroll; bool creditsBuilt = false;
    public Transform creditTextHolder; Vector3 initialPos;
    public GameObject creditTextPrefab;
    public List<ItemObject> itemData = new List<ItemObject>();

    void Start()
    {
        scrollSpeed = 100f;
        if (creditsBuilt) { return; }
        creditsBuilt = true;
        BuildCredits();
        itemData.AddRange(Resources.LoadAll<ItemObject>("Items"));
        StartCoroutine(SortData());
    }
    private void Update()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") > 0) { creditTextHolder.localPosition -= Vector3.up * scrollSpeed; }
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0) { creditTextHolder.localPosition += Vector3.up * scrollSpeed; }
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
        StartCoroutine(WriteData());
        yield return null;
    }
    IEnumerator WriteData()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            if ((itemData[i].ideaCredit != "" && itemData[i].ideaCredit != "Gun-Like Classic") || (itemData[i].flavorCredit != "" && itemData[i].flavorCredit != "Gun-Like Classic"))
            {
                TextMeshProUGUI tmp = Instantiate(creditTextPrefab, creditTextHolder).GetComponent<TextMeshProUGUI>();
                string credit = "" + itemData[i].ideaCredit; if (credit == "" || credit == "Gun-Like Classic") { credit = "Evan,V"; }
                string creditFlavor = "" + itemData[i].flavorCredit; if (creditFlavor == "" || creditFlavor == "Gun-Like Classic") { creditFlavor = "Evan,V"; }
                tmp.text = itemData[i].itemName + " (ID:" + itemData[i].id + ") | " + credit + " | Flavor Text by " + creditFlavor;
            }

            if(25%(i+1) == 0) { yield return new WaitForEndOfFrame(); }
        }
        yield return null;
    }   
}
