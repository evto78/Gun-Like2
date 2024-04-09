using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public List<Sprite> spriteList;

    public List<string> itemList;
    public int itemID;
    public TextMeshPro itemText;
    Rigidbody rb;
    public SpriteRenderer sr;

    private void Start()
    {
        itemText.text = "";
        //itemText.text = itemList[itemID];

        rb = GetComponent<Rigidbody>();
    }

    public void SetItemID(int givenID)
    {
        itemID = givenID;
        sr.sprite = spriteList[givenID];
    }

    public int WhatItem()
    {
        return itemID;
    }
}
