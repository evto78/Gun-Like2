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
    GameDataManager gdm;
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
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        transform.localScale = Vector3.one * Random.Range(0.8f,1.2f);
        if (animatie)
        {
            anim = GetComponent<Animator>();
        }
        player = gdm.phm.gameObject;
        pi = gdm.phm.playerItem;

        cost = Mathf.CeilToInt((gdm.phm.baseCost * (int)(gdm.difficulty * (gdm.roomNumber+1))) * (numOfItems/2f));

        if (free) { cost = 0; }

        costTxt.text = cost.ToString() + "$";

        bool limestoneScale = pi.leftItems[178] + pi.rightItems[178] > 0;
        if (limestoneScale)
        {
            rarity = ItemRarity.GetUnWeightedRandRarity();
        }
        else
        {
            rarity = ItemRarity.GetWeightedRandRarity();
        }

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
        bool limestoneScale = pi.leftItems[178] + pi.rightItems[178] > 0;
        if (limestoneScale)
        {
            for (int i = 0; i < numOfItems; i++)
            {
                itemsSpawned++;
                SpawnItem(ItemRarity.GetUnWeightedRandRarity());
            }
        }
        else
        {
            for (int i = 0; i < numOfItems; i++)
            {
                itemsSpawned++;
                SpawnItem(ItemRarity.GetWeightedRandRarity());
            }
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
