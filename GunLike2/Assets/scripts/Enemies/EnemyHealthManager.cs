using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
    public GameObject item;
    public GameObject itemPossibility;

    public float dropChance;

    public float curHp;
    public float maxHp;

    public float armor;

    public GameObject damageText;

    public TextMeshProUGUI lastDmg;
    public TextMeshProUGUI mostDmg;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI hps;

    UIManager uiManager;
    GameObject player;

    float damagePerSecond;
    float hitsPerSecond;
    float hitcounter;
    float damagecounter;
    float highestDamage;
    float latestDamage;

    float timer;

    void Start()
    {
        curHp = maxHp;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            uiManager = player.GetComponent<UIManager>();
        }
    }

    void Update()
    {
        CalculateStats();
    }

    public void TakeDamage(float dmgTaken, bool ignoreArmor, string textColor, Vector3 hitLocation, string source)
    {
        hitcounter++;

        if (ignoreArmor)
        {
            curHp -= dmgTaken;
            latestDamage = dmgTaken;
        }
        else
        {
            if (armor >= dmgTaken)
            {
                curHp -= 1f;
                latestDamage = 1f;
            }
            else
            {
                curHp -= (dmgTaken - armor);
                latestDamage = dmgTaken - armor;
            }
        }

        PopUpText(latestDamage.ToString(), textColor, hitLocation, source);

        if (latestDamage > highestDamage)
        {
            highestDamage = latestDamage;
        }
        damagecounter += latestDamage;

        lastDmg.text = "Last damage: " + latestDamage;
        mostDmg.text = "Most damage: " + highestDamage;
    }

    public void TakePercentDamage(float pDmgTaken)
    {
        TakeDamage(curHp * pDmgTaken, true, "normalHit", transform.position, "self");
    }

    public void Die()
    {
        OnDeath();

        Destroy(gameObject);
    }

    void CalculateStats()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1f;
            dps.text = "DPS: " + damagecounter;
            hps.text = "HPS: " + hitcounter;
            damagecounter = 0;
            hitcounter = 0;
        }
    }

    void PopUpText(string dmgText, string textColor, Vector3 hitLocation, string source)
    {
        GameObject spawnedText = Instantiate(damageText, player.GetComponentInChildren<Canvas>().gameObject.transform);

        //spawnedText.transform.SetParent(player.GetComponentInChildren<Canvas>().gameObject.transform);

        spawnedText.gameObject.transform.position = hitLocation;
        spawnedText.GetComponent<DamageText>().SetText(dmgText, textColor, hitLocation, source);

        //Debug.DrawLine(hitLocation.position, hitLocation.position + Vector3.forward * 5, Color.cyan, 3f);
    }

    private void OnDeath()
    {
        if (Random.Range(1, 100) <= dropChance)
        {
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

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = player.GetComponent<PlayerItem>().rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
}