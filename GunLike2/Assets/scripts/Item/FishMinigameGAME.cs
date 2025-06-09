using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishMinigameGAME : MonoBehaviour
{
    public FishingMinigame hostScript;
    UIManager uiman;
    int fishingItem;
    public float fishingTimeLeft;
    float difficulty;
    public float spawnInterval;
    float intervalTimer;
    public GameObject fish;
    GameObject player;
    public bool playing;
    public TextMeshProUGUI scoreTxt;
    public int score;
    public Image timer;
    public GameObject itemPossibility;
    public Transform spawnPos;

    public List<Material> fishMatsCommon;
    public List<Material> fishMatsUncommon;
    public List<Material> fishMatsRare;
    public List<Material> fishMatsEpic;
    public List<Material> fishMatsLegendary;
    public List<Material> fishMatsMythical;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        uiman = player.GetComponent<UIManager>();
        fishingItem = 0 + player.GetComponent<PlayerItem>().leftItems[83] + player.GetComponent<PlayerItem>().rightItems[83];
    }

    public void StartMinigame()
    {
        player = GameObject.Find("Player");
        playing = true;
        fishingItem = 0 + player.GetComponent<PlayerItem>().leftItems[83] + player.GetComponent<PlayerItem>().rightItems[83];
        fishingTimeLeft = fishingItem * 5f;
        difficulty = fishingItem;
        spawnInterval = 1;
        intervalTimer = 0;
        score = 0;
        scoreTxt.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timer.fillAmount = fishingTimeLeft / (fishingItem * 5f);
        fishingTimeLeft -= Time.deltaTime;
        intervalTimer -= Time.deltaTime * difficulty;
        if(fishingTimeLeft > 0)
        {
            difficulty = Mathf.CeilToInt(fishingTimeLeft / 2.5f);
            spawnInterval = 1;
            if(intervalTimer <= 0)
            {
                SpawnFish();
                intervalTimer = spawnInterval;
            }
        }
        else if(hostScript.timesUp == false)
        {
            hostScript.timesUp = true;
            int numOfItems = 1 + Mathf.RoundToInt(score / 40);
            int itemsSpawned = 0;
            for (int i = 0; i < numOfItems; i++)
            {
                itemsSpawned++;
                int rand = Random.Range(1, 101);

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
    }

    void SpawnFish()
    {
        //-0.6x - 0.6x
        //-0.8z - 0.55z
        GameObject spawnedFish = Instantiate(fish, transform);
        spawnedFish.transform.localPosition = Vector3.zero;
        spawnedFish.transform.localEulerAngles = Vector3.zero;
        spawnedFish.GetComponent<FishScript>().manager = this;

        int temp = Random.Range(1, 5);
        if(temp == 1) { spawnedFish.transform.localPosition = new Vector3(Random.Range(-0.6f, 0.6f),0f,-0.8f); }
        if(temp == 2) { spawnedFish.transform.localPosition = new Vector3(Random.Range(-0.6f, 0.6f), 0f, 0.55f); }
        if(temp == 3) { spawnedFish.transform.localPosition = new Vector3(-0.6f, 0f, Random.Range(-0.8f,0.55f)); }
        if(temp == 4) { spawnedFish.transform.localPosition = new Vector3(0.6f, 0f, Random.Range(-0.8f,0.55f)); }

        float temp2 = Random.Range(1f, 100f);
        if(temp2 < 50f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsCommon[Random.Range(0, fishMatsCommon.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 1; }
        else if(temp2 < 70f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsUncommon[Random.Range(0, fishMatsUncommon.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 2; }
        else if(temp2 < 80f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsRare[Random.Range(0, fishMatsRare.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 3; }
        else if(temp2 < 90f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsEpic[Random.Range(0, fishMatsEpic.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 4; }
        else if(temp2 < 95f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsLegendary[Random.Range(0, fishMatsLegendary.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 5; }
        else if(temp2 < 101f) { spawnedFish.GetComponentInChildren<MeshRenderer>().material = fishMatsMythical[Random.Range(0, fishMatsMythical.Count)]; spawnedFish.GetComponent<FishScript>().rarity = 6; }

        //spawnedFish.transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        spawnedFish.transform.LookAt(transform.position + transform.forward * Random.Range(-0.5f, 0.5f) + transform.right * Random.Range(-0.5f, 0.5f));
        spawnedFish.transform.localEulerAngles = new Vector3(0, spawnedFish.transform.localEulerAngles.y, 0);
    }
    public void FishCaught(int rarity)
    {
        score += rarity*5;
        scoreTxt.text = score.ToString();
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
}
