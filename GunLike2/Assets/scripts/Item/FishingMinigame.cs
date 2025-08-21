using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    bool playerNear;
    bool playerFishing;
    GameObject player;
    UIManager uiman;
    GunManager gm;
    GameDataManager gdm;
    public Animator tabletHolder;
    public GameObject tablet;
    float tabletAnimTimer;
    public GameObject tabletScreen;
    public Texture2D fishingCursor;
    public bool timesUp;
    bool fished;
    private void Start()
    {
        fished = false;
        player = GameObject.Find("Player");
        uiman = player.GetComponent<UIManager>();
        gm = player.GetComponent<GunManager>();
        gdm = gm.healthMan.gdm;
        playerFishing = false;
        tabletScreen.SetActive(false);
    }
    private void Update()
    {
        tabletAnimTimer -= Time.deltaTime;
        if (playerNear && gm.playerItem.leftItems[83] + gm.playerItem.rightItems[83] > 0)
        {
            if (!playerFishing && (Input.GetKeyDown(gdm.controlsBinds.leftInteract) || Input.GetKeyDown(gdm.controlsBinds.righInteract)) && fished == false)
            {
                playerFishing = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                uiman.fishing = true;
                player.transform.position = transform.position;
                Camera.main.transform.LookAt(tablet.transform);
                gm.leftGunScript.gameObject.SetActive(false);
                gm.rightGunScript.gameObject.SetActive(false);
                tabletAnimTimer = 1.2f;
                Cursor.SetCursor(fishingCursor, new Vector2(32f, 32f), CursorMode.ForceSoftware);
                timesUp = false;
                tabletScreen.SetActive(true); tabletScreen.GetComponent<FishMinigameGAME>().playing = false;
                tabletScreen.SetActive(false); 
            }
        }
        //Debug.Log("1: "+timesUp);
        //if (tabletAnimTimer <= 0) { timesUp = false; }
        if (uiman.fishing && (Input.GetKeyDown(gdm.controlsBinds.pauseMenu) || timesUp))
        {
            playerFishing = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            uiman.fishing = false;
            uiman.playUI.SetActive(true);
            gm.leftGunScript.gameObject.SetActive(true);
            gm.rightGunScript.gameObject.SetActive(true);
            Camera.main.fieldOfView = PlayerPrefs.GetFloat("FOV");
            tabletScreen.SetActive(false);
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            tabletScreen.GetComponent<FishMinigameGAME>().playing = false;
            tabletScreen.SetActive(false);

            fished = true;
        }
        //Debug.Log ("2: "+timesUp);
        if (playerFishing)
        {
            player.GetComponent<Rigidbody>().velocity /= 2f;
            if (tabletAnimTimer > 0) { Camera.main.transform.LookAt(tablet.transform); Camera.main.fieldOfView = 66f; }
            if(tabletAnimTimer <= 0 && tabletScreen.GetComponent<FishMinigameGAME>().playing == false) { tabletScreen.SetActive(true); tabletScreen.GetComponent<FishMinigameGAME>().StartMinigame(); }
        }
        tabletHolder.SetBool("Fishing", playerFishing);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNear = false;
        }
    }
}
