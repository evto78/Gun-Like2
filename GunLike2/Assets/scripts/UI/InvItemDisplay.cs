using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InvItemDisplay : MonoBehaviour
{
    GameObject player;
    PlayerItem playerItemScript;

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

    public Image itemGlobal;

    public Sprite isGlobal;
    public Sprite notGlobal;

    public float timeSinceUpdate;
    RectTransform rectTran;
    private void Start()
    {
        player = GameObject.Find("Player");
        playerItemScript = player.GetComponent<PlayerItem>();
        rectTran = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTran.position = Input.mousePosition;
        CorrectPosition();
        timeSinceUpdate += 1f;
    }
    void CorrectPosition()
    {
        rectTran.pivot = new Vector2(0.5f, 1f);
        Vector2 newPiv = rectTran.pivot;
        if(rectTran.position.y - rectTran.rect.height < 0) { newPiv = new Vector2(newPiv.x, 0f); }
        if(rectTran.position.y > Screen.height) { newPiv = new Vector2(newPiv.x, 1f); }
        if(rectTran.position.x + rectTran.rect.width/2f > Screen.width) { newPiv = new Vector2(1f, newPiv.y); }
        if(rectTran.position.x - rectTran.rect.width/2f < 0) { newPiv = new Vector2(0f, newPiv.y); }
        rectTran.pivot = newPiv;
    }
    private void LateUpdate()
    {
        gameObject.SetActive(timeSinceUpdate <= 2f);
    }
    public void InfoUpdate(int id)
    {
        timeSinceUpdate = 0f;
        ItemObject selectedItem = Resources.Load<ItemObject>("Items/" + id.ToString());

        nameTxt.text = selectedItem.itemName;
        if (Input.GetKey(playerItemScript.healthManager.gdm.instance.controlsBinds.showMoreInformation))
        {
            //id 22 is the irradiated french pastry
            if (selectedItem.id == 22)
            {
                //irradiated french pastry
                string lftBff = "NA";
                string lftDeBff = "NA";
                string rigBff = "NA";
                string rigDeBff = "NA";

                string statToChange = "NA";

                for (int i = 0; i < 29; i++)
                {
                    switch (i)
                    {
                        case 0: statToChange = "Speed"; break;
                        case 1: statToChange = "Sprint Speed"; break;
                        case 2: statToChange = "Jump Height"; break;
                        case 3: statToChange = "Number Of Jumps"; break;
                        case 4: statToChange = "Crit Chance"; break;
                        case 5: statToChange = "Crit Damage"; break;
                        case 6: statToChange = "Weak Spot Damage"; break;
                        case 7: statToChange = "Damage"; break;
                        case 8: statToChange = "Attack Speed"; break;
                        case 9: statToChange = "Reload Speed"; break;
                        case 10: statToChange = "Magazine Size"; break;
                        case 11: statToChange = "Accuracy"; break;
                        case 12: statToChange = "Bullet Speed"; break;
                        case 13: statToChange = "Bullet Size"; break;
                        case 14: statToChange = "Pierce"; break;
                        case 15: statToChange = "Crit Chance"; break;
                        case 16: statToChange = "Crit Damage"; break;
                        case 17: statToChange = "Weak Spot Damage"; break;
                        case 18: statToChange = "Damage"; break;
                        case 19: statToChange = "Attack Speed"; break;
                        case 20: statToChange = "Reload Speed"; break;
                        case 21: statToChange = "Magazine Size"; break;
                        case 22: statToChange = "Accuracy"; break;
                        case 23: statToChange = "Bullet Speed"; break;
                        case 24: statToChange = "Bullet Size"; break;
                        case 25: statToChange = "Pierce"; break;
                        case 26: statToChange = "Max HP"; break;
                        case 27: statToChange = "Passive HP Regen"; break;
                        case 28: statToChange = "Armor"; break;
                    }

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
                buffTxt.text = selectedItem.buff;
                debuffTxt.text = selectedItem.debuff;
                effectTxt.text = selectedItem.effect;
            }
        }
        else
        {
            buffTxt.text = selectedItem.buffSum;
            debuffTxt.text = selectedItem.debuffSum;
            effectTxt.text = selectedItem.effectSum;
        }

        flavorTxt.text = selectedItem.flavor;

        itemSprite.sprite = selectedItem.itemSprite;

        if (selectedItem.globalItem) { itemGlobal.sprite = isGlobal; } else { itemGlobal.sprite = notGlobal; }

        SetRarity(selectedItem.rarity.ToString());

    }
    void SetRarity(string rarity)
    {
        int temp = 0;
        if (rarity == "Common") { temp = 0; }
        else if (rarity == "Uncommon") { temp = 1; }
        else if (rarity == "Rare") { temp = 2; }
        else if (rarity == "Legendary") { temp = 3; }
        else if (rarity == "Mutated") { temp = 4; }
        else if (rarity == "Haunted") { temp = 5; }
        else if (rarity == "Irradiated") { temp = 6; }
        else if (rarity == "Nuclear") { temp = 7; }
        else if (rarity == "Unique") { temp = 8; }

        bgFlavor.sprite = backgroundList[temp];
        bgItem.sprite = backgroundList[temp];
        bgOutline.sprite = backgroundList[temp];
        bgDesc.sprite = backgroundList[temp];
        bgTitle.sprite = backgroundList[temp];
    }
}
