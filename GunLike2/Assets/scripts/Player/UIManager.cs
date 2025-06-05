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

    public GameObject gunkyPng;
    float initalGunkyPngPos;
    float gunkyCounter;

    public string state;
    public bool isPaused;

    public bool fishing;

    private void Start()
    {
        initalGunkyPngPos = gunkyPng.GetComponent<RectTransform>().position.y;

        gunManager = gameObject.GetComponent<GunManager>();

        isPaused = false;

        ChangeState("play");
    }

    // Update is called once per frame
    void Update()
    {
        if (healthManager.dead) { deathUI.SetActive(true); }
        ManageInput();

        UpdatePlayUI();
        UpdateInventoryUI();
        UpdatePauseUI();
    }

    void ManageInput()
    {
        if (isPaused) {Time.timeScale = 0f; }
        else {Time.timeScale = 1f; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (fishing)
            {

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

    void UpdatePlayUI()
    {
        if (mvtScript.isSprinting || mvtScript.slamming || mvtScript.sliding) { crosshair.text = "^"; }
        else { crosshair.text = "+"; }

        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = "FPS: " + fps;

        moneyText.text = healthManager.money + "$";
        if(healthManager.playerItem.gotchaTickets > 0) { gotchaText.text = healthManager.playerItem.gotchaTickets.ToString(); gotchaText.gameObject.SetActive(true); } else { gotchaText.gameObject.SetActive(false); }

        lGunAmmoText.text = gunManager.leftHand.GetComponentInChildren<GunScript>().currentBullets + " / " + gunManager.leftHand.GetComponentInChildren<GunScript>().magSize;
        rGunAmmoText.text = gunManager.rightHand.GetComponentInChildren<GunScript>().currentBullets + " / " + gunManager.rightHand.GetComponentInChildren<GunScript>().magSize;
        healthText.text = Mathf.Round(healthManager.curHp) + " / " + Mathf.Round(healthManager.maxHp);
    }
    void UpdateInventoryUI()
    {
       
    }
    void UpdatePauseUI()
    {
        
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
