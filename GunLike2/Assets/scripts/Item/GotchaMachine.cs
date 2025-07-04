using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotchaMachine : MonoBehaviour
{
    PlayerItem pi;
    public GameObject itemPot;
    bool dispensing;
    void Start()
    {
        pi = GameObject.Find("Player").GetComponent<PlayerItem>();
    }

    private void LateUpdate()
    {
        if(pi.leftItems[75] + pi.rightItems[75] > 0)
        {
            for(int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (dispensing)
        {
            RandomDraw();
            pi.gotchaTickets -= 25;
            if (pi.gotchaTickets < 25) { dispensing = false; }
        }
    }

    public void Activate()
    {
        dispensing = true;
    }
    void RandomDraw()
    {
        int rand = Random.Range(1, 101);
        int rarityID = 0;
        bool limestoneScale = pi.leftItems[178] + pi.rightItems[178] > 0;
        if (limestoneScale)
        {
            rand = Random.Range(0, 8);
            rarityID = rand;
        }
        else
        {
            if (rand < 71) { rarityID = 0; }
            if (rand < 91 && rand > 70) { rarityID = 1; }
            if (rand == 91 || rand == 92) { rarityID = 2; }
            if (rand == 93 || rand == 94) { rarityID = 4; }
            if (rand == 95 || rand == 96) { rarityID = 5; }
            if (rand == 97 || rand == 98) { rarityID = 6; }
            if (rand == 99) { rarityID = 3; }
            if (rand == 100) { rarityID = 7; }
        }
        SpawnItem(rarityID);
    }
    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = pi.rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPot);
        spawnedItem.transform.position = transform.position + Vector3.up * 3f;
        spawnedItem.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(100f, 150f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-25f, 25f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(150f, 200f));
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD, false);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
}
