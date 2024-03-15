using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public List<string> itemList;
    public int itemID;
    public TextMeshPro itemText;

    private void Start()
    {
        itemText.text = itemList[itemID];

    }

    public void SetItemID(int givenID)
    {
        itemID = givenID;
    }

    public int WhatItem()
    {
        return itemID;
    }
}
