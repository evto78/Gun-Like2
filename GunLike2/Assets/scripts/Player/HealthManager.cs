using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class HealthManager : MonoBehaviour
{
	public List<EffectObject> allEffects = new List<EffectObject>();
	public List<int> activeEffectIDS = new List<int>();
	public List<int> activePosEffectIDS = new List<int>();
	public List<int> activeNegEffectIDS = new List<int>();
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

	public float orgGum;
	public float orgGumTimer;
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
	public float evadeChance;
	int clockwork;
	int warcry;
	int chickenCoop;
	public float chickenCoopTimer;
	int canineTooth;
	float canineToothTimer;
	public int divineInter;
	public float divineTimer;
	public EnemyHealthManager lastHitMe;
	public string lastHitMeName;
	EnemyHealthManager markedEnemy;
	public bool attackedThisRoom;
	public GameObject egg;
	public GameObject fly;
	int depleatedRock;
	bool leftSpongeStone; bool rightSpongeStone;
	public int massMutation;
	public int ionParticle;
	public int sunflower;
	public float sunflowerTimer;
	public GameObject sunflowerSun;
	public int pufferfish;
	public ParticleSystem killStreakExplosion;

	public int appleBuff;
	public float fortifyBuff;
	public float sunflowerDebuff;
	float burnTimer;

    public float timeSinceEnemyDied;

	public UIManager uiMan;
	public NEWPlayerMovement playerMvt;
	public PlayerItem playerItem;
	public LocalSoundManager lsm;
	GunManager gunManager;

	public int money;

	public int baseCost;

	public bool dead;
	public bool brokenSpeakerItemDropped;

	public List<EnemyHealthManager> stichedEnemies = new List<EnemyHealthManager>();
	public LineRenderer stichedEffect;

	public WishUI wishPopUp;
	public int wishes;

	public GameDataManager gdm; UnlockManager unlockMan;
	public bool freeRelaxedRevive;
    // Start is called before the first frame update
    private void Awake()
    {
		gunManager = gameObject.GetComponent<GunManager>();
		lsm = gameObject.GetComponent<LocalSoundManager>();
	}
    void Start()
	{
		gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>(); unlockMan = gdm.gameObject.GetComponent<UnlockManager>();
		allEffects.AddRange(gdm.effectData);
		if (gdm.instance.loadingARun == -1) { money = 0; }
		dead = false;
		maxHp = baseMaxHP;
		curHp = maxHp;
		activeEffects = new List<Vector4>();
		foreach(EffectObject effect in allEffects) { activeEffects.Add(new Vector4(0, effect.decayTime, effect.decayTime, effect.type)); }
	}
	public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
	{
		//Base Stats
		float healthRegenMult = 1f; float healthRegenDiv = 1f;
		float armorMult = 1f; float armorDiv = 1f;
		float maxHpMult = 1f; float maxHpDiv = 1f;

		healthRegen = baseHealthRegen;
		armor = baseArmor;
		maxHp = baseMaxHP + appleBuff + Mathf.CeilToInt(fortifyBuff);
		maxHp -= Mathf.CeilToInt(sunflowerDebuff);
		//Before Mult
		leftSpongeStone = givenLeftItems[167]>0; rightSpongeStone = givenRightItems[167]>0;
		//Health Regen
		healthRegenMult += MultAdder(20f, givenLeftItems[2] + givenRightItems[2], false, false);
		healthRegenMult += MultAdder(20f, givenLeftItems[14] + givenRightItems[14], false, false);
		healthRegenMult += MultAdder(20f, givenLeftItems[84] + givenRightItems[84], false, false);
		healthRegenMult += MultAdder(40f, givenLeftItems[92] + givenRightItems[92], false, false);
		healthRegenMult += MultAdder(10f, givenLeftItems[94] + givenRightItems[94], false, false);
		healthRegenMult += MultAdder(40f, givenLeftItems[129] + givenRightItems[129], false, false);
		healthRegenMult += MultAdder(40f, givenLeftItems[131] + givenRightItems[131], false, false);
		healthRegenMult += MultAdder(10f, givenLeftItems[143] + givenRightItems[143], false, false);

		healthRegenDiv += MultAdder(-20f, givenLeftItems[24] + givenRightItems[24], false, false);
		healthRegenDiv += MultAdder(-40f, givenLeftItems[30] + givenRightItems[30], false, false);
		healthRegenDiv += MultAdder(-40f, givenLeftItems[80] + givenRightItems[80], false, false);
		healthRegenDiv += MultAdder(-20f, givenLeftItems[96] + givenRightItems[96], false, false);
		//Armor
		armorMult += MultAdder(10f, givenRightItems[3], true, false); // <- all of the stats being seperated is sponge stones fault. Dont blame me.
		armorMult += MultAdder(10f, givenLeftItems[3], true, true);
		armorMult += MultAdder(10f, givenRightItems[56], true, false);
		armorMult += MultAdder(10f, givenLeftItems[56], true, true);
		armorMult += MultAdder(20f, givenRightItems[61], true, false);
		armorMult += MultAdder(20f, givenLeftItems[61], true, true);
		armorMult += MultAdder(20f, givenRightItems[63], true, false);
		armorMult += MultAdder(20f, givenLeftItems[63], true, true);
		armorMult += MultAdder(20f, givenRightItems[65], true, false);
		armorMult += MultAdder(20f, givenLeftItems[65], true, true);
		armorMult += MultAdder(60f, givenRightItems[115], true, false); 
		armorMult += MultAdder(60f, givenLeftItems[115], true, true); 
		armorMult += MultAdder(20f, givenRightItems[140], true, false);
		armorMult += MultAdder(20f, givenLeftItems[140], true, true);
		armorMult += MultAdder(10f, givenRightItems[143], true, false);
		armorMult += MultAdder(10f, givenLeftItems[143], true, true);
		armorMult += MultAdder(40f, givenRightItems[159], true, false);
		armorMult += MultAdder(40f, givenLeftItems[159], true, true);

		armorDiv += MultAdder(-20f, givenRightItems[12], true, false);
		armorDiv += MultAdder(-20f, givenLeftItems[12], true, true);
		armorDiv += MultAdder(-20f, givenRightItems[66], true, false);
		armorDiv += MultAdder(-20f, givenLeftItems[66], true, true);
		armorDiv += MultAdder(-40f, givenRightItems[166], true, false);
		armorDiv += MultAdder(-40f, givenLeftItems[166], true, true);
		//Max Hp
		maxHpMult += MultAdder(20f, givenLeftItems[12] + givenRightItems[12], false, false);
		maxHpMult += MultAdder(40f, givenLeftItems[23] + givenRightItems[23], false, false);
		maxHpMult += MultAdder(50f, givenLeftItems[30] + givenRightItems[30], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[39] + givenRightItems[39], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[60] + givenRightItems[60], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[61] + givenRightItems[61], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[85] + givenRightItems[85], false, false);
		maxHpMult += MultAdder(10f, givenLeftItems[99] + givenRightItems[99], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[124] + givenRightItems[124], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[140] + givenRightItems[140], false, false);
		maxHpMult += MultAdder(10f, givenLeftItems[143] + givenRightItems[143], false, false);
		maxHpMult += MultAdder(40f, givenLeftItems[159] + givenRightItems[159], false, false);
		maxHpMult += MultAdder(20f, givenLeftItems[166] + givenRightItems[166], false, false);

		maxHpDiv += MultAdder(-40f, givenLeftItems[13] + givenRightItems[13], false, false);
		maxHpDiv += MultAdder(-40f, givenLeftItems[18] + givenRightItems[18], false, false);
		maxHpDiv += MultAdder(-40f, givenLeftItems[79] + givenRightItems[79], false, false);
		maxHpDiv += MultAdder(-60f, givenLeftItems[92] + givenRightItems[92], false, false);
		//Other
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
		warcry = 0 + givenLeftItems[110] + givenRightItems[110];
		chickenCoop = 0 + givenLeftItems[114] + givenRightItems[114];
		canineTooth = 0 + givenLeftItems[130] + givenRightItems[130];
		massMutation = 0 + givenLeftItems[152] + givenRightItems[152];
		divineInter = 0 + givenLeftItems[155] + givenRightItems[155];
		depleatedRock = 0 + givenLeftItems[166] + givenRightItems[166];
		ionParticle = 0 + givenLeftItems[181] + givenRightItems[181];
		wishes = 0 + givenLeftItems[183] + givenRightItems[183];
		sunflower = 0 + givenLeftItems[186] + givenRightItems[186];
		//Applying Mult
		healthRegen *= healthRegenMult; healthRegen /= healthRegenDiv;
		armor *= armorMult; armor /= armorDiv;
		//Bio armor
		if (givenLeftItems[140] + givenRightItems[140] > 0) { maxHp += armor*5f; }
		//Back to Applying Mult
		maxHp *= maxHpMult; maxHp /= maxHpDiv;
		//Irradiated French Pastry
		if (givenLeftItems[22] > 0)
        {
            switch (playerItem.leftIFPStatToBuff)
            {
				case 26: maxHp = Mathf.FloorToInt(maxHp * (givenLeftItems[22] * 2)); break;
				case 27: healthRegen = healthRegen * (givenLeftItems[22] * 2); break;
				case 28: armor = armor * (givenLeftItems[22] * 2); break;
            }
			switch (playerItem.leftIFPStatToDeBuff)
            {
				case 26: maxHp = Mathf.FloorToInt(maxHp * (0.9f / givenLeftItems[22])); break;
				case 27: healthRegen = healthRegen * (0.9f / givenLeftItems[22]); break;
				case 28: armor = armor * (0.9f / givenLeftItems[22]); break;
			}
		}
		if (givenRightItems[22] > 0)
		{
			switch (playerItem.rightIFPStatToBuff)
			{
				case 26: maxHp = Mathf.FloorToInt(maxHp * (givenRightItems[22] * 2)); break;
				case 27: healthRegen = healthRegen * (givenRightItems[22] * 2); break;
				case 28: armor = armor * (givenRightItems[22] * 2); break;
			}
			switch (playerItem.rightIFPStatToDeBuff)
			{
				case 26: maxHp = Mathf.FloorToInt(maxHp * (0.9f / givenRightItems[22])); break;
				case 27: healthRegen = healthRegen * (0.9f / givenRightItems[22]); break;
				case 28: armor = armor * (0.9f / givenRightItems[22]); break;
			}
		}
		//Mutated Rules Modifiers
		maxHp *= gdm.mutatedStatModifiers[26];
		healthRegen *= gdm.mutatedStatModifiers[27];
		armor *= gdm.mutatedStatModifiers[28];
        //OtherDifficultyModifiers
        switch (gdm.difficultyIDSelected)
        {
			case 0: maxHp *= 2f; healthRegen *= 2f; break;
			case 2: healthRegen *= 0.75f; break;
			case 3: healthRegen *= 0.6f; break;
        }

		//status effect buffs / debuffs
		if (activeEffects[14].x > 0f) { healthRegen = healthRegen / (orgGum / 10f + 1f); }
		if (activeEffects[6].x > 0f) { armor = armor * (orgGum / 10f + 1f); }
		if (activeEffects[8].x > 0f) { healthRegen = healthRegen * (orgGum / 10f + 1f); }

		if (activeEffects[24].x > 0f) { armor += activeEffects[24].x; }
		if (activeEffects[25].x > 0f) { armor -= activeEffects[25].x; }
	}
	float MultAdder(float mult, int amount, bool isArmor, bool isLeft)
    {
		if(isArmor && (leftSpongeStone || rightSpongeStone))
        {
            if (isLeft && leftSpongeStone) { amount *= 2; }
            if (!isLeft && rightSpongeStone) { amount *= 2; }
        }
		if(mult > 0) { return mult * (1f / 100f) * amount; }
		if(mult < 0) { return -mult * (1f / 100f) * amount; }
		return 0;
	}
	void Update()
	{
		timeSinceEnemyDied += Time.deltaTime;

        if (dead) { return; }

		if (burnTimer > 0) { burnTimer -= Time.deltaTime; }

		if(experimentalImp <= 0)
        {
			regenTimer -= Time.deltaTime;
			if ((curHp < maxHp) && regenTimer <= 0f)
			{
				if (symGrowth > 0 && curHp / maxHp > 0.8f)
				{
					
					if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { curHp -= (healthRegen / armor) * Time.deltaTime; }
                    else { curHp -= (healthRegen) * Time.deltaTime; }
				}
				else
				{
					curHp += healthRegen * Time.deltaTime;
				}

			}
		}
		
		if (curHp > maxHp) { curHp = maxHp; }

		ItemChecks();
		
		statusEffectsActive = 0;
        ManageEffects();
		DisplayEffects();

		if(curHp <= 0 && activeEffects[21].x < 1) 
		{ 
			if(playerItem.leftItems[155] > 0)//Divine Intervention
            {
				GiveEffect(21, 1f);
				curHp = maxHp;
				divineTimer = 60f + (60f/divineInter);
				if(lastHitMe != null) { lastHitMe.TakeDamage(maxHp, false, HitType.ht.normal, lastHitMe.transform.position, "player"); }
            }
			else if (playerItem.rightItems[155] > 0)
            {
				GiveEffect(21, 1f);
				curHp = maxHp;
				divineTimer = 60f + (60f/divineInter);
				if (lastHitMe != null) { lastHitMe.TakeDamage(maxHp, false, HitType.ht.normal, lastHitMe.transform.position, "player"); }
			}
			else if (playerItem.leftItems[116] > 0)//Another Shot
			{
				GiveEffect(21, 1f);
				curHp = maxHp;
				playerItem.leftItems[116]--;
			}
			else if (playerItem.rightItems[116] > 0)
			{
				GiveEffect(21, 1f);
				curHp = maxHp;
				playerItem.rightItems[116]--;
			}
			else if (freeRelaxedRevive)
            {
				GiveEffect(21, 3f);
				curHp = maxHp;
				freeRelaxedRevive = false;
			}
			else
            {
				dead = true; gdm.instance.AddEmailToQue("Death");
			}
		}
	}

	public void EnemyDied(EnemyHealthManager enemyThatDied, int moneyDropped)
    {
		Debug.Log("Enemy died");
		timeSinceEnemyDied = 0;

		if(activeReactor > 0)
        {
			GiveEffect(18, 1);
        }
		if(gdm.difficultyIDSelected == 0){moneyDropped *= 2;} 

		money += moneyDropped; money += 10 * (playerItem.leftItems[177] + playerItem.rightItems[177]);

		if(enemyThatDied.activeEffects[35].x > 0)
        {
			for(int i = 0; i < enemyThatDied.activeEffects[35].x; i++)
            {
				if(playerItem.leftItems[95] > 0)
                {
					gunManager.leftGunScript.SpawnBulletAtPos(enemyThatDied.transform.position);
                }
				if (playerItem.rightItems[95] > 0)
				{
					gunManager.rightGunScript.SpawnBulletAtPos(enemyThatDied.transform.position);
				}
			}
        }

		if(enemyThatDied == lastHitMe && dead) { gdm.unlockMan.UnlockItem(43); } // Helping Hand in Hand (43)
    }

	void ItemChecks()
	{
		if (orgGum > 0)
		{
			orgGumTimer -= Time.deltaTime + (Time.deltaTime * clockwork);
			if (orgGumTimer <= 0f)
			{
				orgGumTimer = playerItem.FindObjByID(17).baseCooldown;
				int rand;
				if (playerItem.RandomItemEffectRoll(Random.Range(1, 100) > (20 - (2f * orgGum))))
                {
					rand = Random.Range(3, 11);
                }
                else
                {
					rand = Random.Range(12, 15);
                }
				GiveEffect(rand, 1f);
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
					if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { curHp -= ((maxHp / 100) / armor) * Time.deltaTime; }
                    else { curHp -= (maxHp / 100) * Time.deltaTime; }
				}
				else
				{
					if (playerItem.leftItems[115] + playerItem.rightItems[115] > 0) { curHp -= (((maxHp / 100) * 3f) / armor) * Time.deltaTime; }
					else { curHp -= ((maxHp / 100) * 3f) * Time.deltaTime; }
				}
			}
        }

		radioTimer -= Time.deltaTime + (Time.deltaTime * clockwork);
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
			experTimer -= Time.deltaTime + (Time.deltaTime * clockwork);

			if(experTimer <= 0)
            {
				experTimer = 0.5f;
				if (playerItem.RandomItemEffectRoll(Random.Range(1, 100) > (53 - experimentalImp * 3)))
				{
					TakeDamage(-1f * healthRegen, false, null, "Experimental Implant", null);
				}
				else
				{
					TakeDamage(0.5f * healthRegen, false, null, "Experimental Implant", null);
				}
            }
        }

		if(playerItem.leftItems[43] + playerItem.rightItems[43] > 0)
        {
			stichedEnemies.Clear();
			foreach (EnemyHealthManager ehm in gdm.activeEhms)
			{
				if (ehm.activeEffects[32].x > 0f)
				{
					stichedEnemies.Add(ehm);
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
			//evadeChance = (playerItem.modifierList[0]/6f) * 100f;
			if(evadeChance > 75) { evadeChance = 75f; }
        }

		if(chickenCoop > 0)
        {
			chickenCoopTimer -= Time.deltaTime + (Time.deltaTime * clockwork);

			if(chickenCoopTimer < 0)
            {
				GameObject spawnedEgg = Instantiate(egg);
				spawnedEgg.transform.position = transform.position;
				spawnedEgg.transform.position += Vector3.up;
				spawnedEgg.transform.position += transform.forward * -3f;
				spawnedEgg.GetComponent<Egg>().healPer = 5f + (2.25f * chickenCoop);

				chickenCoopTimer = playerItem.FindObjByID(114).baseCooldown;
            }
		}

		if(canineTooth > 0)
        {
			canineToothTimer -= Time.deltaTime + (Time.deltaTime * clockwork);
			if (markedEnemy == null && canineToothTimer <= 0f && gdm.activeEhms.Count > 0)
            {
				markedEnemy = gdm.activeEhms[Random.Range(0, gdm.activeEhms.Count)];
				markedEnemy.GiveEffect(38, 1f);
            }
			if (markedEnemy != null && markedEnemy.activeEffects[38].x < 1 && canineToothTimer <= 0)
            {
				markedEnemy = null;
				canineToothTimer = 25f;
            }
        }

		if(playerItem.leftItems[131] + playerItem.rightItems[131] > 0)
        {
			if(gdm.activeEhms.Count == 0)
            {
				regenTimer = -1f;
                if (!brokenSpeakerItemDropped)
                {
					for(int i = 0; i < playerItem.leftItems[131] + playerItem.rightItems[131]; i++)
                    {
						playerItem.SpawnItem(0, false, 0, false);
					}
					brokenSpeakerItemDropped = true;
                }
            }
        }

		if(playerItem.leftItems[134] + playerItem.rightItems[134] > 0)
        {
            if (!attackedThisRoom && activeEffects[22].x < 2) { GiveEffect(22, 1); }
			if (attackedThisRoom) { activeEffects[22] = new Vector4(0, activeEffects[22].y, activeEffects[22].z, activeEffects[22].w); }
        }

		if(divineInter > 0)
        {
			if(divineTimer > 0) { divineTimer -= Time.deltaTime + (Time.deltaTime * clockwork); }
        }

        if (wishes > 0)
        {
            if (!wishPopUp.gameObject.activeSelf) { wishPopUp.gameObject.SetActive(true); }
            if (!wishPopUp.ready)
            {
				wishPopUp.ready = true; wishPopUp.Popout();
            }
            if (wishPopUp.readWish() != 0)
            {
                switch (wishPopUp.readWish())
                {
					case 1: TakeDamage(-maxHp, false, null, "Wish", null); GiveEffect(21, 3f); wishPopUp.ready = false; break; //Heal
					case 2: money += 5000; wishPopUp.ready = false; break; //Money
					case 3: for (int i = 0; i < 10; i++) { playerItem.SpawnItem(0, false, 0, false); } wishPopUp.ready = false; break; //Item
					case 4: WishSmite(); wishPopUp.ready = false; break; //Smite
                }
				wishPopUp.gameObject.SetActive(wishPopUp.ready);
                if (!wishPopUp.ready)
                {
                    if (playerItem.leftItems[183] > 0 && playerItem.rightItems[183] > 0) { if (Random.Range(0, 2) == 0) { playerItem.leftItems[183]--; } else { playerItem.rightItems[183]--; } }
					else if (playerItem.leftItems[183] > 0) { playerItem.leftItems[183]--; } else { playerItem.rightItems[183]--; }
                }
            }
        }

		if(sunflower > 0)
        {
			sunflowerTimer -= Time.deltaTime + (Time.deltaTime * clockwork);
			if(sunflowerTimer < 0)
            {
				GameObject spawnedSun = Instantiate(sunflowerSun);
				spawnedSun.transform.position = transform.position + new Vector3(Random.Range(-25f, 25f), 100f, Random.Range(-25f, 25f));
				sunflowerTimer = 50f;
            }
			if(activeEffects[27].x < 1) { sunflowerDebuff += Time.deltaTime * 2f; }
        }

		if(playerItem.leftItems[190] > 0) {
			switch (Random.Range(0, 2)) 
			{case 0: playerItem.SpawnItem(0, false, 3, true); break;
			case 1: playerItem.SpawnItem(0, false, 7, true); break;}
			playerItem.SpawnItem(0, false, 0, false);
			playerItem.SpawnItem(0, false, 0, false);
			playerItem.leftItems[190]--; }
		if(playerItem.rightItems[190] > 0) {
			switch (Random.Range(0, 2))
			{case 0: playerItem.SpawnItem(0, false, 3, true); break;
			case 1: playerItem.SpawnItem(0, false, 7, true); break;}
			playerItem.SpawnItem(0, false, 0, false);
			playerItem.SpawnItem(0, false, 0, false);
			playerItem.rightItems[190]--;
		}
	}
	void WishSmite()
    {
		foreach(EnemyHealthManager ehm in gdm.activeEhms)
        {
			ehm.TakeDamage(1000 * (gunManager.leftDmg + gunManager.rightDmg), false, HitType.ht.special, ehm.transform.position, "self");
        }
    }
	void KillCounterExplosion(int killstreak)
	{
		int killCounter = playerItem.leftItems[194] + playerItem.rightItems[194];
		killStreakExplosion.Play();
		float fireWaveDmg = 1 + killstreak * ((gunManager.leftDmg + gunManager.rightDmg) / 2);
		float fireWaveRadias = 40 + (10 * killCounter) + (10 * killstreak) * ((gunManager.leftBulSize + gunManager.rightBulSize) / 2);
		foreach(EnemyHealthManager ehm in gdm.activeEhms)
        {
			if (Vector3.Distance(transform.position, ehm.transform.position) < fireWaveRadias)
            {
				ehm.TakeDamage(fireWaveDmg, false, HitType.ht.normal, ehm.transform.position, "self");
				ehm.GiveEffect(1, 1 + Mathf.CeilToInt((killstreak + killCounter)/2));
				ehm.fireWaveEffect.Play();
				if(ehm.data.enemyName == "Grenade") { ehm.gameObject.GetComponent<GrenadeBrain>().LightFuse(); }
            }
        }
	}
	public void TakeDamage(float damageTaken, bool wasFromExpGrowth, EnemyHealthManager sourceEHM, string sourceName, Transform sourcePos)
	{
		bool wasAtMax = (curHp == maxHp);
		float tempArmor = armor;
		float actualDamageTaken = damageTaken;
		if(ionParticle > 0 && playerItem.RandomItemEffectRoll(Random.Range(0f, 100f) < 0.5f * ionParticle))
        {
			float rand = Random.Range(10f, 1000f);
			damageTaken *= rand;
        }
        if (playerItem.leftItems[140] + playerItem.rightItems[140] > 0)
        {
			tempArmor *= (curHp / maxHp);
        }
		if (damageTaken <= 0)
		{
			//Heal
			curHp -= damageTaken;

			if (depleatedRock > 0) { GiveEffect(25, Mathf.RoundToInt((-damageTaken) / (2f / depleatedRock))); }
		}
		else
		{
			if (playerItem.RandomItemEffectRoll(Random.Range(1, 100) < evadeChance)) { return; }
			if (activeEffects[27].x>0){ damageTaken /= 2f; }
			//Damage
			if (sourceEHM != null) { lastHitMe = sourceEHM; }
			if (sourceName != null) { lastHitMeName = sourceName; }
            if (activeEffects[38].x>0 && Random.Range(0,5)==0)
            {
				damageTaken *= 2f; activeEffects[38] -= new Vector4(1, 0, 0, 0);
            }
            if (damageTaken <= tempArmor)
			{
				//armor has absorbed all damage but min dmg is 1
				curHp -= 1f;
				actualDamageTaken = 1f;
                OnDmgTaken(1, wasFromExpGrowth, sourceEHM, sourceName, sourcePos);
                if (depleatedRock > 0) { GiveEffect(24, 1); }
            }
			else
			{
				//return new hp with dmg reduced by armor
				curHp -= (damageTaken - tempArmor);
				actualDamageTaken = damageTaken - tempArmor;
                OnDmgTaken(damageTaken - tempArmor, wasFromExpGrowth, sourceEHM, sourceName, sourcePos);
				if (depleatedRock > 0) { GiveEffect(24, Mathf.RoundToInt((damageTaken - tempArmor) / (2f / depleatedRock))); }
            }

			//UnlockCheck
			gdm.damageTakenThisRoom += actualDamageTaken;

			regenTimer = 2f;

			if (expGrowth > 0 && (!wasFromExpGrowth || playerItem.RandomItemEffectRoll(Random.Range(0, 100) < 15)))
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
						ehm.QueStandardDamage(damageTaken * (1f / 4f));
					}
                    else
                    {
						ehm.QueStandardDamage(damageTaken * (1f / 8f));
					}

				}
            }

			if(playerItem.leftItems[124] + playerItem.rightItems[124] > 0)
            {
				for(int i = 0; i < (damageTaken * 100) / maxHp; i++)
                {
					GameObject spawnedFly = Instantiate(fly);
					spawnedFly.transform.position = transform.position + new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
					spawnedFly.transform.rotation = transform.rotation;
					spawnedFly.GetComponent<ZipMissle>().damage = (maxHp / 100f);
					spawnedFly.GetComponent<ZipMissle>().thrust *= Random.Range(2f, 4f);
                }
            }

			if(sourceEHM != null && (leftSpongeStone || rightSpongeStone))
            {
				sourceEHM.QueStandardDamage(armor / 4f);
            }

			if(pufferfish > 0)
            {
				sourceEHM.QueStandardDamage(armor * (1 + pufferfish));
            }
		}

		if (curHp != maxHp && wasAtMax && radioDome > 0)
		{
			TakeDamage(maxHp * (15f / 100f), false, null, "Radioactive Dome", null);
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
            }
			if (playerItem.rightItems[65] > 0)
			{
				playerItem.rightItems[65] -= 1;
				playerItem.rightItems[66] += 1;
			}
		}
	}
	void OnDmgTaken(float damageTaken, bool wasFromExpGrowth, EnemyHealthManager source, string sourceName, Transform sourcePos)
	{
		if (sourcePos != null) { uiMan.AddDangerSource(sourcePos, sourcePos.position, false, 1); }
		else if (source != null) { uiMan.AddDangerSource(source.transform, source.transform.position, false, 1); }
		uiMan.flash.Flash(1f - (curHp / maxHp));
		uiMan.healthGear.Flash(damageTaken/maxHp);
    }
	public void GiveEffect(int effectID, float stacksToAdd)
	{
        switch (effectID)
        {
            case 0:
                activeEffects[effectID] = new Vector4(activeEffects[effectID].x + stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type);
                float dotDmgModifer = 1; if (activeEffects[40].x > 0) { dotDmgModifer *= 2; }
                if (activeEffects[effectID].x % 5 == 0) { TakeDamage(activeEffects[effectID].x * 20f * dotDmgModifer, false, null, "Bleed", null); }
                break;
            case 2:
                if (activeEffects[effectID].x == 1) { activeEffects[effectID] = new Vector4(stacksToAdd * 2f, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); }
                else { activeEffects[effectID] = new Vector4(activeEffects[effectID].x + (stacksToAdd * 2f), allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); }
                break;

            case 17: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime * beltFed, allEffects[effectID].decayTime * beltFed, allEffects[effectID].type); break;  // belt fed magazine buff
			case 18: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime * activeReactor, allEffects[effectID].decayTime * activeReactor, allEffects[effectID].type); break;  // active reactor buff
			case 19: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  // Fast Fire partership buff
			case 20: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime + warcry, allEffects[effectID].decayTime + warcry, allEffects[effectID].type); break;  // warcry buff
			case 21: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  //Invaunerability
            case 22: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  //Invisibility (CIRCUS MASK SPESIFIC) (CHANGE THIS IF ADDING GENARIC) (enemies cannot see you)
            case 23: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  //Reload speed buff
            case 27: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  //Sunflower
            case 28: activeEffects[effectID] = new Vector4(stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;  //AK47 smoke gernade

            default: activeEffects[effectID] = new Vector4(activeEffects[effectID].x + stacksToAdd, allEffects[effectID].decayTime, allEffects[effectID].decayTime, allEffects[effectID].type); break;
        }
        //Effect max stack management
        if (activeEffects[2].x < 1) { activeEffects[2] = new Vector4(0, allEffects[2].decayTime, allEffects[2].decayTime, allEffects[2].type); }
        if (activeEffects[16].x > (numOfBunnies + 2)) { activeEffects[16] = new Vector4(numOfBunnies+2, allEffects[16].decayTime, allEffects[16].decayTime, allEffects[16].type); }
		if (activeEffects[18].x > 1) { activeEffects[18] = new Vector4(1, allEffects[18].decayTime * activeReactor, allEffects[18].decayTime * activeReactor, allEffects[18].type); }
		if (activeEffects[19].x > 1) { activeEffects[19] = new Vector4(1, allEffects[19].decayTime, allEffects[19].decayTime, allEffects[19].type); }
		if (activeEffects[23].x > 1) { activeEffects[23] = new Vector4(1, allEffects[23].decayTime, allEffects[23].decayTime, allEffects[23].type); }
		if (activeEffects[28].x > 1) { activeEffects[28] = new Vector4(1, allEffects[28].decayTime, allEffects[28].decayTime, allEffects[28].type); }
	}
	public int statusEffectsActive;
	void ManageEffects()
	{
		activeEffectIDS = new List<int>();
		activePosEffectIDS = new List<int>();
		activeNegEffectIDS = new List<int>();

		Vector4 q = new Vector4(0, 0, 0, 0);

		for (int i = 0; i < activeEffects.Count; i++)
		{
			q = activeEffects[i];

			if (i == 28) { uiMan.smokeBlindEffect.SetActive(q.x > 0); }

            //if there are any stacks of this effect
            if (q.x > 0)
			{
				statusEffectsActive++;
				activeEffectIDS.Add(i);
				if (q.w > 0) { activePosEffectIDS.Add(i); }
				else if (q.w < 0) { activeNegEffectIDS.Add(i); }

				//run effects that happen every frame
				switch (i)
				{
					case 21: curHp = maxHp; break;
					case 1: if (burnTimer > 0.2f) { burnTimer = 0; TakeDamage(1f, false, null, "Burn", null); } break;
				}

                //progress timer and remove stacks as needed, unless effect lasts forever
                if (allEffects[i].decayTime >= 0)
				{
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
                        switch (i)
                        {
                            case 2: TakeDamage((q.x + 1f) * 50f, false, null, "Radiation", null); break;
                            case 29: KillCounterExplosion((int)q.x+1); q.x = 0f; break;
                        }
                    }
                }
			}

			activeEffects[i] = q;
		}

	}
	void DisplayEffects()
	{
		uiMan.effectsText.text = "";

		for (int i = 0; i < activeEffects.Count; i++)
		{
			if (activeEffects[i].x > 0)
			{
				uiMan.effectsText.text = uiMan.effectsText.text + " <br>" + allEffects[i].displayName + "(" + activeEffects[i].x + ") (" + Mathf.Round(activeEffects[i].z) + ")";
			}
		}
	}

}
