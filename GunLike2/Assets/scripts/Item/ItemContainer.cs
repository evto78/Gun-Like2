using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemContainer : MonoBehaviour
{
    Animator anim;
    public int cost;
    public GameObject hatch;
    public GameObject itemPossibility;
    public Transform spawnPos;
    GameObject player;
    bool interacted;
    public bool animatie;
    public int numOfItems;
    int itemsSpawned;
    public float priceModifier = 1;
    public bool free;
    public int rarity;
    public GameObject internalItemPos;
    List<MeshRenderer> meshes = new List<MeshRenderer>();
    public bool eyeballLooking;
    public float timeSinceEye;

    public TextMeshProUGUI costTxt;
    void Start()
    {
        if (animatie)
        {
            anim = GetComponent<Animator>();
        }
        player = GameObject.Find("Player");

        cost = player.GetComponent<HealthManager>().baseCost;
        if (free) { cost = 0; }

        costTxt.text = cost.ToString() + "$";

        int rand = Random.Range(1, 101);

        if (rand < 71) { rarity = 0; }
        if (rand < 91 && rand > 70) { rarity = 1; }
        if (rand == 91 || rand == 92) { rarity = 2; }
        if (rand == 93 || rand == 94) { rarity = 4; }
        if (rand == 95 || rand == 96) { rarity = 5; }
        if (rand == 97 || rand == 98) { rarity = 6; }
        if (rand == 99) { rarity = 3; }
        if (rand == 100) { rarity = 7; }

        meshes.AddRange(GetComponentsInChildren<MeshRenderer>());
    }

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = spawnPos.position;
        spawnedItem.GetComponent<Rigidbody>().AddForce(spawnPos.transform.forward * Random.Range(100f, 150f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(150f, 200f));
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
        
    }
    public void Interact()
    {
        if (interacted) { return; }
        if (player.GetComponent<HealthManager>().money < Mathf.RoundToInt(cost * priceModifier)) { return; }
        Destroy(internalItemPos);
        player.GetComponent<HealthManager>().money -= Mathf.RoundToInt(cost * priceModifier);

        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        interacted = true;
        gameObject.GetComponent<Collider>().enabled = false;
        if (animatie) { anim.SetTrigger("Open"); }
        hatch.GetComponent<Collider>().isTrigger = true;
        hatch.AddComponent<Rigidbody>();
        hatch.GetComponent<Rigidbody>().AddForce((spawnPos.transform.forward * 1000f) + (Vector3.one * Random.Range(-100f,100f)));
        hatch.GetComponent<Rigidbody>().AddTorque(Vector3.one * Random.Range(-100f,100f));
        hatch.transform.SetParent(null);
        Destroy(hatch, 10f);

        itemsSpawned = 0;
        for(int i = 0; i < numOfItems; i++)
        {
            itemsSpawned++;
            int rand = Random.Range(1, 101);
            List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;
            if (i == 0) { SpawnItem(rarity); rand = 999; }
            if (rand < 71) { SpawnItem(0); }
            if (rand < 91 && rand > 70) { SpawnItem(1); }
            if (rand == 91 || rand == 92) { SpawnItem(2); }
            if (rand == 93 || rand == 94) { SpawnItem(4); }
            if (rand == 95 || rand == 96) { SpawnItem(5); }
            if (rand == 97 || rand == 98) { SpawnItem(6); }
            if (rand == 99) { SpawnItem(3); }
            if (rand == 100) { SpawnItem(7); }
        }

        Destroy(costTxt.transform.gameObject);
    }
    private void Update()
    {
        if (free) { cost = 0; }
        costTxt.text = Mathf.RoundToInt(cost * priceModifier).ToString() + "$";
        timeSinceEye += Time.deltaTime;
        if(timeSinceEye > 0.25f)
        {
            eyeballLooking = false;
            internalItemPos.transform.GetChild(rarity).gameObject.SetActive(false);
            foreach (MeshRenderer mr in meshes)
            {
                mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 1f);
            }
        }
        
    }
    private void LateUpdate()
    {
        if (eyeballLooking)
        {
            internalItemPos.transform.GetChild(rarity).gameObject.SetActive(true);
            foreach (MeshRenderer mr in meshes)
            {
                mr.material.color = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 0.2f);
            }
        }
    }

}
