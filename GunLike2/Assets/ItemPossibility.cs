using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPossibility : MonoBehaviour
{
    public GameObject item;

    public int rarity;
    public List<List<int>> rarityList;

    public GameObject commonPS;
    public GameObject uncommonPS;
    public GameObject rarePS;
    public GameObject legendaryPS;
    public GameObject mutatedPS;
    public GameObject hauntedPS;
    public GameObject irradiadedPS;
    public GameObject nuclearPS;
    public GameObject uniquePS;

    public void SetRarity(int givenRarity)
    {
        rarity = givenRarity;

        if(rarity == 0) { commonPS.SetActive(true);}
        if(rarity == 1) { uncommonPS.SetActive(true);}
        if(rarity == 2) { rarePS.SetActive(true);}
        if(rarity == 3) { legendaryPS.SetActive(true);}
        if(rarity == 4) { mutatedPS.SetActive(true);}
        if(rarity == 5) { hauntedPS.SetActive(true);}
        if(rarity == 6) { irradiadedPS.SetActive(true);}
        if(rarity == 7) { nuclearPS.SetActive(true);}
        if(rarity == 8) { uniquePS.SetActive(true);}
    }

    public void OnInteract()
    {
        SpawnItem(rarityList[rarity][Random.Range(0, rarityList[rarity].Count)]);
    }

    void SpawnItem(int iD)
    {
        GameObject spawnedItem = Instantiate(item);
        spawnedItem.transform.position = transform.position;
        spawnedItem.transform.rotation = transform.rotation;

        spawnedItem.GetComponent<Item>().SetItemID(iD);
    }
}
