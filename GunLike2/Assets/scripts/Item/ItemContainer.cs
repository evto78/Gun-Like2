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
    PlayerItem pi;
    bool interacted;
    public bool animatie;
    public int numOfItems;
    int itemsSpawned;
    public float priceModifier = 1;
    public bool free;
    public int rarity;
    public GameObject internalItemPos;
    List<SmartMeshRen> sMRS = new List<SmartMeshRen>();
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
        pi = player.GetComponent<PlayerItem>();

        cost = player.GetComponent<HealthManager>().baseCost;
        if (free) { cost = 0; }

        costTxt.text = cost.ToString() + "$";

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
        rarity = rarityID;

        if (player.GetComponent<PlayerItem>().leftItems[142] + player.GetComponent<PlayerItem>().rightItems[142] > 0) { rarity = 8; }

        sMRS.Clear();
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            SmartMeshRen smr = new SmartMeshRen();
            smr.mr = mr; smr.mat = mr.material; smr.matColor = mr.material.color;
            sMRS.Add(smr);
        }
    }

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = spawnPos.position;
        spawnedItem.GetComponent<Rigidbody>().AddForce(spawnPos.transform.forward * Random.Range(100f, 150f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(150f, 200f));
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD, false);
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

        if(pi.healthManager.ionParticle > 0 && Random.Range(0f, 100f) < 0.5f * pi.healthManager.ionParticle) { int rand = Random.Range(5, 20); numOfItems += rand; Debug.Log("Miracle of: "+rand); }

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
        if (player.GetComponent<PlayerItem>().leftItems[142] + player.GetComponent<PlayerItem>().rightItems[142] > 0) { rarity = 8; }
        if (free) { cost = 0; }
        costTxt.text = Mathf.RoundToInt(cost * priceModifier).ToString() + "$";
        timeSinceEye += Time.deltaTime;
        if(timeSinceEye > 0.25f)
        {
            eyeballLooking = false;
            if (internalItemPos != null) { internalItemPos.transform.GetChild(rarity).gameObject.SetActive(false); }
            foreach (SmartMeshRen smr in sMRS)
            {
                if (smr.mr != null)
                {
                    smr.mr.material.color = new Color(smr.matColor.r, smr.matColor.g, smr.matColor.b, 1f);
                }
            }
        }
        
    }
    private void LateUpdate()
    {
        if (eyeballLooking)
        {
            if (internalItemPos != null) { internalItemPos.transform.GetChild(rarity).gameObject.SetActive(true); }
            foreach (SmartMeshRen smr in sMRS)
            {
                if(smr.mr != null)
                {
                    smr.mr.material.color = new Color(smr.matColor.r, smr.matColor.g, smr.matColor.b, 0.2f);
                }
            }
        }
    }

}
