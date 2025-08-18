using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lGunAmmoText;
    public TextMeshProUGUI rGunAmmoText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI effectsText; //   <---- Changed by heath manager script since it holds the effect info.
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI crosshair;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI gotchaText;//item 75 gotcha

    public GunManager gunManager;
    public HealthManager healthManager;
    public SettingsScript settings;

    public NEWPlayerMovement mvtScript;

    public GameObject damageText;
    public GameObject myCanvas;

    int fps = 0;

    public Camera mainCamera;
    public Camera overlayCamera;

    public GameObject playUI;
    public GameObject inventoryUI;
    public GameObject pauseUI;
    public GameObject deathUI;
    public GameObject bowchargeUI;

    public GameObject gunkyPng;
    public GameObject smokeBlindEffect;
    float initalGunkyPngPos;
    float gunkyCounter;

    public string state;
    public bool isPaused;

    public bool fishing;

    public List<GameObject> bossHealthBars;

    public GameObject ammoDisplayTextHolder;
    public GameObject ammoDisplaySimpleHolder;
    public GameObject ammoDisplayDetailHolder;

    public Image ammoDisplayLeftFill;
    public Image ammoDisplayRightFill;
    public Transform AmmoDisplayLeftFront; List<Image> adlfs = new List<Image>(); VerticalLayoutGroup lfVLG = null;
    public Transform AmmoDisplayLeftMiddle; List<Image> adlms = new List<Image>(); VerticalLayoutGroup lmVLG = null;
    public Transform AmmoDisplayLeftBack; List<Image> adlbs = new List<Image>(); VerticalLayoutGroup lbVLG = null;
    public Transform AmmoDisplayRightFront; List<Image> adrfs = new List<Image>(); VerticalLayoutGroup rfVLG = null;
    public Transform AmmoDisplayRightMiddle; List<Image> adrms = new List<Image>(); VerticalLayoutGroup rmVLG = null;
    public Transform AmmoDisplayRightBack; List<Image> adrbs = new List<Image>(); VerticalLayoutGroup rbVLG = null;
    public List<Sprite> bulletUISPRITES; int ammoDisplayType = 2;
    bool ammoListsBuilt;

    public TextMeshProUGUI intensityTxt; public GearUI difficultyGear;  // used by gdm
	public GearUI gearscript; // used by gdm
    public List<GameObject> diffImages;

    public TextMeshProUGUI mutationIDText;
    public TextMeshProUGUI timer;
    public List<int> fpsHistory = new List<int>();

    public List<Color> roomNumColors; 
    private void Start()
    {
        ammoListsBuilt = false;
        bossHealthBars.Clear();
        for(int i = 0; i < playUI.transform.GetChild(0).childCount; i++)
        {
            bossHealthBars.Add(playUI.transform.GetChild(0).GetChild(i).gameObject);
        }

        initalGunkyPngPos = gunkyPng.GetComponent<RectTransform>().position.y;

        gunManager = gameObject.GetComponent<GunManager>();

        isPaused = false;

        settings.Apply();

        ChangeState("play");
        ammoDisplayType = PlayerPrefs.GetInt("BULLETDISPLAYTYPE");
        ammoDisplayTextHolder.SetActive(true);
        switch (ammoDisplayType)
        {
            case 1: ammoDisplaySimpleHolder.SetActive(true); ammoDisplayDetailHolder.SetActive(false); break;
            case 2: ammoDisplayDetailHolder.SetActive(true); ammoDisplaySimpleHolder.SetActive(false); break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer.text = FormatTimeToTimer((int)healthManager.gdm.timeSpent);
        if (healthManager.dead) { deathUI.SetActive(true); }
        ManageInput();

        if (!fishing)
        {
            UpdatePlayUI();
            UpdateInventoryUI();
            UpdatePauseUI();
        }
        else
        {
            playUI.SetActive(false); inventoryUI.SetActive(false); pauseUI.SetActive(false); isPaused = false;
        }
    }
    string FormatTimeToTimer(int time)
    {
        string output = "";
        int seconds = time; int mins = seconds / 60; int hours = mins / 60;
        seconds -= mins * 60; mins -= hours * 60;
        string sOut = seconds.ToString(); string mOut = mins.ToString(); string hOut = hours.ToString();
        if (sOut.Length < 2) { sOut = "0" + sOut; } if (mOut.Length < 2) { mOut = "0" + mOut; }
        output = hOut + ":" + mOut + ":" + sOut; return output;
    }
    void ManageInput()
    {
        if (isPaused) {Time.timeScale = 0f; }
        else {Time.timeScale = 1f; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (fishing)
            {
                //fishing minigame handles it.
            }
            else
            {
                if (state == "play") { ChangeState("pause"); }
                else if (state == "inventory") { ChangeState("play"); }
                else if (state == "pause") { ChangeState("play"); }
            }
        }
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I)) && !fishing)
        {
            if (state == "play") { ChangeState("inventory"); }
            else if (state == "inventory") { ChangeState("play"); }
        }
    }

    public void ChangeState(string newState)
    {
        state = newState;
        if (state == "play") { playUI.SetActive(true); inventoryUI.SetActive(false); pauseUI.SetActive(false); isPaused = false; Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        if (state == "inventory") { playUI.SetActive(false); inventoryUI.SetActive(true); pauseUI.SetActive(false); isPaused = false; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        if (state == "pause") { playUI.SetActive(false); inventoryUI.SetActive(false); pauseUI.SetActive(true); isPaused = true; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

        if (state == "inventory")
        {
            inventoryUI.GetComponent<InventoryScript>().UpdateInventory();
        }
    }
    public List<int> RequestFPSInfo() { return fpsHistory; }
    void UpdatePlayUI()
    {
        if (mvtScript.isSprinting || mvtScript.slamming || mvtScript.sliding) { crosshair.text = "^"; }
        else { crosshair.text = "+"; }

        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsHistory.Add(fps); if (fpsHistory.Count > 240) { fpsHistory.RemoveAt(0); }
        fpsText.text = "FPS: " + fps;

        moneyText.text = healthManager.money + "$";
        if(healthManager.playerItem.gotchaTickets > 0) { gotchaText.text = healthManager.playerItem.gotchaTickets.ToString(); gotchaText.transform.parent.gameObject.SetActive(true); } else { gotchaText.transform.parent.gameObject.SetActive(false); }
        
        BulletSpriteVisuals();
        healthText.text = Mathf.Round(healthManager.curHp) + " / " + Mathf.Round(healthManager.maxHp);

        bowchargeUI.SetActive(gunManager.leftBowAct + gunManager.rightBowAct > 0);

        DifficultyVisualsUpdate();
    }
    void DifficultyVisualsUpdate()
    {
        int diffID = healthManager.gdm.difficultyIDSelected;
        diffImages[diffID].SetActive(true);
        string newTxt = ((int)(healthManager.gdm.unroundedDiff * 100f)).ToString(); newTxt.Insert(newTxt.Length - 2, ".");
        intensityTxt.text = newTxt;
        int txtColorId = healthManager.gdm.roomsUntilBoss; if(txtColorId > roomNumColors.Count) { txtColorId = roomNumColors.Count; }
        gearscript.txt.color = roomNumColors[txtColorId-1];
    }
    void BulletSpriteVisuals()
    {

        ammoDisplayTextHolder.SetActive(true);
        switch (ammoDisplayType)
        {
            case 0: ammoDisplayDetailHolder.SetActive(false); ammoDisplaySimpleHolder.SetActive(false); break;
            case 1: ammoDisplaySimpleHolder.SetActive(true); ammoDisplayDetailHolder.SetActive(false); break;
            case 2: ammoDisplayDetailHolder.SetActive(true); ammoDisplaySimpleHolder.SetActive(false); break;
        }
        lGunAmmoText.text = gunManager.leftGunScript.currentBullets + " / " + gunManager.leftGunScript.magSize;
        rGunAmmoText.text = gunManager.rightGunScript.currentBullets + " / " + gunManager.rightGunScript.magSize;
        switch (ammoDisplayType)
        {
            case 0: break;
            case 1:
                ammoDisplayLeftFill.fillAmount = (float)gunManager.leftGunScript.currentBullets/gunManager.leftGunScript.magSize;
                ammoDisplayRightFill.fillAmount = (float)gunManager.rightGunScript.currentBullets / gunManager.rightGunScript.magSize;
                break;
            case 2:
                if (!ammoListsBuilt)
                {
                    ammoListsBuilt = true; adlfs = new List<Image>(); adlms = new List<Image>(); adlbs = new List<Image>(); adrfs = new List<Image>(); adrms = new List<Image>(); adrbs = new List<Image>();
                    foreach (Image img in AmmoDisplayLeftFront.GetComponentsInChildren<Image>()) { adlfs.Add(img); }
                    foreach (Image img in AmmoDisplayLeftMiddle.GetComponentsInChildren<Image>()) { adlms.Add(img); }
                    foreach (Image img in AmmoDisplayLeftBack.GetComponentsInChildren<Image>()) { adlbs.Add(img); }
                    foreach (Image img in AmmoDisplayRightFront.GetComponentsInChildren<Image>()) { adrfs.Add(img); }
                    foreach (Image img in AmmoDisplayRightMiddle.GetComponentsInChildren<Image>()) { adrms.Add(img); }
                    foreach (Image img in AmmoDisplayRightBack.GetComponentsInChildren<Image>()) { adrbs.Add(img); }
                    lfVLG = AmmoDisplayLeftFront.GetComponent<VerticalLayoutGroup>(); rfVLG = AmmoDisplayRightFront.GetComponent<VerticalLayoutGroup>();
                    lmVLG = AmmoDisplayLeftMiddle.GetComponent<VerticalLayoutGroup>(); rmVLG = AmmoDisplayRightMiddle.GetComponent<VerticalLayoutGroup>();
                    lbVLG = AmmoDisplayLeftBack.GetComponent<VerticalLayoutGroup>(); rbVLG = AmmoDisplayRightBack.GetComponent<VerticalLayoutGroup>();
                }
                int leftMS = Mathf.CeilToInt(gunManager.leftGunScript.magSize); int leftCB = gunManager.leftGunScript.currentBullets;
                int rightMS = Mathf.CeilToInt(gunManager.rightGunScript.magSize); int rightCB = gunManager.rightGunScript.currentBullets;

                //Change Sprite
                Sprite leftSprite = bulletUISPRITES[0];
                Sprite rightSprite = bulletUISPRITES[0];
                switch (gunManager.leftGunScript.bulletType)
                {
                    case GunScript.BulletType.standard:
                        switch (gunManager.leftGunScript.gunType)
                        {
                            case GunObjectData.GunType.AeroRifle: leftSprite = bulletUISPRITES[2]; break;
                            case GunObjectData.GunType.Vector3: leftSprite = bulletUISPRITES[2]; break;
                            case GunObjectData.GunType.BulkFedDoubleBarrel: leftSprite = bulletUISPRITES[1]; break;
                            case GunObjectData.GunType.Crossbow: leftSprite = bulletUISPRITES[3]; break;
                            case GunObjectData.GunType.ArcherFish: leftSprite = bulletUISPRITES[5]; break;
                            case GunObjectData.GunType.HandCannon: leftSprite = bulletUISPRITES[4]; break;
                            case GunObjectData.GunType.MutatedKnife: leftSprite = bulletUISPRITES[8]; break;
                            default: leftSprite = bulletUISPRITES[0]; break;
                        }
                        break;
                    case GunScript.BulletType.nerf: leftSprite = bulletUISPRITES[6]; break;
                    case GunScript.BulletType.oil: leftSprite = bulletUISPRITES[7]; break;
                }
                switch (gunManager.rightGunScript.bulletType)
                {
                    case GunScript.BulletType.standard:
                        switch (gunManager.rightGunScript.gunType)
                        {
                            case GunObjectData.GunType.AeroRifle: rightSprite = bulletUISPRITES[2]; break;
                            case GunObjectData.GunType.Vector3: rightSprite = bulletUISPRITES[2]; break;
                            case GunObjectData.GunType.BulkFedDoubleBarrel: rightSprite = bulletUISPRITES[1]; break;
                            case GunObjectData.GunType.Crossbow: rightSprite = bulletUISPRITES[3]; break;
                            case GunObjectData.GunType.ArcherFish: rightSprite = bulletUISPRITES[5]; break;
                            case GunObjectData.GunType.HandCannon: rightSprite = bulletUISPRITES[4]; break;
                            case GunObjectData.GunType.MutatedKnife: rightSprite = bulletUISPRITES[8]; break;
                            default: rightSprite = bulletUISPRITES[0]; break;
                        }
                        break;
                    case GunScript.BulletType.nerf: rightSprite = bulletUISPRITES[6]; break;
                    case GunScript.BulletType.oil: rightSprite = bulletUISPRITES[7]; break;
                }

                //Adjust spacing
                if (leftMS <= 6) { lfVLG.spacing = 3; }
                else if (leftMS <= 20) { lfVLG.spacing = Mathf.Lerp(3, 0, ((float)leftMS / 20f)); }
                else if (leftMS <= 40) { lfVLG.spacing = Mathf.Lerp(0, -8, ((float)leftMS / 40f)); }
                lmVLG.spacing = lfVLG.spacing; lbVLG.spacing = lfVLG.spacing;
                if (rightMS <= 6) { rfVLG.spacing = 3; }
                else if (rightMS <= 20) { rfVLG.spacing = Mathf.Lerp(3, 0, ((float)rightMS / 20f)); }
                else if (rightMS <= 40) { rfVLG.spacing = Mathf.Lerp(0, -8, ((float)rightMS / 40f)); }
                rmVLG.spacing = rfVLG.spacing; rbVLG.spacing = rfVLG.spacing;

                //Update Display
                Color avaliableCol = new Color(1, 1, 1, 0.5f);
                Color unavaliableCol = new Color(0.25f, 0.25f, 0.25f, 0.2f);
                for (int i = 0; i < adlfs.Count; i++)
                {
                    int repBul = i;
                    if (repBul < leftCB)
                    {
                        adlfs[repBul].gameObject.SetActive(true);
                        adlfs[repBul].sprite = leftSprite;
                        adlfs[repBul].color = avaliableCol;
                    }
                    else if (repBul < leftMS)
                    {
                        adlfs[repBul].gameObject.SetActive(true);
                        adlfs[repBul].sprite = leftSprite;
                        adlfs[repBul].color = unavaliableCol;
                    }
                    else { adlfs[repBul].gameObject.SetActive(false); }
                    repBul += 40;
                    if (repBul < leftCB)
                    {
                        adlms[repBul - 40].gameObject.SetActive(true);
                        adlms[repBul - 40].sprite = leftSprite;
                        adlms[repBul - 40].color = avaliableCol;
                    }
                    else if (repBul < leftMS)
                    {
                        adlms[repBul - 40].gameObject.SetActive(true);
                        adlms[repBul - 40].sprite = leftSprite;
                        adlms[repBul - 40].color = unavaliableCol;
                    }
                    else { adlms[repBul - 40].gameObject.SetActive(false); }
                    repBul += 40;
                    if (repBul < leftCB)
                    {
                        adlbs[repBul - 80].gameObject.SetActive(true);
                        adlbs[repBul = 80].sprite = leftSprite;
                        adlbs[repBul - 80].color = avaliableCol;
                    }
                    else if (repBul < leftMS)
                    {
                        adlbs[repBul - 80].gameObject.SetActive(true);
                        adlbs[repBul - 80].sprite = leftSprite;
                        adlbs[repBul - 80].color = unavaliableCol;
                    }
                    else { adlbs[repBul - 80].gameObject.SetActive(false); }
                }
                for (int i = 0; i < adrfs.Count; i++)
                {
                    int repBul = i;
                    if (repBul < rightCB)
                    {
                        adrfs[repBul].gameObject.SetActive(true);
                        adrfs[repBul].sprite = rightSprite;
                        adrfs[repBul].color = avaliableCol;
                    }
                    else if (repBul < rightMS)
                    {
                        adrfs[repBul].gameObject.SetActive(true);
                        adrfs[repBul].sprite = rightSprite;
                        adrfs[repBul].color = unavaliableCol;
                    }
                    else { adrfs[repBul].gameObject.SetActive(false); }
                    repBul += 40;
                    if (repBul < rightCB)
                    {
                        adrms[repBul - 40].gameObject.SetActive(true);
                        adrms[repBul - 40].sprite = rightSprite;
                        adrms[repBul - 40].color = avaliableCol;
                    }
                    else if (repBul < rightMS)
                    {
                        adrms[repBul - 40].gameObject.SetActive(true);
                        adrms[repBul - 40].sprite = rightSprite;
                        adrms[repBul - 40].color = unavaliableCol;
                    }
                    else { adrms[repBul - 40].gameObject.SetActive(false); }
                    repBul += 40;
                    if (repBul < rightCB)
                    {
                        adrbs[repBul - 80].gameObject.SetActive(true);
                        adrbs[repBul - 80].sprite = rightSprite;
                        adrbs[repBul - 80].color = avaliableCol;
                    }
                    else if (repBul < rightMS)
                    {
                        adrbs[repBul - 80].gameObject.SetActive(true);
                        adrbs[repBul - 80].sprite = rightSprite;
                        adrbs[repBul - 80].color = unavaliableCol;
                    }
                    else { adrbs[repBul - 80].gameObject.SetActive(false); }
                }
                break;
        }
    }
    void UpdateInventoryUI()
    {
       
    }
    void UpdatePauseUI()
    {
        ammoDisplayType = PlayerPrefs.GetInt("BULLETDISPLAYTYPE");
        int diffID = healthManager.gdm.difficultyIDSelected;
        if(diffID == 4)
        {
            List<int> mutatedRules = healthManager.gdm.mutatedRules;
            mutationIDText.transform.parent.gameObject.SetActive(true);
            mutationIDText.text = mutatedRules[0] + "|" + mutatedRules[1] + "|" + mutatedRules[2] + "|" + mutatedRules[3] + "|" + mutatedRules[4] + "|" + mutatedRules[5];
        } else { mutationIDText.transform.parent.gameObject.SetActive(false); }
    }

    public void VisionOfGunky()
    {
        gunkyCounter = 0;
        StartCoroutine(GunkyFade());
    }
    IEnumerator GunkyFade()
    {
        gunkyPng.SetActive(true);
        while(gunkyCounter < 1f)
        {
            gunkyCounter += Time.deltaTime / 4f;
            gunkyPng.GetComponent<Image>().color = new Color(1, 1, 1, gunkyCounter);
            gunkyPng.transform.position = new Vector3(gunkyPng.transform.position.x, initalGunkyPngPos - (initalGunkyPngPos*1.5f)*gunkyCounter, 0f);
            yield return null;
        }
        gunkyPng.SetActive(false);
        yield return null;
    }
}
