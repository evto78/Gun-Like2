using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
	List<List<int>> rarityList = new List<List<int>>();

	public List<Vector4> activeEffects = new List<Vector4>();
	// x == stacks of effect
	// y == Time until 1 stack of effect goes away
	// z == Timeleft until 1 stack is removed
	// w == 1 if effect is positive,0 if effect is neutral, and -1 if effect is negative

	public int maxHp = 100;
	public float curHp;
	public float armor = 5;
	public float healthRegen = 1f;
	float regenTimer;

	float orgGum;
	float orgGumTimer;
	float expGrowth;
	GameObject expGrowthExplosion;

	// Start is called before the first frame update
	void Start()
	{
		curHp = maxHp;
	}
	public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
	{
		rarityList = givenRarityList;

		healthRegen = 1f * ((givenLeftItems[2] + givenRightItems[2]) / 10f + 1f) * ((givenLeftItems[14] + givenRightItems[14]) / 10f + 1f);
		armor = 5f * ((givenLeftItems[3] + givenRightItems[3] - 1) / 25f + 1f) / ((givenLeftItems[12] + givenRightItems[12]) / 10f + 1f);
		maxHp = Mathf.FloorToInt(100f * ((givenLeftItems[12] + givenRightItems[12]) / 5f + 1f) / ((givenLeftItems[13] + givenRightItems[13]) / 5f + 1f));

		orgGum = 0f + givenLeftItems[17] + givenRightItems[17];
		expGrowth = 0f + givenLeftItems[18] + givenRightItems[18];

		//status effect buffs / debuffs

		if (activeEffects[14].x > 0f) { healthRegen = healthRegen / (orgGum / 10 + 1f); }
		if (activeEffects[6].x > 0f) { armor = armor * (orgGum / 10 + 1f); }
		if (activeEffects[8].x > 0f) { healthRegen = healthRegen * (orgGum / 10 + 1f); }
	}
	void Update()
	{
		regenTimer -= Time.deltaTime;
		if ((curHp < maxHp) && regenTimer <= 0f) { curHp += healthRegen * Time.deltaTime; }
		if (curHp > maxHp) { curHp = maxHp; }

		itemChecks();
	}

	void itemChecks()
	{
		if (orgGum > 0)
		{
			orgGumTimer -= Time.deltaTime;
			if (orgGumTimer <= 0f)
			{
				orgGumTimer = 20f;

				if (Random.Range(1, 100) > (20 - (2f * orgGum)))
				{
					int rand = Random.Range(3, 11);
					if (rand == 3) { GiveEffect("organic bannana", 1f); }
					if (rand == 3) { GiveEffect("organic apple", 1f); }
					if (rand == 3) { GiveEffect("organic berry", 1f); }
					if (rand == 3) { GiveEffect("organic choco", 1f); }
					if (rand == 3) { GiveEffect("organic lemon", 1f); }
					if (rand == 3) { GiveEffect("organic lime", 1f); }
					if (rand == 3) { GiveEffect("organic grape", 1f); }
					if (rand == 3) { GiveEffect("organic mint", 1f); }
					if (rand == 3) { GiveEffect("organic spicy", 1f); }
				}
				else
				{
					int rand = Random.Range(12, 15);
					if (rand == 12) { GiveEffect("organic red meat", 1f); }
					if (rand == 13) { GiveEffect("organic white meat", 1f); }
					if (rand == 14) { GiveEffect("organic pink meat", 1f); }
					if (rand == 15) { GiveEffect("organic gray meat", 1f); }
				}
			}
		}
	}

	public void TakeDamage(float damageTaken, bool wasFromExpGrowth)
	{
		if (damageTaken <= 0)
		{
			//Heal
			curHp += damageTaken;
		}
		else
		{
			//Damage
			if (damageTaken <= armor)
			{
				//armor has absorbed all damage but min dmg is 1
				curHp -= 1f;
			}
			else
			{
				//return new hp with dmg reduced by armor
				curHp -= (damageTaken - armor);
			}
			regenTimer = 2f;

			if (expGrowth > 0 && (!wasFromExpGrowth || Random.Range(1, 100) < 16))
			{
				GameObject createdGrowthExplosion = Instantiate(expGrowthExplosion, transform.position, transform.rotation);
				createdGrowthExplosion.GetComponent<ExplosiveGrowthScript>().Explode(expGrowth, damageTaken);
			}
		}
	}

	public void GiveEffect(string effectGiven, float stacksToAdd)
	{
		//damage over time
		if (effectGiven == "bleed") { activeEffects[0] = new Vector4(activeEffects[0].x + stacksToAdd, 3f, 3f, -1f); }
		if (effectGiven == "burn") { activeEffects[1] = new Vector4(activeEffects[1].x + stacksToAdd, 2f, 2f, -1f); }
		if (effectGiven == "radiation") { activeEffects[2] = new Vector4(activeEffects[1].x + stacksToAdd, 6f, 6f, -1f); }

		//item effects
		if (effectGiven == "organic bannana") { activeEffects[3] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic apple") { activeEffects[4] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic berry") { activeEffects[5] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic choco") { activeEffects[6] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic lemon") { activeEffects[7] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic lime") { activeEffects[8] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic grape") { activeEffects[9] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic mint") { activeEffects[10] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic spicy") { activeEffects[11] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, 1f); }
		if (effectGiven == "organic red meat") { activeEffects[12] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, -1f); }
		if (effectGiven == "organic white meat") { activeEffects[13] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, -1f); }
		if (effectGiven == "organic pink meat") { activeEffects[14] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, -1f); }
		if (effectGiven == "organic gray meat") { activeEffects[15] = new Vector4(activeEffects[0].x + stacksToAdd, 20f, 20f, -1f); }

		if (effectGiven == "bunny hop buff") { activeEffects[16] = new Vector4(activeEffects[0].x + stacksToAdd, 1f, 1f, 1f); }
	}

	void ManageEffects()
	{
		Vector4 q = new Vector4(0, 0, 0, 0);

		for (int i = 0; i < activeEffects.Count - 1; i++)
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
					if (q.x! < 1f) { q.z = q.y; }

					//run effects that happen when timer ends
					if (i == 0 || i == 1 || i == 2) { TakeDamage(q.x + 1f, false); }
				}
			}
		}
	}

}