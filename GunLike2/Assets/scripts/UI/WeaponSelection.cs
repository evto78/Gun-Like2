using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelection : MonoBehaviour
{
    public List<Sprite> sprites;
    //0 is null
    //from there goes down the list
    public Image leftHand;
    public Image rightHand;
    int leftHandVal;
    int rightHandVal;
    public Button readyBtn;

    private void Start()
    {
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
        leftHand.sprite = sprites[leftHandVal];
        rightHand.sprite = sprites[rightHandVal];

        if((leftHandVal > 0 && rightHandVal > 0) && (leftHandVal != rightHandVal))
        {
            readyBtn.interactable = true;
        }
        else
        {
            readyBtn.interactable = false;
        }
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

        PlayerPrefs.SetInt("leftHandGunSelect", leftHandVal -1);
        PlayerPrefs.SetInt("rightHandGunSelect", rightHandVal -1);
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
