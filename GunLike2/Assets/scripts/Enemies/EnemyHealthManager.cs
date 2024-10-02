using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{

    public float curHp;
    public float maxHp;

    public float armor;

    public GameObject damageText;

    public TextMeshProUGUI lastDmg;
    public TextMeshProUGUI mostDmg;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI hps;

    public UIManager uiManager;
    public GameObject player;

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
}