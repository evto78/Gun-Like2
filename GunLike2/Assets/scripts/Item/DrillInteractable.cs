using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrillInteractable : MonoBehaviour
{
    bool interacted;
    Animator anim;
    public int cost;
    public GameObject itemPossibility;
    public GameObject grenadeEnemy;
    public Transform spawnPos;
    PlayerItem pi;
    GameDataManager gdm;
    int numOfItems;
    int itemsSpawned;
    public float priceModifier = 1;

    public TextMeshProUGUI costTxt;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
        pi = gdm.phm.playerItem;

        cost = Mathf.CeilToInt(gdm.phm.baseCost * priceModifier * (int)(gdm.difficulty * (gdm.roomNumber + 1)));
        costTxt.text = cost.ToString() + "$";
        interacted = false;
        anim.speed = 0.5f;
    }
    public void Interact()
    {
        if (interacted) { return; }
        if (pi.healthManager.money < cost) { return; }
        pi.healthManager.money -= cost;
        pi.spentMoneyThisRoom = true;

        interacted = true;
        anim.SetTrigger("Start");
        StartCoroutine(Drill());
    }
    IEnumerator Drill()
    {
        gdm.pointsLeft += gdm.flatPointsPerDifficulty * gdm.difficulty / 2;
        yield return new WaitForSeconds(7.5f / anim.speed);
        SpawnItems();
        yield return new WaitForSeconds(2.5f / anim.speed);
        interacted = false;
        yield return null;
    }
    void SpawnItems()
    {
        int luck = Random.Range(0, 10);
        switch (luck)
        {
            case 0: Instantiate(grenadeEnemy, spawnPos.position+spawnPos.forward, spawnPos.rotation); break;
            case 1: SpawnItem(GetRarity()); break;
            case 2: SpawnItem(GetRarity()); break;
            case 3: SpawnItem(GetRarity()); SpawnItem(GetRarity()); break;
            case 4: SpawnItem(GetRarity()); SpawnItem(GetRarity()); break;
            case 5: for (int i = 0; i < 3; i++) { SpawnItem(GetRarity()); }; break;
            case 6: for (int i = 0; i < 3; i++) { SpawnItem(GetRarity()); }; break;
            case 7: for (int i = 0; i < 4; i++) { SpawnItem(GetRarity()); }; break;
            case 8: for (int i = 0; i < 5; i++) { SpawnItem(GetRarity()); }; break;
            case 9: for (int i = 0; i < 8; i++) { SpawnItem(GetRarity()); }; break;
        }
    }
    int GetRarity()
    {
        int rarity;
        bool limestoneScale = pi.leftItems[178] + pi.rightItems[178] > 0;
        if (limestoneScale)
        {
            rarity = ItemRarity.GetUnWeightedRandRarity();
        }
        else
        {
            rarity = ItemRarity.GetWeightedRandRarity();
        }
        return rarity;
    }
    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = pi.rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = spawnPos.position;
        spawnedItem.GetComponent<Rigidbody>().AddForce(spawnPos.transform.forward * Random.Range(100f, 150f));
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(150f, 200f));
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD, false);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
}
