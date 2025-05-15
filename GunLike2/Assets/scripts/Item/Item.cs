using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemObject itemObj;

    public List<Material> backgroundList = new List<Material>();

    public int itemID;
    public TextMeshPro itemText;
    Rigidbody rb;
    public SpriteRenderer sr;

    public MeshRenderer mr;

    public GameObject player;
    PlayerItem playerItem;

    private void Start()
    {
        itemText.text = "";

        rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        
    }

    public void SetItemID(int givenID)
    {
        //happens b4 start
        Debug.Log("spawning item: " + givenID);
        itemObj = Resources.Load<ItemObject>("Items/"+givenID.ToString());
        itemID = givenID;
        sr.sprite = itemObj.itemSprite;
    }

    public int WhatItem()
    {
        return itemID;
    }

    public void StayStill()
    {
        //rb.velocity = Vector3.zero;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
}
