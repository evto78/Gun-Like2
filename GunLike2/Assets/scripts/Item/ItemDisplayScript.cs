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
        transform.LookAt(cam.transform.position + Vector3.up);
    }

    public void InfoUpdate(int iD, Vector3 itemPos)
    {
        transform.position = new Vector3(itemPos.x, itemPos.y+1f, itemPos.z);

        nameTxt.text = itemScript.itemList[iD];
        if (Input.GetKey(KeyCode.C))
        {
            //id 22 is the irradiated french pastry
            if(iD == 22)
            {
                //irradiated french pastry
                string lftBff = "NA";
                string lftDeBff = "NA";
                string rigBff = "NA";
                string rigDeBff = "NA";

                string statToChange = "NA";

                for(int i = 0; i < 29; i++)
                {
                    if (i == 0) { statToChange = "Speed"; }
                    if (i == 1) { statToChange = "Sprint Speed"; }
                    if (i == 2) { statToChange = "Jump Height"; }
                    if (i == 3) { statToChange = "Number Of Jumps"; }
                    if (i == 4) { statToChange = "Crit Chance"; }
                    if (i == 5) { statToChange = "Crit Damage"; }
                    if (i == 6) { statToChange = "Weak Spot Damage"; }
                    if (i == 7) { statToChange = "Damage"; }
                    if (i == 8) { statToChange = "Attack Speed"; }
                    if (i == 9) { statToChange = "Reload Speed"; }
                    if (i == 10) { statToChange = "Magazine Size"; }
                    if (i == 11) { statToChange = "Accuracy"; }
                    if (i == 12) { statToChange = "Bullet Speed"; }
                    if (i == 13) { statToChange = "Bullet Size"; }
                    if (i == 14) { statToChange = "Pierce"; }
                    if (i == 15) { statToChange = "Crit Chance"; }
                    if (i == 16) { statToChange = "Crit Damage"; }
                    if (i == 17) { statToChange = "Weak Spot Damage"; }
                    if (i == 18) { statToChange = "Damage"; }
                    if (i == 19) { statToChange = "Attack Speed"; }
                    if (i == 20) { statToChange = "Reload Speed"; }
                    if (i == 21) { statToChange = "Magazine Size"; }
                    if (i == 22) { statToChange = "Accuracy"; }
                    if (i == 23) { statToChange = "Bullet Speed"; }
                    if (i == 24) { statToChange = "Bullet Size"; }
                    if (i == 25) { statToChange = "Pierce"; }
                    if (i == 26) { statToChange = "Max HP"; }
                    if (i == 27) { statToChange = "Passive HP Regen"; }
                    if (i == 28) { statToChange = "Armor"; }

                    if (playerItemScript.leftIFPStatToBuff == i) { lftBff = statToChange; }
                    if (playerItemScript.leftIFPStatToDeBuff == i) { lftDeBff = statToChange; }
                    if (playerItemScript.rightIFPStatToBuff == i) { rigBff = statToChange; }
                    if (playerItemScript.rightIFPStatToDeBuff == i) { rigDeBff = statToChange; }
                }

                buffTxt.text = "Left Buff: " + lftBff + " X 2(X2)" + " Right Buff: " + rigBff + " X 2(X2)";
                debuffTxt.text = "Left Debuff: " + lftDeBff + " X 0.9(X0.9)" + " Right Debuff: " + rigDeBff + " X 0.9(X0.9)";
                effectTxt.text = "Locks in on pick up.";
            }
            else
            {
                buffTxt.text = describeList[0][iD];
                debuffTxt.text = describeList[1][iD];
                effectTxt.text = describeList[2][iD];
            }
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
