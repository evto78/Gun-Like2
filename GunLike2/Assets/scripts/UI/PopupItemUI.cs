using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupItemUI : MonoBehaviour
{
    public GameObject popupNotif;
    class popupData
    {
        public int change; public ItemObject data;
    }
    List<popupData> popupQue = new List<popupData>();
    int activePopups;
    private void Update()
    {
        if(popupQue.Count == 0 || transform.childCount > 10) { return; }
        PrintFromQue();
    }
    void PrintFromQue()
    {
        int change = popupQue[0].change; ItemObject data = popupQue[0].data;
        GameObject spawnedNotif = Instantiate(popupNotif);
        Transform t = spawnedNotif.transform;
        t.SetParent(transform);
        t.position = Vector3.zero;

        t.GetChild(1).GetComponent<Image>().sprite = data.itemSprite;
        if (change > 0) { t.GetChild(2).GetComponent<TextMeshProUGUI>().text = "+" + change.ToString(); }
        else { t.GetChild(2).GetComponent<TextMeshProUGUI>().text = change.ToString(); }
        t.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.itemName;
        float lifetime = 3f / (popupQue.Count);
        Destroy(spawnedNotif, lifetime);
        popupQue.RemoveAt(0);
    }
    public void CreateNotif(int change, ItemObject data)
    {
        popupData pData = new popupData();
        pData.change = change;
        pData.data = data;
        popupQue.Add(pData);
    }
}
