using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelection : MonoBehaviour
{
    public Sprite handL;
    public Sprite handR;
    List<GunObjectData> gunObjectData;
    public Image leftHand;
    public Image rightHand;
    int leftHandVal;
    int rightHandVal;
    public Button readyBtn;
    MainMenuManager menuManager;
    public WeaponInfoDisplay infoDisplay;
    private void Awake()
    {
        menuManager = GameObject.Find("Main Menu Manager").GetComponent<MainMenuManager>();
    }
    private void Start()
    {
        infoDisplay.gameObject.SetActive(false);
        gunObjectData = menuManager.gunObjectData;
        leftHandVal = 0;
        rightHandVal = 0;

        if (PlayerPrefs.HasKey("leftHandGunSelect"))
        {
            leftHandVal = PlayerPrefs.GetInt("leftHandGunSelect")+1;
            if(leftHandVal == 0) { leftHandVal--; }
        }
        if (PlayerPrefs.HasKey("rightHandGunSelect"))
        {
            rightHandVal = PlayerPrefs.GetInt("rightHandGunSelect")+1;
            if(rightHandVal == 0) { rightHandVal--; }
        }
    }
    private void Update()
    {
        if(leftHandVal == -1) { leftHand.sprite = gunObjectData[0].icon; }
        else if (leftHandVal == 0) { leftHand.sprite = handL; }
        else { leftHand.sprite = gunObjectData[leftHandVal].icon; }
        if(rightHandVal == -1) { rightHand.sprite = gunObjectData[0].icon; }
        else if (rightHandVal == 0) { rightHand.sprite = handR; }
        else { rightHand.sprite = gunObjectData[rightHandVal].icon; }

        readyBtn.interactable = (leftHandVal != 0 && rightHandVal != 0) && (leftHandVal != rightHandVal);
    }
    public void WeaponHover(int id, int soundId)
    {
        infoDisplay.gameObject.SetActive(true);
        infoDisplay.InfoUpdate(gunObjectData[id]);

        if(id == 0) { id = -1; }

        if (Input.GetMouseButtonDown(0)) { SelectWeapon(id, 0, soundId); }
        if (Input.GetMouseButtonDown(1)) { SelectWeapon(id, 1, soundId); }
    }
    public void SelectWeapon(int id, int input, int soundId)
    {
        switch (input)
        {
            case 0:
                leftHandVal = id;
                break;
            case 1:
                rightHandVal = id;
                break;
        }
        menuManager.usp.PlaySoundByKey(soundId);

        int leftSetVal = leftHandVal; int rightSetVal = rightHandVal;

        if(leftSetVal != -1) { leftSetVal -= 1; }
        if(rightSetVal != -1) { rightSetVal -= 1; }

        PlayerPrefs.SetInt("leftHandGunSelect", leftSetVal);
        PlayerPrefs.SetInt("rightHandGunSelect", rightSetVal);
    }
    public void DeselectWeapon(string hand)
    {
        if(hand == "left")
        {
            leftHandVal = 0;
        }
        else
        {
            rightHandVal = 0;
        }
    }
}
