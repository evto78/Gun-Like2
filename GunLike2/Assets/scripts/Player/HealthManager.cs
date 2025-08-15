using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
	public GearUI gearscript; // used by gdm
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

	public int appleBuff;
	public float fortifyBuff;
	public float sunflowerDebuff;

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

	public GameDataManager gdm;
	public bool freeRelaxedRevive;
    // Start is called before the first frame update
    private void Awake()
    {
		gunManager = gameObject.GetComponent<GunManager>();
		lsm = gameObject.GetComponent<LocalSoundManager>();
	}
    void Start()
	{
		gdm = GameObject.FindGameObjectWithTag("gdm").GetComponent<GameDataManager>();
		money = 0;
		dead = false;
		maxHp = baseMaxHP;
		curHp = maxHp;
	}
	public void StatUpdate(List<int> givenLeftItems, List<int> givenRightItems, List<List<int>> givenRarityList)
	{
		rarityList = givenRarityList;
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

		itemChecks();
		ManageEffects();
		DisplayEffects();

		if(curHp <= 0 && activeEffects[21].x < 1) 
		{ 
			if(playerItem.leftItems[155] > 0)//Divine Intervention
            {
				GiveEffect(PlayerEffectType.effectName.invaunerabiility, 1f);
				curHp = maxHp;
				divineTimer = 60f + (60f/divineInter);
				if(lastHitMe != null) { lastHitMe.TakeDamage(maxHp, false, HitType.ht.normal, lastHitMe.transform.position, "player"); }
            }
			else if (playerItem.rightItems[155] > 0)
            {
				GiveEffect(PlayerEffectType.effectName.invaunerabiility, 1f);
				curHp = maxHp;
				divineTimer = 60f + (60f/divineInter);
				if (lastHitMe != null) { lastHitMe.TakeDamage(maxHp, false, HitType.ht.normal, lastHitMe.transform.position, "player"); }
			}
			else if (playerItem.leftItems[116] > 0)//Another Shot
			{
				GiveEffect(PlayerEffectType.effectName.invaunerabiility, 1f);
				curHp = maxHp;
				playerItem.leftItems[116]--;
			}
			else if (playerItem.rightItems[116] > 0)
			{
				GiveEffect(PlayerEffectType.effectName.invaunerabiility, 1f);
				curHp = maxHp;
				playerItem.rightItems[116]--;
			}
			else if (freeRelaxedRevive)
            {
				GiveEffect(PlayerEffectType.effectName.invaunerabiility, 3f);
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
		timeSinceEnemyDied = 0;

		if(activeReactor > 0)
        {
			GiveEffect(PlayerEffectType.effectName.activeReactor, 1);
        }
		if(gdm.difficultyIDSelected == 0){moneyDropped *= 2;} 

		money += moneyDropped; money += 10 * (playerItem.leftItems[177] + playerItem.rightItems[177]);

		if(enemyThatDied.activeEffects[8].x > 0)
        {
			for(int i = 0; i < enemyThatDied.activeEffects[8].x; i++)
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
    }

	void itemChecks()
	{
		if (orgGum > 0)
		{
			orgGumTimer -= Time.deltaTime + (Time.deltaTime * clockwork);
			if (orgGumTimer <= 0f)
			{
				orgGumTimer = playerItem.FindObjByID(17).baseCooldown;

				if (Random.Range(1, 100) > (20 - (2f * orgGum)))
				{
					int rand = Random.Range(3, 11);
					if (rand == 3) { GiveEffect(PlayerEffectType.effectName.orgBannana, 1f); }
					if (rand == 4) { GiveEffect(PlayerEffectType.effectName.orgApple, 1f); }
					if (rand == 5) { GiveEffect(PlayerEffectType.effectName.orgBerry, 1f); }
					if (rand == 6) { GiveEffect(PlayerEffectType.effectName.orgChoco, 1f); }
					if (rand == 7) { GiveEffect(PlayerEffectType.effectName.orgLemon, 1f); }
					if (rand == 8) { GiveEffect(PlayerEffectType.effectName.orgLime, 1f); }
					if (rand == 9) { GiveEffect(PlayerEffectType.effectName.orgGrape, 1f); }
					if (rand == 10) { GiveEffect(PlayerEffectType.effectName.orgMint, 1f); }
					if (rand == 11) { GiveEffect(PlayerEffectType.effectName.orgSpicy, 1f); }
				}
				else
				{
					int rand = Random.Range(12, 15);
					if (rand == 12) { GiveEffect(PlayerEffectType.effectName.orgRedMeat, 1f); }
					if (rand == 13) { GiveEffect(PlayerEffectType.effectName.orgWhiteMeat, 1f); }
					if (rand == 14) { GiveEffect(PlayerEffectType.effectName.orgPinkMeat, 1f); }
					if (rand == 15) { GiveEffect(PlayerEffectType.effectName.orgGrayMeat, 1f); }
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

				if(Random.Range(1,100) > (53 - experimentalImp * 3))
                {
					TakeDamage(-1f * healthRegen, false, null);
                }
                else
                {
					if(curHp > 0.5f * healthRegen)
                    {
						TakeDamage(0.5f * healthRegen, false, null);
					}
                }
            }
        }

		if(playerItem.leftItems[43] + playerItem.rightItems[43] > 0)
        {
			stichedEnemies.Clear();
			foreach (EnemyHealthManager ehm in gdm.activeEhms)
			{
				if (ehm.activeEffects[5].x > 0f)
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
			if (markedEnemy == null && canineToothTimer <= 0f)
            {
				markedEnemy = gdm.activeEhms[Random.Range(0, gdm.activeEhms.Count)];
				markedEnemy.GiveEffect("marked", 1f);
            }
			if (markedEnemy != null && markedEnemy.activeEffects[11].x < 1 && canineToothTimer <= 0)
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
            if (!attackedThisRoom && activeEffects[22].x < 2) { GiveEffect(PlayerEffectType.effectName.invisibility, 1); }
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
					case 1: TakeDamage(-maxHp, false, null); GiveEffect(PlayerEffectType.effectName.invaunerabiility, 3f); wishPopUp.ready = false; break; //Heal
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
	public void TakeDamage(float damageTaken, bool wasFromExpGrowth, EnemyHealthManager source)
	{
		bool wasAtMax = (curHp == maxHp);
		float tempArmor = armor;
		if(ionParticle > 0 && Random.Range(0f, 100f) < 0.5f * ionParticle)
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

			if (depleatedRock > 0) { GiveEffect(PlayerEffectType.effectName.depleatedRockDebuff, Mathf.RoundToInt((-damageTaken) / (2f / depleatedRock))); }
		}
		else
		{
			if (Random.Range(1, 100) < evadeChance) { return; }
			if(activeEffects[27].x>0){ damageTaken /= 2f; }
			//Damage
			if (source != null) { lastHitMe = source; }
			if (damageTaken <= tempArmor)
			{
				//armor has absorbed all damage but min dmg is 1
				curHp -= 1f;
				if (depleatedRock > 0) { GiveEffect(PlayerEffectType.effectName.depleatedRockBuff, 1); }
			}
			else
			{
				//return new hp with dmg reduced by armor
				curHp -= (damageTaken - tempArmor);
				if (depleatedRock > 0) { GiveEffect(PlayerEffectType.effectName.depleatedRockBuff, Mathf.RoundToInt((damageTaken - tempArmor) / (2f / depleatedRock))); }
			}
			regenTimer = 2f;

			if (expGrowth > 0 && (!wasFromExpGrowth || Random.Range(0, 100) < 15))
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

			if(source != null && (leftSpongeStone || rightSpongeStone))
            {
				source.QueStandardDamage(armor / 4f);
            }

			if(pufferfish > 0)
            {
				source.QueStandardDamage(armor * (1 + pufferfish));
            }
		}

		if (curHp != maxHp && wasAtMax && radioDome > 0)
		{
			TakeDamage(maxHp * (15f / 100f), false, null);
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
	public void GiveEffect(PlayerEffectType.effectName effectGiven, float stacksToAdd)
	{
        switch (effectGiven)
        {
			//damage over time
			case PlayerEffectType.effectName.bleed: activeEffects[0] = new Vector4(activeEffects[0].x + stacksToAdd, 3f, 3f, -1f); break;
			case PlayerEffectType.effectName.burn: activeEffects[1] = new Vector4(activeEffects[1].x + stacksToAdd, 2f, 2f, -1f); break;
			case PlayerEffectType.effectName.radiation: activeEffects[2] = new Vector4(activeEffects[2].x + stacksToAdd, 6f, 6f, -1f); break;
			//item effects
			case PlayerEffectType.effectName.orgBannana: activeEffects[3] = new Vector4(activeEffects[3].x + stacksToAdd, 20f, 20f, 1f); break; // reload speed buff
			case PlayerEffectType.effectName.orgApple: activeEffects[4] = new Vector4(activeEffects[4].x + stacksToAdd, 20f, 20f, 1f); break; // crit chance buff
			case PlayerEffectType.effectName.orgBerry: activeEffects[5] = new Vector4(activeEffects[5].x + stacksToAdd, 20f, 20f, 1f); break; // weak point damage buff
			case PlayerEffectType.effectName.orgChoco: activeEffects[6] = new Vector4(activeEffects[6].x + stacksToAdd, 20f, 20f, 1f); break;// armor buff
			case PlayerEffectType.effectName.orgLemon: activeEffects[7] = new Vector4(activeEffects[7].x + stacksToAdd, 20f, 20f, 1f); break; // atk spd buff
			case PlayerEffectType.effectName.orgLime: activeEffects[8] = new Vector4(activeEffects[8].x + stacksToAdd, 20f, 20f, 1f); break;// regen buff
			case PlayerEffectType.effectName.orgGrape: activeEffects[9] = new Vector4(activeEffects[9].x + stacksToAdd, 20f, 20f, 1f); break;// jump height buff
			case PlayerEffectType.effectName.orgMint: activeEffects[10] = new Vector4(activeEffects[10].x + stacksToAdd, 20f, 20f, 1f); break; // move spd buff
			case PlayerEffectType.effectName.orgSpicy: activeEffects[11] = new Vector4(activeEffects[11].x + stacksToAdd, 20f, 20f, 1f); break;  // dmg buff
			case PlayerEffectType.effectName.orgRedMeat: activeEffects[12] = new Vector4(activeEffects[12].x + stacksToAdd, 20f, 20f, -1f); break;  // dmg debuff
			case PlayerEffectType.effectName.orgWhiteMeat: activeEffects[13] = new Vector4(activeEffects[13].x + stacksToAdd, 20f, 20f, -1f); break;  // atk spd debuff
			case PlayerEffectType.effectName.orgPinkMeat: activeEffects[14] = new Vector4(activeEffects[14].x + stacksToAdd, 20f, 20f, -1f); break;  // regen debuff
			case PlayerEffectType.effectName.orgGrayMeat: activeEffects[15] = new Vector4(activeEffects[15].x + stacksToAdd, 20f, 20f, -1f); break;  // sprint speed debuff
			case PlayerEffectType.effectName.bunnyHop: activeEffects[16] = new Vector4(activeEffects[16].x + stacksToAdd, 1f, 1f, 1f); break;  // irradiated bunny slippers buff
			case PlayerEffectType.effectName.pantsFalling: activeEffects[17] = new Vector4(stacksToAdd, 0.1f * beltFed, 0.1f * beltFed, 1f); break;  // belt fed magazine buff
			case PlayerEffectType.effectName.activeReactor: activeEffects[18] = new Vector4(stacksToAdd, activeReactor * 5f, activeReactor * 5f, 1f); break;  // active reactor buff
			case PlayerEffectType.effectName.fastFire: activeEffects[19] = new Vector4(stacksToAdd, 1f, 1f, 1f); break;  // Fast Fire partership buff
			case PlayerEffectType.effectName.warcry: activeEffects[20] = new Vector4(stacksToAdd, 1f + warcry, 1f + warcry, 1f); break;  // warcrybuff
			case PlayerEffectType.effectName.invaunerabiility: activeEffects[21] = new Vector4(stacksToAdd, 5f, 5f, 1f); break; //Invaunerability
			case PlayerEffectType.effectName.invisibility: activeEffects[22] = new Vector4(stacksToAdd, 1f, 1f, 1f); break; //Invisibility (CIRCUS MASK SPESIFIC) (CHANGE THIS IF ADDING GENARIC) (enemies cannot see you)
			case PlayerEffectType.effectName.smokingGun: activeEffects[23] = new Vector4(stacksToAdd, float.PositiveInfinity, float.PositiveInfinity, 1f); break; //Reload speed buff
			case PlayerEffectType.effectName.depleatedRockBuff: activeEffects[24] = new Vector4(activeEffects[24].x + stacksToAdd, 0.5f, 0.5f, 1f); break; //Depleated Rock BUFF
			case PlayerEffectType.effectName.depleatedRockDebuff: activeEffects[25] = new Vector4(activeEffects[25].x + stacksToAdd, 0.5f, 0.5f, -1f); break; //Deplaated Rock DEBUFF
			case PlayerEffectType.effectName.chaosEngine: activeEffects[26] = new Vector4(activeEffects[26].x + stacksToAdd, 3f, 3f, 1f); break; //Chaos Engine
			case PlayerEffectType.effectName.sunny: activeEffects[27] = new Vector4(stacksToAdd, 1f, 1f, 1f); break; //Sunflower
			case PlayerEffectType.effectName.smokeBlind: activeEffects[28] = new Vector4(stacksToAdd, 0.1f, 0.1f, -1f); break; //AK47 smoke gernade
		}
		//Effect max stack management
		if (activeEffects[16].x > (numOfBunnies + 2)) { activeEffects[16] = new Vector4(numOfBunnies+2f, 1f, 1f, 1f); }
		if (activeEffects[18].x > 1) { activeEffects[18] = new Vector4(1, activeReactor * 5f, activeReactor * 5f, 1f); }
		if (activeEffects[19].x > 1) { activeEffects[19] = new Vector4(1, 1f, 1f, 1f); }
		if (activeEffects[23].x > 1) { activeEffects[23] = new Vector4(1, 5f, 5f, 1f); }
		if (activeEffects[28].x > 1) { activeEffects[28] = new Vector4(1, 0.1f, 0.1f, -1f); }
	}
	void ManageEffects()
	{
		Vector4 q = new Vector4(0, 0, 0, 0);

		for (int i = 0; i < activeEffects.Count; i++)
		{
			q = activeEffects[i];

			if (i == 28) { uiMan.smokeBlindEffect.SetActive(q.x > 0); }

			//if there are any stacks of this effect
			if (q.x > 0)
			{
				//run effects that happen every frame
				if (i == 21) { curHp = maxHp; }

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
					if (i == 0 || i == 1 || i == 2) { TakeDamage((q.x + 1f)*5f, false, null); }
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
                switch (i)
                {
					case 0: strToAdd = "bleed";break;
					case 1: strToAdd = "burn";break;
					case 2: strToAdd = "radiation";break;
					case 3: strToAdd = "organic bannana";break;
					case 4: strToAdd = "organic apple";break;
					case 5: strToAdd = "organic berry";break;
					case 6: strToAdd = "organic choco"; break;
					case 7: strToAdd = "organic lemon"; break;
					case 8: strToAdd = "organic lime"; break;
					case 9: strToAdd = "organic grape"; break;
					case 10: strToAdd = "organic mint"; break;
					case 11: strToAdd = "organic spicy"; break;
					case 12: strToAdd = "organic red meat"; break;
					case 13: strToAdd = "organic white meat"; break;
					case 14: strToAdd = "organic pink meat"; break;
					case 15: strToAdd = "organic gray meat"; break;
					case 16: strToAdd = "bunny hop"; break;
					case 17: strToAdd = "pants falling"; break;
					case 18: strToAdd = "active reactor"; break;
					case 19: strToAdd = "fast fire"; break;
					case 20: strToAdd = "warcry"; break;
					case 21: strToAdd = "invaunerability"; break;
					case 22: strToAdd = "invisible"; break;
					case 23: strToAdd = "smoking gun"; break;
					case 24: strToAdd = "depleated rock buff"; break;
					case 25: strToAdd = "depleated rock debuff"; break;
					case 26: strToAdd = "chaos engine"; break;
					case 27: strToAdd = "sunny"; break;
					case 28: strToAdd = "Smoked"; break;
				}
				uiMan.effectsText.text = uiMan.effectsText.text + " <br>" + strToAdd + "(" + activeEffects[i].x + ") (" + Mathf.Round(activeEffects[i].z) + ")";
			}
		}
	}

}
