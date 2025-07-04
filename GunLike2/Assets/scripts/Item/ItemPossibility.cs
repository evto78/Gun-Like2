using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPossibility : MonoBehaviour
{
    public GameObject itemPossibility;
    public GameObject item;

    public int rarity;
    public int idover;
    public bool overrideid;
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

    public GameObject shrodingerBox;
    public GameObject shrodingerOptions;
    bool isShrodinger;
    GameObject option1;
    int oid1;
    GameObject option2;
    int oid2;
    Vector3 lockedPos;
    bool locked;
    bool limestoneScale;

    float timer;
    PlayerItem pi;
    public bool isDoubled;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        pi = player.GetComponent<PlayerItem>();

        limestoneScale = pi.leftItems[178] + pi.rightItems[178] > 0;

        option1 = null;
        option2 = null;

        locked = false;

        if (pi.rightItems[137] + pi.leftItems[137] > 0)
        {
            if (Random.Range(1, 100) < 50 + (25 * pi.rightItems[137] + pi.leftItems[137]))
            {
                overrideid = true; idover = pi.horrorItems[Random.Range(0, pi.horrorItems.Count)];
                for(int i = 0; i < pi.rarityList.Count; i++)
                {
                    if (pi.rarityList[i].Contains(idover)) { SetRarity(i, false); } 
                }
            }
        }
        if (pi.leftItems[142] + pi.rightItems[142] > 0)
        {
            overrideid = true; idover = 143; SetRarity(8, false);
        }
        if (pi.rightItems[68] + pi.leftItems[68] > 0) { isShrodinger = true; } else { isShrodinger = false; }
        if (overrideid) { isShrodinger = false; }
        shrodingerBox.SetActive(isShrodinger);

        rarityList = pi.rarityList;
        timer = 0.5f;
        if(pi.rightItems[49]+ pi.leftItems[49] > 0)
        {
            if(Random.Range(1,100) > 80)
            {
                int rand = Random.Range(1, 101);
                int rarityID = 0;

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
                GameObject spawnedItem;
                spawnedItem = Instantiate(itemPossibility);
                spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                spawnedItem.GetComponent<Rigidbody>().AddForce((spawnedItem.transform.position - player.transform.position) * -20f + Vector3.up * 250f);
                spawnedItem.GetComponent<ItemPossibility>().SetRarity(rarityID, false);
            }
        }
        
        int doubleOrNothing = pi.rightItems[120] + pi.leftItems[120];
        if (doubleOrNothing > 0 && !isDoubled)
        {
            Debug.Log("New Item:");
            int doubling = 1;
            for(int i = 0; i < doubleOrNothing; i++)
            {
                int temp = Random.Range(1, 100);
                if (temp < 40)
                {
                    doubling *= 2;
                }
                else if (temp > 60)
                {
                    doubling = 0;
                }
            }
            if(doubling > 64) { doubling = 64; }
            Debug.Log(doubling);
            if(doubling == 0)
            {
                GameObject spawnedItem;
                spawnedItem = Instantiate(itemPossibility);
                spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                spawnedItem.GetComponent<Rigidbody>().AddForce((spawnedItem.transform.position - player.transform.position)*-20f + Vector3.up * 250f);
                spawnedItem.GetComponent<ItemPossibility>().overrideid = true;
                spawnedItem.GetComponent<ItemPossibility>().idover = pi.unstableItems[Random.Range(0, pi.unstableItems.Count)];
                spawnedItem.GetComponent<ItemPossibility>().SetRarity(8,false);
                spawnedItem.GetComponent<ItemPossibility>().isDoubled = true;
                Destroy(gameObject);
            }
            else if (doubling > 1)
            {
                doubling--;
                for (int y = 0; y < doubling; y++)
                {
                    int rand = Random.Range(1, 101);
                    int rarityID = 0;

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

                    GameObject spawnedItem;
                    spawnedItem = Instantiate(itemPossibility);
                    spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                    spawnedItem.GetComponent<Rigidbody>().AddForce((spawnedItem.transform.position - player.transform.position) * -20f + Vector3.up * 250f);
                    spawnedItem.GetComponent<ItemPossibility>().SetRarity(rarityID,false);
                    spawnedItem.GetComponent<ItemPossibility>().isDoubled = true;

                }
            }
        }
    }

    public void SetRarity(int givenRarity, bool doesNotMatter)
    {
        player = GameObject.Find("Player");
        pi = player.GetComponent<PlayerItem>();
        timer = 0.5f;

        rarity = givenRarity;

        if(rarity < 2 && pi.MasterCardCheck())
        {
            pi.masterCardChance = 0f;
            int rand = Random.Range(1, 11);
            switch (rand)
            {
                case 1: rarity = 2; break;
                case 2: rarity = 2; break;
                case 3: rarity = 4; break;
                case 4: rarity = 4; break;
                case 5: rarity = 5; break;
                case 6: rarity = 5; break;
                case 7: rarity = 6; break;
                case 8: rarity = 6; break;
                case 9: rarity = 3; break;
                case 10: rarity = 7; break;
            }
        } 

        commonPS.SetActive(false);
        uncommonPS.SetActive(false);
        rarePS.SetActive(false); 
        legendaryPS.SetActive(false); 
        mutatedPS.SetActive(false); 
        hauntedPS.SetActive(false); 
        irradiadedPS.SetActive(false); 
        nuclearPS.SetActive(false); 
        uniquePS.SetActive(false); 

        switch (rarity)
        {
            case 0: commonPS.SetActive(true); break;
            case 1: uncommonPS.SetActive(true); break;
            case 2: rarePS.SetActive(true); break;
            case 3: legendaryPS.SetActive(true); break;
            case 4: mutatedPS.SetActive(true); break;
            case 5: hauntedPS.SetActive(true); break;
            case 6: irradiadedPS.SetActive(true); break;
            case 7: nuclearPS.SetActive(true); break;
            case 8: uniquePS.SetActive(true); break;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (locked) { transform.position = lockedPos; }

        if(option1 != null) { option1.transform.localPosition = Vector3.zero; }
        if(option2 != null) { option2.transform.localPosition = Vector3.zero; }

        if(option1 != null && option2 == null) { if (oid1 != oid2) { ChangeRarity(oid1); } Destroy(option1); Destroy(gameObject); }
        if(option2 != null && option1 == null) { if (oid1 != oid2) { ChangeRarity(oid2); } Destroy(option2); Destroy(gameObject); }
    }
    void ChangeRarity(int id)
    {
        int prevRarity = 0;
        foreach(List<int> rarity in pi.rarityList)
        {
            if (rarity.Contains(id)) { prevRarity = pi.rarityList.IndexOf(rarity); }
        }
        pi.rarityList[prevRarity].Remove(id);
        if(pi.rarityList.Count > prevRarity + 1) { pi.rarityList[prevRarity + 1].Add(id); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(timer <= 0f)
        {
            OnInteract("collision");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (timer <= 0f)
        {
            OnInteract("collision");
        }
    }

    public void OnInteract(string source)
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        if (isShrodinger)
        {
            if(source == "player")
            {
                shrodingerOptions.SetActive(true);
                if(shrodingerOptions.transform.GetChild(0).childCount == 0)
                {
                    gameObject.GetComponent<Collider>().enabled = false;
                    locked = true;
                    lockedPos = transform.position;
                    option1 = Instantiate(item, shrodingerOptions.transform.GetChild(0));
                    option1.GetComponent<Item>().SetItemID(rarityList[rarity][Random.Range(0, rarityList[rarity].Count)]);
                    option1.GetComponent<Rigidbody>().useGravity = false;
                    option2 = Instantiate(item, shrodingerOptions.transform.GetChild(1));
                    option2.GetComponent<Item>().SetItemID(rarityList[rarity][Random.Range(0, rarityList[rarity].Count)]);
                    option2.GetComponent<Rigidbody>().useGravity = false;
                    oid1 = option1.GetComponent<Item>().itemID;
                    oid2 = option2.GetComponent<Item>().itemID;

                    Destroy(gameObject, 45f);
                }
            }
        }
        else if (source == "collision")
        {
            if (overrideid)
            {
                SpawnItem(idover);
            }
            else
            {
                SpawnItem(rarityList[rarity][Random.Range(0, rarityList[rarity].Count)]);
            }
            
            Destroy(gameObject);
        }
    }
    public void Interact()
    {
        OnInteract("player");
    }
    void SpawnItem(int iD)
    {
        GameObject spawnedItem = Instantiate(item);
        spawnedItem.transform.position = transform.position;
        spawnedItem.transform.rotation = transform.rotation;

        spawnedItem.GetComponent<Item>().SetItemID(iD);
    }
}
