using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
	public List<MonoBehaviour> brains;
    public GameObject frozenEffect;
    public GameObject item;
    public GameObject itemPossibility;

    public GameObject bloodOrb;
    public GameObject deadlyOrb;
    public GameObject burnedPapers;

    public int moneyDrop;
    public int dropVariance;

    public float dropChance;

    public float curHp;
    public float maxHp;

    public float armor;

    public GameObject damageText;
    public float latestDamage;

    GameObject player;
    PlayerItem playerItem;
    HealthManager playerHM;

    public GameObject effectIcon;
    List<GameObject> icons;
    public Transform effectHolder;

    int featherton;

    bool died;
    public bool didOnDeath;

    public List<Vector4> activeEffects = new List<Vector4>();
    // x == stacks of effect
    // y == Time until 1 stack of effect goes away
    // z == Timeleft until 1 stack is removed
    // w == 1 if effect is positive,0 if effect is neutral, and -1 if effect is negative

    List<float> dmgQued = new List<float>();

    void Start()
    {
        activeEffects = new List<Vector4>();
        for(int i = 0; i < 11; i++)
        {
            activeEffects.Add(Vector4.zero);
        }

        icons = new List<GameObject>();
        foreach(Vector4 effect in activeEffects)
        {
            GameObject spawnedIcon = Instantiate(effectIcon);
            spawnedIcon.transform.SetParent(effectHolder, false);
            icons.Add(spawnedIcon);
        }

        curHp = maxHp;

        player = GameObject.FindWithTag("Player");
        playerItem = player.GetComponent<PlayerItem>();
        playerHM = player.GetComponent<HealthManager>();
    }

    void Update()
    {
        ManageEffects();
        featherton = 0 + playerItem.leftItems[87] + playerItem.rightItems[87];
        if (curHp <= 0 && !died) { Die(); died = true; }
        
        if(dmgQued.Count > 0)
        {
            TakeDamage(dmgQued[0], true, "normalHit", transform.position, "self");
            dmgQued.RemoveAt(0);
        }
    }

    public void TakeDamage(float dmgTaken, bool ignoreArmor, string textColor, Vector3 hitLocation, string source)
    {
        if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { ignoreArmor = false; }
        if (activeEffects[7].x > 0) { dmgTaken = dmgTaken * (1f + 0.1f * playerItem.leftItems[69] + playerItem.rightItems[69]); }
        if(activeEffects[9].x > 0) { dmgTaken += dmgTaken * 0.2f; }

        if(activeEffects[6].x > 0 && playerItem.leftItems[52] + playerItem.rightItems[52] > 0)
        {
            dmgTaken = dmgTaken * 2f;
        }

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

        if (playerHM.stichedEnemies.Count > 0 && playerItem.leftItems[51] + playerItem.rightItems[51] > 0)
        {
            foreach (EnemyHealthManager ehm in playerHM.stichedEnemies)
            {
                if(dmgTaken * 0.25f > 1f)
                {
                    ehm.QueStandardDamage(dmgTaken * (1f / 4f));
                }
            }
        }

        if(playerItem.leftItems[126] + playerItem.rightItems[126] > 0 && dmgTaken >= 2f)
        {
            GameObject spawnedPapers = Instantiate(burnedPapers, transform.position, transform.rotation);
            spawnedPapers.GetComponent<BurnedPapers>().damage = dmgTaken;
            spawnedPapers.GetComponent<BurnedPapers>().collidedWith.Add(gameObject);
            spawnedPapers.transform.localScale = Vector3.one * ((playerItem.leftItems[126] + playerItem.rightItems[126])/2f);
        }

        if(source == "left")
        {
            //irradiated battle plans
            float chance = playerItem.leftItems[80] * 25f; if(chance > 50) { chance = 50; }
            if (Random.Range(1,100)<chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage,false);
                latestDamage = 0;
            }
        }
        else if (source == "right")
        {
            //irradiated battle plans
            float chance = playerItem.rightItems[80] * 25f; if(chance > 50) { chance = 50; }
            if (Random.Range(1, 100) < chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage, false);
                latestDamage = 0;
            }
        }
        if(latestDamage != 0)
        {
            PopUpText(latestDamage.ToString(), textColor, hitLocation, source);
            gameObject.SendMessage("TookDmg", SendMessageOptions.DontRequireReceiver);
        }

        if (curHp <= 0 && !died) { Die(); died = true; }
    }

    public void TakePercentDamage(float pDmgTaken)
    {
        TakeDamage(curHp * pDmgTaken, true, "normalHit", transform.position, "self");

        if (curHp <= 0) { Die(); }
    }
    public void QueStandardDamage(float damage)
    {
        dmgQued.Add(damage);
    }
    public void OnHitEffect(int jam)
    {
        if(jam > 0 && Random.Range(1, 100) <= 10 + (5 * jam))
        {
            GiveEffect("jammed", 1);

            if(activeEffects[3].x > jam) { activeEffects[3] = new Vector4(jam, activeEffects[3].y, activeEffects[3].z, activeEffects[3].w); }
        }
        if(playerItem.leftItems[79] + playerItem.rightItems[79] > 0)
        {
            if(Random.Range(1,100) < (playerItem.leftItems[79] + playerItem.rightItems[79]) * 10)
            {
                GameObject spawnedOrb;
                if (Random.Range(1, 100) < 20) { spawnedOrb = Instantiate(deadlyOrb); } else { spawnedOrb = Instantiate(bloodOrb); }
                spawnedOrb.transform.position = transform.position + Vector3.up;
                Destroy(spawnedOrb, 60f);
            }
        }
        if(activeEffects[7].x > 0 && playerItem.leftItems[72] + playerItem.rightItems[72] > 0)
        {
            player.GetComponent<GunManager>().SpawnAxe((transform.position - player.transform.position).normalized);
        }
    }

    public void Die()
    {
        OnDeath();

        Destroy(gameObject);
    }

    public void GiveEffect(string effectGiven, float stacksToAdd)
    {
        //Genaric DOT
        if (effectGiven == "bleed") { activeEffects[0] = new Vector4(activeEffects[0].x + stacksToAdd, 3f, 3f, -1f); }
        if (effectGiven == "burn") { activeEffects[1] = new Vector4(activeEffects[1].x + stacksToAdd, 2f, 2f, -1f); }
        if (effectGiven == "radiation") { activeEffects[2] = new Vector4(activeEffects[2].x + stacksToAdd, 6f, 6f, -1f); }

        //Item effects
        if (effectGiven == "jammed") { activeEffects[3] = new Vector4(activeEffects[3].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }
        if (effectGiven == "lucky") { activeEffects[4] = new Vector4(activeEffects[4].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 0f); }
        if (effectGiven == "stiched") { activeEffects[5] = new Vector4(activeEffects[5].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }
        if (effectGiven == "frozen") { activeEffects[6] = new Vector4(activeEffects[6].x + stacksToAdd, 10f, 10f, -1f); }
        if (effectGiven == "gunked") { activeEffects[7] = new Vector4(activeEffects[7].x + stacksToAdd, 3f, 3f, -1f); }
        if (effectGiven == "storage") { activeEffects[8] = new Vector4(activeEffects[8].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 0f); }
        if (effectGiven == "gas") { activeEffects[9] = new Vector4(activeEffects[9].x + stacksToAdd, 1f, 1f, -1f); }
        if (effectGiven == "blind") { activeEffects[10] = new Vector4(activeEffects[10].x + stacksToAdd, 1f, 1f, -1f); }
    }

    void ManageEffects()
    {
        Vector4 q = new Vector4(0, 0, 0, 0);

        for (int i = 0; i < activeEffects.Count; i++)
        {
            q = activeEffects[i];

            if (i == 6 && q.x > 0)
            {
                foreach (MonoBehaviour brain in brains)
                {
                    brain.enabled = false;
                }
                frozenEffect.SetActive(true);
            }
            else if (i == 6 && q.x <= 0)
            {
                foreach (MonoBehaviour brain in brains)
                {
                    brain.enabled = true;
                }
                frozenEffect.SetActive(false);
            }
            if (i == 10 && q.x > 0)
            {
                foreach (MonoBehaviour brain in brains)
                {
                    brain.enabled = false;
                }
            }
            else if (i == 10 && q.x <= 0 && activeEffects[6].x<1f)
            {
                foreach (MonoBehaviour brain in brains)
                {
                    brain.enabled = true;
                }
            }

            //if there are any stacks of this effect
            if (q.x > 0)
            {

                //progress timer and remove stacks as needed
                if (q.z > 0f)
                {
                    q.z -= Time.deltaTime;
                }
                else
                {
                    //If player has anti-antidode do not remove stacks when timer runs out
                    int antiAnti = playerItem.leftItems[41] + playerItem.rightItems[41];
                    if (antiAnti < 1)
                    {
                        if (Random.Range(0f, 2f) <= 1f) { q.x -= 1f; }
                    }
                    else
                    {
                        if(Random.Range(0f, 10f/antiAnti) <= 1f) { q.x -= 1f; }
                    }
                    if (q.x! > 0f) { q.z = q.y; }

                    //run effects that happen when timer ends
                    if (i == 0)
                    { 
                        if((playerItem.leftItems[50] + playerItem.rightItems[50]) > 0)
                        {
                            TakeDamage((q.x + 1f) * 20f, true, "normalHit", transform.position, "self");
                        }
                        else
                        {
                            TakeDamage((q.x + 1f) * 10f, true, "normalHit", transform.position, "self");
                        }
                        
                    }
                    if (i == 1) 
                    {
                        if ((playerItem.leftItems[48] + playerItem.rightItems[48]) > 0)
                        {
                            TakeDamage((q.x + 1f) * 20f, true, "normalHit", transform.position, "self");
                        }
                        else
                        {
                            TakeDamage((q.x + 1f) * 10f, true, "normalHit", transform.position, "self");
                        }
                    }
                    if (i == 2) { TakeDamage((q.x + 1f)*50f, true, "normalHit", transform.position, "self"); }
                }
            }

            activeEffects[i] = q;
        }

        for(int i = 0; i < activeEffects.Count; i++)
        {
            Vector4 effect = activeEffects[i];
            if (effect.x > 0f)
            {
                icons[i].SetActive(true);
                icons[i].GetComponent<EffectIconScript>().UpdateEffectIcon(i, Mathf.RoundToInt(effect.x));
            }
            else
            {
                icons[i].SetActive(false);
            }
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

    public void OnDeath()
    {
        if (didOnDeath) { return; } else { didOnDeath = true; }

        if(activeEffects[4].x > 0f) { moneyDrop += Mathf.RoundToInt((moneyDrop / 10f) * activeEffects[4].x); }
        playerHM.EnemyDied(this, Random.Range(moneyDrop - dropVariance, moneyDrop + dropVariance));

        //gunlike classic
        if(playerItem.leftItems[38] + playerItem.rightItems[38] > 0)
        {
            dropChance = (playerItem.leftItems[38] + playerItem.rightItems[38])*10f;
            if (Random.Range(1, 100) <= dropChance)
            {
                int rand = Random.Range(1, 101);
                List<List<int>> raritys = playerItem.rarityList;

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
        //gotcha machine
        if (playerItem.leftItems[75] + playerItem.rightItems[75] > 0)
        {
            dropChance = 50;
            if (Random.Range(1, 100) <= dropChance)
            {
                playerItem.gotchaTickets += Mathf.RoundToInt(maxHp / 25f);
            }
        }
        //ton of feathers
        if(featherton > 0)
        {
            float maxDist = 10f + (featherton * 30f);
            List<EnemyHealthManager> possibleTargets = new List<EnemyHealthManager>();
            List<float> dist = new List<float>();

            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if(enemy.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager healthMan))
                {
                    possibleTargets.Add(healthMan);
                    dist.Add(Vector3.Distance(transform.position, enemy.transform.position));
                }
            }
            float lowestHP = float.PositiveInfinity;
            int lowestHPIndex = 0;
            for (int i = 0; i < possibleTargets.Count; i++)
            {
                if (possibleTargets[i].curHp < lowestHP && dist[i] <= maxDist && possibleTargets[i] != this && possibleTargets[i].died == false && possibleTargets[i].didOnDeath == false)
                {
                    lowestHP = possibleTargets[i].curHp;
                    lowestHPIndex = i;
                }
            }
            bool playerTargeted = false;
            if(playerHM.curHp < lowestHP && Vector3.Distance(transform.position, player.transform.position) < maxDist)
            {
                if(Random.Range(1,100) < 100 - (featherton * 25)) { playerTargeted = true; }
            }
            if(lowestHP != float.PositiveInfinity && !playerTargeted) { possibleTargets[lowestHPIndex].TakeDamage(latestDamage, true, "normalhit", possibleTargets[lowestHPIndex].transform.position, "self"); Debug.DrawLine(transform.position, possibleTargets[lowestHPIndex].transform.position, Color.red, 1f); }
            else if (lowestHP != float.PositiveInfinity && playerTargeted) { playerHM.TakeDamage(latestDamage, false); }
        }
    }

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = playerItem.rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
}