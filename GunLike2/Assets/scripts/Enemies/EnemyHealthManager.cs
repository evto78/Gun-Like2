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
    public float latestDamage;

    GameObject player;

    public GameObject effectIcon;
    List<GameObject> icons;
    public Transform effectHolder;

    public List<Vector4> activeEffects = new List<Vector4>();
    // x == stacks of effect
    // y == Time until 1 stack of effect goes away
    // z == Timeleft until 1 stack is removed
    // w == 1 if effect is positive,0 if effect is neutral, and -1 if effect is negative

    void Start()
    {
        icons = new List<GameObject>();
        foreach(Vector4 effect in activeEffects)
        {
            GameObject spawnedIcon = Instantiate(effectIcon);
            spawnedIcon.transform.SetParent(effectHolder, false);
            icons.Add(spawnedIcon);
        }

        curHp = maxHp;

        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        ManageEffects();

        
    }

    public void TakeDamage(float dmgTaken, bool ignoreArmor, string textColor, Vector3 hitLocation, string source)
    {
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
        gameObject.SendMessage("TookDmg", SendMessageOptions.DontRequireReceiver);

        if (curHp <= 0) { Die(); }
    }

    public void TakePercentDamage(float pDmgTaken)
    {
        TakeDamage(curHp * pDmgTaken, true, "normalHit", transform.position, "self");

        if (curHp <= 0) { Die(); }
    }

    public void OnHitEffect(int jam)
    {
        if(jam > 0 && Random.Range(1, 100) <= 10 + (5 * jam))
        {
            GiveEffect("jammed", 1);

            if(activeEffects[3].x > jam) { activeEffects[3] = new Vector4(jam, activeEffects[3].y, activeEffects[3].z, activeEffects[3].w); }
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
    }

    void ManageEffects()
    {
        Vector4 q = new Vector4(0, 0, 0, 0);

        for (int i = 0; i < activeEffects.Count; i++)
        {
            q = activeEffects[i];

            //if there are any stacks of this effect
            if (q.x > 0)
            {
                //run effects that happen every frame


                //progress timer and remove stacks as needed
                if (q.z > 0f)
                {
                    q.z -= Time.deltaTime;
                }
                else
                {
                    q.x -= 1f;
                    if (q.x! > 0f) { q.z = q.y; }

                    //run effects that happen when timer ends
                    if (i == 0 || i == 1 || i == 2) { TakeDamage(q.x + 1f, true, "normalHit", transform.position, "self"); }
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

    private void OnDeath()
    {
        player.GetComponent<HealthManager>().EnemyDied(gameObject);

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