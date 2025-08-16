using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelection : MonoBehaviour
{
    public Sprite hand;
    List<GunObjectData> gunObjectData;
    //0 is null
    //from there goes down the list
    public Image leftHand;
    public Image rightHand;
    int leftHandVal;
    int rightHandVal;
    public Button readyBtn;
    MainMenuManager menuManager;
    private void Awake()
    {
        menuManager = GameObject.Find("Main Menu Manager").GetComponent<MainMenuManager>();
    }
    private void Start()
    {
        gunObjectData = menuManager.gunObjectData;
        leftHandVal = 0;
        rightHandVal = 0;

        if (PlayerPrefs.HasKey("leftHandGunSelect"))
        {
            leftHandVal = PlayerPrefs.GetInt("leftHandGunSelect")+1;
        }
        if (PlayerPrefs.HasKey("rightHandGunSelect"))
        {
            rightHandVal = PlayerPrefs.GetInt("rightHandGunSelect")+1;
        }
    }
    private void Update()
    {
        if(leftHandVal == -1) { leftHand.sprite = gunObjectData[0].icon; }
        else if (leftHandVal == 0) { leftHand.sprite = hand; }
        else { leftHand.sprite = gunObjectData[leftHandVal].icon; }
        if(rightHandVal == -1) { rightHand.sprite = gunObjectData[0].icon; }
        else if (rightHandVal == 0) { rightHand.sprite = hand; }
        else { rightHand.sprite = gunObjectData[rightHandVal].icon; }

        readyBtn.interactable = (leftHandVal != 0 && rightHandVal != 0) && (leftHandVal != rightHandVal);
    }
    public void SelectWeapon(int id)
    {
        if(leftHandVal == 0)
        {
            leftHandVal = id;
        }
        else if(rightHandVal == 0)
        {
            rightHandVal = id;
        }
        else
        {
            leftHandVal = id;
        }

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
