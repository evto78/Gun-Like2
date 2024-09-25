using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayScript : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public GameObject item;
    Item itemScript;
    PlayerItem playerItemScript;

    public List<List<string>> describeList = new List<List<string>>();
    // [0] is the buffs
    // [1] is the debuffs
    // [2] is the effects
    // [3] is the flavor text
    // [4] is the buff summery
    // [5] is the debuff summery
    // [6] is the effect summery
    // add more as needed \/

    public List<string> buffList = new List<string>();
    public List<string> debuffList = new List<string>();
    public List<string> effectList = new List<string>();
    public List<string> flavorList = new List<string>();

    public List<string> buffSummeryList = new List<string>();
    public List<string> debuffSummeryList = new List<string>();
    public List<string> effectSummeryList = new List<string>();

    public List<Sprite> backgroundList = new List<Sprite>();

    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI buffTxt;
    public TextMeshProUGUI debuffTxt;
    public TextMeshProUGUI effectTxt;
    public TextMeshProUGUI flavorTxt;

    public Image bgItem;
    public Image bgFlavor;
    public Image bgOutline;
    public Image bgDesc;
    public Image bgTitle;
    public Image itemSprite;

    private void Start()
    {
        itemScript = item.GetComponent<Item>();
        playerItemScript = player.GetComponent<PlayerItem>();

        describeList.Add(buffList);
        describeList.Add(debuffList);
        describeList.Add(effectList);
        describeList.Add(flavorList);
        describeList.Add(buffSummeryList);
        describeList.Add(debuffSummeryList);
        describeList.Add(effectSummeryList);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.transform.position);
    }

    public void InfoUpdate(int iD, Vector3 itemPos)
    {
        transform.position = new Vector3(itemPos.x, itemPos.y+1f, itemPos.z);

        nameTxt.text = itemScript.itemList[iD];
        if (Input.GetKey(KeyCode.C))
        {
            buffTxt.text = describeList[0][iD];
            debuffTxt.text = describeList[1][iD];
            effectTxt.text = describeList[2][iD];
        }
        else
        {
            buffTxt.text = describeList[4][iD];
            debuffTxt.text = describeList[5][iD];
            effectTxt.text = describeList[6][iD];
        }
        
        flavorTxt.text = describeList[3][iD];

        itemSprite.sprite = itemScript.spriteList[iD];

        for (int i = 0; i < playerItemScript.rarityList.Count; i++)
        {
            if (playerItemScript.rarityList[i].Contains(iD)) { bgFlavor.sprite = backgroundList[i]; bgItem.sprite = backgroundList[i]; bgOutline.sprite = backgroundList[i]; bgDesc.sprite = backgroundList[i]; bgTitle.sprite = backgroundList[i]; }
        }

    }
}
