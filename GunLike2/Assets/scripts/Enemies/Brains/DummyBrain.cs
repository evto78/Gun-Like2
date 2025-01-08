using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DummyBrain : MonoBehaviour
{
    EnemyHealthManager healthMan;

    float hitcounter;
    float damagecounter;
    float highestDamage;
    float latestDamage;

    float timer;

    public TextMeshProUGUI lastDmg;
    public TextMeshProUGUI mostDmg;
    public TextMeshProUGUI dps;
    public TextMeshProUGUI hps;

    void Start()
    {
        healthMan = GetComponent<EnemyHealthManager>();
    }

    void Update()
    {
        CalculateStats();
    }

    public void TookDmg()
    {
        hitcounter++;

        latestDamage = healthMan.latestDamage;

        if (latestDamage > highestDamage)
        {
            highestDamage = latestDamage;
        }
        damagecounter += latestDamage;

        lastDmg.text = "Last damage: " + latestDamage;
        mostDmg.text = "Most damage: " + highestDamage;

        CalculateStats();
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
}
