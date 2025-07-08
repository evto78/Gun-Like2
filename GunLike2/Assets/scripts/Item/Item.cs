using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemObject itemObj;

    public List<Material> backgroundList = new List<Material>();

    public int itemID;
    Rigidbody rb;
    public SpriteRenderer sr;

    public MeshRenderer mr;

    public GameObject player;
    PlayerItem playerItem;
    public TrailRenderer trail;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

        player = GameObject.FindWithTag("Player");
        playerItem = player.GetComponent<PlayerItem>();
        if (playerItem.leftItems[142] + playerItem.rightItems[142] > 0) { SetItemID(143); }
    }

    public void SetItemID(int givenID)
    {
        itemObj = Resources.Load<ItemObject>("Items/"+givenID.ToString());
        itemID = givenID;
        sr.sprite = itemObj.itemSprite;
        if (player == null){player = GameObject.FindWithTag("Player");}
        playerItem = player.GetComponent<PlayerItem>();
        mr.material = backgroundList[playerItem.FindRarityByID(itemID)];
        trail.material = mr.material;
    }

    public int WhatItem()
    {
        return itemID;
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerItem = player.GetComponent<PlayerItem>();

            for (int i = 0; i < playerItem.rarityList.Count; i++)
            {
                if (playerItem.rarityList[i].Contains(itemID)) { mr.material = backgroundList[i]; }
            }
        }
    }
    public void Taken()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
