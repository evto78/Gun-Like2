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

    public TextMeshProUGUI costTxt;
    void Start()
    {
        if (animatie)
        {
            anim = GetComponent<Animator>();
        }
        player = GameObject.Find("Player");

        cost = player.GetComponent<HealthManager>().baseCost;

        costTxt.text = cost.ToString() + "$";
    }

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = spawnPos.position;
        spawnedItem.GetComponent<Rigidbody>().AddForce(spawnPos.transform.forward * 100f);
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 200f);
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }

    public void Interact()
    {
        if (interacted) { return; }
        if (player.GetComponent<HealthManager>().money < cost) { return; }

        player.GetComponent<HealthManager>().money -= cost;

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

        int rand = Random.Range(1, 100);
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        if (rand < 71) { SpawnItem(0); }
        if (rand < 91 && rand > 70) { SpawnItem(1); }
        if (rand == 91 || rand == 92) { SpawnItem(2); }
        if (rand == 93 || rand == 94) { SpawnItem(4); }
        if (rand == 95 || rand == 96) { SpawnItem(5); }
        if (rand == 97 || rand == 98) { SpawnItem(6); }
        if (rand == 99) { SpawnItem(3); }
        if (rand == 100) { SpawnItem(7); }
    }

}
