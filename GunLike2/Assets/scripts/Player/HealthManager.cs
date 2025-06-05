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

	public float baseMaxHP = 100f;
	public float baseArmor = 5f;
	public float baseHealthRegen = 1f;

	public float maxHp;
	public float curHp;
	public float armor;
	public float healthRegen;
	float regenTimer;

	float orgGum;
	float orgGumTimer;
	float expGrowth;
	public GameObject expGrowthExplosion;
	public GameObject radioactiveDomesExplosion;
	int numOfBunnies;
	int symGrowth;
	int beltFed;
	int activeReactor;
	int radioDome;
	float radioTimer;
	int radiosQued;
	int experimentalImp;
	float experTimer;
	int partialInt;
	float evadeChance;
	int clockwork;

	public float timeSinceEnemyDied;

	public UIManager uiMan;
	public NEWPlayerMovement playerMvt;
	public PlayerItem playerItem;

	public int money;

	public int baseCost;

	public bool dead;

	public List<EnemyHealthManager> stichedEnemies = new List<EnemyHealthManager>();
	public LineRenderer stichedEffect;

	// Start is called before the first frame update
	void Start()
	{
		money = 0;
		dead = false;
		maxHp = baseMaxHP;
		curHp = maxHp;
	}
	public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
	{
		rarityList = givenRarityList;

		healthRegen = baseHealthRegen;
		armor = baseArmor;
		maxHp = baseMaxHP;

		healthRegen = Calc(20f, givenLeftItems[2] + givenRightItems[2], healthRegen);
		healthRegen = Calc(20f, givenLeftItems[14] + givenRightItems[14], healthRegen);
		healthRegen = Calc(-10f, givenLeftItems[24] + givenRightItems[24], healthRegen);
		healthRegen = Calc(-20f, givenLeftItems[30] + givenRightItems[30], healthRegen);
		healthRegen = Calc(-20f, givenLeftItems[80] + givenRightItems[80], healthRegen);
		armor = Calc(10f, givenLeftItems[3] + givenRightItems[3], armor);
		armor = Calc(10f, givenLeftItems[56] + givenRightItems[56], armor);
		armor = Calc(20f, givenLeftItems[61] + givenRightItems[61], armor);
		armor = Calc(20f, givenLeftItems[63] + givenRightItems[63], armor);
		armor = Calc(20f, givenLeftItems[65] + givenRightItems[65], armor);
		armor = Calc(-10f, givenLeftItems[12] + givenRightItems[12], armor);
		armor = Calc(-10f, givenLeftItems[66] + givenRightItems[66], armor);
		maxHp = Mathf.FloorToInt(Calc(20f, givenLeftItems[12] + givenRightItems[12], maxHp));
		maxHp = Mathf.FloorToInt(Calc(40f, givenLeftItems[23] + givenRightItems[23], maxHp));
		maxHp = Mathf.FloorToInt(Calc(20f, givenLeftItems[39] + givenRightItems[39], maxHp));
		maxHp = Mathf.FloorToInt(Calc(20f, givenLeftItems[60] + givenRightItems[60], maxHp));
		maxHp = Mathf.FloorToInt(Calc(20f, givenLeftItems[61] + givenRightItems[61], maxHp));
		maxHp = Mathf.FloorToInt(Calc(-20f, givenLeftItems[13] + givenRightItems[13], maxHp));
		maxHp = Mathf.FloorToInt(Calc(-20f, givenLeftItems[18] + givenRightItems[18], maxHp));
		maxHp = Mathf.FloorToInt(Calc(-20f, givenLeftItems[79] + givenRightItems[79], maxHp));

		orgGum = 0f + givenLeftItems[17] + givenRightItems[17];
		expGrowth = 0f + givenLeftItems[18] + givenRightItems[18];
		numOfBunnies = 0 + givenLeftItems[20] + givenRightItems[20];
		symGrowth = 0 + givenLeftItems[23] + givenRightItems[23];
		beltFed = 0 + givenLeftItems[29] + givenRightItems[29];
		activeReactor = 0 + givenLeftItems[30] + givenRightItems[30];
		radioDome = 0 + givenLeftItems[37] + givenRightItems[37];
		experimentalImp = 0 + givenLeftItems[39] + givenRightItems[39];
		partialInt = 0 + givenLeftItems[73] + givenRightItems[73];
		clockwork = 0 + givenLeftItems[81] + givenRightItems[81];

		if (activeReactor > 0) { maxHp = Mathf.FloorToInt(Calc(50f, givenLeftItems[30] + givenRightItems[30], maxHp)); }

		//Irradiated French Pastry
		if (givenLeftItems[22] > 0)
        {
			if (playerItem.leftIFPStatToBuff == 26) { maxHp = Mathf.FloorToInt(maxHp * (givenLeftItems[22] * 2)); }
			if (playerItem.leftIFPStatToBuff == 27) { healthRegen = healthRegen * (givenLeftItems[22] * 2); }
			if (playerItem.leftIFPStatToBuff == 28) { armor = armor * (givenLeftItems[22] * 2); }

			if (playerItem.leftIFPStatToDeBuff == 26) { maxHp = Mathf.FloorToInt(maxHp * (0.9f / givenLeftItems[22])); }
			if (playerItem.leftIFPStatToDeBuff == 27) { healthRegen = healthRegen * (0.9f / givenLeftItems[22]); }
			if (playerItem.leftIFPStatToDeBuff == 28) { armor = armor * (0.9f / givenLeftItems[22]); }
		}
		if (givenRightItems[22] > 0)
		{
			if (playerItem.rightIFPStatToBuff == 26) { maxHp = Mathf.FloorToInt(maxHp * (givenRightItems[22] * 2)); }
			if (playerItem.rightIFPStatToBuff == 27) { healthRegen = healthRegen * (givenRightItems[22] * 2); }
			if (playerItem.rightIFPStatToBuff == 28) { armor = armor * (givenRightItems[22] * 2); }

			if (playerItem.rightIFPStatToBuff == 26) { maxHp = Mathf.FloorToInt(maxHp * (0.9f / givenRightItems[22])); }
			if (playerItem.rightIFPStatToBuff == 27) { healthRegen = healthRegen * (0.9f / givenRightItems[22]); }
			if (playerItem.rightIFPStatToBuff == 28) { armor = armor * (0.9f / givenRightItems[22]); }
		}

		//status effect buffs / debuffs

		if (activeEffects[14].x > 0f) { healthRegen = healthRegen / (orgGum / 10f + 1f); }
		if (activeEffects[6].x > 0f) { armor = armor * (orgGum / 10f + 1f); }
		if (activeEffects[8].x > 0f) { healthRegen = healthRegen * (orgGum / 10f + 1f); }
	}

	float Calc(float modifier, int amount, float baseVal)
	{
		float result = baseVal;

        if (amount <= 0) { return result; }

		if (modifier > 0)
		{
			//Buff

			for (int i = 0; i < amount; i++)
			{
				result = result + result * (modifier / 100);
			}
		}
		else if (modifier < 0)
		{
			//Debuff
			modifier = modifier * -1f;

			for (int i = 0; i < amount; i++)
			{
				result = result - result * (modifier / 100);
			}
		}

		if(Mathf.FloorToInt(result) <= 0)
        {
			result = Mathf.CeilToInt(result);
        }

		return result;
	}
	void Update()
	{
		timeSinceEnemyDied += Time.deltaTime;

        if (dead) { return; }

		if(experimentalImp <= 0)
        {
			regenTimer -= Time.deltaTime;
			if ((curHp < maxHp) && regenTimer <= 0f)
			{
				if (symGrowth > 0 && curHp / maxHp > 0.8f)
				{
					curHp -= healthRegen * Time.deltaTime;
				}
				else
				{
					curHp += healthRegen * Time.deltaTime;
				}

			}
		}
		
		if (curHp > maxHp) { curHp = maxHp; }

		itemChecks();
		ManageEffects();
		DisplayEffects();

		if(curHp <= 0) { dead = true; }
	}

	public void EnemyDied(GameObject enemyThatDied, int moneyDropped)
    {
		timeSinceEnemyDied = 0;

		if(activeReactor > 0)
        {
			GiveEffect("active reactor", 1);
        }

		money += moneyDropped;
    }

	void itemChecks()
	{
		if (orgGum > 0)
		{
			orgGumTimer -= Time.deltaTime + (Time.deltaTime * clockwork * 0.25f);
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

		if(activeReactor > 0)
        {
			if(activeEffects[18].x > 0f)
            {
				if (curHp / maxHp > 0.33f)
				{
					curHp += (maxHp / 100) * Time.deltaTime * 2f;
				}
				else
				{
					curHp += ((maxHp / 100) * 3f) * Time.deltaTime * 2f;
				}
			}
            else
            {
				if (curHp / maxHp > 0.33f)
				{
					curHp -= (maxHp / 100) * Time.deltaTime;
				}
				else
				{
					curHp -= ((maxHp / 100) * 3f) * Time.deltaTime;
				}
			}
        }

		radioTimer -= Time.deltaTime + (Time.deltaTime * clockwork * 0.25f);
		if(radioTimer <= 0 && radiosQued > 0)
        {
			GameObject spawnedRadioDome = Instantiate(radioactiveDomesExplosion);
			spawnedRadioDome.transform.position = transform.position;
			spawnedRadioDome.GetComponent<RadioactiveDomes>().damage = maxHp * (15f / 100f);
			radiosQued -= 1;
			radioTimer = 0.5f;
		}

		if(experimentalImp > 0)
        {
			experTimer -= Time.deltaTime + (Time.deltaTime * clockwork * 0.25f);

			if(experTimer <= 0)
            {
				experTimer = 0.5f;

				if(Random.Range(1,100) > (53 - experimentalImp * 3))
                {
					TakeDamage(-1f * healthRegen, false);
					Debug.Log("Healing for: " + -1f * healthRegen);
                }
                else
                {
					if(curHp > 0.5f * healthRegen)
                    {
						TakeDamage(0.5f * healthRegen, false);
						Debug.Log("Damaging for: " + 0.5f * healthRegen);
					}
                }
            }
        }

		if(playerItem.leftItems[43] + playerItem.rightItems[43] > 0)
        {
			stichedEnemies.Clear();
			foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
				if(enemy.TryGetComponent<EnemyHealthManager>(out EnemyHealthManager enemyHealthMan))
                {
					if(enemyHealthMan.activeEffects[5].x > 0f)
                    {
						stichedEnemies.Add(enemyHealthMan);
                    }
                }
            }
			stichedEffect.positionCount = 1;
			stichedEffect.SetPosition(0, transform.position - Vector3.up);
			stichedEffect.positionCount = (stichedEnemies.Count * 2) + 1;
			int index = 1;
			foreach(EnemyHealthManager ehm in stichedEnemies)
            {
				stichedEffect.SetPosition(index, ehm.transform.position);
				stichedEffect.SetPosition(index+1, transform.position - Vector3.up);
				index += 2;
            }

		}

		if(partialInt > 0)
        {
			evadeChance = (playerItem.modifierList[0]/6f) * 100f;
			if(evadeChance > 75) { evadeChance = 75f; }
        }
	}

	public void TakeDamage(float damageTaken, bool wasFromExpGrowth)
	{
		bool wasAtMax = (curHp == maxHp);
		if (damageTaken <= 0)
		{
			//Heal
			curHp -= damageTaken;
		}
		else
		{
            if (Random.Range(1, 100) < evadeChance) { Debug.Log("Blocked!"); return; }
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

			if(stichedEnemies.Count > 0)
            {
				foreach(EnemyHealthManager ehm in stichedEnemies)
                {
					if(playerItem.leftItems[51] + playerItem.rightItems[51] > 0)
                    {
						ehm.TakeDamage(damageTaken * (1f / 4f), true, "normalHit", ehm.transform.position, "self");
					}
                    else
                    {
						ehm.TakeDamage(damageTaken * (1f / 8f), true, "normalHit", ehm.transform.position, "self");
					}
					
                }
            }
		}

		if (curHp != maxHp && wasAtMax && radioDome > 0)
		{
			TakeDamage(maxHp * (15f / 100f), false);
			GameObject spawnedRadioDome = Instantiate(radioactiveDomesExplosion);
			spawnedRadioDome.transform.position = transform.position;
			spawnedRadioDome.GetComponent<RadioactiveDomes>().damage = maxHp * (15f / 100f);

			radioTimer = 0.5f;
			radiosQued += radioDome - 1;
		}

		if (curHp / maxHp < 0.2f)
        {
			if(playerItem.leftItems[65] > 0)
            {
				playerItem.leftItems[65] -= 1;
				playerItem.leftItems[66] += 1;
				playerItem.OnItemDestroy(65, -1, "left");
				playerItem.OnItemGain(66, 1, "left");
            }
			if (playerItem.rightItems[65] > 0)
			{
				playerItem.rightItems[65] -= 1;
				playerItem.rightItems[66] += 1;
				playerItem.OnItemDestroy(65, -1, "right");
				playerItem.OnItemGain(66, 1, "right");
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

		if (effectGiven == "bunny hop buff") { activeEffects[16] = new Vector4(activeEffects[16].x + stacksToAdd, 1f, 1f, 1f); } // irradiated bunny slippers buff

		if (effectGiven == "pants falling") { activeEffects[17] = new Vector4(stacksToAdd, 0.1f * beltFed, 0.1f * beltFed, 1f); } // belt fed magazine buff

		if (effectGiven == "active reactor") { activeEffects[18] = new Vector4(stacksToAdd, activeReactor * 5f, activeReactor * 5f, 1f); } // active reactor buff
		if (effectGiven == "fast fire") { activeEffects[19] = new Vector4(stacksToAdd, 1f, 1f, 1f); } // Fast Fire partership buff

		//Effect max stack management
		if (activeEffects[16].x > (numOfBunnies + 2)) { activeEffects[16] = new Vector4(numOfBunnies+2f, 1f, 1f, 1f); }
		if (activeEffects[18].x > 1) { activeEffects[18] = new Vector4(1, activeReactor * 5f, activeReactor * 5f, 1f); }
		if (activeEffects[19].x > 1) { activeEffects[19] = new Vector4(1, 1f, 1f, 1f); }
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
					if (q.x > 0f) { q.z = q.y; }

					//run effects that happen when timer ends
					if (i == 0 || i == 1 || i == 2) { TakeDamage((q.x + 1f)*5f, false); }
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
				if (i == 17) { strToAdd = "pants falling"; }
				if (i == 18) { strToAdd = "active reactor"; }
				uiMan.effectsText.text = uiMan.effectsText.text + " <br>" + strToAdd + "(" + activeEffects[i].x + ") (" + Mathf.Round(activeEffects[i].z) + ")";
			}
		}

	}

}
