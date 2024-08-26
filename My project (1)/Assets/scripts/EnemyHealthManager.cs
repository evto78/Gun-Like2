using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{

    public float curHp;
    public float maxHp;

    public float armor;

    //public GameObject damageText;

    public TextMeshProUGUI lastDmg;
    public TextMeshProUGUI mostDmg;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI hps;

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
    }

    void Update()
    {
        CalculateStats();
    }

    public void TakeDamage(float dmgTaken, bool ignoreArmor, string textColor, Transform hitLocation)
    {
        hitcounter++;

        if (ignoreArmor)
        {
            curHp -= dmgTaken;
            latestDamage = dmgTaken;
            //PopUpText(Mathf.RoundToInt(curHp -= dmgTaken), textColor, hitLocation);
        }
        else
        {
            if (armor >= dmgTaken)
            {
                curHp -= 1f;
                latestDamage = 1f;
                //PopUpText(1, textColor, hitLocation);
            }
            else
            {
                curHp -= (dmgTaken - armor);
                latestDamage = dmgTaken - armor;
                //PopUpText(Mathf.RoundToInt(dmgTaken - armor), textColor, hitLocation);
            }
        }

        if (latestDamage > highestDamage)
        {
            highestDamage = latestDamage;
        }
        damagecounter += latestDamage;

        lastDmg.text = "Last damage: " + latestDamage;
        mostDmg.text = "Most damage: " + highestDamage;
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

    void PopUpText(int dmgText, string textColor, Transform hitLocation)
    {
        //GameObject spawnedText = Instantiate(damageText);
        //spawnedText.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
        //spawnedText.GetComponent<DamageText>().SetText(dmgText.ToString(), textColor);
    }
}