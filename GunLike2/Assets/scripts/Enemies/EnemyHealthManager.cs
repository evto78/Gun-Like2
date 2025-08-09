using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyHealthManager : MonoBehaviour
{
    [Header("BASE STATS:")]
    public float baseMaxHp;
    public float baseArmor;
    public float baseDamage;

    [Header("SCALING:")]
    public float difficultyStatScaling;
    public float difficultyScale;

    [Header("OTHER:")]

    public Spawnable data;
    public List<MonoBehaviour> brains;
    public GameObject frozenEffect;
    public GameObject markedEffect;
    public GameObject feathersEffect;
    public ParticleSystem ChemicalEffect;
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

    protected GameObject player;
    protected PlayerItem playerItem;
    public HealthManager playerHM;

    public GameDataManager gdm;

    public GameObject effectIcon;
    protected List<GameObject> icons;
    public Transform effectHolder;

    protected int featherton;

    protected bool died;
    public bool didOnDeath;

    protected int numOfActiveEffects;
    public List<Vector4> activeEffects = new List<Vector4>();
    // x == stacks of effect
    // y == Time until 1 stack of effect goes away
    // z == Timeleft until 1 stack is removed
    // w == 1 if effect is positive,0 if effect is neutral, and -1 if effect is negative

    List<float> dmgQued = new List<float>();
    protected float burnTimer; public bool refundPoints;

    protected string sourceOfLastDamage;
    protected virtual void Awake()
    {
        if (gdm == null)
        {
            //Gather references
            gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
            gdm.activeEhms.Add(this);
            playerHM = gdm.phm;
            playerItem = gdm.phm.playerItem;
            player = gdm.phm.gameObject;
        }
        else
        {
            gdm.activeEhms.Add(this);
            playerHM = gdm.phm;
            playerItem = gdm.phm.playerItem;
            player = gdm.phm.gameObject;
        }

        //Effect Setup
        activeEffects = new List<Vector4>();
        icons = new List<GameObject>();
        int effectsToAdd = 17;
        for (int i = 0; i < effectsToAdd; i++)
        {
            activeEffects.Add(Vector4.zero);
        }
        for (int i = 0; i < effectsToAdd; i++)
        {
            GiveEffect(i.ToString(), 0f);
            GameObject spawnedIcon = Instantiate(effectIcon);
            spawnedIcon.transform.SetParent(effectHolder, false);
            icons.Add(spawnedIcon);
        }
        ManageEffects();
        //Check if mutated
        if (playerHM.massMutation > 0 && Random.Range(1, 100) < 2.5f + ((playerHM.massMutation - 1) * 5f))
        {
            GiveEffect("mutated", 1);
            baseMaxHp *= 2f;
            baseArmor *= 2f;
            baseDamage *= 2f;
            transform.localScale *= 1.5f;
        }
        //Base Stat setup
        maxHp = baseMaxHp * difficultyScale * gdm.difficulty;
        armor = baseArmor * difficultyScale * gdm.difficulty;
        //Make sure is at fullHP
        curHp = maxHp;
        //NOW GO GET EM SOILDER!!!
        LateAwake();
    }
    void LateAwake() { }
    void Update()
    {
        burnTimer += Time.deltaTime;
        numOfActiveEffects = 0;
        ManageEffects();
        featherton = 0 + playerItem.leftItems[87] + playerItem.rightItems[87];
        if (curHp <= 0 && !died) { Die(true, ""); died = true; }

        if (dmgQued.Count > 0)
        {
            TakeDamage(dmgQued[0], true, HitType.ht.normal, transform.position, "self");
            dmgQued.RemoveAt(0);
        }
    }

    public virtual void TakeDamage(float dmgTaken, bool ignoreArmor, HitType.ht hit, Vector3 hitLocation, string source)
    {
        sourceOfLastDamage = source;
        if (hit == HitType.ht.weak || hit == HitType.ht.critweak || hit == HitType.ht.special) { ignoreArmor = true; }
        foreach(MonoBehaviour brain in brains)
        {
            if(hit == HitType.ht.critweak || hit == HitType.ht.weak)
            {
                brain.SendMessage("WeakHit", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                brain.SendMessage("Hit", SendMessageOptions.DontRequireReceiver);
            }
        }
        float tempArmor = armor;
        if (playerHM.ionParticle > 0 && Random.Range(0f, 100f) < 0.5f * playerHM.ionParticle)
        {
            float rand = Random.Range(10f, 1000f);
            dmgTaken *= rand;
        }
        if (activeEffects[0].x > 0) { tempArmor *= 0.25f; }
        if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { ignoreArmor = false; }
        if (activeEffects[7].x > 0) { dmgTaken = dmgTaken * (1f + 0.1f * playerItem.leftItems[69] + playerItem.rightItems[69]); }
        if(activeEffects[9].x > 0) { dmgTaken += dmgTaken * 0.2f; }
        if(playerHM.activeEffects[22].x > 0 && playerItem.leftItems[134]+playerItem.rightItems[134]>1) { dmgTaken *= 1.25f * (playerItem.leftItems[134] + playerItem.rightItems[134] - 1); }
        if(playerItem.leftItems[146]>0 && source == "left") { dmgTaken *= (1.05f+0.05f * playerItem.leftItems[146])*numOfActiveEffects; }
        if(playerItem.rightItems[146]>0 && source == "right") { dmgTaken *= (1.05f+0.05f * playerItem.rightItems[146])*numOfActiveEffects; }

        if (hit != HitType.ht.crit && hit != HitType.ht.critweak && hit != HitType.ht.special)
        {
            if (activeEffects[11].x > 0 && Random.Range(0,2) == 0) { 
                if(hit == HitType.ht.normal) { hit = HitType.ht.crit; }
                if(hit == HitType.ht.weak) { hit = HitType.ht.critweak; }
                if (source == "left") { dmgTaken *= playerItem.gunManager.leftGunScript.critDamage; }
                if (source == "right") { dmgTaken *= playerItem.gunManager.rightGunScript.critDamage; }
                if(playerItem.leftItems[130] + playerItem.rightItems[130] > 1) { dmgTaken *= (1.2f * (playerItem.leftItems[130] + playerItem.rightItems[130] - 1)); }
            }
        }
        else if(activeEffects[11].x > 0 && playerItem.leftItems[130] + playerItem.rightItems[130] > 1)
        {
            dmgTaken *= (1.2f * (playerItem.leftItems[130] + playerItem.rightItems[130] - 1));
        }

        if((source == "left" && playerItem.leftItems[127] > 0))
        {
            if(curHp >= maxHp)
            {
                if(Random.Range(1,100) < (playerItem.leftItems[127] * 2.5f) + 5f)
                {
                    curHp = -100f;
                    if (source == "left" && playerItem.leftItems[133] > 0) { playerItem.gunManager.leftGunScript.echoDmg = dmgTaken / 1.5f; }
                    if (source == "right" && playerItem.rightItems[133] > 0) { playerItem.gunManager.rightGunScript.echoDmg = dmgTaken / 1.5f; }
                    Die(true, source);
                }
            }
        }
        if((source == "right" && playerItem.rightItems[127] > 0))
        {
            if (curHp >= maxHp)
            {
                if (Random.Range(1, 100) < (playerItem.rightItems[127] * 2.5f) + 5f)
                {
                    curHp = -100f;
                    if (source == "left" && playerItem.leftItems[133] > 0) { playerItem.gunManager.leftGunScript.echoDmg = dmgTaken / 1.5f; }
                    if (source == "right" && playerItem.rightItems[133] > 0) { playerItem.gunManager.rightGunScript.echoDmg = dmgTaken / 1.5f; }
                    Die(true, source);
                }
            }
        }

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
            if (tempArmor >= dmgTaken)
            {
                curHp -= 1f;
                latestDamage = 1f;
            }
            else
            {
                curHp -= (dmgTaken - tempArmor);
                latestDamage = dmgTaken - tempArmor;
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
            //saveData
            playerItem.gunManager.leftDamageDATA += dmgTaken; playerItem.gunManager.leftHitsDATA++;
            if(dmgTaken > playerItem.gunManager.leftMaxDmgDATA) { playerItem.gunManager.leftMaxDmgDATA = dmgTaken; }

            //irradiated battle plans
            float chance = playerItem.leftItems[80] * 25f; if(chance > 50) { chance = 50; }
            if (Random.Range(1,100)<chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage,false,this);
                latestDamage = 0;
            }
        }
        else if (source == "right")
        {
            //saveData
            playerItem.gunManager.rightDamageDATA += dmgTaken; playerItem.gunManager.rightHitsDATA++;
            if (dmgTaken > playerItem.gunManager.rightMaxDmgDATA) { playerItem.gunManager.rightMaxDmgDATA = dmgTaken; }

            //irradiated battle plans
            float chance = playerItem.rightItems[80] * 25f; if(chance > 50) { chance = 50; }
            if (Random.Range(1, 100) < chance)
            {
                curHp += latestDamage;
                playerHM.TakeDamage(-latestDamage, false,this);
                latestDamage = 0;
            }
        }
        if(latestDamage != 0)
        {
            PopUpText(latestDamage.ToString(), hit, hitLocation, source);
            gameObject.SendMessage("TookDmg", SendMessageOptions.DontRequireReceiver);
        }

        if (curHp <= 0 && !died) { died = true; 
            if(source == "left" && playerItem.leftItems[133] > 0) { playerItem.gunManager.leftGunScript.echoDmg = dmgTaken / 1.5f; }
            if(source == "right" && playerItem.rightItems[133] > 0) { playerItem.gunManager.rightGunScript.echoDmg = dmgTaken / 1.5f; }
            Die(true, source);
        }
    }

    public void TakePercentDamage(float pDmgTaken, string source)
    {
        TakeDamage(curHp * pDmgTaken, true, HitType.ht.normal, transform.position, "self");

        if (curHp <= 0) { Die(true, source); }
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
            playerItem.gunManager.SpawnAxe((transform.position - player.transform.position).normalized);
        }
    }

    public virtual void Die(bool sendByHm, string source)
    {
        //on death effects
        OnDeath();

        //on actual destruction
        if(source == "left" || sourceOfLastDamage == "left") { playerItem.gunManager.leftKillsDATA++; } else if(source == "right" || sourceOfLastDamage == "right") { playerItem.gunManager.rightKillsDATA++; }
        if (data != null && refundPoints){gdm.pointsLeft += data.pointCost / 1.5f;} // refund some points
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (gdm.activeEhms.Contains(this)) { gdm.activeEhms.Remove(this); }
    }
    public void GiveEffect(string effectGiven, float stacksToAdd)
    {
        int effectID = -1;
        if(int.TryParse(effectGiven, out int id)) { effectID = id; }
        //Genaric DOT
        if (effectID == 0 || effectGiven == "bleed") { activeEffects[0] = new Vector4(activeEffects[0].x + stacksToAdd, 4f, 4f, -1f);
            float dotDmgModifer = 1; if (activeEffects[13].x > 0) { dotDmgModifer *= 2; }
            if ((playerItem.leftItems[50] + playerItem.rightItems[50]) > 0) { dotDmgModifer *= 2; }
            if (activeEffects[0].x % 5 == 0) { QueStandardDamage(activeEffects[0].x*20f*dotDmgModifer); }
        }
        if (effectID == 1 || effectGiven == "burn") { activeEffects[1] = new Vector4(activeEffects[1].x + stacksToAdd, 2f, 2f, -1f); }
        if (effectID == 2 || effectGiven == "radiation") {  
            if(activeEffects[2].x == 1) { activeEffects[2] = new Vector4(stacksToAdd * 2f, 6f, 6f, -1f); }
            else { activeEffects[2] = new Vector4(activeEffects[2].x + (stacksToAdd * 2f), 6f, 6f, -1f); }
        }

        //Item effects
        if (effectID == 3 || effectGiven == "jammed") { activeEffects[3] = new Vector4(activeEffects[3].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }//Jam
        if (effectID == 4 || effectGiven == "lucky") { activeEffects[4] = new Vector4(activeEffects[4].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 0f); }//Silver4Cash
        if (effectID == 5 || effectGiven == "stiched") { activeEffects[5] = new Vector4(activeEffects[5].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }//HelpingHandInHand
        if (effectID == 6 || effectGiven == "frozen") { activeEffects[6] = new Vector4(activeEffects[6].x + stacksToAdd, 10f, 10f, -1f); }//CoolAsIce
        if (effectID == 7 || effectGiven == "gunked") { activeEffects[7] = new Vector4(activeEffects[7].x + stacksToAdd, 3f, 3f, -1f); }//Gunky's blessing
        if (effectID == 8 || effectGiven == "storage") { activeEffects[8] = new Vector4(activeEffects[8].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 0f); }//Improvised Storage
        if (effectID == 9 || effectGiven == "gas") { activeEffects[9] = new Vector4(activeEffects[9].x + stacksToAdd, 1f, 1f, -1f); }//Gas Gernade attachment
        if (effectID == 10 || effectGiven == "blind") { activeEffects[10] = new Vector4(activeEffects[10].x + stacksToAdd, 1f, 1f, -1f); }//Broken Lightbulb
        if (effectID == 11 || effectGiven == "marked") { activeEffects[11] = new Vector4(activeEffects[11].x + stacksToAdd, 25f, 25f, -1f); }//Canine Tooth
        if (effectID == 12 || effectGiven == "webbed") { activeEffects[12] = new Vector4(activeEffects[12].x + stacksToAdd, 1f, 1f, 1f); }//Table Leg slow effect
        if (effectID == 13 || effectGiven == "enzymes") { activeEffects[13] = new Vector4(activeEffects[13].x + stacksToAdd, 5f, 5f, -1f); }//Enzymes
        if (effectID == 14 || effectGiven == "chemical A") { activeEffects[14] = new Vector4(activeEffects[14].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }//Chemical Agents
        if (effectID == 15 || effectGiven == "chemical B") { activeEffects[15] = new Vector4(activeEffects[15].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, -1f); }//Chemical Agents
        if (effectID == 16 || effectGiven == "mutated") { activeEffects[16] = new Vector4(activeEffects[16].x + stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 1f); }//Mass Mutation

        if(activeEffects[2].x < 1) { activeEffects[2] = new Vector4(0, 6f, 6f, -1f); }
    }
    public void RandomDebuff()
    {
        List<int> debuffs = new List<int>();
        foreach(Vector4 effect in activeEffects)
        {
            if(effect.w == -1f) { debuffs.Add(activeEffects.IndexOf(effect)); }
        }
        int temp = debuffs[Random.Range(0, debuffs.Count)];
        GiveEffect(temp.ToString(), 1f);
    }
    protected virtual void ManageEffects()
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
            if(i == 11 && q.x > 0)
            {
                markedEffect.SetActive(true);
            }
            else if(i == 11 && q.x < 1)
            {
                markedEffect.SetActive(false);
            }

            //if there are any stacks of this effect
            if (q.x > 0)
            {
                numOfActiveEffects++;
                float dotDmgModifer = 1; if (activeEffects[13].x > 0) { dotDmgModifer *= 2; }
                //burn
                if (i == 1 && burnTimer >= 0.2f)
                {
                    if ((playerItem.leftItems[48] + playerItem.rightItems[48]) > 0) { dotDmgModifer *= 2; }
                    QueStandardDamage(1f * dotDmgModifer);
                    burnTimer = 0;
                }

                //progress timer and remove stacks as needed
                if (q.z > 0f)
                {
                    q.z -= Time.deltaTime;
                }
                else
                {
                    //run effects that happen when timer ends
                    if (i == 2)
                    {
                        QueStandardDamage(q.x * 50f * dotDmgModifer);
                    }
                    //If player has anti-antidode chance to not remove stacks when timer runs out
                    int antiAnti = playerItem.leftItems[41] + playerItem.rightItems[41];
                    if (antiAnti > 0)
                    {
                        if (Random.Range(0, 9+antiAnti) == 0)
                        {
                            q.x -= 1f;
                            if (i == 2) { q.x += 1f; q.x /= 2f; if (q.x < 1f) { q.x = 0f; } }
                        }
                    }
                    else
                    {
                        q.x -= 1f;
                        if (i == 2)
                        {
                            q.x += 1f; q.x /= 2f; if (q.x < 1f) { q.x = 0f; }
                        }
                    }
                    if (q.x! > 0f) { q.z = q.y; }
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

    protected void PopUpText(string dmgText, HitType.ht hit, Vector3 hitLocation, string source)
    {
        GameObject spawnedText = Instantiate(damageText, player.GetComponentInChildren<Canvas>().gameObject.transform);

        //spawnedText.transform.SetParent(player.GetComponentInChildren<Canvas>().gameObject.transform);

        spawnedText.gameObject.transform.position = hitLocation;
        spawnedText.GetComponent<DamageText>().SetText(dmgText, hit, hitLocation, source);

        //Debug.DrawLine(hitLocation.position, hitLocation.position + Vector3.forward * 5, Color.cyan, 3f);
    }
    
    public void OnDeath()
    {
        if (didOnDeath) { return; } else { didOnDeath = true; }

        if(activeEffects[4].x > 0f) { moneyDrop += Mathf.RoundToInt((moneyDrop / 10f) * activeEffects[4].x); }
        playerHM.EnemyDied(this, (((int)gdm.difficulty+1)/2) * Random.Range(moneyDrop - dropVariance, moneyDrop + dropVariance));

        //gunlike classic
        if(playerItem.leftItems[38] + playerItem.rightItems[38] > 0)
        {
            dropChance = (playerItem.leftItems[38] + playerItem.rightItems[38])*10f;
            if (Random.Range(1, 100) <= dropChance)
            {
                int rand = Random.Range(1, 101);
                int rarityID = 0;
                bool limestoneScale = playerItem.leftItems[178] + playerItem.rightItems[178] > 0;
                if (limestoneScale)
                {
                    rarityID = ItemRarity.GetUnWeightedRandRarity();
                }
                else
                {
                    rarityID = ItemRarity.GetWeightedRandRarity();
                }
                SpawnItem(rarityID);
            }
        }
        //crate crab
        if (data.enemyName == "Crate Crab")
        {
            bool limestoneScale = playerItem.leftItems[178] + playerItem.rightItems[178] > 0; int rarityID;
            if (limestoneScale)
            {
                rarityID = ItemRarity.GetUnWeightedRandRarity();
            }
            else
            {
                rarityID = ItemRarity.GetWeightedRandRarity();
            }
            SpawnItem(rarityID);
            
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
            foreach(EnemyHealthManager ehm in gdm.activeEhms)
            {
                possibleTargets.Add(ehm);
                dist.Add(Vector3.Distance(transform.position, ehm.transform.position));
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
            if(lowestHP != float.PositiveInfinity && !playerTargeted) { possibleTargets[lowestHPIndex].TakeDamage(latestDamage, true, HitType.ht.normal, possibleTargets[lowestHPIndex].transform.position, "self"); Debug.DrawLine(transform.position, possibleTargets[lowestHPIndex].transform.position, Color.red, 1f); }
            else if (lowestHP != float.PositiveInfinity && playerTargeted) { playerHM.TakeDamage(latestDamage, false, this); }
            feathersEffect.SetActive(true);
            feathersEffect.transform.SetParent(null);
            Destroy(feathersEffect, 2f);
        }
        //mass mutation
        if (activeEffects[16].x > 0)
        {
            int rand = Random.Range(1, 101);
            int rarityID = 0;
            bool limestoneScale = playerItem.leftItems[178] + playerItem.rightItems[178] > 0;
            if (limestoneScale)
            {
                rarityID = ItemRarity.GetUnWeightedRandRarity();
            }
            else
            {
                rarityID = ItemRarity.GetWeightedRandRarity();
            }
            SpawnItem(rarityID);
        }
    }

    private void SpawnItem(int iD)
    {
        List<List<int>> raritys = playerItem.rarityList;

        GameObject spawnedItem;
        spawnedItem = Instantiate(itemPossibility);
        spawnedItem.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawnedItem.GetComponent<Rigidbody>().AddForce(Vector3.up * 500f);
        spawnedItem.GetComponent<ItemPossibility>().SetRarity(iD, false);
        spawnedItem.GetComponent<ItemPossibility>().rarityList = raritys;
    }
}