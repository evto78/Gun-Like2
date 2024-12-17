using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryScript : MonoBehaviour
{
    GameObject player;
    public GameObject item;

    public List<Vector4> leftInventory;
    public List<Vector4> rightInventory;

    public GameObject leftInvObj;
    public GameObject rightInvObj;

    List<Sprite> itemSprites;
    public List<Sprite> itemBg;

    public List<List<GameObject>> leftSlots = new List<List<GameObject>>();
    public List<List<GameObject>> rightSlots = new List<List<GameObject>>();

    private void Start()
    {
        player = GameObject.Find("Player");

        itemSprites = item.GetComponent<Item>().spriteList;

        for(int r = 0; r < 20; r++)
        {
            leftSlots.Add(new List<GameObject>());
            rightSlots.Add(new List<GameObject>());

            for (int c = 0; c < 11; c++)
            {
                leftSlots[r].Add(leftInvObj.transform.GetChild(0).GetChild(r).GetChild(c).gameObject);
                rightSlots[r].Add(rightInvObj.transform.GetChild(0).GetChild(r).GetChild(c).gameObject);
            }
        }
        
    }

    public void ArrangeInventory()
    {
        int leftItemsToBeAdded = leftInventory.Count;
        int rightItemsToBeAdded = rightInventory.Count;

        int leftItemsIndex = 0;
        int rightItemsIndex = 0;

        //left
        for(int i = 0; i < leftSlots.Count; i++)
        {
            for(int q = 0; q < leftSlots[i].Count; q++)
            {
                if(leftItemsToBeAdded > 0)
                {
                    //0 - BG, 1 - Sprite, 2 - Text
                    leftSlots[i][q].transform.GetChild(0).GetComponent<Image>().sprite = itemBg[Mathf.RoundToInt(leftInventory[leftItemsIndex].z)];
                    leftSlots[i][q].transform.GetChild(1).GetComponent<Image>().sprite = itemSprites[Mathf.RoundToInt(leftInventory[leftItemsIndex].x)];
                    leftSlots[i][q].transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    leftSlots[i][q].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""+Mathf.RoundToInt(leftInventory[leftItemsIndex].y);
                    leftItemsToBeAdded--;
                    leftItemsIndex++;
                    
                }
                else
                {
                    leftSlots[i][q].transform.GetChild(0).GetComponent<Image>().sprite = itemBg[0];
                    leftSlots[i][q].transform.GetChild(1).GetComponent<Image>().sprite = null;
                    leftSlots[i][q].transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    leftSlots[i][q].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }

        //right
        for (int i = 0; i < rightSlots.Count; i++)
        {
            for (int q = 0; q < rightSlots[i].Count; q++)
            {
                if (rightItemsToBeAdded > 0)
                {
                    //0 - BG, 1 - Sprite, 2 - Text
                    rightSlots[i][q].transform.GetChild(0).GetComponent<Image>().sprite = itemBg[Mathf.RoundToInt(rightInventory[rightItemsIndex].z)];
                    rightSlots[i][q].transform.GetChild(1).GetComponent<Image>().sprite = itemSprites[Mathf.RoundToInt(rightInventory[rightItemsIndex].x)];
                    rightSlots[i][q].transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    rightSlots[i][q].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "" + Mathf.RoundToInt(rightInventory[rightItemsIndex].y);
                    rightItemsToBeAdded--;
                    rightItemsIndex++;

                }
                else
                {
                    rightSlots[i][q].transform.GetChild(0).GetComponent<Image>().sprite = itemBg[0];
                    rightSlots[i][q].transform.GetChild(1).GetComponent<Image>().sprite = null;
                    rightSlots[i][q].transform.GetChild(1).GetComponent<Image>().color = new Color(1,1,1,0);
                    rightSlots[i][q].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }
    }
}
