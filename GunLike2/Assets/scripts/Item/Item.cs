using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public ItemObject itemObj;

    public List<Material> backgroundList = new List<Material>();
    public List<Color> shineColors;
    public int itemID;
    Rigidbody rb;
    public SpriteRenderer sr;

    public MeshRenderer mr;

    public GameObject player;
    PlayerItem playerItem;
    public TrailRenderer trail;
    public ParticleSystem ps;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

        player = GameObject.FindWithTag("Player");
        playerItem = player.GetComponent<PlayerItem>();
        if (playerItem.leftItems[142] + playerItem.rightItems[142] > 0) { SetItemID(143); }

        GameObject.Find("LevelBuilder").GetComponent<LevelBuilder>().placed.Add(gameObject);

        playerItem.healthManager.gdm.spawnedItems.Add(this);
    }
    private void OnDestroy()
    {
        if (playerItem.healthManager.gdm.spawnedItems.Contains(this)) { playerItem.healthManager.gdm.spawnedItems.Remove(this); }
    }
    private void LateUpdate()
    {
        if (playerItem.healthManager.gdm.mutatedRules.Contains(16))
        {
            playerItem.ForcePickup(itemID);
            Taken();
        }
    }
    public void SetItemID(int givenID)
    {
        itemObj = Resources.Load<ItemObject>("Items/"+givenID.ToString());
        itemID = givenID;
        sr.sprite = itemObj.itemSprite;
        if (player == null){player = GameObject.FindWithTag("Player");}
        playerItem = player.GetComponent<PlayerItem>();
        int rarity = playerItem.FindRarityByID(itemID);
        mr.material = backgroundList[rarity];
        trail.material = mr.material;
        ParticleSystem.MainModule psm = ps.main;
        psm.startColor = new ParticleSystem.MinMaxGradient(new Color(1,1,1,0), shineColors[rarity]);
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

            mr.material = backgroundList[playerItem.FindRarityByID(itemID)];
        }
        if (rb.isKinematic) { rb.AddForce(-Vector3.up * 50f, ForceMode.Impulse); }
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
    public IEnumerator GlideToPosition(Vector3 targetPosition)
    {
        float progress = Vector3.Distance(transform.position, targetPosition);

        while (progress > 1) 
        {
            progress = Vector3.Distance(transform.position, targetPosition);
            transform.position += (targetPosition - transform.position).normalized * Time.deltaTime * 10f;
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPosition;

        yield return null;
    }
}
