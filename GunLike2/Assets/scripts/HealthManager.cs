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
	public GameObject expGrowthExplosion;
	int numOfBunnies;

	public UIManager uiMan;
	public PlayerMovement playerMvt;

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
		numOfBunnies = 0 + givenLeftItems[20] + givenRightItems[20];

		//status effect buffs / debuffs

		if (activeEffects[14].x > 0f) { healthRegen = healthRegen / (orgGum / 10f + 1f); }
		if (activeEffects[6].x > 0f) { armor = armor * (orgGum / 10f + 1f); }
		if (activeEffects[8].x > 0f) { healthRegen = healthRegen * (orgGum / 10f + 1f); }
	}
	void Update()
	{
		regenTimer -= Time.deltaTime;
		if ((curHp < maxHp) && regenTimer <= 0f) { curHp += healthRegen * Time.deltaTime; }
		if (curHp > maxHp) { curHp = maxHp; }

		itemChecks();
		ManageEffects();
		DisplayEffects();
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
					if (rand == 4) { GiveEffect("organic apple", 1f); }
					if (rand == 5) { GiveEffect("organic berry", 1f); }
					if (rand == 6) { GiveEffect("organic choco", 1f); }
					if (rand == 7) { GiveEffect("organic lemon", 1f); }
					if (rand == 8) { GiveEffect("organic lime", 1f); }
					if (rand == 9) { GiveEffect("organic grape", 1f); }
					if (rand == 10) { GiveEffect("organic mint", 1f); }
					if (rand == 11) { GiveEffect("organic spicy", 1f); }
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
		if (effectGiven == "radiation") { activeEffects[2] = new Vector4(activeEffects[2].x + stacksToAdd, 6f, 6f, -1f); }

		//item effects
		if (effectGiven == "organic bannana") { activeEffects[3] = new Vector4(activeEffects[3].x + stacksToAdd, 20f, 20f, 1f); } // reload speed buff
		if (effectGiven == "organic apple") { activeEffects[4] = new Vector4(activeEffects[4].x + stacksToAdd, 20f, 20f, 1f); } // crit chance buff
		if (effectGiven == "organic berry") { activeEffects[5] = new Vector4(activeEffects[5].x + stacksToAdd, 20f, 20f, 1f); } // weak point damage buff
		if (effectGiven == "organic choco") { activeEffects[6] = new Vector4(activeEffects[6].x + stacksToAdd, 20f, 20f, 1f); } // armor buff
		if (effectGiven == "organic lemon") { activeEffects[7] = new Vector4(activeEffects[7].x + stacksToAdd, 20f, 20f, 1f); } // atk spd buff
		if (effectGiven == "organic lime") { activeEffects[8] = new Vector4(activeEffects[8].x + stacksToAdd, 20f, 20f, 1f); } // regen buff
		if (effectGiven == "organic grape") { activeEffects[9] = new Vector4(activeEffects[9].x + stacksToAdd, 20f, 20f, 1f); } // jump height buff
		if (effectGiven == "organic mint") { activeEffects[10] = new Vector4(activeEffects[10].x + stacksToAdd, 20f, 20f, 1f); } // move spd buff
		if (effectGiven == "organic spicy") { activeEffects[11] = new Vector4(activeEffects[11].x + stacksToAdd, 20f, 20f, 1f); } // dmg buff
		if (effectGiven == "organic red meat") { activeEffects[12] = new Vector4(activeEffects[12].x + stacksToAdd, 20f, 20f, -1f); } // dmg debuff
		if (effectGiven == "organic white meat") { activeEffects[13] = new Vector4(activeEffects[13].x + stacksToAdd, 20f, 20f, -1f); } // atk spd debuff
		if (effectGiven == "organic pink meat") { activeEffects[14] = new Vector4(activeEffects[14].x + stacksToAdd, 20f, 20f, -1f); } // regen debuff
		if (effectGiven == "organic gray meat") { activeEffects[15] = new Vector4(activeEffects[15].x + stacksToAdd, 20f, 20f, -1f); } // sprint speed debuff

		if (effectGiven == "bunny hop buff") { activeEffects[16] = new Vector4(activeEffects[16].x + stacksToAdd, 1f, 1f, 1f); }

		//Effect max stack management
		if (activeEffects[16].x > (numOfBunnies + 2)) { activeEffects[16] = new Vector4(numOfBunnies+2f, 1f, 1f, 1f); }
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

					if (playerMvt.timeSinceGrounded > 0f && i == 16) { q.z += Time.deltaTime; }
				}
				else
				{
					q.x -= 1f;
					if (q.x! < 1f) { q.z = q.y; }

					//run effects that happen when timer ends
					if (i == 0 || i == 1 || i == 2) { TakeDamage(q.x + 1f, false); }
				}
			}

			activeEffects[i] = q;
		}

	}

	void DisplayEffects()
	{
		string strToAdd = "";
		uiMan.effectsText.text = "";

		for (int i = 0; i < activeEffects.Count; i++)
		{
			if (activeEffects[i].x > 0)
			{
				if (i == 0) { strToAdd = "bleed"; }
				if (i == 1) { strToAdd = "burn"; }
				if (i == 2) { strToAdd = "radiation"; }
				if (i == 3) { strToAdd = "organic bannana"; }
				if (i == 4) { strToAdd = "organic apple"; }
				if (i == 5) { strToAdd = "organic berry"; }
				if (i == 6) { strToAdd = "organic choco"; }
				if (i == 7) { strToAdd = "organic lemon"; }
				if (i == 8) { strToAdd = "organic lime"; }
				if (i == 9) { strToAdd = "organic grape"; }
				if (i == 10) { strToAdd = "organic mint"; }
				if (i == 11) { strToAdd = "organic spicy"; }
				if (i == 12) { strToAdd = "organic red meat"; }
				if (i == 13) { strToAdd = "organic white meat"; }
				if (i == 14) { strToAdd = "organic pink meat"; }
				if (i == 15) { strToAdd = "organic gray meat"; }
				if (i == 16) { strToAdd = "bunny hop"; }
				uiMan.effectsText.text = uiMan.effectsText.text + " <br>" + strToAdd + "(" + activeEffects[i].x + ") (" + Mathf.Round(activeEffects[i].z) + ")";
			}
		}

	}

}
