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

    float timer;

    private void Start()
    {
        option1 = null;
        option2 = null;

        locked = false;
        if(GameObject.Find("Player").GetComponent<PlayerItem>().rightItems[68] + GameObject.Find("Player").GetComponent<PlayerItem>().leftItems[68] > 0) { isShrodinger = true; } else { isShrodinger = false; }
        if (overrideid) { isShrodinger = false; }
        shrodingerBox.SetActive(isShrodinger);

        rarityList = GameObject.Find("Player").GetComponent<PlayerItem>().rarityList;
        timer = 0.5f;
        if(GameObject.Find("Player").GetComponent<PlayerItem>().rightItems[49]+ GameObject.Find("Player").GetComponent<PlayerItem>().leftItems[49] > 0)
        {
            if(Random.Range(1,100) > 80)
            {
                int rand = Random.Range(1, 101);
                int rarityID = 0;

                if (rand < 71) { rarityID = 0; }
                if (rand < 91 && rand > 70) { rarityID = 1; }
                if (rand == 91 || rand == 92) { rarityID = 2; }
                if (rand == 93 || rand == 94) { rarityID = 4; }
                if (rand == 95 || rand == 96) { rarityID = 5; }
                if (rand == 97 || rand == 98) { rarityID = 6; }
                if (rand == 99) { rarityID = 3; }
                if (rand == 100) { rarityID = 7; }

                GameObject spawnedItem;
                spawnedItem = Instantiate(itemPossibility);
                spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
                spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
                spawnedItem.GetComponent<ItemPossibility>().SetRarity(rarityID);
            }
        }
    }

    public void SetRarity(int givenRarity)
    {
        timer = 0.5f;

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
        PlayerItem pi = GameObject.Find("Player").GetComponent<PlayerItem>();

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
