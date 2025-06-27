using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopCrate : MonoBehaviour
{
    Animator anim;
    public int cost;
    public GameObject itemPossibility;
    public Transform spawnPos;
    GameObject player;
    PlayerItem pi;
    public int id;
    int rarity;
    float timer;

    public TextMeshProUGUI costTxt;
    public Image img;
    public Image rarityBG;
    public List<Sprite> rarityBGs;
    public float priceModifier = 1;

    bool overrideId; int idOver;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        pi = player.GetComponent<PlayerItem>();

        //pickID
        int rand = Random.Range(1, 101);
        int temp = 0;
        List<List<int>> raritys = pi.rarityList;

        if (rand < 71) { temp = 0; }
        if (rand < 91 && rand > 70) { temp = 1; }
        if (rand == 91 || rand == 92) { temp = 2; }
        if (rand == 93 || rand == 94) { temp = 4; }
        if (rand == 95 || rand == 96) { temp = 5; }
        if (rand == 97 || rand == 98) { temp = 6; }
        if (rand == 99) { temp = 3; }
        if (rand == 100) { temp = 7; }

        if (pi.leftItems[142] + pi.rightItems[142] > 0) { temp = 8; overrideId = true; idOver = 143; }

        cost = Mathf.RoundToInt(player.GetComponent<HealthManager>().baseCost * (1.2f + temp * 2f));
        if(temp == 3 || temp == 7) { cost *= 3; }
        if(temp == 8) { cost /= Mathf.RoundToInt(1.2f + temp); }

        costTxt.text = cost.ToString() + "$";

        id = raritys[temp][Random.Range(0, raritys[temp].Count)];
        if (overrideId) { id = idOver; }
        rarity = temp;
        rarityBG.sprite = rarityBGs[rarity];
        img.sprite = pi.FindObjByID(id).itemSprite;
    }
    private void SpawnItem(int iD)
    {
        cost = Mathf.RoundToInt(cost * 1.1f);

        timer = 0.25f;
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = spawnPos.position;
        spawnedItem.GetComponent<Rigidbody>().AddForce(spawnPos.transform.forward * Random.Range(100f, 150f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(150f, 200f));
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(rarity);
        spawnedItem.GetComponent<ItemPossibility>().overrideid = true;
        spawnedItem.GetComponent<ItemPossibility>().idover = iD;
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
    public void Interact()
    {
        if (player.GetComponent<HealthManager>().money < (cost * priceModifier) || timer > 0f) { return; }

        player.GetComponent<HealthManager>().money -= Mathf.RoundToInt(cost * priceModifier);

        anim.SetTrigger("Open");

        SpawnItem(id);
    }
    private void Update()
    {
        if(pi.leftItems[142] + pi.rightItems[142] > 0)
        {
            if (id != 143) { id = 143;  img.sprite = pi.FindObjByID(id).itemSprite; }
            overrideId = true; idOver = 143; rarity = 8; rarityBG.sprite = rarityBGs[rarity];
        }

        timer -= Time.deltaTime;
        costTxt.text = Mathf.RoundToInt(cost * priceModifier).ToString() + "$";
        int i = 0;
        foreach (List<int> rarList in pi.rarityList)
        {
            if (pi.rarityList[i].Contains(id)) { rarity = i; }
            i++;
        }
        rarityBG.sprite = rarityBGs[rarity];
    }
}
