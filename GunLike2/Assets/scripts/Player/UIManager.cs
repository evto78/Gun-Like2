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
    //public TextMeshProUGUI velocityText;

    public HitScanGunScript revScript;
    public GunScript gunScript;
    public HealthManager healthManager;

    public GameObject damageText;
    public GameObject myCanvas;

    int fps = 0;

    public Camera mainCamera;
    public Camera overlayCamera;

    private GameObject playUI;
    private GameObject inventoryUI;
    private GameObject pauseUI;

    public string state;

    private void Start()
    {
        playUI = GameObject.Find("Play UI");
        inventoryUI = GameObject.Find("Inventory UI");
        pauseUI = GameObject.Find("Pause UI");

        ChangeState("play");
    }

    // Update is called once per frame
    void Update()
    {
        ManageInput();

        UpdatePlayUI();
        UpdateInventoryUI();
        UpdatePauseUI();
    }

    void ManageInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(state == "play") { ChangeState("pause"); }
            else if(state == "inventory") { ChangeState("play"); }
            else if(state == "pause") { ChangeState("play"); }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (state == "play") { ChangeState("inventory"); }
            else if (state == "inventory") { ChangeState("play"); }
        }
    }

    public void ChangeState(string newState)
    {
        state = newState;
        if (state == "play") { playUI.SetActive(true); inventoryUI.SetActive(false); pauseUI.SetActive(false); }
        if (state == "inventory") { playUI.SetActive(false); inventoryUI.SetActive(true); pauseUI.SetActive(false); }
        if (state == "pause") { playUI.SetActive(false); inventoryUI.SetActive(false); pauseUI.SetActive(true); }
    }

    void UpdatePlayUI()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        fpsText.text = "FPS: " + fps;

        lGunAmmoText.text = revScript.currentBullets + " / " + revScript.magSize;
        rGunAmmoText.text = gunScript.currentBullets + " / " + gunScript.magSize;
        healthText.text = Mathf.Round(healthManager.curHp) + " / " + Mathf.Round(healthManager.maxHp);
    }
    void UpdateInventoryUI()
    {

    }
    void UpdatePauseUI()
    {

    }
}
